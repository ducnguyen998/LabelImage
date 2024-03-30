using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImageLibrary.Objects
{
    public interface IInteractiveObject
    {
        bool IsSelected { get; set; }

        bool IsInteractable { get; set; }
    }
}
