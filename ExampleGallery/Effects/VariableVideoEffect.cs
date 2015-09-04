using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Collections.Generic;

namespace ExampleGallery.Effects
{
    public sealed class VariableVideoEffect : IBasicVideoEffect
    {
        private VideoEncodingProperties _currentEncodingProperties;
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration;

        public float Amount
        {
            get
            {
                if (_configuration != null && _configuration.ContainsKey("Amount"))
                    return (float)_configuration["Amount"];

                return 1f;
            }
            set
            {
                _configuration["Amount"] = value;
            }
        }

        public void SetProperties(IPropertySet configuration)
        {
            _configuration = configuration;
        }

        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {
            _currentEncodingProperties = encodingProperties;
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
