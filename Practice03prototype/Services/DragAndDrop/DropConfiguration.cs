using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace Practice03prototype.Services.DragAndDrop
{
    public class DropConfiguration : DependencyObject
    {
        public static readonly DependencyProperty DropBitmapCommandProperty =
            DependencyProperty.Register("DropBitmapCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropHtmlCommandProperty =
            DependencyProperty.Register("DropHtmlCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropRtfCommandProperty =
            DependencyProperty.Register("DropRtfCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropStorageItemsCommandProperty =
            DependencyProperty.Register("DropStorageItemsCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropTextCommandProperty =
            DependencyProperty.Register("DropTextCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropApplicationLinkCommandProperty =
            DependencyProperty.Register("DropApplicationLinkCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropWebLinkCommandProperty =
            DependencyProperty.Register("DropWebLinkCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropDataViewCommandProperty =
            DependencyProperty.Register("DropDataViewCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragEnterCommandProperty =
            DependencyProperty.Register("DragEnterCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragOverCommandProperty =
            DependencyProperty.Register("DragOverCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragLeaveCommandProperty =
            DependencyProperty.Register("DragLeaveCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public ICommand DropBitmapCommand
        {
            get { return (ICommand)this.GetValue(DropBitmapCommandProperty); }
            set { this.SetValue(DropBitmapCommandProperty, value); }
        }

        public ICommand DropHtmlCommand
        {
            get { return (ICommand)this.GetValue(DropHtmlCommandProperty); }
            set { this.SetValue(DropHtmlCommandProperty, value); }
        }

        public ICommand DropRtfCommand
        {
            get { return (ICommand)this.GetValue(DropRtfCommandProperty); }
            set { this.SetValue(DropRtfCommandProperty, value); }
        }

        public ICommand DropStorageItemsCommand
        {
            get { return (ICommand)this.GetValue(DropStorageItemsCommandProperty); }
            set { this.SetValue(DropStorageItemsCommandProperty, value); }
        }

        public ICommand DropTextCommand
        {
            get { return (ICommand)this.GetValue(DropTextCommandProperty); }
            set { this.SetValue(DropTextCommandProperty, value); }
        }

        public ICommand DropApplicationLinkCommand
        {
            get { return (ICommand)this.GetValue(DropApplicationLinkCommandProperty); }
            set { this.SetValue(DropApplicationLinkCommandProperty, value); }
        }

        public ICommand DropWebLinkCommand
        {
            get { return (ICommand)this.GetValue(DropWebLinkCommandProperty); }
            set { this.SetValue(DropWebLinkCommandProperty, value); }
        }

        public ICommand DropDataViewCommand
        {
            get { return (ICommand)this.GetValue(DropDataViewCommandProperty); }
            set { this.SetValue(DropDataViewCommandProperty, value); }
        }

        public ICommand DragEnterCommand
        {
            get { return (ICommand)this.GetValue(DragEnterCommandProperty); }
            set { this.SetValue(DragEnterCommandProperty, value); }
        }

        public ICommand DragOverCommand
        {
            get { return (ICommand)this.GetValue(DragOverCommandProperty); }
            set { this.SetValue(DragOverCommandProperty, value); }
        }

        public ICommand DragLeaveCommand
        {
            get { return (ICommand)this.GetValue(DragLeaveCommandProperty); }
            set { this.SetValue(DragLeaveCommandProperty, value); }
        }

        public async Task ProcessComandsAsync(DataPackageView dataview)
        {
            if (this.DropDataViewCommand != null)
            {
                this.DropDataViewCommand.Execute(dataview);
            }

            if (dataview.Contains(StandardDataFormats.ApplicationLink) && this.DropApplicationLinkCommand != null)
            {
                Uri uri = await dataview.GetApplicationLinkAsync();
                this.DropApplicationLinkCommand.Execute(uri);
            }

            if (dataview.Contains(StandardDataFormats.Bitmap) && this.DropBitmapCommand != null)
            {
                RandomAccessStreamReference stream = await dataview.GetBitmapAsync();
                this.DropBitmapCommand.Execute(stream);
            }

            if (dataview.Contains(StandardDataFormats.Html) && this.DropHtmlCommand != null)
            {
                string html = await dataview.GetHtmlFormatAsync();
                this.DropHtmlCommand.Execute(html);
            }

            if (dataview.Contains(StandardDataFormats.Rtf) && this.DropRtfCommand != null)
            {
                string rtf = await dataview.GetRtfAsync();
                this.DropRtfCommand.Execute(rtf);
            }

            if (dataview.Contains(StandardDataFormats.StorageItems) && this.DropStorageItemsCommand != null)
            {
                IReadOnlyList<IStorageItem> storageItems = await dataview.GetStorageItemsAsync();
                this.DropStorageItemsCommand.Execute(storageItems);
            }

            if (dataview.Contains(StandardDataFormats.Text) && this.DropTextCommand != null)
            {
                string text = await dataview.GetTextAsync();
                this.DropTextCommand.Execute(text);
            }

            if (dataview.Contains(StandardDataFormats.WebLink) && this.DropWebLinkCommand != null)
            {
                Uri uri = await dataview.GetWebLinkAsync();
                this.DropWebLinkCommand.Execute(uri);
            }
        }
    }
}
