using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelImageLibrary.Utils
{
    public class ObjectDraggingAgent
    {
        private double delta;

        public ObjectDraggingAgent()
        {
            this.Reset();
        }

        public void Increase()
        {
            this.delta += 0.25;
        }

        public void Reset()
        {
            this.delta = +0.75;
        }

        public double Factor() => delta;
    }
}
