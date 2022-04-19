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
    class RelationshipHandler
    {
        public SpriteVisual drawingSurfaceVisual;
        public CompositionDrawingSurface drawingSurface;

        //int drawCount;

        public Visual Visual { get { return drawingSurfaceVisual; } }

        public Size Size { get { return drawingSurface.Size; } }
        //public RelationshipHandler RH;
        private Graph<Entity> graph;
        //public Graph<Entity> graph;
       // public Dictionary<string, int> RenderedXPoints;
        //public Entity Entity;
        private Size EntitySize;
        public RelationshipHandler(Compositor compositor, CompositionGraphicsDevice compositionGraphicsDevice, Size entitySize, Graph<Entity> graph) 
        {
            //Entity = entity;
            this.EntitySize = entitySize;
            this.graph = graph;
            //SetFromGraph(entitySize);
                drawingSurfaceVisual = compositor.CreateSpriteVisual();
                drawingSurface = compositionGraphicsDevice.CreateDrawingSurface(SetFromGraph(entitySize), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                drawingSurfaceVisual.Brush = compositor.CreateSurfaceBrush(drawingSurface);
                DrawDrawingSurface();
                compositionGraphicsDevice.RenderingDeviceReplaced += CompositionGraphicsDevice_RenderingDeviceReplaced;

        }
        private Dictionary<string, bool> CapturedRelationships;
        private Dictionary<string, int> CapturedEntities;
        private Size SetFromGraph(Size entitySize)
        {
            //OutputClipboardText();
            CapturedRelationships = new Dictionary<string, bool>();
            int MaxCount = 0;
            //OgSize = new Vector2((float)window.Bounds.Width, (float)window.Bounds.Height);
            CapturedEntities = new Dictionary<string, int>();

            foreach (List<GraphNode<Entity>> nodes in graph.ConnectedNodes)
            {

                if (nodes == null)
                    break;

                if (!CapturedEntities.ContainsKey(nodes[0].Value.Description))
                {
                    CapturedEntities.Add(nodes[0].Value.Description, 2);
                }
                else 
                    CapturedEntities[nodes[0].Value.Description] += 2;
                if (CapturedEntities[nodes[0].Value.Description] > MaxCount)
                    MaxCount = CapturedEntities[nodes[0].Value.Description];
            }
            foreach (List<GraphNode<Entity>> nodes in graph.ConnectedNodes)
            {

                if (nodes == null)
                    break;

                foreach (Relationship rel in nodes[0].Value.Relationships.Values)
                {

                    if (!CapturedRelationships.ContainsKey(rel.ToEntity.Name + rel.FromEntity.Name))
                    {
                        CapturedRelationships.Add(rel.ToEntity.Name + rel.FromEntity.Name, true);
                    }

                }

            }
            return new Size((CapturedEntities.Count *(EntitySize.Width*1.25) + EntitySize.Width), (MaxCount * (EntitySize.Height*1.25))+EntitySize.Height);

            //RenderDone = false;
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

            using (var ds = CanvasComposition.CreateDrawingSession(drawingSurface))
            {

                //
                //                
                foreach(List<GraphNode<Entity>> ent in graph.ConnectedNodes) 
                {
                    
                    if (ent != null)
                    {
                        GraphNode<Entity> e = ent[0];
                        Rect TempShape = new Rect { Height = this.EntitySize.Height, Width = this.EntitySize.Width, X = e.xPoint*1.25, Y = e.yPoint*1.25};
                        ds.FillRoundedRectangle(TempShape, 10, 10, Colors.LightBlue);
                        ds.DrawRoundedRectangle(TempShape, 10, 10, Colors.Gray, 2);


                        foreach (Relationship rl in e.Value.Relationships.Values)
                        {
                            GraphNode<Entity> link = graph.BFS(rl.ToEntity.Name);
                            Rect ToShape = new Rect { Height = rl.Size().Height, Width = rl.Size().Width, X = link.xPoint*1.25, Y = link.yPoint*1.25 };
                            Point To = this.To(new Entity { EntityShape = ToShape }, new Entity { EntityShape = TempShape });
                            Point From = this.From(new Entity { EntityShape = ToShape }, new Entity { EntityShape = TempShape });

                            ds.DrawLine(new Vector2((float)From.X, (float)From.Y), new Vector2((float)To.X, (float)To.Y), Colors.Black);
                        }
                        ds.DrawText(e.Name, TempShape, Colors.Black, new CanvasTextFormat()
                        {
                            FontFamily = "Comic Sans MS",
                            FontSize = 12,
                            WordWrapping = CanvasWordWrapping.WholeWord,
                            VerticalAlignment = CanvasVerticalAlignment.Center,
                            HorizontalAlignment = CanvasHorizontalAlignment.Center
                        });
                    }

                }
               
            }
           
        }
        private Point To(Entity ToEntity, Entity FromEntity)
        {
            Point point = new Point();
            if (ToEntity.EntityShape.Top >= FromEntity.EntityShape.Top)
            {
                //To entity is further down, set point Y to reflect mid point to be the top point of the rect
                point.Y = ToEntity.EntityShape.Top;
            }
            else
            {
                //To entity is above, set the point to the bottom of the entity
                point.Y = ToEntity.EntityShape.Bottom;
            }

            if (ToEntity.EntityShape.Left >= FromEntity.EntityShape.Left)
            {
                //To entity is further right. set the x point to the left of the to entity
                point.X = ToEntity.EntityShape.Left;

            }
            else
            {
                point.X = ToEntity.EntityShape.Right;
            }
            return point;
        }
        private Point From(Entity ToEntity, Entity FromEntity)
        {
            Point point = new Point();
            if (FromEntity.EntityShape.Top >= ToEntity.EntityShape.Top)
            {
                //To entity is further down, set point Y to reflect mid point to be the top point of the rect
                point.Y = Math.Max(FromEntity.EntityShape.Top, 10);
            }
            else
            {
                //To entity is above, set the point to the bottom of the entity
                point.Y = Math.Max(FromEntity.EntityShape.Bottom, 10);

            }

            if (FromEntity.EntityShape.Left >= ToEntity.EntityShape.Left)
            {
                //To entity is further right. set the x point to the left of the to entity
                point.X = Math.Max(FromEntity.EntityShape.Left, 10);

            }
            else
            {
                point.X = Math.Max(FromEntity.EntityShape.Right, 10);
            }

            return point;
        }




    }
        

    }

