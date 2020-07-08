// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using VirtualizingLayout = Microsoft.UI.Xaml.Controls.VirtualizingLayout;
using VirtualizingLayoutContext = Microsoft.UI.Xaml.Controls.VirtualizingLayoutContext;

namespace Practice03prototype.Common
{
    public class VariedImageSizeLayout : VirtualizingLayout
    {
        public double Width { get; set; } = 150;
        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            // データ収集が変更されたため、すべてのインデックスの境界が無効になりました。
            // すべての境界を再評価し、次のメジャーでキャッシュする必要があります。.
            this.m_cachedBounds.Clear();
            this.m_firstIndex = this.m_lastIndex = 0;
            this.cachedBoundsInvalid = false;
            this.InvalidateMeasure();
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var viewport = context.RealizationRect;

            Debug.WriteLine($"availableSize X,Y：{availableSize.Width}, {availableSize.Height}");

            if(availableSize.Width != this.m_lastAvailableWidth || this.cachedBoundsInvalid)
            {
                this.UpdateCachedBounds(availableSize);
                this.m_lastAvailableWidth = availableSize.Width;
            }

            // 列オフセットの初期化
            int numColumns = (int)(availableSize.Width / 150);
            Debug.WriteLine($"列数-150：{numColumns}");




            if(this.m_columnOffsets.Count == 0)
            {
                for(int i = 0; i < numColumns; i++)
                {
                    this.m_columnOffsets.Add(0);
                }
            }

            if(numColumns % 2 == 0)
            {
                this.Width = 150;
                Debug.WriteLine($"this.Width-0：{this.Width}");
            }
            else
            {


                if(numColumns < 2)
                {

                }
                else
                {
                    this.Width = 230;
                    numColumns = (int)(availableSize.Width / 201);
                    Debug.WriteLine($"this.Width-1：{this.Width}");
                    //                    this.Width = (int)(availableSize.Width / (numColumns - 1));
                    // numColumns = numColumns - 1;


                }
                //              return this.GetExtentSize(availableSize);
            }


            Debug.WriteLine($"列数：{numColumns}");

            this.m_firstIndex = this.GetStartIndex(viewport);
            int currentIndex = this.m_firstIndex;
            double nextOffset = -1.0;

            // 開始インデックスからビューポートの終わりまでの項目を計測します。
            while(currentIndex < context.ItemCount && nextOffset < viewport.Bottom)
            {
                var child = context.GetOrCreateElementAt(currentIndex);
                child.Measure(new Size(this.Width, availableSize.Height));

                if(currentIndex >= this.m_cachedBounds.Count)
                {
                    // このインデックスには境界線がありません。レイアウトしてキャッシュします。
                    int columnIndex = this.GetIndexOfLowestColumn(this.m_columnOffsets, out nextOffset);
                    this.m_cachedBounds.Add(new Rect(columnIndex * this.Width, nextOffset, this.Width, child.DesiredSize.Height));
                    this.m_columnOffsets[columnIndex] += child.DesiredSize.Height;
                }
                else
                {
                    if(currentIndex + 1 == this.m_cachedBounds.Count)
                    {
                        // Last element. Use the next offset.
                        this.GetIndexOfLowestColumn(this.m_columnOffsets, out nextOffset);
                    }
                    else
                    {
                        nextOffset = this.m_cachedBounds[currentIndex + 1].Top;
                    }
                }

                this.m_lastIndex = currentIndex;
                currentIndex++;
            }




            var extent = this.GetExtentSize(availableSize);

            Debug.WriteLine($"extent,Y：{extent.Width}, {extent.Height}");

            return extent;
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if(this.m_cachedBounds.Count > 0)
            {
                for(int index = this.m_firstIndex; index <= this.m_lastIndex; index++)
                {
                    var child = context.GetOrCreateElementAt(index);
                    child.Arrange(this.m_cachedBounds[index]);
                }
            }
            return finalSize;
        }

        private void UpdateCachedBounds(Size availableSize)
        {


            int numColumns = (int)(availableSize.Width / this.Width);
            this.m_columnOffsets.Clear();
            for(int i = 0; i < numColumns; i++)
            {
                this.m_columnOffsets.Add(0);
            }

            for(int index = 0; index < this.m_cachedBounds.Count; index++)
            {
                int columnIndex = this.GetIndexOfLowestColumn(this.m_columnOffsets, out var nextOffset);
                var oldHeight = this.m_cachedBounds[index].Height;
                this.m_cachedBounds[index] = new Rect(columnIndex * this.Width, nextOffset, this.Width, oldHeight);
                this.m_columnOffsets[columnIndex] += oldHeight;
            }

            this.cachedBoundsInvalid = false;
        }

        private int GetStartIndex(Rect viewport)
        {
            int startIndex = 0;
            if(this.m_cachedBounds.Count == 0)
            {
                startIndex = 0;
            }
            else
            {
                // ビューポートと交差する最初のインデックスを見つけます。
                for(int i = 0; i < this.m_cachedBounds.Count; i++)
                {
                    var currentBounds = this.m_cachedBounds[i];
                    if(currentBounds.Y < viewport.Bottom &&
                        currentBounds.Bottom > viewport.Top)
                    {
                        startIndex = i;
                        break;
                    }
                }
            }

            return startIndex;
        }

        private int GetIndexOfLowestColumn(List<double> columnOffsets, out double lowestOffset)
        {
            int lowestIndex = 0;
            lowestOffset = columnOffsets[lowestIndex];
            for(int index = 0; index < columnOffsets.Count; index++)
            {
                var currentOffset = columnOffsets[index];
                if(lowestOffset > currentOffset)
                {
                    lowestOffset = currentOffset;
                    lowestIndex = index;
                }
            }

            return lowestIndex;
        }

        private Size GetExtentSize(Size availableSize)
        {
            double largestColumnOffset = this.m_columnOffsets[0];
            for(int index = 0; index < this.m_columnOffsets.Count; index++)
            {
                var currentOffset = this.m_columnOffsets[index];
                if(largestColumnOffset < currentOffset)
                {
                    largestColumnOffset = currentOffset;
                }
            }



            return new Size(availableSize.Width, largestColumnOffset);
        }

        int m_firstIndex = 0;
        int m_lastIndex = 0;
        double m_lastAvailableWidth = 0.0;
        List<double> m_columnOffsets = new List<double>();
        List<Rect> m_cachedBounds = new List<Rect>();
        private bool cachedBoundsInvalid = false;
    }
}
