using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public class ObjectLabel : ObservableObject
    {
        public static ObjectLabel Default = new ObjectLabel() { Color = Brushes.GreenYellow, Name = "Default" };

        public ICommand DeleteCommand { get; set; }
        public ICommand ModifyCommand { get; set; }

        public enum ECommand
        {
            Delete, Modify, Choosen
        }

        public event EventHandler<ECommand> CommandRaised;

        public ObjectLabel()
        {
            this.DeleteCommand = new RelayCommand(() => this.OnCommandRaised(ECommand.Delete));
            this.ModifyCommand = new RelayCommand(() => this.OnCommandRaised(ECommand.Modify));
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
                }
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsChoosen
        {
            get
            {
                return isChoosen;
            }
            set
            {
                if (isChoosen != value)
                {
                    isChoosen = value;
                    OnPropertyChanged();

                    if (isChoosen)
                    {
                        OnCommandRaised(ECommand.Choosen);
                    }
                }
            }
        }

        private void OnCommandRaised(ECommand command)
        {
            this.CommandRaised?.Invoke(this, command);
        }

        private Brush color;

        private string name;

        private bool isChoosen;
    }
}
