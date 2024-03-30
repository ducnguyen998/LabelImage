using LabelImageLibrary.Behaviors;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LabelImageLibrary.Controls
{
    public class TagEditBox : TextBox
    {
        public ObservableCollection<TagEditBoxBehavior> BehaviorCollection
        {
            get { return (ObservableCollection<TagEditBoxBehavior>)GetValue(BehaviorCollectionProperty); }
            set { SetValue(BehaviorCollectionProperty, value); }
        }

        public static readonly DependencyProperty BehaviorCollectionProperty =
            DependencyProperty.Register("BehaviorCollection", typeof(ObservableCollection<TagEditBoxBehavior>), typeof(TagEditBox), new PropertyMetadata(null, OnBehaviorCollectionChanged));

        private static void OnBehaviorCollectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is TagEditBox tagEdit)
            {
                var behaviors = Interaction.GetBehaviors(tagEdit);

                if (args.NewValue is ObservableCollection<TagEditBoxBehavior> newCollection)
                {
                    newCollection.CollectionChanged += (s, e) =>
                    {
                        if (e.OldItems != null)
                        {
                            foreach (TagEditBoxBehavior behaviorAbstract in e.OldItems)
                            {
                                behaviors.Remove(behaviorAbstract);
                            }
                        }

                        if (e.NewItems != null)
                        {
                            foreach (TagEditBoxBehavior behaviorAbstract in e.NewItems)
                            {
                                behaviors.Add(behaviorAbstract);
                            }
                        }
                    };

                    {
                        {
                            foreach (TagEditBoxBehavior behaviorAbstract in newCollection)
                            {
                                behaviors.Add(behaviorAbstract);
                            }
                        }
                    }
                }
            }
        }
    }
}
