using LabelImageLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace LabelImageLibrary.Objects
{
    public class ObjectPolygon : ObjectAbstract
    {
        public ObjectPolygon(ObjectLabel objectLabel) : base(objectLabel)
        {
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();
            this.maxCountPoints = 1000;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var c0 = this.ObjectPointCollection?.Count;
            if (c0 < 2 || c0 == null) return;

            var points = this.ObjectPointCollection.Select(p => p.Position).ToArray();

            StreamGeometry streamGeometry = new StreamGeometry();

            using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(points[0], isFilled: this.isCreated, isClosed: this.isCreated);
                streamGeometryContext.PolyLineTo(points.Skip(1).ToList(), isStroked: true, isSmoothJoin: true);
            }

            streamGeometry.Freeze();

            if (this.IsSelected && this.isInteractable)
            {
                drawingContext.DrawGeometry(this.Color.GetLighter(0.3), new Pen(Brushes.White, 2.0), streamGeometry);
            }
            else if (this.IsMouseOver && this.isInteractable)
            {
                drawingContext.DrawGeometry(this.Color.GetLighter(0.3), new Pen(this.Color, 2.0), streamGeometry);
            }
            else
            {
                drawingContext.DrawGeometry(Brushes.Transparent, new Pen(this.Color, 1.0), streamGeometry);
            }

            DrawLabel(drawingContext, points);
        }
    }
}
