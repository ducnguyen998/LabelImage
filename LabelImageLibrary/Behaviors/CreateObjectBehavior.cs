using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace LabelImageLibrary.Behaviors
{
    public class CreateObjectBehavior : CanvasContainerBehaviorAbstract
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.GetAllObjects().ForEach(obj =>
            {
                obj.IsInteractable = false;
            });
        }

        protected override void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            base.OnPreviewKeyDown(sender, e);

            if (
                e.Key == Key.Up ||
                e.Key == Key.Down ||
                e.Key == Key.Left ||
                e.Key == Key.Right
                )
            {
                e.Handled = true;
            }

            if (e.Key == Key.Escape )
            {
                this.createObject.IsCreated = true;
                this.AssociatedObject.GraphicCollection.Remove(this.createObject);
            }
        }

        protected override void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(sender, e);

            if (this.createObject.IsCreated) return;

            var isMakeEndPointDuplicate = this.createObject.IsMakeEndPointDuplicate();

            if (this.createObject.IsPointCollectionAvailable() && !isMakeEndPointDuplicate)
            {
                var point0 = e.GetPosition(this.AssociatedObject);

                this.createObject.ObjectPointCollection.Last().ApplyTranslation(point0);
                this.createObject.ObjectPointCollection.Add(new ObjectPoint()
                {
                    Color = Brushes.Blue,
                    Position = point0
                });
            }
            else
            {
                if (isMakeEndPointDuplicate)
                {
                    this.createObject.ObjectPointCollection.Remove(this.createObject.ObjectPointCollection.Last());
                }

                this.createObject.IsCreated = true;
                this.AssociatedObject.Cursor = Cursors.Arrow;
                this.Create(createShape, createObject.Annotation.Label);
            }

            this.AssociatedObject.GetAllObjects().ForEach(obj =>
            {
                obj.IsSelected = false;
            });

            this.scrollableViewbox.Focus();
        }

        protected override void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            base.OnPreviewMouseMove(sender, e);

            if (this.createObject.IsCreated == false)
            {
                this.createObject.ObjectPointCollection.Last().ApplyTranslation(e.GetPosition(this.AssociatedObject));
            }
        }

        protected override void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(sender, e);

            if (Keyboard.Modifiers != ModifierKeys.Control) return;

            if (e.Delta > 0)
            {
                this.layoutToolbox.ZoomIn(0.03);
            }
            else
            {
                this.layoutToolbox.ZoomOut(0.03);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.createObject.IsCreated == false)
            {
                this.AssociatedObject.GraphicCollection.Remove(this.createObject);
            }
        }

        private ObjectAbstract createObject;

        private EShape createShape;


        public void Create(EShape shape, ObjectLabel label)
        {
            if (label == null)
            {
                label = ObjectLabel.Default;
            }

            this.createShape = shape;

            switch (shape)
            {
                case EShape.Circle:
                    this.createObject = new ObjectCircle(label);
                    break;
                case EShape.Rectangle:
                    this.createObject = new ObjectRectangle(label);
                    break;
                case EShape.Polyline:
                    this.createObject = new ObjectPolyline(label);
                    break;
                case EShape.Polygon:
                    this.createObject = new ObjectPolygon(label);
                    break;
            }

            this.AssociatedObject.GraphicCollection.Add(this.createObject);
            var _createColor = label.Color;
            this.createObject.IsCreated = false;
            this.createObject.IsInteractable = true;
            this.createObject.Color = _createColor;
            this.createObject.ObjectPointCollection.Add(new ObjectPoint()
            {
                Color = _createColor,
                Position = new System.Windows.Point(0, 0)
            });
            this.AssociatedObject.Cursor = Cursors.Cross;
        }

        public void ChangeLabel(ObjectLabel label)
        {
            if (this.createObject == null) return;

            this.createObject.Annotation.Label = label; 
        }
    }
}
