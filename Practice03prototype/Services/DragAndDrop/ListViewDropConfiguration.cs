using System.Windows.Input;

using Windows.UI.Xaml;

namespace Practice03prototype.Services.DragAndDrop
{
    public class ListViewDropConfiguration : DropConfiguration
    {
        public static readonly DependencyProperty DragItemsStartingCommandProperty =
            DependencyProperty.Register("DragItemsStartingCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragItemsCompletedCommandProperty =
            DependencyProperty.Register("DragItemsCompletedCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public ICommand DragItemsStartingCommand
        {
            get { return (ICommand)this.GetValue(DragItemsStartingCommandProperty); }
            set { this.SetValue(DragItemsStartingCommandProperty, value); }
        }

        public ICommand DragItemsCompletedCommand
        {
            get { return (ICommand)this.GetValue(DragItemsCompletedCommandProperty); }
            set { this.SetValue(DragItemsCompletedCommandProperty, value); }
        }
    }
}
