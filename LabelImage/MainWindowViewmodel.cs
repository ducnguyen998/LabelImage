using CommunityToolkit.Mvvm.ComponentModel;
using LabelImageLibrary;
using LabelImageLibrary.Displays;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImage
{
    public class MainWindowViewmodel : ObservableObject
    {
        public InteractiveDisplayViewmodel Display { get; set; }

        public MainWindowViewmodel()
        {
            var labelImageProvider = new LabelImageProvider();

            Display = labelImageProvider.GetServiceProvider().GetRequiredService<InteractiveDisplayViewmodel>();    
        }
    }
}
