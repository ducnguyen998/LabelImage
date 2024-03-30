using LabelImageLibrary.Controls;
using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using LabelImageLibrary.Utils;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LabelImageLibrary.Behaviors
{
    public class CanvasContainerBehaviorAbstract : Behavior<CanvasContainer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
            this.AssociatedObject.PreviewMouseUp += OnPreviewMouseUp;
            this.AssociatedObject.PreviewMouseMove += OnPreviewMouseMove;
            this.AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
            this.AssociatedObject.SizeChanged += OnSizeChanged;

            this.scrollableViewbox = this.AssociatedObject.GetGridViewboxParent().GetScrollViewerParent();
            this.scrollableViewbox.PreviewKeyDown += OnPreviewKeyDown;
            this.scrollableViewbox.PreviewKeyUp += OnPreviewKeyUp;

            this.draggingAgent = new ObjectDraggingAgent();
            this.layoutToolbox = new CanvasLayoutToolbox(this.AssociatedObject, this.scrollableViewbox);
        }


        protected virtual void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        protected virtual void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        protected virtual void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
        }

        protected virtual void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.layoutToolbox.SyncScaleObjects(e);
        }

        protected virtual void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        protected virtual void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
            this.AssociatedObject.PreviewMouseUp -= OnPreviewMouseUp;
            this.AssociatedObject.PreviewMouseMove -= OnPreviewMouseMove;
            this.AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            this.AssociatedObject.SizeChanged -= OnSizeChanged;

            this.scrollableViewbox.PreviewKeyDown -= OnPreviewKeyDown;
            this.scrollableViewbox.PreviewKeyUp -= OnPreviewKeyUp;
        }

        protected FixedScrollViewer scrollableViewbox;

        protected ObjectAbstract selectedObject;

        protected Point lastMousePosition;

        protected ObjectDraggingAgent draggingAgent;

        protected CanvasLayoutToolbox layoutToolbox;


        public void DestroySelectedObject()
        {
            this.AssociatedObject.GraphicCollection.Remove(selectedObject);
        }

        public CanvasLayoutToolbox GetLayoutToolbox()
        {
            return this.layoutToolbox;
        }
    }
}
