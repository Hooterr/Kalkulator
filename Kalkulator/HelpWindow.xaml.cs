using System;
using System.Windows;


namespace Kalkulator
{
    /// <summary>
    /// Help Window
    /// </summary>
    public partial class HelpWindow : Window
    {
        #region Construcotr

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="StrVersion">Program's version to be displayed in the window</param>
        public HelpWindow(String StrVersion)
        {
            InitializeComponent();

            // Display the version
            txtField1.Text = "Wersja: " + StrVersion;
        }

        #endregion
    }
}
