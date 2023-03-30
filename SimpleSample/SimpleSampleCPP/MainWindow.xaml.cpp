// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

#include "pch.h"
#include "MainWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::SimpleSampleCPP::implementation
{
    MainWindow::MainWindow()
    {
        InitializeComponent();
    }
}


void winrt::SimpleSampleCPP::implementation::MainWindow::canvasControl_Draw(winrt::Microsoft::Graphics::Canvas::UI::Xaml::CanvasControl const& sender, winrt::Microsoft::Graphics::Canvas::UI::Xaml::CanvasDrawEventArgs const& args)
{
    args.DrawingSession().DrawEllipse(155, 115, 80, 30, Microsoft::UI::Colors::Black(), 3);
    args.DrawingSession().DrawText(L"Hello, world!", 100, 100, Microsoft::UI::Colors::Colors::Yellow());
}
