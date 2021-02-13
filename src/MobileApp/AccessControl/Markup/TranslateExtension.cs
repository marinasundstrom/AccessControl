using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccessControl.Markup
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private static bool _initialized = false;
        private readonly static IResourceContainer _resourceContainer;

        static TranslateExtension()
        {
            _resourceContainer = App.ServiceProvider.GetService<IResourceContainer>();
        }

        public TranslateExtension()
        {
            _initialized = true;        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!_initialized)
                throw new NullReferenceException($"{nameof(TranslateExtension)} can not be called as it was not initialized. You must call Init() first.");

            if (Text == null)
                return "";

            var translation = _resourceContainer.GetString(Text);

            if (translation == null)
            {
                Debug.WriteLine(String.Format("Key '{0}' was not found in resources.", Text)); // I want to know about this during debugging

                translation = Text; // Returns the key, which gets displayed to the user as a last resort effort to display something meaningful
            }

            return translation;
        }
    }
}
