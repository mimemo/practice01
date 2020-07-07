using System;
using System.Linq;
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

            this.repeater2.ItemsSource = Enumerable.Range(0, 500);
        }


    }
}
