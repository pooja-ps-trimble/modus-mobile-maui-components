﻿using CoreGraphics;

using Trimble.Modus.Components.Overlay.Pages;

using UIKit;

namespace Trimble.Modus.Components.Platforms.iOS
{
    internal class PopupWindow : UIWindow
    {
        public PopupWindow(IntPtr handle) : base(handle)
        {
        }

        public PopupWindow()
        {

        }

        public PopupWindow(UIWindowScene uiWindowScene) : base(uiWindowScene)
        {

        }

        public override UIView HitTest(CGPoint point, UIEvent? uievent)
        {
            var platformHandler = (PopupPageRenderer?)RootViewController;
            var renderer = platformHandler?.Handler;
            var hitTestResult = base.HitTest(point, uievent);

            if (!(platformHandler?.Handler?.VirtualView is PopupPage formsElement))
                return hitTestResult;

            if (formsElement.InputTransparent)
                return null!;

            if ((formsElement.BackgroundInputTransparent || formsElement.CloseWhenBackgroundIsClicked ) && renderer?.PlatformView == hitTestResult)
            {
                formsElement.SendBackgroundClick();
                if (formsElement.BackgroundInputTransparent)
                {
                    return null!; //fires off other handlers? If hit test returns null, it seems that other elements will process the click instead
                }
            }
            return hitTestResult;
                
        }
    }
}
