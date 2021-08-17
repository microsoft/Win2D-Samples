# Win2D samples

These sample projects show how to use the Win2D graphics API.

- This gallery is currently being updated to use **ProjectReunion**.  UAP is in preview in ProjectReunion.0.5 
  Because these samples are UAP, they are using **ProjectReunion.0.5.0-prerelease**
  The exception is Simple Sample Desktop, which is not a UAP but a Desktop App.
  Simple Sample Desktop uses the latest and greatest verison of Project Reunion at the time of this writing, which is **ProjectReunion.0.5.7**

- [Not yet updated for ProjectReunion] **Example Gallery** is the main Win2D demo and sample application. This is a C# XAML 
  application, and contains many different examples showing various capabilities of 
  Win2D, with a menu to select between them. Start here to get an idea of what Win2D can 
  do and how use it.

    - *Example Gallery is also available in the
      [Store](https://www.microsoft.com/store/apps/9NBLGGGXWT9F).*

- [Not yet updated for ProjectReunion] **Composition Example** shows how to use Win2D to draw onto a Windows.UI.Composition 
  drawing surface or swapchain.

- [Not yet updated for ProjectReunion] **CoreWindow Example** is a C# application that draws directly to a CoreWindow, 
  without using XAML.

- **Simple Sample** is, yup, you guessed it! Pretty much the simplest possible app that 
  uses Win2D - it's our version of the venerable "hello world".

- **Simple Smaple Desktop** is the same version of SimpleSample, but updated to be a Reunion Desktop instead of a UAP.

- [Not yet updated for ProjectReunion] **[Win2D-MazeGame](https://github.com/microsoft/Win2DMazeGame)** (in a separate repository)
  is the skeleton of a classic maze chase game for Windows 10, written in C# and using Win2D.

These samples reference the NuGet package [Microsoft.Graphics.Win2D](http://www.nuget.org/packages/Microsoft.Graphics.Win2D), 
which will be automatically downloaded when you build the sample, so there is no need to 
separately install Win2D itself. Win2D [source code](http://github.com/Microsoft/Win2D) 
is also available for those who like to see how the sausage is made.

For more information about Win2D, see its [documentation](http://microsoft.github.io/Win2D).

---
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact
[opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
