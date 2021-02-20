using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AccessControl.Views;
using AppService;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace AccessControl.ViewModels
{
    public class ItemsViewModel : BindableBase
    {
        private readonly IItemsClient itemsClient;
        private readonly ILogger<ItemsViewModel> logger;

        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel(IItemsClient itemsClient, ILogger<ItemsViewModel> logger)
        {
            this.itemsClient = itemsClient;
            this.logger = logger;

            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await itemsClient.PostAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await itemsClient.GetAllAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
