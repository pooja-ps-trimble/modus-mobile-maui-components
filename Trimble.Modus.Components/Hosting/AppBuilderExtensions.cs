﻿using Microsoft.Maui.LifecycleEvents;
using Trimble.Modus.Components.Handlers;
using Trimble.Modus.Components.Popup.Pages;

namespace Trimble.Modus.Components.Hosting;

/// <summary>
/// Extensions for MauiAppBuilder
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// Initializes the Trimble Modus Library
    /// </summary>
    /// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/> </param>
    /// <param name="options"><see cref="Options"/></param>
    /// <returns><see cref="MauiAppBuilder"/> initialized for <see cref="CommunityToolkit.Maui"/></returns>
    public static MauiAppBuilder UseTrimbleModus(this MauiAppBuilder builder)
    {
        builder
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(d =>
                {
                    d.OnBackPressed(activity => Droid.Implementation.AndroidPopups.SendBackPressed());
                });

#endif
            })
            .ConfigureMauiHandlers(handlers => SetHandlers(handlers));

        return builder;
    }

    /// <summary>
    /// Automatically sets up lifecycle events and maui handlers, with the additional option to have additional back press logic
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="backPressHandler"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseTrimbleModus(this MauiAppBuilder builder, Action? backPressHandler)
    {
        builder
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(d =>
                {

                    d.OnBackPressed(activity => Droid.Implementation.AndroidPopups.SendBackPressed(backPressHandler));
                });
#endif
            })
            .ConfigureMauiHandlers(handlers => SetHandlers(handlers));

        return builder;
    }

    private static void SetHandlers(IMauiHandlersCollection handlers)
    {
        {
            handlers.AddHandler(typeof(BorderlessEntry), typeof(EntryHandler));

            handlers.AddHandler(typeof(BorderlessEditor), typeof(EditorHandler));

            handlers.AddHandler(typeof(Label), typeof(LabelHandler));

#if ANDROID
            handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
#endif
#if IOS
                handlers.AddHandler(typeof(PopupPage), typeof(Platforms.iOS.PopupPageHandler));
#endif
#if WINDOWS
                handlers.AddHandler(typeof(PopupPage), typeof(Platforms.Windows.PopupPageHandler));
#endif
        }
    }
}
