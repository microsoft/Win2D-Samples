# Win2D samples

These sample projects show how to use the Win2D graphics API.

- **Example Gallery** is the main Win2D demo and sample application. This is a C# XAML 
  application, and contains many different examples showing various capabilities of 
  Win2D, with a menu to select between them. Start here to get an idea of what Win2D can 
  do and how use it.

    - *Example Gallery is also available in the Store for
      [Windows](http://apps.microsoft.com/windows/en-us/app/win2d-example-gallery/b668cfe1-e280-4c1e-adc1-09b7981ab084) and
      [Phone](http://www.windowsphone.com/en-us/store/app/win2d-example-gallery/8797b9cb-1443-475f-9a43-dd959de6fcc1)*.

- **CoreWindow Example** is a C# application that draws directly to a CoreWindow, 
  without using XAML.

- **Simple Sample** is, yup, you guessed it! Pretty much the simplest possible app that 
  uses Win2D - it's our version of the venerable "hello world".

Each sample contains two different Visual Studio solution files. The *.81.sln 
version is for Windows 8.1 and Windows Phone 8.1, while *.uap.sln is for 
Windows 10 (Universal Windows Platform).

These samples reference the [Win2D NuGet package](http://www.nuget.org/packages/Win2D), 
which will be automatically downloaded when you build them, so there is no need to 
separately install Win2D itself. Win2D [source code](http://github.com/Microsoft/Win2D) 
is also available for those who like to see how the sausage is made.

For more information about Win2D, see its [documentation](http://microsoft.github.io/Win2D).
