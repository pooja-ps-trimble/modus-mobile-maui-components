using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMCard : ContentView
{
    #region Fields 
    private readonly TapGestureRecognizer _tapGestureRecognizer;
    private Shadow _shadow;
    private Border _border;
    private EventHandler _clicked;
    private const int _borderRadius = 2;
    #endregion

    #region Public properties
    public new Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }
    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    public event EventHandler Clicked
    {
        add { _clicked += value; }
        remove { _clicked -= value; }
    }
    public ICommand Command
    {
        get => (Command)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    #region Bindable Properties

    public new static readonly BindableProperty PaddingProperty =
       BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(TMCard), defaultValue: new Thickness(16), propertyChanged: OnPaddingPropertyChanged);

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TMCard), defaultValue: false, propertyChanged: SelectedChanged);

    public static readonly BindableProperty ClickedEventProperty =
          BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(TMCard));

    public static readonly BindableProperty CommandProperty =
           BindableProperty.Create(nameof(Command), typeof(Command), typeof(TMCard), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TMCard), null);

    #endregion
    #region Property Changes
    private static void SelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var card = (TMCard)bindable;
        if ((bool)newValue)
        {
            card._border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
            card._border.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueDark);
        }
        else
        {
            card._border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
            card._border.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
        }
    }
    private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable is TMCard tMCard)
        {
            tMCard._border.Padding = (Thickness)newValue;
        }
    }
    #endregion

    public TMCard()
    {        
        int radius = 15;
        Point offset = new Point(-1, 1);
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            radius = 3;
            offset = new Point(0, 2);
        }
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            radius = 3;
        }

        _shadow = new Shadow
        {
            Brush = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9),
            Radius = radius,
            Opacity = 0.6F,
            Offset = offset
        };
        _border = new Border
        {
            Padding = Padding,
            Shadow = _shadow,
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White),
            Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent),
            StrokeShape = new Rectangle
            {
                RadiusX = _borderRadius,
                RadiusY = _borderRadius
            },

        };
        GestureRecognizers.Add(_tapGestureRecognizer = new TapGestureRecognizer());
        _tapGestureRecognizer.NumberOfTapsRequired = 1;
        _tapGestureRecognizer.Tapped += OnTapped;
    }
    /// <summary>
    /// Triggered when tapping the card
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnTapped(object sender, EventArgs e)
    {
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, e);
        _border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray1);
        _border.Content.Opacity = 0.3;
        this.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            _border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
            _border.Content.Opacity = 1;
            return false;
        });
    }
    #region Overriden Methods
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (_border != null && Content is View view)
        {
            _border.Content = view;
            Content = _border;
        }
    }
    #endregion

}
