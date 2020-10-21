using System.ComponentModel;
using Xamarin.Forms;
using Xam.ViewModels;

namespace Xam.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}