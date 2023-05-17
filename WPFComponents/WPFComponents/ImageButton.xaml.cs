using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFComponents
{
    /// <summary>
    /// A button that displays an icon instead of text.
    /// <br/>
    /// The <see cref="ImageButton"/> can be enabled and disabled. When enabled the <see cref="EnabledIconSource"/> will be used. The <see cref="DisabledIconSource"/> will be used otherwise.
    /// <br/>
    /// Also a <see cref="Tooltip"/> and the <see cref="Size"/> of the <see cref="ImageButton"/> can be configured.
    /// </summary>
    public partial class ImageButton : UserControl
    {
        /// <summary>
        /// Event that will be triggered, whenever the enabled <see cref="ImageButton"/> is clicked.
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// Tooltip that will be shown when hovering above the <see cref="ImageButton"/>.
        /// </summary>
        public object Tooltip { get => btn.ToolTip; set => btn.ToolTip = value; }
        /// <summary>
        /// Size of the button.
        /// <br/>
        /// Default: <c>48</c>
        /// </summary>
        public double Size { get; set; } = 48;
        /// <summary>
        /// Image that will be used if the <see cref="ImageButton"/> is enabled.
        /// </summary>
        public ImageSource EnabledIconSource { get; set; }
        /// <summary>
        /// Image that will be used if the <see cref="ImageButton"/> is disabled.
        /// </summary>
        public ImageSource DisabledIconSource { get; set; } = null;

        public ImageButton()
        {
            InitializeComponent();
            DataContext = this;
            IsEnabledChanged += ImageButton_IsEnabledChanged;
            RefreshGUI();
        }
        private void RefreshGUI()
        {
            btn.Visibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
            disabledIcon.Visibility = IsEnabled ? Visibility.Collapsed : Visibility.Visible;
            if (!IsEnabled && DisabledIconSource == null) DisabledIconSource = EnabledIconSource;
        }

        private void ImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) => RefreshGUI();
        private void btn_Click(object sender, RoutedEventArgs e) => Click?.Invoke(this, e);
    }
}
