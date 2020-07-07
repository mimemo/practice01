using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Practice03prototype.Helpers;
using Practice03prototype.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Practice03prototype.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/settings.md
    public class SettingsViewModel : Observable
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return this._elementTheme; }

            set { this.Set(ref this._elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return this._versionDescription; }

            set { this.Set(ref this._versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (this._switchThemeCommand == null)
                {
                    this._switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            this.ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return this._switchThemeCommand;
            }
        }

        public Visibility FeedbackLinkVisibility => Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported() ? Visibility.Visible : Visibility.Collapsed;

        private ICommand _launchFeedbackHubCommand;

        public ICommand LaunchFeedbackHubCommand
        {
            get
            {
                if (this._launchFeedbackHubCommand == null)
                {
                    this._launchFeedbackHubCommand = new RelayCommand(
                        async () =>
                        {
                            // This launcher is part of the Store Services SDK https://docs.microsoft.com/windows/uwp/monetize/microsoft-store-services-sdk
                            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
                            await launcher.LaunchAsync();
                        });
                }

                return this._launchFeedbackHubCommand;
            }
        }

        public SettingsViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            this.VersionDescription = this.GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
