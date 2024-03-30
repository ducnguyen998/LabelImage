using CommunityToolkit.Mvvm.ComponentModel;
using LabelImageLibrary.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImageLibrary.Displays
{
    public class AnnotationListViewmodel : ObservableObject
    {
        private readonly LabelListViewmodel labelListDisplay;

        private readonly ImageEditorViewmodel imageEditorDisplay;

        public ObservableCollection<ObjectAnnotation> AnnotationCollection { get; set; }

        public ObjectAnnotation SelectedAnnotation
        {
            get 
            { 
                return selectedAnnotation; 
            }
            set 
            { 
                if (selectedAnnotation != value)
                {
                    selectedAnnotation = value;
                    OnPropertyChanged();
                    OnSelectedAnnotationChanged();

                    foreach (ObjectAbstract objectAbstract in imageEditorDisplay.GraphicCollection)
                    {
                        objectAbstract.IsSelected     = objectAbstract.Annotation == value;
                        objectAbstract.IsInteractable = objectAbstract.Annotation == value;
                    }
                }
            }
        }

        public event EventHandler<ObjectAnnotation> SelectedAnnotationChanged;

        public AnnotationListViewmodel(IServiceProvider serviceProvider)
        {
            this.labelListDisplay = serviceProvider.GetRequiredService<LabelListViewmodel>();
            this.imageEditorDisplay = serviceProvider.GetRequiredService<ImageEditorViewmodel>();
            this.AnnotationCollection = serviceProvider.GetRequiredService<ObservableCollection<ObjectAnnotation>>();

            this.imageEditorDisplay.GraphicChanged += ImageEditorDisplay_GraphicChanged;
        }

        private void ImageEditorDisplay_GraphicChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ObjectAbstract objectAbstract in e.NewItems)
                {
                    objectAbstract.Annotation.LabelCollection = labelListDisplay.LabelCollection;
                    this.AnnotationCollection.Add(objectAbstract.Annotation);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ObjectAbstract objectAbstract in e.OldItems)
                {
                    objectAbstract.Annotation.LabelCollection = labelListDisplay.LabelCollection;
                    this.AnnotationCollection.Remove(objectAbstract.Annotation);
                }
            }
        }


        private ObjectAnnotation selectedAnnotation;

        private void OnSelectedAnnotationChanged()
        {
            this.SelectedAnnotationChanged?.Invoke(this, selectedAnnotation);
        }
    }
}
