using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabelImageLibrary.Objects
{
    public partial class ObjectPoint
    {
        private ControlTemplate CreateEllipseThumbTemplate(Brush color)
        {
            var template = new ControlTemplate(typeof(Thumb));
            var ellipseFactory = new FrameworkElementFactory(typeof(Ellipse));

            ellipseFactory.SetValue(Shape.FillProperty, color);
            ellipseFactory.SetValue(Shape.StrokeProperty, color);
            ellipseFactory.SetValue(Shape.StrokeThicknessProperty, 1.0);
            ellipseFactory.SetValue(Shape.WidthProperty, thumbSize);
            ellipseFactory.SetValue(Shape.HeightProperty, thumbSize);
            ellipseFactory.SetValue(Shape.MarginProperty, new Thickness(thumbSize / -2.0));

            template.VisualTree = ellipseFactory;
            return template;
        }

        private ControlTemplate CreateEllipseThumbTemplateOnSelect(Brush color)
        {
            var template = new ControlTemplate(typeof(Thumb));
            var ellipseFactory = new FrameworkElementFactory(typeof(Ellipse));

            ellipseFactory.SetValue(Shape.FillProperty, color);
            ellipseFactory.SetValue(Shape.StrokeProperty, Brushes.White);
            ellipseFactory.SetValue(Shape.StrokeThicknessProperty, 2.0);
            ellipseFactory.SetValue(Shape.WidthProperty, thumbSize);
            ellipseFactory.SetValue(Shape.HeightProperty, thumbSize);
            ellipseFactory.SetValue(Shape.MarginProperty, new Thickness(thumbSize / -2.0));

            template.VisualTree = ellipseFactory;
            return template;
        }

        private ControlTemplate CreateEllipseThumbTemplateOnFreeze(Brush color)
        {
            var template = new ControlTemplate(typeof(Thumb));
            var ellipseFactory = new FrameworkElementFactory(typeof(Ellipse));

            ellipseFactory.SetValue(Shape.FillProperty, color);
            ellipseFactory.SetValue(Shape.StrokeProperty, color);
            ellipseFactory.SetValue(Shape.StrokeThicknessProperty, 1.0);
            ellipseFactory.SetValue(Shape.WidthProperty, 2.0);
            ellipseFactory.SetValue(Shape.HeightProperty, 2.0);
            ellipseFactory.SetValue(Shape.MarginProperty, new Thickness(thumbSize / -2.0));

            template.VisualTree = ellipseFactory;
            return template;
        }

        private ControlTemplate CreateRectangleThumbTemplate(Brush color)
        {
            var scale = 1.4;
            var template = new ControlTemplate(typeof(Thumb));
            var rectangleFactory = new FrameworkElementFactory(typeof(Rectangle));

            rectangleFactory.SetValue(Shape.FillProperty, Brushes.White);
            rectangleFactory.SetValue(Shape.StrokeProperty, color);
            rectangleFactory.SetValue(Shape.StrokeThicknessProperty, 1.0);
            rectangleFactory.SetValue(Shape.WidthProperty, thumbSize * scale);
            rectangleFactory.SetValue(Shape.HeightProperty, thumbSize * scale);
            rectangleFactory.SetValue(Shape.MarginProperty, new Thickness(thumbSize / -(scale / 2.0)));

            template.VisualTree = rectangleFactory;
            return template;
        }

        private Style CreateThumbStyle(Brush color)
        {
            var workingColor = color.Clone() as Brush;
            var style = new Style(typeof(ObjectPoint));

            style.Setters
                .Add(new Setter(Control.TemplateProperty, this.CreateEllipseThumbTemplate(workingColor)));
            style.Triggers
                .Add(new DataTrigger()
                {
                    Binding = new Binding(nameof(IsSelected))
                    {
                        RelativeSource = new RelativeSource(RelativeSourceMode.Self)
                    },
                    Value = true,
                    Setters =
                    {
                        new Setter(Control.TemplateProperty, this.CreateEllipseThumbTemplateOnSelect(workingColor))
                    }
                });
            style.Triggers
                .Add(new Trigger()
                {
                    Property = ObjectPoint.IsMouseOverProperty,
                    Value = true,
                    Setters =
                    {
                        new Setter(Control.TemplateProperty, this.CreateRectangleThumbTemplate(workingColor))
                    }
                });
            style.Triggers
                .Add(new DataTrigger()
                {
                    Binding = new Binding(nameof(IsInteractable))
                    {
                        RelativeSource = new RelativeSource(RelativeSourceMode.Self)
                    },
                    Value = false,
                    Setters =
                    {
                        new Setter(Control.TemplateProperty, this.CreateEllipseThumbTemplateOnFreeze(workingColor))
                    }
                });

            return style;
        }
    }
}
