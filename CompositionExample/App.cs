// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Popups;


namespace CompositionExample
{
    class App : IFrameworkView
    {
        bool RenderDone = false;
        CoreWindow window;
        Compositor compositor;
        ContainerVisual rootVisual;
        CompositionTarget compositionTarget;
        CanvasDevice device;
        CompositionGraphicsDevice compositionGraphicsDevice;
        List<RelationshipHandler> relationshipHandler;
        List<RelationshipLine> relationshipLines;
        Dictionary<string, List<string>> keyValuePairs;
        Dictionary<string, Point> pointMap;
        private Graph<Entity> graph;

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Random rnd = new Random();
        Vector2 OgSize;
        //Default entity size
        Size size;
        public void Initialize(CoreApplicationView applicationView)
        {
            pointMap = new Dictionary<string, Point>();
            relationshipHandler = new List<RelationshipHandler>();
            relationshipLines = new List<RelationshipLine>();
            applicationView.Activated += applicationView_Activated;
            
        }

        private void Window_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            RenderDone = false;
            var ignoredTask = UpdateVisualsLoop();
        }

        public void Uninitialize()
        {
            //swapChainRenderer?.Dispose();
            cancellationTokenSource.Cancel();
        }

        void applicationView_Activated(CoreApplicationView sender, IActivatedEventArgs args)
        {
            CoreWindow.GetForCurrentThread().Activate();
        }

        public void Load(string entryPoint)
        {
        }

