using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;

namespace CompositionExample
{
    public class  Entity
    {
        public Dictionary<string, Relationship> Relationships { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Color EntityColor { get; set; }
        public Rect EntityShape { get; set; }
        public Entity()
        {
            Relationships = new Dictionary<string, Relationship>();
        }
    }
}
