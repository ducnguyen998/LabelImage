using LabelImageLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LabelImageLibrary.Objects
{
    public class ObjectCircle : ObjectAbstract
    {
        public ObjectCircle(ObjectLabel objectLabel) : base(objectLabel)
        {
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();
            this.maxCountPoints = 2;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var c0 = this.ObjectPointCollection?.Count;
            if (c0 < this.maxCountPoints || c0 == null) return;

            var p1 = this.ObjectPointCollection[0].Position;
            var p2 = this.ObjectPointCollection[1].Position;

            var R0 = (p2 - p1).Length;

            if (this.IsSelected && this.isInteractable)
            {
                drawingContext.DrawEllipse(this.Color.GetLighter(0.3), new Pen(Brushes.White, 2.0), p1, R0, R0);
            }
            else if (this.IsMouseOver && this.isInteractable)
            {
                drawingContext.DrawEllipse(this.Color.GetLighter(0.3), new Pen(this.Color, 2.0), p1, R0, R0);
            }
            else
            {
                drawingContext.DrawEllipse(Brushes.Transparent, new Pen(this.Color, 1.0), p1, R0, R0);
            }

            DrawLabel(drawingContext, ObjectPointCollection.Select(p => p.Position).ToArray());
        }
    }
}