        public void Run()
        {
            CoreWindow.GetForCurrentThread().Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);

        }

        public async void SetWindow(CoreWindow window)
        {
            this.window = window;
            OgSize = new Vector2((float)window.Bounds.Width, (float)window.Bounds.Height);
            Clipboard.ContentChanged += Clipboard_ContentChanged;
            window.SizeChanged += Window_SizeChanged;

            if (!Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                var dialog = new MessageDialog("This version of Windows does not support the Composition APIs.");
                await dialog.ShowAsync();
                CoreApplication.Exit();
                return;
            }

            window.PointerPressed += Window_PointerPressed;

            CoreApplication.Suspending += CoreApplication_Suspending;
            DisplayInformation.DisplayContentsInvalidated += DisplayInformation_DisplayContentsInvalidated;

            compositor = new Compositor();

            CreateDevice();

            size = new Size(window.Bounds.Width,window.Bounds.Height);
            size.Width *= 0.10;
            size.Height *= 0.10;
            
            rootVisual = compositor.CreateContainerVisual();
            compositionTarget = compositor.CreateTargetForCurrentView();
            compositionTarget.Root = rootVisual;

            OutputClipboardText();

            var ignoredTask = UpdateVisualsLoop();
        }

        private void Clipboard_ContentChanged(object sender, object e)
        {
            size = new Size(window.Bounds.Width, window.Bounds.Height);
            size.Width *= 0.10;
            size.Height *= 0.10;

            rootVisual = compositor.CreateContainerVisual();
            //compositionTarget = compositor.CreateTargetForCurrentView();
            compositionTarget.Root = rootVisual;

            relationshipHandler.Clear();
            OutputClipboardText();
            var ignoredTask = UpdateVisualsLoop();

        }
      

        
        void OutputClipboardText()
        {
            graph = new Graph<Entity>(1000);
            pointMap = new Dictionary<string, Point>();
            List<string> props = new List<string>();
            keyValuePairs = new Dictionary<string, List<string>>();


            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var task = Task.Run(async () => await dataPackageView.GetTextAsync());
                var result = task.Result;
                
                string text = result.ToString();
                string[] pastedRows = Regex.Split(text.TrimEnd("\r\n".ToCharArray()), "\r\n");

                
                int DescriptionCount = 1;
                foreach (string cell in pastedRows[0].Split(new char[] { '\t' }))
                {

                    keyValuePairs.Add(cell, new List<string>());
                    props.Add(cell);
                    pointMap.Add(cell,new Point(DescriptionCount, 0));
                    DescriptionCount += 1;
                }



                for (int r = 1; r < pastedRows.Length; r++)
                {
                    string[] pastedRowCells = pastedRows[r].Split(new char[] { '\t' });

                    GraphNode<Entity> tempNode = null;
                    for (int i = 0; i < pastedRowCells.Length; i++)
                    {
                        GraphNode<Entity> Link = graph.BFS(pastedRowCells[i]);
                        if(Link==null)
                        {
                            Point temp = new Point(pointMap[props[i]].X, pointMap[props[i]].Y + 1);
                            if(!pointMap.ContainsKey(pastedRowCells[i]))
                                pointMap.Add(pastedRowCells[i], new Point((pointMap[props[i]].X)+((pointMap[props[i]].X)*.10), pointMap[props[i]].Y+ ((pointMap[props[i]].Y) * .10)));
                            pointMap[props[i]] = temp;

                            //Point tempPoint = 
                            Link = graph.ConnectedNodes[graph.AddNode(pastedRowCells[i], new Entity { Name = pastedRowCells[i], EntityColor = Colors.Black, Description = props[i], EntityShape = new Rect(new Point(pointMap[pastedRowCells[i]].X * size.Width, pointMap[pastedRowCells[i]].Y * size.Height), size) })][0];
                            graph.ConnectedNodes[Link.NodeIndex][0].xPoint = (int)((int)temp.X*size.Width);
                            graph.ConnectedNodes[Link.NodeIndex][0].yPoint = (int)((int)temp.Y*size.Height);
                        }
                            
                        keyValuePairs[props[i]].Add(pastedRowCells[i]);
                        if (tempNode != null)
                        {
                            
                            if (!tempNode.Value.Relationships.ContainsKey(Link.Name))
                            {
                                //pointMap.Add(Link.Name + tempNode.Name, new Point(,) );
                                graph.ConnectDirectToAndFromNode(Link.Name, tempNode.Name, 1);
                                //Relationship rel = 
                                tempNode.Value.Relationships.Add(Link.Name, new Relationship { Color = Colors.Wheat, FromEntity = graph.ConnectedNodes[tempNode.NodeIndex][0].Value, ToEntity = graph.ConnectedNodes[Link.NodeIndex][0].Value });
                                //Link.Value.Relationships.Add(Link.Name,rel);
                                
                            }
                        }
                            
                        tempNode = Link;
                    }

                }
                relationshipHandler.Add(new RelationshipHandler(compositor, compositionGraphicsDevice, size, graph));
                rootVisual.Children.InsertAtTop(relationshipHandler[0].Visual);
                RenderDone = false;
            }
        }
        

        async Task UpdateVisualsLoop()
        {
            var token = cancellationTokenSource.Token;

            while (!token.IsCancellationRequested && !RenderDone)
            {
                //UpdateVisual(swapChainRenderer.Visual, swapChainRenderer.Size);
                foreach(RelationshipHandler rh in relationshipHandler)
               {
                    UpdateVisual(rh.Visual, rh.Size);
                }
                //foreach (RelationshipLine rl in relationshipLines)
                //{
                //    UpdateVisualRelationship(rl.Visual, rl.Size, rl.Relationship);
                //    //UpdateVisualRelationship(rl.Visual, rl.Size, rl.Relationship.FromEntity);
                //}
                    
                

                await Task.Delay(TimeSpan.FromSeconds(2));
                RenderDone = true;
            }
        }

        void UpdateVisual(Visual visual, Size size)
        {
            UpdateVisualPosition(visual, size);
            UpdateVisualOpacity(visual);
        }

        void UpdateVisualPosition(Visual visual, Size size)
        {
            var oldOffset = visual.Offset;
            Vector2 newSize = new Vector2((float)window.Bounds.Width, (float)window.Bounds.Height);
            Vector2 oldVisSize = new Vector2((float)size.Width, (float)size.Height);
            Vector2 newVisSize = oldVisSize * (newSize / OgSize);

            var newOffset = new Vector3(
                (float)(((visual.CenterPoint.X) * newVisSize.X)),
                (float)(((visual.CenterPoint.Y) * newVisSize.Y)),
                0);
            visual.Offset = newOffset;
            //entity.EntityShape = new Rect(newOffset.X, newOffset.Y, newVisSize.X, newVisSize.Y);

            if(newVisSize!=oldVisSize)
                AnimateSizeChange(visual, oldVisSize, newVisSize);
            AnimateOffset(visual, oldOffset, newOffset);
        }

        void UpdateVisualRelationship(Visual visual, Size size, Relationship rel)
        {
            //var oldOffset = visual.Offset;
            //Vector2 newSize = new Vector2((float)window.Bounds.Width, (float)window.Bounds.Height);
            //Vector2 oldVisSize = new Vector2((float)size.Width, (float)size.Height);
            //Vector2 newVisSize = oldVisSize * (newSize / OgSize);
            ////var newOffset = new Vector3();
            //var newOffset = new Vector3(
            //   (float)((rel.To().X) * newVisSize.X),
            //   (float)(((rel.To().X) * newVisSize.Y)),
            //   0);


            //if (rel.FromEntity.EntityShape.Left > rel.ToEntity.EntityShape.Left)
            //{
            //    newOffset.X = (float)((rel.Size().Width/ (pointMap[rel.FromEntity.Name].X * size.Width)) * newVisSize.X);
            //}
            //else
            //{
            //    newOffset.X = (float)((rel.Size().Width/ (pointMap[rel.ToEntity.Name].X * size.Width)) * newVisSize.X);
            //}
            //if (rel.FromEntity.EntityShape.Left > rel.ToEntity.EntityShape.Left)
            //{
            //    newOffset.Y = (float)((rel.Size().Height / (pointMap[rel.FromEntity.Name].Y*size.Height)) * newVisSize.Y);
            //}
            //else
            //{
            //    newOffset.Y = (float)((rel.Size().Height/(pointMap[rel.ToEntity.Name].Y*size.Height)) * newVisSize.Y);
            //}
            //if (newVisSize != oldVisSize)
            //    AnimateSizeChange(visual, oldVisSize, newVisSize);
            //AnimateOffset(visual, oldOffset, newOffset);



        }

        void UpdateVisualOpacity(Visual visual)
        {
            var oldOpacity = visual.Opacity;
            var newOpacity = 1;

            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(0, oldOpacity);
            animation.InsertKeyFrame(1, newOpacity);

            visual.Opacity = newOpacity;
            
            visual.StartAnimation("Opacity", animation);
        }
        void AnimateSizeChange(Visual visual, Vector2 oldSize, Vector2 newSize)
        {
            var animation = compositor.CreateVector2KeyFrameAnimation();
            animation.InsertKeyFrame(0, oldSize);
            animation.InsertKeyFrame(1, newSize);
            animation.Duration = TimeSpan.FromSeconds(1);

            visual.StartAnimation("Size", animation);
        }

        void AnimateOffset(Visual visual, Vector3 oldOffset, Vector3 newOffset)
        {
            var animation = compositor.CreateVector3KeyFrameAnimation();
            animation.InsertKeyFrame(0, oldOffset);
            animation.InsertKeyFrame(1, newOffset);
            animation.Duration = TimeSpan.FromSeconds(1);

            visual.StartAnimation("Offset", animation);
        }

        void Window_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            //swapChainRenderer.Paused = !swapChainRenderer.Paused;
        }

        void CoreApplication_Suspending(object sender, SuspendingEventArgs args)
        {
            try
            {
                device.Trim();
            }
            catch (Exception e) when (device.IsDeviceLost(e.HResult))
            {
                device.RaiseDeviceLost();
            }
        }

        void CreateDevice()
        {
            device = CanvasDevice.GetSharedDevice();
            device.DeviceLost += Device_DeviceLost;

            if (compositionGraphicsDevice == null)
            {
                compositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, device);
            }
            else
            {
                CanvasComposition.SetCanvasDevice(compositionGraphicsDevice, device);
            }

            //if (swapChainRenderer != null)
                //swapChainRenderer.SetDevice(device, new Size(window.Bounds.Width, window.Bounds.Height));
        }

        void Device_DeviceLost(CanvasDevice sender, object args)
        {
            device.DeviceLost -= Device_DeviceLost;
            var unwaitedTask = window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => CreateDevice());
        }

        void DisplayInformation_DisplayContentsInvalidated(DisplayInformation sender, object args)
        {
            // The display contents could be invalidated due to a lost device, or for some other reason.
            // We check this by calling GetSharedDevice, which will make sure the device is still valid before returning it.
            // If the shared device has been lost, GetSharedDevice will automatically raise its DeviceLost event.
            CanvasDevice.GetSharedDevice();
        }
    }

    class ViewSource : IFrameworkViewSource
    {
        public IFrameworkView CreateView()
        {
            return new App();
        }

        static void Main(string[] args)
        {
            CoreApplication.Run(new ViewSource());
        }
    }
}
