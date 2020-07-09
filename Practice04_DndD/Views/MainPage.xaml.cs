using System;
using System.Collections.ObjectModel;
using System.Linq;
using Practice04_DndD.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Practice04_DndD.Views
{

    public class Epage
    {
        public int Index { get; set; } = -1;
    }

    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();


        public ObservableCollection<Epage> Pages1 { get; set; } = new ObservableCollection<Epage>();
        public ObservableCollection<Epage> Pages2 { get; set; } = new ObservableCollection<Epage>();


        public MainPage()
        {
            this.InitializeComponent();


            foreach(var item in Enumerable.Range(0, 100))
            {
                this.Pages1.Add(new Epage() { Index = item });
                this.Pages2.Add(new Epage() { Index = item });
            }
        }

        private void Grid_Drop1(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ドロップされました");

            if((e.OriginalSource as Grid)?.DataContext is Epage item)
            {
                System.Diagnostics.Debug.WriteLine("Epageでした1" + item.Index);
            }
        }

        private void Grid_Drop2(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ドロップされました");

            if((e.OriginalSource as Grid)?.DataContext is Epage item)
            {
                System.Diagnostics.Debug.WriteLine("Epageでした2" + item.Index);
            }
        }

        private void Grid_DragOver1(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Link;
        }

        private void Grid_DragOver2(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Link;
        }

        private const string EPAGE = "EPAGE";

        private void Grid_DragStarting1(UIElement sender, DragStartingEventArgs args)
        {
            //if((sender as Grid)?.DataContext is Epage item)
            //{
            //    args.AllowedOperations = DataPackageOperation.Link;
            //    args.Data.SetData(EPAGE, item);
            //}
        }

        private void Grid_DragStarting2(UIElement sender, DragStartingEventArgs args)
        {
            //if((sender as Grid)?.DataContext is Epage item)
            //{
            //    args.AllowedOperations = DataPackageOperation.Link;
            //    args.Data.SetData(EPAGE, item);
            //}
        }
    }
}
