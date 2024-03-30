using LabelImageLibrary.Controls;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LabelImageLibrary.Behaviors
{
    public class TagEditBoxBehavior : Behavior<TagEditBox>
    {
        public event EventHandler<string> TagConfirmed;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.TagConfirmed?.Invoke(this, this.AssociatedObject.Text);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
        }
    }
}
