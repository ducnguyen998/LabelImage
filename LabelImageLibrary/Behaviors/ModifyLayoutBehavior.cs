using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LabelImageLibrary.Behaviors
{
    public class ModifyLayoutBehavior : CanvasContainerBehaviorAbstract
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.GetAllObjects().ForEach(obj =>
            {
                obj.IsInteractable = true;
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

            var translateVector = new Vector(0, 0);

            this.draggingAgent.Increase();

            if (e.Key == Key.Up)
            {
                translateVector = new Vector(0, -draggingAgent.Factor());
            }
            else if (e.Key == Key.Down)
            {
                translateVector = new Vector(0, +draggingAgent.Factor());
            }
            else if (e.Key == Key.Left)
            {
                translateVector = new Vector(-draggingAgent.Factor(), 0);
            }
            else if (e.Key == Key.Right)
            {
                translateVector = new Vector(+draggingAgent.Factor(), 0);
            }

            if (this.selectedObject != null && this.selectedObject is ObjectNull == false)
            {
                this.selectedObject.DoDrag(translateVector);
            }
        }

        protected override void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            base.OnPreviewKeyUp(sender, e);

            this.draggingAgent.Reset();
        }

        protected override void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(sender, e);

            if (e.Source is ObjectAbstract) return;

            this.AssociatedObject.GetAllObjects().ForEach(obj =>
            {
                obj.IsSelected = false;
            });

            this.lastMousePosition = e.GetPosition(this.scrollableViewbox);
        }

        protected override void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(sender, e);

            if (e.Source is ObjectAbstract obj)
            {
                this.selectedObject = obj;
            }

            this.AssociatedObject.Cursor = Cursors.Arrow;
        }

        protected override void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            base.OnPreviewMouseMove(sender, e);

            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (e.Source is ObjectAbstract || e.Source is ObjectPoint) return;

            var newMousePosition = e.GetPosition(this.scrollableViewbox);

            var vector = newMousePosition - this.lastMousePosition;

            this.layoutToolbox.ScrollBy(vector);

            this.lastMousePosition = newMousePosition;

            this.AssociatedObject.Cursor = Cursors.ScrollAll;
        }

        protected override void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(sender, e);

            if (e.Source is ObjectAbstract) return;

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
        }
    }
}
