using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace LabelImageLibrary.Objects
{
    public class ObjectAnnotation : ObservableObject
    {
        public ObservableCollection<ObjectLabel> LabelCollection { get; set; }

        public ObjectAnnotation(ObjectLabel label)
        {
            this.label = label;
        }

        public ObjectLabel Label
        {
            get
            {
                return label;
            }
            set
            {
                if (label != value)
                {
                    label = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    OnPropertyChanged();
                }
            }
        }


        private ObjectLabel label;

        private bool isVisible = true;

    }
}
