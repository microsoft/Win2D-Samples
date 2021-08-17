#pragma once

#include "Class.g.h"

namespace winrt::ExampleGalleryDesktop_Direct3DInterop::implementation
{
    struct Class : ClassT<Class>
    {
        Class() = default;

        int32_t MyProperty();
        void MyProperty(int32_t value);
    };
}

namespace winrt::ExampleGalleryDesktop_Direct3DInterop::factory_implementation
{
    struct Class : ClassT<Class, implementation::Class>
    {
    };
}
