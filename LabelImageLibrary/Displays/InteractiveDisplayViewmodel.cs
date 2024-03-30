using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabelImageLibrary.Behaviors;
using LabelImageLibrary.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LabelImageLibrary.Displays
{
    public class InteractiveDisplayViewmodel : ObservableObject
    {
        public LabelListViewmodel LabelListDisplay { get; set; }
        public AnnotationListViewmodel AnnotationListDisplay { get; set; }
        public ImageEditorViewmodel ImageEditorDisplay { get; set; }

        public InteractiveDisplayViewmodel(IServiceProvider serviceProvider)
        {
            this.LabelListDisplay = serviceProvider.GetRequiredService<LabelListViewmodel>();   
            this.AnnotationListDisplay = serviceProvider.GetRequiredService<AnnotationListViewmodel>();   
            this.ImageEditorDisplay = serviceProvider.GetRequiredService<ImageEditorViewmodel>();   
        }
    }
}
