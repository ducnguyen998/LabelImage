using LabelImageLibrary.Behaviors;
using LabelImageLibrary.Controls;
using LabelImageLibrary.Displays;
using LabelImageLibrary.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LabelImageLibrary.Helpers
{
    public static class LayoutHelper
    {
        public class ParentRetriever<TElement> where TElement : class
        {
            public static TElement GetParent(FrameworkElement element)
            {
                var dependencyObject = element.Parent;

                if (dependencyObject is TElement parent)
                {
                    return parent;
                }

                return null;
            }
        }

        public class ChildrenRetriever<TElement> where TElement : class
        {
            public static TElement GetChild(Panel element)
            {
                return element.Children.OfType<TElement>().FirstOrDefault();
            }

            public static TElement GetChild(ScrollViewer element)
            {
                return element.Content as TElement;
            }
        }


        public static FixedScrollViewer GetScrollViewerParent(this GridViewbox element)
        {
            return ParentRetriever<FixedScrollViewer>.GetParent(element);
        }

        public static GridViewbox GetGridViewboxParent(this FrameworkElement element)
        {
            return ParentRetriever<GridViewbox>.GetParent(element);
        }

        public static CanvasContainer GetCanvasContainerParent(this ObjectAbstract objectShape)
        {
            return ParentRetriever<CanvasContainer>.GetParent(objectShape);
        }

        public static ObjectAbstract GetContainerShape(this ObjectPoint objectPoint)
        {
            return ParentRetriever<ObjectAbstract>.GetParent(objectPoint);
        }


        public static FixedScrollViewer GetFixedScrollViewer(this InteractiveDisplay interactiveDisplay)
        {
            return interactiveDisplay.FindName("FixedScrollViewer") as FixedScrollViewer;   
        }

        public static GridViewbox GetGridViewbox(this ScrollViewer scrollViewer)
        {
            return ChildrenRetriever<GridViewbox>.GetChild(scrollViewer);   
        }

        public static Image GetImage(this GridViewbox gridViewbox)
        {
            return ChildrenRetriever<Image>.GetChild(gridViewbox);
        }

        public static CanvasContainer GetCanvasContainer(this GridViewbox gridViewbox)
        {
            return ChildrenRetriever<CanvasContainer>.GetChild(gridViewbox);
        }


        public static void SyncLayout(this CanvasContainer container, Image syncSource, double scaleX, double scaleY)
        {
            container.Width  = scaleX * syncSource.Source.Width;
            container.Height = scaleY * syncSource.Source.Height;
        }

        public static ObservableCollection<ObjectAbstract> GetAllGraphics(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ObservableCollection<ObjectAbstract>>();
        }

        public static ObservableCollection<CanvasContainerBehaviorAbstract> GetAllBehaviors(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ObservableCollection<CanvasContainerBehaviorAbstract>>();
        }

        public static CanvasContainerBehaviorAbstract GetCurrentCanvasContainerBehavior(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetAllBehaviors().FirstOrDefault();
        }

        public static List<ObjectAbstract> GetAllObjects(this CanvasContainer canvasContainer)
        {
            return canvasContainer.Children.OfType<ObjectAbstract>().ToList();
        }

        public static List<ObjectPoint> GetAllPoints(this ObjectAbstract obj)
        {
            return obj.ObjectPointCollection.OfType<ObjectPoint>().ToList();
        }
    }
}
