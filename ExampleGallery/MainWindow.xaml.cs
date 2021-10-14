using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExampleGallery
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // filling the Navigation Menu with the samples
            var exampleDefinitions = ExampleDefinitions.Definitions;
            foreach (ExampleDefinition ed in exampleDefinitions)
            {
                NavigationViewItem navItem = new NavigationViewItem();
                navItem.Content = ed.Name;
                ExampleMenu.MenuItems.Add(navItem);
            }

            ContentFrame.Navigate(typeof(MainPage));
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if ((string) args.InvokedItem == "Home")
            {
                ContentFrame.Navigate(typeof(MainPage));
            }
            else
            {
                var invokedExample = ExampleDefinitions.Definitions.First(x => (string)x.Name == (string)args.InvokedItem);
                ContentFrame.Navigate(typeof(ExamplePage), invokedExample);
            }
        }
    }
}
