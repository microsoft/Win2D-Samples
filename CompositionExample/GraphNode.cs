using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompositionExample
{
    public class GraphNode<T>
    {
        private T _value;
        private string _name;
        public int NodeIndex;
        public int xPoint;
        public int yPoint;
        public int width;
        public int height;
        public int TotalReqSupport;

        public string RenderShape;

        public GraphNode(string name, T value, int NodePosition, int xPoint = 0, int yPoint = 0)
        {
            TotalReqSupport = 0;
            NodeIndex = NodePosition;
            _name = name;
            _value = value;

        }

        public string Name
        {
            get
            {

                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("The node name cannot be null, empty, or whitespace");

                _name = value;
            }
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
}
