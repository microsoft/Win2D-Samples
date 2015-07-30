# Win2D samples

These sample projects show how to use the Win2D graphics API.

- **Example Gallery** is the main Win2D demo and sample application. This is a C# XAML 
  application, and contains many different examples showing various capabilities of 
  Win2D, with a menu to select between them. Start here to get an idea of what Win2D can 
  do and how use it.

    - *Example Gallery is also available in the
      [Store](https://www.microsoft.com/store/apps/9NBLGGGXWT9F).*

- **CoreWindow Example** is a C# application that draws directly to a CoreWindow, 
  without using XAML.

- **Simple Sample** is, yup, you guessed it! Pretty much the simplest possible app that 
  uses Win2D - it's our version of the venerable "hello world".

Each sample contains two different Visual Studio solution files. The *.uap.sln 
version is for Windows 10 (Universal Windows Platform), while *.81.sln is for 
Windows 8.1 and Windows Phone 8.1.

These samples reference the NuGet packages [Win2D.uwp](http://www.nuget.org/packages/Win2D.uwp) 
or [Win2D.win81](http://www.nuget.org/packages/Win2D.win81), which will be 
automatically downloaded when you build the sample, so there is no need to 
separately install Win2D itself. Win2D [source code](http://github.com/Microsoft/Win2D) 
is also available for those who like to see how the sausage is made.

For more information about Win2D, see its [documentation](http://microsoft.github.io/Win2D).
