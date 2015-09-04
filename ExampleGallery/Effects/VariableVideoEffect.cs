using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Collections.Generic;

namespace ExampleGallery.Effects
{
    /// <summary>
    /// Win2D Emboss Effect http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_EmbossEffect.htm
    /// Amount - min is 0f, max is 10f, default is 1f
    /// </summary>
    public sealed class VariableVideoEffect : IBasicVideoEffect
    {
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration;

        /// <summary>
        /// The intensity of the Emboss Effect
        /// </summary>
        public float Amount
        {
            get
            {
                //check to see if there is an "Amount" key and get the value
                if (_configuration != null && _configuration.ContainsKey("Amount"))
                    return (float)_configuration["Amount"];

                //otherwise use the Default value of 1
                return 1f;
            }
            set
            {
                _configuration["Amount"] = value;
            }
        }

        public void SetProperties(IPropertySet configuration)
        {
            //the PropertySet passed in the VideoEffectDefinition instance
            _configuration = configuration;
        }

        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {
            _canvasDevice = CanvasDevice.CreateFromDirect3D11Device(device);
        }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            using (CanvasBitmap inputBitmap = CanvasBitmap.CreateFromDirect3D11Surface(_canvasDevice, context.InputFrame.Direct3DSurface))
            using (CanvasRenderTarget renderTarget = CanvasRenderTarget.CreateFromDirect3D11Surface(_canvasDevice, context.OutputFrame.Direct3DSurface))
            using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
            {

                var emboss = new EmbossEffect
                {
                    Source = inputBitmap,
                    Amount = Amount,
                    Angle = 0f
                };

                ds.DrawImage(emboss);
            }
        }

        public void Close(MediaEffectClosedReason reason)
        {
            _canvasDevice?.Dispose();
        }

        public void DiscardQueuedFrames()
        {
            //no frames cached
        }

        public bool IsReadOnly => false;
        public IReadOnlyList<VideoEncodingProperties> SupportedEncodingProperties => new List<VideoEncodingProperties>();
        public MediaMemoryTypes SupportedMemoryTypes => MediaMemoryTypes.Gpu;
        public bool TimeIndependent => false;
    }
}
