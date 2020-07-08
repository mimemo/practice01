using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using LayoutContext = Microsoft.UI.Xaml.Controls.LayoutContext;
using VirtualizingLayout = Microsoft.UI.Xaml.Controls.VirtualizingLayout;
using VirtualizingLayoutContext = Microsoft.UI.Xaml.Controls.VirtualizingLayoutContext;

namespace Practice03prototype.Common
{
    class ActivityFeedLayout : VirtualizingLayout
    {
        #region Layout parameters

        //  依存関係プロパティのコピーをキャッシュして、レイアウト中に GetValue を呼び出さないようにします。
        private double _rowSpacing;
        private double _colSpacing;
        private Size _minItemSize = Size.Empty;

        /// <summary>
        /// 行の間に含める空白のGutterのサイズを取得または設定します。
        /// </summary>
        public double RowSpacing
        {
            get { return this._rowSpacing; }
            set { this.SetValue(RowSpacingProperty, value); }
        }

        public static readonly DependencyProperty RowSpacingProperty =
            DependencyProperty.Register(
                "RowSpacing",
                typeof(double),
                typeof(ActivityFeedLayout),
                new PropertyMetadata(0, OnPropertyChanged));

        /// <summary>
        /// 同じ行の項目間に含める空白のGutterのサイズを取得または設定します。
        /// </summary>
        public double ColumnSpacing
        {
            get { return this._colSpacing; }
            set { this.SetValue(ColumnSpacingProperty, value); }
        }

        public static readonly DependencyProperty ColumnSpacingProperty =
            DependencyProperty.Register(
                "ColumnSpacing",
                typeof(double),
                typeof(ActivityFeedLayout),
                new PropertyMetadata(0, OnPropertyChanged));

        public Size MinItemSize
        {
            get { return this._minItemSize; }
            set { this.SetValue(MinItemSizeProperty, value); }
        }

        public static readonly DependencyProperty MinItemSizeProperty =
            DependencyProperty.Register(
                "MinItemSize",
                typeof(Size),
                typeof(ActivityFeedLayout),
                new PropertyMetadata(Size.Empty, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var layout = obj as ActivityFeedLayout;
            if(args.Property == RowSpacingProperty)
            {
                layout._rowSpacing = (double)args.NewValue;
            }
            else if(args.Property == ColumnSpacingProperty)
            {
                layout._colSpacing = (double)args.NewValue;
            }
            else if(args.Property == MinItemSizeProperty)
            {
                layout._minItemSize = (Size)args.NewValue;
            }
            else
            {
                throw new InvalidOperationException("Don't know what you are talking about!");
            }

            layout.InvalidateMeasure();
        }

        #endregion

        #region Setup / teardown

        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.InitializeForContextCore(context);

            if(!(context.LayoutState is ActivityFeedLayoutState state))
            {
                // 理論的には）レイアウトは複数の要素で同時に使用される可能性があるので、必要な状態を保存します。
                // 実際には、Xbox Activity Feed の場合は、おそらく単一のインスタンスしかありません。
                context.LayoutState = new ActivityFeedLayoutState();
            }
        }

        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.UninitializeForContextCore(context);

            // clear any state
            context.LayoutState = null;
        }

        #endregion

        #region Layout

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            if(this.MinItemSize == Size.Empty)
            {
                var firstElement = context.GetOrCreateElementAt(0);
                firstElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                // メンバー値を直接設定してレイアウトの無効化をスキップする
                this._minItemSize = firstElement.DesiredSize;
            }

            System.Diagnostics.Debug.WriteLine($"{context.RealizationRect.X},{context.RealizationRect.Y},|| {this.MinItemSize.Height},{this.MinItemSize.Width} ");

            // どの行を実現する必要があるかを決定します。 どの行も同じ高さで、3つの項目しか含まれていないことがわかっています。 
            // これを使って、実現される矩形内の最初の項目と最後の項目のインデックスを決定します。
            var firstRowIndex = Math.Max(
                (int)(context.RealizationRect.Y / (this.MinItemSize.Height + this.RowSpacing)) - 1,
                0);
            var lastRowIndex = Math.Min(
                (int)(context.RealizationRect.Bottom / (this.MinItemSize.Height + this.RowSpacing)) + 1,
                (int)(context.ItemCount / 3));

            // これらの行に表示されるアイテムを特定し、各アイテムのための長方形を決定します。
            var state = context.LayoutState as ActivityFeedLayoutState;
            state.LayoutRects.Clear();

            // 最初に表示されたアイテムのインデックスを保存します。 アレンジ時の起点として使用します。
            state.FirstRealizedIndex = firstRowIndex * 2;

            // 空いているスペースを埋めるために伸縮する理想的なアイテム幅
            double desiredItemWidth = Math.Max(this.MinItemSize.Width, (availableSize.Width - this.ColumnSpacing * 1) / 2);

