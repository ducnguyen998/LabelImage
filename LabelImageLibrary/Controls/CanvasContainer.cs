using LabelImageLibrary.Behaviors;
using LabelImageLibrary.Objects;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LabelImageLibrary.Controls
{
    public class CanvasContainer : Canvas
    {
        public ObservableCollection<ObjectAbstract> GraphicCollection
        {
            get { return (ObservableCollection<ObjectAbstract>)GetValue(GraphicCollectionProperty); }
            set { SetValue(GraphicCollectionProperty, value); }
        }

        public static readonly DependencyProperty GraphicCollectionProperty =
            DependencyProperty.Register("GraphicCollection", typeof(ObservableCollection<ObjectAbstract>), typeof(CanvasContainer), new PropertyMetadata(null, OnGraphicCollectionChanged));

        private static void OnGraphicCollectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is CanvasContainer canvasContainer)
            {
                if (args.NewValue is ObservableCollection<ObjectAbstract> newCollection)
                {
                    newCollection.CollectionChanged += (s, e) =>
                    {
                        if (e.NewItems != null)
                        {
                            foreach (ObjectAbstract shape in e.NewItems)
                            {
                                canvasContainer.Children.Add(shape);
                            }
                        }

                        if (e.OldItems != null)
                        {
                            foreach (ObjectAbstract shape in e.OldItems)
                            {
                                canvasContainer.Children.Remove(shape);
                            }
                        }
                    };

                    foreach (var shape in newCollection)
                    {
                        canvasContainer.Children.Add(shape);
                    }
                }
            }
        }



        public ObservableCollection<CanvasContainerBehaviorAbstract> BehaviorCollection
        {
            get { return (ObservableCollection<CanvasContainerBehaviorAbstract>)GetValue(BehaviorCollectionProperty); }
            set { SetValue(BehaviorCollectionProperty, value); }
        }

        public static readonly DependencyProperty BehaviorCollectionProperty =
            DependencyProperty.Register("BehaviorCollection", typeof(ObservableCollection<CanvasContainerBehaviorAbstract>), typeof(CanvasContainer), new PropertyMetadata(null, OnBehaviorCollectionChanged));

        private static void OnBehaviorCollectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is CanvasContainer canvasContainer)
            {
                var behaviors = Interaction.GetBehaviors(canvasContainer);

                if (args.NewValue is ObservableCollection<CanvasContainerBehaviorAbstract> newCollection)
                {
                    newCollection.CollectionChanged += (s, e) =>
                    {
                        if (e.OldItems != null)
                        {
                            foreach (CanvasContainerBehaviorAbstract behaviorAbstract in e.OldItems)
                            {
                                behaviors.Remove(behaviorAbstract);
                            }
                        }

                        if (e.NewItems != null)
                        {
                            foreach (CanvasContainerBehaviorAbstract behaviorAbstract in e.NewItems)
                            {
                                behaviors.Add(behaviorAbstract);
                            }
                        }
                    };

                    //{
                    //    {
                    //        foreach (CanvasContainerBehaviorAbstract behaviorAbstract in newCollection)
                    //        {
                    //            behaviors.Add(behaviorAbstract);
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}
