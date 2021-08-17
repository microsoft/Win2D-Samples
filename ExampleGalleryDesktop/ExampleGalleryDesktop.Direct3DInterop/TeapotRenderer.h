// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

#pragma once

namespace ExampleGallery
{
    namespace Direct3DInterop
    {
        using namespace winrt;
        using namespace ::DirectX;
        using namespace winrt::Microsoft::Graphics::Canvas;
        //using namespace Microsoft::WRL;
        using namespace winrt::Windows::Foundation::Numerics;
        using namespace winrt::Windows::Graphics::DirectX::Direct3D11;


        // Interop helper shows how to mix Win2D with Direct3D rendering.
        // Implemented in C++/CX, this class serves as a bridge between the C# Win2D code
        // in Direct3DInteropExample.xaml.cs, and Direct3D teapot rendering functionality
        // provided by the DirectX Tool Kit (https://github.com/Microsoft/DirectXTK).
        class TeapotRenderer sealed
        {
        public:
            TeapotRenderer(ICanvasResourceCreator const& resourceCreator);

            void SetWorld(float4x4 value);
            void SetView(float4x4 value);
            void SetProjection(float4x4 value);
            
            void SetTexture(IDirect3DSurface const& texture);

            void Draw(CanvasDrawingSession const& drawingSession);

        private:
            void SetRenderTarget(ID2D1DeviceContext* d2dContext);

            // DirectX Tool Kit objects.
            std::unique_ptr<GeometricPrimitive> m_teapot;
            std::unique_ptr<BasicEffect> m_basicEffect;

            // Direct3D objects.
            com_ptr<ID3D11Device> m_device;
            com_ptr<ID3D11DeviceContext> m_deviceContext;

            com_ptr<ID3D11InputLayout> m_inputLayout;

            com_ptr<ID3D11DepthStencilView> m_depthStencilView;
            UINT m_depthStencilWidth;
            UINT m_depthStencilHeight;
        };
    }
}
