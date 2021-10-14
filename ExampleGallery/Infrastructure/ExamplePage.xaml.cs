// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ExampleGallery
{
    public sealed partial class ExamplePage : Page
    {

        public ExamplePage()
        {
            this.InitializeComponent();         

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this.DataContext = new ExampleDefinition("An Example", null);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            var example = e.Parameter as ExampleDefinition;
            if (example != null)
            {
                this.DataContext = example;
                if (example.Control != null)
                {
                    var control = Activator.CreateInstance(example.Control);
                    this.exampleContent.Children.Add((UIElement)control);
                    DeveloperTools.ExampleControlCreated(example.Name, (UserControl)control);
                }                
            }
        }
    }
}
