using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImageLibrary.Objects
{
    public class ObjectNull : ObjectAbstract
    {
        public ObjectNull(ObjectLabel objectLabel = null) : base(objectLabel)
        {
        }

        public override string ToString()
        {
            return "[Object Null]";
        }
    }
}
