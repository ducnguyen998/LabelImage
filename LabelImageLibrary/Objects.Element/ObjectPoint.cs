using LabelImageLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace LabelImageLibrary.Objects
{
    public partial class ObjectPoint : Thumb, INotifyPropertyChanged, IInteractiveObject, IColoredObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<Point> PositionChanged;

        protected void OnPositionChanged()
        {
            this.PositionChanged?.Invoke(this, this.position);
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
                }
            }
        }

        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged();
                    OnPositionChanged();
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
                    UpdateStyle();
                }
            }
        }

        protected bool isSelected;

        protected bool isInteractable;

        protected Point position;

        protected Brush color;

        protected readonly double thumbSize = 10.0;

        protected ObjectAbstract containerShape;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.DoInitialize();
        }

        protected virtual void DoInitialize()
        {
            this.isInteractable = true;
            this.containerShape = this.GetContainerShape();
            this.Style = this.CreateThumbStyle(this.color);
            this.DragDelta += OnDragDelta;
            this.Render();
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.isInteractable == false) return;

            this.position.X += e.HorizontalChange;
            this.position.Y += e.VerticalChange;
            this.Render();
        }

        private void UpdateStyle()
        {
            this.Style = CreateThumbStyle(color);
            this.Render();
        }

        public void ApplyTranslation(Vector translateVector)
        {
            this.position.X += translateVector.X;
            this.position.Y += translateVector.Y;
            this.Render();
        }

        public void ApplyTranslation(Point translatePoint)
        {
            this.position.X = translatePoint.X;
            this.position.Y = translatePoint.Y;
            this.Render();
        }

        public void ApplyScaleWith(Point center, double scale)
        {
            this.position = new Point()
            {
                X = center.X + scale * (this.Position.X - center.X),
                Y = center.Y + scale * (this.Position.Y - center.Y)
            };
            this.Render();
        }

        public virtual void Render() 
        { 
            this.RenderTransform = new TranslateTransform(this.position.X, this.position.Y);
            this.OnPositionChanged();
            this.InvalidateVisual();
        }

        public bool IsAlmostDuplicate(ObjectPoint other)
        {
            return (this.position - other.Position).Length <= this.thumbSize;
        }
    }
}
