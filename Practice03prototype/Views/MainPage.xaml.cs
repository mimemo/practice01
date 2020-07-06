using System;

using Practice03prototype.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Practice03prototype.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
