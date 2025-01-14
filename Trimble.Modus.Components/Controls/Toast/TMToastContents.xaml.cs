using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : PopupPage

{
    #region Private Properties

    private const int DELAYTIME = 5000;

    #endregion

    #region Public Properties

    public ImageSource LeftIconSource { get; set; }

    public string Message { get; set; }

    public string RightIconText { get; set; }

    public double ToastWidthRequest { get; set; }

    public Color ToastBackground { get; set; }

    public Color TextColor { get; set; }

    #endregion

    internal TMToastContents(string message, string actionButtonText, ToastTheme theme, Action action, bool isDismissable)
    {
        InitializeComponent();
        SetTheme(theme.ToString());
        PopupData(message, actionButtonText, action, isDismissable);
        BindingContext = this;
        CloseAfterDelay();
    }

    #region Private Methods
    private void SetTheme(String toastTheme)
    {
        ToastTheme theme = (ToastTheme)Enum.Parse(typeof(ToastTheme), toastTheme);
        switch (theme)
        {
            case ToastTheme.Dark:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                LeftIconSource = ImageSource.FromFile(ImageConstants.ToastDarkThemeIcon);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastWhiteCloseIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;

            case ToastTheme.Primary:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlueCloseIcon);
                LeftIconSource = ImageSource.FromFile(ImageConstants.BlueInfoIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                break;

            case ToastTheme.Secondary:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray1);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlackCloseIcon);
                LeftIconSource = ImageSource.FromFile(ImageConstants.SolidHelpIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;

            case ToastTheme.Danger:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.RedPale);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlackCloseIcon);
                LeftIconSource = ImageSource.FromFile(ImageConstants.ToastDangerIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;

            case ToastTheme.Warning:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.YellowPale);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlackCloseIcon);
                LeftIconSource = ImageSource.FromFile(ImageConstants.WarningIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;

            case ToastTheme.Success:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.GreenPale);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlackCloseIcon);
                LeftIconSource = ImageSource.FromFile(ImageConstants.ValidIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;
                
            default:
                ToastBackground = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlackCloseIcon);
                LeftIconSource = ImageSource.FromFile(ImageConstants.GreyInfoIcon);
                TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;
        }

    }
    private void CloseButtonClicked(object sender, EventArgs e)

    {
        PopupService.Instance.RemovePageAsync(this, true);

    }
    public void CloseAfterDelay()
    {
        Task.Run(async () =>
        {
            await Task.Delay(DELAYTIME);
            await PopupService.Instance.RemovePageAsync(this, true);
        });
    }

    private void PopupData(string message, string actionButtonText, Action action, bool isDismissable)
    {
        RightIconText = actionButtonText;
        actionButton.Text = RightIconText;
        actionButton.TextColor = TextColor;
        actionButton.BackgroundColor = ToastBackground;

        if (string.IsNullOrEmpty(RightIconText))
        {
            closeButton.IsVisible = isDismissable;
            actionButton.IsVisible = false;
            if (isDismissable)
            {
                closeButton.Clicked += (sender, args) =>
                {
                    action?.Invoke();
                };
            }
        }
        else
        {
            closeButton.IsVisible = false;
            actionButton.IsVisible = true;
            actionButton.Clicked += (sender, args) =>
            {
                action?.Invoke();
            };
        }

        var idiom = DeviceInfo.Current.Idiom;
        setWidth(idiom);
        Message = message;
    }

    private void setWidth(DeviceIdiom idiom)
    {
        double minimumTabletWidth = 480;
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        if (idiom == DeviceIdiom.Phone)
        {
            toastLayout.Padding = new Thickness(16, 0, 16, 10);

        }
        else if (idiom == DeviceIdiom.Tablet)
        {
            toastLayout.Padding = new Thickness(0, 0, 0, 10);
            toastLayout.MinimumWidthRequest = minimumTabletWidth;
            toastLayout.MaximumWidthRequest = deviceWidth * maximumTabletWidthPercentage;
            toastLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;

        }
    }
    #endregion
}
