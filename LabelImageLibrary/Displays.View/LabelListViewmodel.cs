using CommunityToolkit.Mvvm.ComponentModel;
using LabelImageLibrary.Behaviors;
using LabelImageLibrary.Controls;
using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LabelImageLibrary.Displays
{
    public class LabelListViewmodel : ObservableObject
    {
        public ObservableCollection<ObjectLabel> LabelCollection { get; set; }

        public ObservableCollection<TagEditBoxBehavior> TagEditBehaviorCollection { get; set; }

        public ObjectLabel ChoosenLabel
        {
            get 
            { 
                return choosenLabel; 
            }
            set 
            { 
                if (choosenLabel != value)
                {
                    choosenLabel = value;
                    OnPropertyChanged();
                    Console.WriteLine("Selected : " + choosenLabel.Name);
                }
            }
        }

        public string TagName
        {
            get
            {
                return tagName;
            }
            set
            {
                if (tagName != value)
                {
                    tagName = value;
                    OnPropertyChanged();
                }
            }
        }


        public LabelListViewmodel(IServiceProvider serviceProvider)
        {
            this.LabelCollection = serviceProvider.GetRequiredService<ObservableCollection<ObjectLabel>>();
            this.LabelCollection.CollectionChanged += OnLabelCollectionChanged;

            this.LabelCollection.Add(new ObjectLabel() { Name = "ducnguyen.998", Color = Brushes.Orange });
            this.LabelCollection.Add(new ObjectLabel() { Name = "Car", Color = Brushes.Red });
            this.LabelCollection.Add(new ObjectLabel() { Name = "Panel", Color = Brushes.Green });
            this.LabelCollection.Add(new ObjectLabel() { Name = "Laptop", Color = Brushes.Blue });
            this.LabelCollection.Add(new ObjectLabel() { Name = "Tesla", Color = Brushes.BlueViolet });
            this.LabelCollection.Add(new ObjectLabel() { Name = "Samsung", Color = Brushes.Teal });

            this.TagEditBehaviorCollection = new ObservableCollection<TagEditBoxBehavior>();
            this.TagEditBehaviorCollection.Add(new TagEditBoxBehavior());
            this.TagEditBehaviorCollection[0].TagConfirmed += OnTagEditConfirmed;
        }

        private void OnLabelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ObjectLabel label in e.NewItems)
                {
                    label.CommandRaised += LabelCommandRaised;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ObjectLabel label in e.OldItems)
                {
                    label.CommandRaised -= LabelCommandRaised;
                }
            }
        }

        private void LabelCommandRaised(object sender, ObjectLabel.ECommand e)
        {
            var label = sender as ObjectLabel;

            switch (e)
            {
                case ObjectLabel.ECommand.Delete:
                    this.LabelCollection.Remove(label);
                    break;
                case ObjectLabel.ECommand.Modify:
                    break;
                case ObjectLabel.ECommand.Choosen:
                    this.ChoosenLabel = label;
                    break;
            }
        }

        private void OnTagEditConfirmed(object sender, string e)
        {
            if (string.IsNullOrEmpty(e) == false && this.LabelCollection.ToList().Find(x => x.Name == e) == null)
            {
                this.LabelCollection.Add(new ObjectLabel() { Name = e, Color = ColorHelper.GetRandomBrush() });
            }

            {
                this.TagName = e;
                this.TagName = "";
            }
        }



        private ObjectLabel choosenLabel;

        private string tagName;

    }
}
