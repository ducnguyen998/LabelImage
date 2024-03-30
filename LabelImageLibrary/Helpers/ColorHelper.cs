using LabelImageLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImageLibrary.Helpers
{
    public static class ColorHelper
    {
        public static System.Windows.Media.SolidColorBrush GetLighter(this System.Windows.Media.Brush color, double opacity)
        {
            System.Windows.Media.SolidColorBrush lighterColor = color.Clone() as System.Windows.Media.SolidColorBrush;

            lighterColor.Opacity = opacity;

            return lighterColor;
        }

        public static IEnumerable<System.Drawing.Color> GetAllSystemColors()
        {
            return typeof(System.Drawing.Color)
                .GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Select(p => p.GetValue(null))
                .OfType<System.Drawing.Color>();
        }

        public static System.Windows.Media.Brush GetRandomBrush()
        {
            var colors = GetAllSystemColors().ToList();

            var randIdx = new Random().Next(0, colors.Count - 1);

            var randColor = colors[randIdx];

            return new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromArgb(
                randColor.A,
                randColor.R,
                randColor.G,
                randColor.B
                ));
        }

        public static System.Windows.Media.Brush GetRandomBrush(ObservableCollection<ObjectLabel> objectLabels)
        {
            var colors = GetAllSystemColors().ToList();

            var randIdx = new Random().Next(0, colors.Count - 1);

            var randColor = colors[randIdx];



            return new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromArgb(
                randColor.A,
                randColor.R,
                randColor.G,
                randColor.B
                ));
        }
    }
}
