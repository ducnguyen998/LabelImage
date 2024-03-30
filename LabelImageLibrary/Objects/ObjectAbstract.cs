using LabelImageLibrary.Controls;
using LabelImageLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LabelImageLibrary.Objects
{
    public class ObjectAbstract : Canvas, INotifyPropertyChanged, IInteractiveObject, IColoredObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ObjectPoint> ObjectPointCollection { get; set; }

        public ObjectAbstract(ObjectLabel objectLabel)
        {
            this.annotation = new ObjectAnnotation(objectLabel);
            this.annotation.PropertyChanged += OnAnnotationPropertyChanged;
        }

        private void OnAnnotationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Label")
            {
                if (this.annotation.Label == null)
                {
                    this.annotation.Label = ObjectLabel.Default;
                }

                this.Color = this.annotation.Label.Color;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                    Render();

                    foreach (ObjectPoint objectPoint in this.ObjectPointCollection)
                    {
                        objectPoint.IsSelected = value;
                    }
                }
            }
        }

        public bool IsInteractable
        {
            get
            {
                return isInteractable;
            }
            set
            {
                if (isInteractable != value)
                {
                    isInteractable = value;
                    OnPropertyChanged();
                    Render();

                    foreach (ObjectPoint objectPoint in this.ObjectPointCollection)
                    {
                        objectPoint.IsInteractable = value;
                    }
                }
            }
        }

        public bool IsCreated
        {
            get
            {
                return isCreated;
            }
            set
            {
                if (isCreated != value)
                {
                    isCreated = value;
                    OnPropertyChanged();
                    Render();
                }
            }
        }

        public Brush Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged();

                    foreach (ObjectPoint objectPoint in ObjectPointCollection)
                    {
                        objectPoint.Color = color;
                    }
                }
            }
        }

        public ObjectAnnotation Annotation
        {
            get
            {
                return annotation;
            }
            set
            {
                if (annotation != value)
                {
                    annotation = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.DoInitialize();
        }

        protected virtual void DoInitialize()
        {
            this.isInteractable = true;
            this.ObjectPointCollection = new ObservableCollection<ObjectPoint>();
            this.ObjectPointCollection.CollectionChanged += OnObjectPointCollectionChanged;
            this.canvasContainer = this.GetCanvasContainerParent();
            this.canvasContainer.PreviewMouseMove += OnCanvasContainerPreviewMouseMove;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            this.ReleaseMouseCapture();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.Render();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.Render();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            this.DoScale(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.isInteractable == false) return;

            this.canvasContainer.GetAllObjects().ForEach(obj =>
            {
                obj.IsSelected = obj == this;
            });

            this.mousePickPosition = e.GetPosition(this);
        }


        protected virtual void DrawLabel(DrawingContext drawingContext, Point[] points)
        {
            var drawP0 = points.First();
            var drawPoint = new Point(drawP0.X, drawP0.Y - 20);

#pragma warning disable CS0618 // Type or member is obsolete
            drawingContext.DrawText(new FormattedText(
                this.annotation.Label.Name,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"), 10, Brushes.White
                ), drawPoint);
#pragma warning restore CS0618 // Type or member is obsolete
        }
        private void OnCanvasContainerPreviewMouseMove(object sender, MouseEventArgs e)
        {
            this.DoDrag(e);
        }

        private void OnObjectPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ObjectPoint objectPoint in e.NewItems)
                {
                    objectPoint.PositionChanged += OnPositionChanged;
                    this.Children.Add(objectPoint);
                }
            }

            if (e.OldItems != null)
            {
                foreach (ObjectPoint objectPoint in e.OldItems)
                {
                    objectPoint.PositionChanged -= OnPositionChanged;
                    this.Children.Remove(objectPoint);
                }
            }
        }

        private void OnPositionChanged(object sender, Point e)
        {
            this.Render();
        }

        private void DoDrag(MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || e.Source is ObjectPoint || this.isInteractable == false) return;

            var translateVector = e.GetPosition(this) - this.mousePickPosition;

            this.DoDrag(translateVector);
        }

        public virtual void Render()
        {
            this.InvalidateVisual();
        }

        public void DoDrag(Vector translateVector)
        {
            if (this.IsSelected == false) return;

            foreach (ObjectPoint objectPoint in this.ObjectPointCollection)
            {
                objectPoint.ApplyTranslation(translateVector);
            }

            this.mousePickPosition += translateVector;
        }

        public void DoScale(MouseWheelEventArgs e)
        {
            if (this.IsSelected == false) return;

            Point currentPosition;

            if (e.Source is ObjectPoint obj)
            {
                currentPosition = obj.Position;
            }
            else
            {
                currentPosition = e.GetPosition(this);
            }

            foreach (ObjectPoint objectPoint in this.ObjectPointCollection)
            {
                objectPoint.ApplyScaleWith(currentPosition, e.Delta > 0 ? 1.1 : 0.9);
            }
        }

        public bool IsPointCollectionEmpty()
        {
            return this.ObjectPointCollection.Count == 0;
        }

        public bool IsPointCollectionAvailable()
        {
            return this.ObjectPointCollection.Count < this.maxCountPoints;
        }

        public bool IsMakeEndPointDuplicate()
        {
            return this.ObjectPointCollection.Count > 2 && this.ObjectPointCollection.Last().IsAlmostDuplicate(this.ObjectPointCollection.First());
        }

        protected bool isSelected;

        protected bool isInteractable;

        protected bool isCreated;

        protected Brush color;

        protected ObjectAnnotation annotation;

        protected CanvasContainer canvasContainer;

        protected Point mousePickPosition;

        protected int maxCountPoints;
    }
}
