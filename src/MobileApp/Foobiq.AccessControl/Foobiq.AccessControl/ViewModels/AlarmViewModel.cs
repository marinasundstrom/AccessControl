using Foobiq.AccessControl.AppService;
using Foobiq.AccessControl.AppService.Contracts;
using Foobiq.AccessControl.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Foobiq.AccessControl.ViewModels
{
    public class AlarmViewModel : BindableBase, IDisposable
    {
        private const string DeviceId = "AccessPoint1";

        private readonly IAlarmClient alarmClient;
        private readonly IAlarmNotificationClient alarmNotificationClient;
        private readonly INavigationService _navigationService;
        private IDisposable subscription;
        private string state;
        private TimeSpan accessTime = TimeSpan.FromSeconds(10);
        private bool armOnClose = true;
        private bool lockOnClose = true;
        private int accessTimeValue = 10;
        private bool isArmed;
        private bool isDisarmed;

        public AlarmViewModel(
            IAlarmClient alarmClient, 
            IAlarmNotificationClient alarmNotificationClient, 
            INavigationService navigationService)
        {
            this.alarmClient = alarmClient;
            this.alarmNotificationClient = alarmNotificationClient;
            _navigationService = navigationService;
            ArmCommand = new Command(async () => await ExecuteArmCommand());
            DisarmCommand = new Command(async () => await ExecuteDisarmCommand());
            ConfigureCommand = new Command(async () => await ExecuteConfigureCommand());
        }

        private async Task ExecuteArmCommand()
        {
            await alarmClient.ArmAsync(DeviceId);
        }

        private async Task ExecuteDisarmCommand()
        {
            await alarmClient.DisarmAsync(DeviceId);
        }

        public async Task InitializeAsync()
        {
            try
            {
                await Task.WhenAll(
                    GetConfiguration(), 
                    GetAlarmState());

                subscription = this.alarmNotificationClient.WhenMessageReceived.Subscribe(notification =>
                {
                    //State = notification.Title;
                    var obj = JObject.Parse(notification.Title);
                    if (obj.Value<string>("Event") == "AlarmEvent")
                    {
                        var alarmState = obj.Value<string>("AlarmState");
                        SetState((AlarmState)Enum.Parse(typeof(AlarmState), alarmState));
                    }
                });

            }
            catch (Exception)
            {
                SecureStorage.Remove("jwt_token");
            }

            await alarmNotificationClient.StartAsync();
        }

        private async Task GetAlarmState()
        {
            var stateResult = await alarmClient.GetStateAsync(DeviceId);
            SetState(stateResult.AlarmState);
        }

        private async Task GetConfiguration()
        {
            var config = await alarmClient.GetConfigurationAsync(DeviceId);
            AccessTimeValue = (int)config.AccessTime.TotalSeconds;
            LockOnClose = config.LockOnClose;
            ArmOnClose = config.ArmOnClose;
        }

        private void SetState(AlarmState alarmState)
        {
            switch (alarmState)
            {
                case AlarmState.Armed:
                    IsArmed = true;
                    IsDisarmed = false;
                    break;

                case AlarmState.Disarmed:
                    IsArmed = false;
                    IsDisarmed = true;
                    break;
            }
        }

        public new string Title => "Alarm";

        public string State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        public Command ArmCommand { get; set; }

        public Command DisarmCommand { get; set; }

        public int AccessTimeValue
        {
            get => accessTimeValue;
            set
            {
                SetProperty(ref accessTimeValue, value);
                AccessTime = TimeSpan.FromSeconds(accessTimeValue);
            }
        }

        public TimeSpan AccessTime
        {
            get => accessTime;
            set => SetProperty(ref accessTime, value);
        }

        public bool LockOnClose
        {
            get => lockOnClose;
            set => SetProperty(ref lockOnClose, value);
        }

        public bool ArmOnClose
        {
            get => armOnClose;
            set => SetProperty(ref armOnClose, value);
        }

        public bool IsArmed
        {
            get => isArmed;
            set => SetProperty(ref isArmed, value);
        }

        public bool IsDisarmed
        {
            get => isDisarmed;
            set => SetProperty(ref isDisarmed, value);
        }

        public Command ConfigureCommand { get; set; }

        private async Task ExecuteConfigureCommand()
        {
            await alarmClient.ConfigureAsync(DeviceId, new SetAlarmConfigurationCommand
            {
                DeviceId = DeviceId,
                AccessTime = AccessTime,
                ArmOnClose = ArmOnClose,
                LockOnClose = LockOnClose
            });
        }

        public void Dispose()
        {
            subscription.Dispose();
        }
    }
}
