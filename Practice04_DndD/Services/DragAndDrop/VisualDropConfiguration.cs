using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Practice04_DndD.Services.DragAndDrop
{
    public class VisualDropConfiguration : DependencyObject
    {
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(VisualDropConfiguration), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsCaptionVisibleProperty =
            DependencyProperty.Register("IsCaptionVisible", typeof(bool), typeof(VisualDropConfiguration), new PropertyMetadata(true));

        public static readonly DependencyProperty IsContentVisibleProperty =
            DependencyProperty.Register("IsContentVisible", typeof(bool), typeof(VisualDropConfiguration), new PropertyMetadata(true));

        public static readonly DependencyProperty IsGlyphVisibleProperty =
            DependencyProperty.Register("IsGlyphVisible", typeof(bool), typeof(VisualDropConfiguration), new PropertyMetadata(true));

        public static readonly DependencyProperty DragStartingImageProperty =
            DependencyProperty.Register("DragStartingImage", typeof(ImageSource), typeof(VisualDropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropOverImageProperty =
            DependencyProperty.Register("DropOverImage", typeof(ImageSource), typeof(VisualDropConfiguration), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)this.GetValue(CaptionProperty); }
            set { this.SetValue(CaptionProperty, value); }
        }

        public bool IsCaptionVisible
        {
            get { return (bool)this.GetValue(IsCaptionVisibleProperty); }
            set { this.SetValue(IsCaptionVisibleProperty, value); }
        }

        public bool IsContentVisible
        {
            get { return (bool)this.GetValue(IsContentVisibleProperty); }
            set { this.SetValue(IsContentVisibleProperty, value); }
        }

        public bool IsGlyphVisible
        {
            get { return (bool)this.GetValue(IsGlyphVisibleProperty); }
            set { this.SetValue(IsGlyphVisibleProperty, value); }
        }

        public ImageSource DragStartingImage
        {
            get { return (ImageSource)this.GetValue(DragStartingImageProperty); }
            set { this.SetValue(DragStartingImageProperty, value); }
        }

        public ImageSource DropOverImage
        {
            get { return (ImageSource)this.GetValue(DropOverImageProperty); }
            set { this.SetValue(DropOverImageProperty, value); }
        }
    }
}
