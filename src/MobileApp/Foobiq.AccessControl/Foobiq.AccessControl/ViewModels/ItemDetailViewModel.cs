using System;
using Foobiq.AccessControl.AppService.Contracts;

namespace Foobiq.AccessControl.ViewModels
{
    public class ItemDetailViewModel : BindableBase
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
