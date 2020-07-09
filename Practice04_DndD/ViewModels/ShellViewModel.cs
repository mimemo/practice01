using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Practice04_DndD.Helpers;
using Practice04_DndD.Services;

using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace Practice04_DndD.ViewModels
{
    public class ShellViewModel : Observable
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool _isBackEnabled;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _loadedCommand;
        private ICommand _itemInvokedCommand;

        public bool IsBackEnabled
        {
            get { return this._isBackEnabled; }
            set { this.Set(ref this._isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return this._selected; }
            set { this.Set(ref this._selected, value); }
        }

        public ICommand LoadedCommand => this._loadedCommand ?? (this._loadedCommand = new RelayCommand(this.OnLoaded));

        public ICommand ItemInvokedCommand => this._itemInvokedCommand ?? (this._itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(this.OnItemInvoked));

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            this._navigationView = navigationView;
            this._keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += this.Frame_NavigationFailed;
            NavigationService.Navigated += this.Frame_Navigated;
            this._navigationView.BackRequested += this.OnBackRequested;
        }

        private async void OnLoaded()
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            this._keyboardAccelerators.Add(this._altLeftKeyboardAccelerator);
            this._keyboardAccelerators.Add(this._backKeyboardAccelerator);
            await Task.CompletedTask;
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            var item = this._navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            this.IsBackEnabled = NavigationService.CanGoBack;
            this.Selected = this._navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => this.IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }
    }
}