            // Foreach item between the first and last index, 
            //     Call GetElementOrCreateElementAt which causes an element to either be realized or retrieved 
            //       from a recycle pool
            //     Measure the element using an appropriate size
            // 
            // Any element that was previously realized which we don't retrieve in this pass (via a call to 
            // GetElementOrCreateAt) will be automatically cleared and set aside for later re-use.  
            // Note: While this work fine, it does mean that more elements than are required may be
            // created because it isn't until after our MeasureOverride completes that the unused elements 
            // will be recycled and available to use.  We could avoid this by choosing to track the first/last
            // index from the previous layout pass.  The diff between the previous range and current range 
            // would represent the elements that we can preemptively make available for re-use by calling 
            // context.RecycleElement(element).
            //最初のインデックスと最後のインデックスの間の項目を検索します。
            //GetElementOrCreateElementAt を呼び出すと、要素が実現されるか、リサイクルプールから取得されます。
            //適切なサイズを使用して要素を測定します。

            //以前に実現された要素で、このパスで（GetElementOrCreateAt の呼び出しを介して）取得しなかったものは、自動的にクリアされ、後で再利用するために脇に置かれます。 
            //注意してください。
            //これは正常に動作しますが、必要以上に多くの要素が作成される可能性があることを意味します。 
            //これは、前回のレイアウトパスの最初 / 最後のインデックスを追跡することを選択することで回避できます。 
            //前の範囲と現在の範囲の差分は、context.RecycleElement(element) を呼び出すことで再利用可能にすることができる要素を表します。

            for(int rowIndex = firstRowIndex; rowIndex < lastRowIndex; rowIndex++)
            {
                int firstItemIndex = rowIndex * 2;
                var boundsForCurrentRow = this.CalculateLayoutBoundsForRow(rowIndex, desiredItemWidth);

                for(int columnIndex = 0; columnIndex < 2; columnIndex++)
                {
                    var index = firstItemIndex + columnIndex;
                    var rect = boundsForCurrentRow[index % 2];
                    var container = context.GetOrCreateElementAt(index);

                    container.Measure(
                        new Size(boundsForCurrentRow[columnIndex].Width, boundsForCurrentRow[columnIndex].Height));

                    state.LayoutRects.Add(boundsForCurrentRow[columnIndex]);
                }
            }

            // 最後の項目の下/右の位置がどうなるかを計算して、すべてのコンテンツ（表示されていてもいなくても）のサイズを計算して返す。
            var extentHeight = ((int)(context.ItemCount / 3) - 1) * (this.MinItemSize.Height + this.RowSpacing) + this.MinItemSize.Height;

            System.Diagnostics.Debug.WriteLine($"{desiredItemWidth},{extentHeight} ");


            // これをレイアウトのサイズとして報告する
            return new Size(desiredItemWidth * 2 + this.ColumnSpacing * 1, extentHeight);
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            // コンテナのキャッシュを再計算
            var state = context.LayoutState as ActivityFeedLayoutState;
            var virtualContext = context as VirtualizingLayoutContext;
            int currentIndex = state.FirstRealizedIndex;

            foreach(var arrangeRect in state.LayoutRects)
            {
                var container = virtualContext.GetOrCreateElementAt(currentIndex);
                container.Arrange(arrangeRect);
                currentIndex++;
            }

            return finalSize;
        }

        #endregion
        #region Helper methods

        private Rect[] CalculateLayoutBoundsForRow(int rowIndex, double desiredItemWidth)
        {
            var boundsForRow = new Rect[2];

            var yoffset = rowIndex * (this.MinItemSize.Height + this.RowSpacing);
            boundsForRow[0].Y = boundsForRow[1].Y = yoffset;
            boundsForRow[0].Height = boundsForRow[1].Height = this.MinItemSize.Height;

            if(rowIndex % 2 == 0)
            {
                // Left tile (narrow)
                boundsForRow[0].X = 0;
                boundsForRow[0].Width = desiredItemWidth * 1.5;
                // Middle tile (narrow)
                boundsForRow[1].X = boundsForRow[0].Right + this.ColumnSpacing;
                boundsForRow[1].Width = desiredItemWidth * 0.5;
            }
            else
            {
                // Left tile (wide)
                boundsForRow[0].X = 0;
                boundsForRow[0].Width = desiredItemWidth * 0.5;

                // Middle tile (narrow)
                boundsForRow[1].X = boundsForRow[0].Right + this.ColumnSpacing;
                boundsForRow[1].Width = desiredItemWidth * 1.5;
            }

            return boundsForRow;
        }

        #endregion
    }

    internal class ActivityFeedLayoutState
    {
        public int FirstRealizedIndex { get; set; }

        /// <summary>
        /// FirstRealizedIndexで始まる項目のレイアウト矩形配列のリスト。
        /// </summary>
        public List<Rect> LayoutRects
        {
            get
            {
                if(this._layoutRects == null)
                {
                    this._layoutRects = new List<Rect>();
                }

                return this._layoutRects;
            }
        }

        private List<Rect> _layoutRects;
    }
}
