using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Practice03prototype.Helpers;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;

namespace Practice03prototype.ViewModels
{

    public class Epage
    {
        public int Index { get; set; } = -1;
    }

    public class MainViewModel : Observable
    {
        public BooleanNotifier IsOpenBibliographyPanel { get; set; } = new BooleanNotifier(true);
        public BooleanNotifier IsOpenPreferencePanel { get; set; } = new BooleanNotifier(true);

        public ReactiveCollection<Epage> Pages = new ReactiveCollection<Epage>();

        public MainViewModel()
        {
            this.Pages.AddRangeOnScheduler(Enumerable.Range(0, 400).Select(x => new Epage() { Index = x }));
        }
    }
}
