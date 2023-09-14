﻿using DemoApp.ViewModels;

namespace DemoApp.Views.TabViewItems;

public partial class TabViewThree : ContentView
{
    public TabViewThree()
    {
        InitializeComponent();
        BindingContext = new ButtonSamplePageViewModel();
    }

    private void TMButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ButtonSamplePage());
    }
}

