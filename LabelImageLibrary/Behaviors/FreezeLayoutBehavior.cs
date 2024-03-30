using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LabelImageLibrary.Behaviors
{
    public class FreezeLayoutBehavior : CanvasContainerBehaviorAbstract
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.GetAllObjects().ForEach(obj =>
            {
                obj.IsSelected = false;
                obj.IsInteractable = false;
            });
        }

        protected override void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(sender, e);

            if (e.Delta > 0)
            {
                this.layoutToolbox.ZoomIn(0.03);
            }
            else
            {
                this.layoutToolbox.ZoomOut(0.03);
            }
        }

        protected override void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(sender, e);

            if (e.Source is ObjectAbstract == false)
            {
                this.AssociatedObject.GetAllObjects().ForEach(obj =>
                {
                    obj.IsSelected = false;
                });
            }
           
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

            var newMousePosition = e.GetPosition(this.scrollableViewbox);

            var vector = newMousePosition - this.lastMousePosition;

            this.layoutToolbox.ScrollBy(vector);

            this.lastMousePosition = newMousePosition;
            this.AssociatedObject.Cursor = Cursors.ScrollAll;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
