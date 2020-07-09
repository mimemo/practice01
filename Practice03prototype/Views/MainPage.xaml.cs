using System;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Practice03prototype.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Practice03prototype.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel Vm { get; } = new MainViewModel();

        public MainPage()
        {

            this.InitializeComponent();

        }

        private void repeater2_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            var iRepeater = sender as ItemsRepeater;
            if(iRepeater == null)
            {
                return;
            }

            var col = (int)(iRepeater.ActualWidth / 300) * 2;


            this.unilayout.MaximumRowsOrColumns = col;



        }
    }
}
