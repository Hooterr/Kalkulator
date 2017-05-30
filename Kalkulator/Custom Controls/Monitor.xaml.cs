using System;
using System.Windows;
using System.Windows.Controls;

namespace Kalkulator
{

    /// <summary>
    /// Custom monitor
    /// </summary>
    public partial class Monitor : UserControl
    {
        #region Construcotr

        /// <summary>
        /// Default constructor
        /// </summary>
        public Monitor()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties and Variables

        /// <summary>
        /// Dependency Property Text
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(Monitor), new FrameworkPropertyMetadata(String.Empty));

        /// <summary>
        /// Displays this text
        /// </summary>
        public String Text
        {
            get { return GetValue(TextProperty).ToString(); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Dependency Property Text Alignment
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment),
            typeof(Monitor), new PropertyMetadata(default(TextAlignment)) );

        /// <summary>
        /// Alignments the tekst
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment) GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        #endregion
    }
}
