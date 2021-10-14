// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ExampleGallery
{
    public sealed partial class MainPage : Page
    {
        // We track the last visual state we set to avoid redundant GoToState calls
        // (these cause flickering on 8.1 apps running on Win10).
        private enum VisualState { Unknown, Big, Small, Tiny };
        private VisualState currentVisualState = VisualState.Unknown;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var example = (ExampleDefinition)e.ClickedItem;
            this.Frame.Navigate(typeof(ExamplePage), example);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;

            var newVisualState = currentVisualState;

            if (width > 1000)
                newVisualState = VisualState.Big;
            else if (width > 350)
                newVisualState = VisualState.Small;
            else
                newVisualState = VisualState.Tiny;

            if (currentVisualState != newVisualState)
            {
                VisualStateManager.GoToState(this, newVisualState.ToString(), false);
                currentVisualState = newVisualState;
            }            
        }
    }
}
