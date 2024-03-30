using CommunityToolkit.Mvvm.ComponentModel;
using LabelImageLibrary.Behaviors;
using LabelImageLibrary.Displays;
using LabelImageLibrary.Helpers;
using LabelImageLibrary.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImageLibrary
{
    public class LabelImageProvider
    {
        private readonly IServiceProvider serviceProvider;

        public LabelImageProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(s => s);

            services.AddSingleton<ObservableCollection<ObjectLabel>>();
            services.AddSingleton<ObservableCollection<ObjectAnnotation>>();
            services.AddSingleton<ObservableCollection<ObjectAbstract>>();
            services.AddSingleton<ObservableCollection<CanvasContainerBehaviorAbstract>>();

            services.AddSingleton<CreateObjectBehavior>();
            services.AddSingleton<ModifyLayoutBehavior>();
            services.AddSingleton<FreezeLayoutBehavior>();

            services.AddSingleton<TagEditBoxBehavior>();

            services.AddSingleton<AnnotationListViewmodel>();
            services.AddSingleton<ImageEditorViewmodel>();
            services.AddSingleton<LabelListViewmodel>();

            services.AddSingleton<InteractiveDisplayViewmodel>();

            this.serviceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider GetServiceProvider()
        {
            return this.serviceProvider;
        }

        public ObservableCollection<ObjectAbstract> GetGraphicCollection()
        {
            return this.serviceProvider.GetRequiredService<ObservableCollection<ObjectAbstract>>();
        }

        public ObservableCollection<CanvasContainerBehaviorAbstract> GetBehaviorCollection()
        {
            return this.serviceProvider.GetRequiredService<ObservableCollection<CanvasContainerBehaviorAbstract>>();
        }
    }
}
