using System;
using System.Security.Cryptography.X509Certificates;
using Practice03prototype.Helpers;
using Reactive.Bindings.Notifiers;

namespace Practice03prototype.ViewModels
{
    public class MainViewModel : Observable
    {
        public BooleanNotifier IsOpenBibliographyPanel { get; set; } = new BooleanNotifier(true);
        public BooleanNotifier IsOpenPreferencePanel { get; set; } = new BooleanNotifier(true);

        public MainViewModel()
        {
        }
    }
}
