using LabelImageLibrary.Controls;
using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LabelImageLibrary.Utils
{
    public class CanvasLayoutToolbox
    {
        private readonly CanvasContainer canvasContainer;

        private readonly FixedScrollViewer scrollViewer;

        private Size currentSize;

        public CanvasLayoutToolbox(CanvasContainer canvasContainer, FixedScrollViewer scrollViewer)
        {
            this.canvasContainer = canvasContainer;
            this.scrollViewer = scrollViewer;
            this.currentSize = canvasContainer.RenderSize;
        }

        public void ZoomIn(double delta)
        {
            var image = this.canvasContainer.GetGridViewboxParent().GetImage();

            var scaleX = image.LayoutTransform.Value.M11;
            var scaleY = image.LayoutTransform.Value.M22;

            if (scaleX >= 10 || scaleY >= 10) return;

            scaleX += delta;
            scaleY += delta;

            image.LayoutTransform = new ScaleTransform(scaleX, scaleY);

            this.canvasContainer.SyncLayout(image, scaleX, scaleY);
        }

        public void ZoomOut(double delta)
        {
            var image = this.canvasContainer.GetGridViewboxParent().GetImage();

            var scaleX = image.LayoutTransform.Value.M11;
            var scaleY = image.LayoutTransform.Value.M22;

            if (scaleX <= 0.2 || scaleY <= 0.2) return;

            scaleX -= delta;
            scaleY -= delta;

            image.LayoutTransform = new ScaleTransform(scaleX, scaleY);

            this.canvasContainer.SyncLayout(image, scaleX, scaleY);
        }

        public void FitLayout()
        {
            var image = this.canvasContainer.GetGridViewboxParent().GetImage();

            if (image.Source == null) return;   

            var ki = (double)image.Source.Height / (double)image.Source.Width;

            var kc = (double)scrollViewer.ActualHeight / (double)scrollViewer.ActualWidth;

            var scale = 1.0;

            if (ki < kc)
            {
                scale = (double)scrollViewer.ActualWidth  / image.Source.Width;
            }
            else
            {
                scale = (double)scrollViewer.ActualHeight / image.Source.Height;
            }

            image.LayoutTransform = new ScaleTransform(scale, scale);

            this.canvasContainer.SyncLayout(image, scale, scale);
        }

        public void ScrollBy(Vector dragVector)
        {
            var hOffset = this.scrollViewer.HorizontalOffset;
            var vOffset = this.scrollViewer.VerticalOffset;

            var deltaX = hOffset - dragVector.X;
            var deltaY = vOffset - dragVector.Y;

            if (deltaX < 0 || deltaY < 0) return;

            this.scrollViewer.ScrollToHorizontalOffset(deltaX);
            this.scrollViewer.ScrollToVerticalOffset(deltaY);
            this.scrollViewer.InvalidateScrollInfo();
        }

        public void SyncScaleObjects(SizeChangedEventArgs e)
        {
            if (this.currentSize.Width != 0 && this.currentSize.Height != 0)
            {
                var scale = e.NewSize.Width / this.currentSize.Width;

                this.canvasContainer.GetAllObjects().ForEach(obj =>
                {
                    obj.GetAllPoints().ForEach(point =>
                    {
                        point.ApplyScaleWith(new Point(), scale);
                    });
                });
            }

            this.currentSize = e.NewSize;
        }
    }
}
