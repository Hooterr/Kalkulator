using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kalkulator
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
	public partial class CustomButton : UserControl
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomButton()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }

        #endregion

        #region Public Properties and Variables

        /// <summary>
        /// Image Source Property
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CustomButton), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Get and sets Image source to the button
        /// </summary>
        public ImageSource ImageSource
        {
            get { return GetValue(ImageSourceProperty) as ImageSource; }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Click event
        /// </summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomButton));

        /// <summary>
        /// Gets or sets click event to the button
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        #endregion
    }
}
