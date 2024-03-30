using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LabelImageLibrary.Controls
{
    public class FixedScrollViewer : ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (e.Delta > 0)
                {
                    this.LineLeft();
                }
                else
                {
                    this.LineRight();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
        }
    }
}
