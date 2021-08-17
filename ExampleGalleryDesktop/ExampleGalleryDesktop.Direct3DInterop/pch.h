#pragma once
#include <windows.h>
#include <unknwn.h>
#include <restrictederrorinfo.h>
#include <hstring.h>

// Undefine GetCurrentTime macro to prevent
// conflict with Storyboard::GetCurrentTime
#undef GetCurrentTime

// #include <wrl.h>
#include <memory>
#include <vector>
#include <WindowsNumerics.h>

// DirectX headers.
#include <d2d1_1.h>
#include <d2d1_3.h>
#include <d2d1helper.h>
#include <DirectXMath.h>
#include <Windows.Graphics.DirectX.Direct3D11.interop.h>
#include <d3d11.h>

// DirectX Tool Kit headers.
#include <GeometricPrimitive.h>
#include <Effects.h>

#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.ApplicationModel.Activation.h>
#include <winrt/Microsoft.UI.Composition.h>
#include <winrt/Microsoft.UI.Xaml.h>
#include <winrt/Microsoft.UI.Xaml.Controls.h>
#include <winrt/Microsoft.UI.Xaml.Controls.Primitives.h>
#include <winrt/Microsoft.UI.Xaml.Data.h>
#include <winrt/Microsoft.UI.Xaml.Interop.h>
#include <winrt/Microsoft.UI.Xaml.Markup.h>
#include <winrt/Microsoft.UI.Xaml.Media.h>
#include <winrt/Microsoft.UI.Xaml.Navigation.h>
#include <winrt/Microsoft.UI.Xaml.Shapes.h>

#include <winrt/Microsoft.Graphics.Canvas.h>
#include <Microsoft.Graphics.Canvas.native.h>
