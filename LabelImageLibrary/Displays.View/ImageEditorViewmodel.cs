using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelImageLibrary.Behaviors;
using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LabelImageLibrary.Displays
{
    public class ImageEditorViewmodel : ObservableObject
    {
        private readonly IServiceProvider serviceProvider;

        public ICommand CreateObjectCommand { get; set; }
        public ICommand DeleteObjectCommand { get; set; }
        public ICommand FitCommand { get; set; }
        public ICommand ModifyCommand { get; set; }
        public ICommand FreezeCommand { get; set; }

        public ObservableCollection<ObjectAbstract> GraphicCollection { get; set; }

        public ObservableCollection<CanvasContainerBehaviorAbstract> BehaviorCollection { get; set; }


        public event EventHandler<NotifyCollectionChangedEventArgs> GraphicChanged;

        public ImageEditorViewmodel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.InitializeCommands();
            this.InitializeCollections();
            this.InitializeEvents();
        }

        private void InitializeCommands()
        {
            this.CreateObjectCommand = new RelayCommand(CreateObject);
            this.DeleteObjectCommand = new RelayCommand(DeleteObject);
            this.FitCommand = new RelayCommand(FitLayout);
            this.ModifyCommand = new RelayCommand(ModifyLayout);
            this.FreezeCommand = new RelayCommand(FreezeLayout);
        }

        private void InitializeCollections()
        {
            this.GraphicCollection = this.serviceProvider.GetAllGraphics();
            this.BehaviorCollection = this.serviceProvider.GetAllBehaviors();
        }

        private void InitializeEvents()
        {
            this.GraphicCollection.CollectionChanged += (s, e) => OnGraphicChanged(e);

            this.serviceProvider.GetRequiredService<LabelListViewmodel>().PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "ChoosenLabel")
                {
                    this.serviceProvider.GetRequiredService<CreateObjectBehavior>().ChangeLabel((s as LabelListViewmodel).ChoosenLabel);
                }
            };
        }


        private void CreateObject()
        {
            this.ChangeLabelMode(LabelMode.CreateObject);
            this.serviceProvider.GetRequiredService<CreateObjectBehavior>().Create(EShape.Rectangle, serviceProvider.GetRequiredService<LabelListViewmodel>().ChoosenLabel);
        }

        private void DeleteObject()
        {
            this.serviceProvider.GetCurrentCanvasContainerBehavior().DestroySelectedObject();
        }

        private void FitLayout()
        {
            this.serviceProvider.GetCurrentCanvasContainerBehavior().GetLayoutToolbox().FitLayout();
        }

        private void ModifyLayout()
        {
            this.ChangeLabelMode(LabelMode.Modify);
        }

        private void FreezeLayout()
        {
            this.ChangeLabelMode(LabelMode.Freeze);
        }

        private void ChangeLabelMode(LabelMode labelMode)
        {
            var behaviors = this.serviceProvider.GetAllBehaviors();

            behaviors.Remove(behaviors.LastOrDefault());

            switch (labelMode)
            {
                case LabelMode.Freeze:
                    behaviors.Add(serviceProvider.GetRequiredService<FreezeLayoutBehavior>());
                    break;
                case LabelMode.Modify:
                    behaviors.Add(serviceProvider.GetRequiredService<ModifyLayoutBehavior>());
                    break;
                case LabelMode.CreateObject:
                    behaviors.Add(serviceProvider.GetRequiredService<CreateObjectBehavior>());
                    break;
            }
        }

        private void OnGraphicChanged(NotifyCollectionChangedEventArgs args)
        {
            this.GraphicChanged?.Invoke(this, args);   
        }
    }
}
