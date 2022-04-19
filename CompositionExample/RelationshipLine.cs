using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using System.Numerics;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Input;
using Windows.ApplicationModel.DataTransfer;
using System.Text.RegularExpressions;
using Windows.UI.Composition;
using Windows.Graphics.DirectX;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.Canvas.Text;

namespace CompositionExample
{
    class RelationshipLine
    {
        public SpriteVisual drawingSurfaceVisual;
        public CompositionDrawingSurface drawingSurface;

        public Visual Visual { get { return drawingSurfaceVisual; } }

        public Size Size { get { return drawingSurface.Size; } }
        //public RelationshipHandler RH;
        //private double ToX;
        //private double ToY;
        //private double FromX;
        //private double FromY;

        public Relationship Relationship;
        public RelationshipLine(Compositor compositor, CompositionGraphicsDevice compositionGraphicsDevice, Relationship relationship) 
        {
            Relationship= relationship;
            //graph = new Graph<Entity>(10000);
            Rect ToRect = Relationship.ToEntity.EntityShape;
            Rect FromRect = Relationship.FromEntity.EntityShape;
            // ToX = 0;
            //ToY = 0;
            //FromX = 0;
            //FromY = 0;
            //Size ToSize = ;



            //if (ToRect.Left > FromRect.Left)
            //{
            //    ToX = ToRect.Left;
            //    FromX = FromRect.Left;
            //    ToSize.Width = ToX - FromX;
            //}
            //else
            //{
            //    ToX = ToRect.Left;
            //    FromX = FromRect.Left;
            //    ToSize.Width = FromX - ToX;
            //}

            //if (ToRect.Top > FromRect.Top)
            //{
            //    ToY = ToRect.Top;
            //    FromX = FromRect.Top;
            //    ToSize.Height = ToY - FromY;
            //}
            //else
            //{
            //    ToY = ToRect.Top;
            //    FromY = FromRect.Top;
            //    ToSize.Height = FromY - ToY;
            //}
            //if (ToRect.X > FromRect.X)
            //{
            //    ToX = (float)ToRect.X;
            //    FromX = (float)(FromRect.X + (FromRect.Width / 2.0));


            //}
            //else
            //{
            //    ToX= (float)(ToRect.X + (ToRect.Width/ 2.0));
            //    FromX = (float)FromRect.X;
            //}

            //if (ToRect.Y > FromRect.Y)
            //{
            //    ToY = (float)ToRect.Y;
            //    FromY = (float)((float)FromRect.Y + FromRect.Height);
            //}
            //else
            //{
            //    ToY = (float)((float)ToRect.Y + (ToRect.Height / 2));
            //    FromY = (float)FromRect.Y;
            //}

            //ToSize.Width = 1+Math.Max(relationship.ToEntity.EntityShape.Right, relationship.FromEntity.EntityShape.Right) -
            //    Math.Min(relationship.ToEntity.EntityShape.Right, relationship.FromEntity.EntityShape.Right);
            //ToSize.Height = 1+Math.Max(relationship.ToEntity.EntityShape.Top, relationship.FromEntity.EntityShape.Top) -
            //    Math.Min(relationship.ToEntity.EntityShape.Top, relationship.FromEntity.EntityShape.Top);
            //this.Size = Relationship.Size();
            drawingSurfaceVisual = compositor.CreateSpriteVisual();
            drawingSurface = compositionGraphicsDevice.CreateDrawingSurface(Relationship.Size(), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                drawingSurfaceVisual.Brush = compositor.CreateSurfaceBrush(drawingSurface);
                DrawDrawingSurface();
                compositionGraphicsDevice.RenderingDeviceReplaced += CompositionGraphicsDevice_RenderingDeviceReplaced;
        }

        void CompositionGraphicsDevice_RenderingDeviceReplaced(CompositionGraphicsDevice sender, RenderingDeviceReplacedEventArgs args)
        {
            DrawDrawingSurface();
        }

        void DrawDrawingSurface()
        {
            //double X = 0;
            //double Y = 0;
            //double len = 50;
            //int RenderColumn = 1;
            if(Relationship.Size().Width >0 && Relationship.Size().Height >0)
            using (var ds = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                //ds.Clear(Colors.Transparent);
                //= new Rect { Height = this.Size.Height, Width=this.Size.Width, X=0, Y=0 };
                Point From = Relationship.From();
                Point To = Relationship.To();
                ds.DrawLine(new Vector2((float)From.X, (float)From.Y),new Vector2((float)To.X, (float)To.Y),Colors.Black);

            }
        }
        }
        

    }

