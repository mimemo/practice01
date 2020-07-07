using System;

using Practice03prototype.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Practice03prototype.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = this.ViewModel;
            this.ViewModel.Initialize(this.shellFrame, this.navigationView, this.KeyboardAccelerators);
        }
    }
}
