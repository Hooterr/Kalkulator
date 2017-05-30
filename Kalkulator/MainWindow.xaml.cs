using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


//Author: Maksymilian Lach

namespace Kalkulator
{
    #region Enums

    /// <summary>
    /// Enum type describing program state
    /// </summary>
    enum Operation
    {
        /// <summary>
        /// Program's doing nothing
        /// </summary>
        none = 0,

        /// <summary>
        /// An error has occured
        /// </summary>
        error,

        /// <summary>
        /// Program's waiting for value to do addition
        /// </summary>
        addition,

        /// <summary>
        /// Program's waiting for value to do substraction
        /// </summary>
        substraction,

        /// <summary>
        /// Program's waiting for value to do multiplication
        /// </summary>
        multiplication,

        /// <summary>
        /// Program's waiting for value to do division
        /// </summary>
        division,

        /// <summary>
        /// Program's waiting for value to do squarerooting
        /// </summary>
        squareroot,

        /// <summary>
        /// Printing result
        /// </summary>
        result,
    }

    /// <summary>
    /// Enum type describing errors that have occured
    /// </summary>
    enum Error
    {
        /// <summary>
        /// No error
        /// </summary>
        none = 0,

        /// <summary>
        /// Division by 0 error
        /// </summary>
        divisionby0,

        /// <summary>
        /// Square rooting a negative number
        /// </summary>
        badroot
    }

    #endregion

    /// <summary>
    /// Main class
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // Add key codes to the private dictionary
            AddKeyCodes();

            // Show 0 in the result monitor
            txtDisplay.Text = "0";
        }

        #endregion

        #region Private variables

        /// <summary>
        /// Last error that has occured
        /// </summary>
        private Error m_eError = Error.none;

        /// <summary>
        /// Last operation selected
        /// </summary>
        private Operation m_eLastOperation = Operation.none;

        /// <summary>
        /// A dictionary containing key codes
        /// </summary>
        private Dictionary<string, string> KeyCodes = new Dictionary<string, string>();

        /// <summary>
        /// Stores the value user has saved
        /// </summary>
        private String strMemory = String.Empty;

        /// <summary>
        /// Stores if the next value is written by the user or automatically added by program
        /// </summary>
        private bool b_NextValueWritten = false;

        #region Getting Program Version

        private static System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

        private static System.Reflection.AssemblyName assemblyName = assembly.GetName();

        private static Version version = assemblyName.Version;

        /// <summary>
        /// Conatins program's version
        /// </summary>
        private String StrVersion = version.ToString();

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Gets called when a number button is clicked
        /// </summary>
        /// <param name="sender">Button that raised the event</param>
        /// <param name="e"></param>
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            // Stores the button tag
            string ButtonTag = string.Empty;

            // Cast sender to appropriate type
            Button oButton = (Button)sender;

            // Get the tag
            ButtonTag = oButton.Tag.ToString();

            // Call the method that does the logic and pass the button tag
            NumberButtonProceed(ButtonTag);
        }

        /// <summary>
        /// Gets called when a operation button is clicked
        /// </summary>
        /// <param name="sender">Button that raised the event</param>
        /// <param name="e"></param>
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {   
            // Stores the button tag
            string ButtonTag = string.Empty;

            try
            {
                // Cast sender to appropriate type
                Button oButton = (Button)sender;

                // Get the tag
                ButtonTag = oButton.Tag.ToString();
            }
            catch(InvalidCastException)
            {
                // If we get here it means that button we clicked is a custom button (e.g. square root button)
                // Cast sender to CustomButton
                var oButton = (CustomButton)sender;

                // And get the tag
                ButtonTag = oButton.Tag.ToString();
            }
            // Call the method that does the logic
            OperationButtonProceed(ButtonTag);
        }

        /// <summary>
        /// Gets called when any button on the keyboard is pressed down
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Arguments</param>
        private void Key_Down(object sender, KeyEventArgs e)
        {
            // Stores the button tag
            string Tag = string.Empty;

            // Try to find the tag in the dictionary...
            if (KeyCodes.TryGetValue(e.Key.ToString(), out Tag))
            {
                // If the index of the pressed key is less or equals in the dictionary than last number key...
                // We presume that in the dictionary we have number buttons and then function buttons
                if (KeyCodes.Keys.ToList().IndexOf(e.Key.ToString()) <= KeyCodes.Keys.ToList().IndexOf("NumPad9"))
                    // Call the method that does the number button logic
                    NumberButtonProceed(Tag);
                else
                    // Or call the method that does the operation button logic
                    OperationButtonProceed(Tag);
            }
            // If the tag is not in the dictionary it means that this button is irrelevant to us (e.g. shift key was pressed) 
            else
                return;
        }


        /// <summary>
        /// Gets called when the user wants to drag and move the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mouse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Gets called when the close button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //Close the entire appliaction
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Get called when the help button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new window, pass program version to the constructor
            HelpWindow subWindow = new HelpWindow(StrVersion);

            // Show the window
            subWindow.Show();
        }

#endregion

        #region Private Methods

        /// <summary>
        /// Adds key codes to the dictionary
        /// </summary>
        private void AddKeyCodes()
        {
            // Left value: windows key code
                                            // Right value: our button tag
                          
            KeyCodes.Add("D0", "0");
            KeyCodes.Add("D1", "1");
            KeyCodes.Add("D2", "2");
            KeyCodes.Add("D3", "3");
            KeyCodes.Add("D4", "4");
            KeyCodes.Add("D5", "5");
            KeyCodes.Add("D6", "6");
            KeyCodes.Add("D7", "7");
            KeyCodes.Add("D8", "8");
            KeyCodes.Add("D9", "9");
            KeyCodes.Add("NumPad0", "0");
            KeyCodes.Add("NumPad1", "1");
            KeyCodes.Add("NumPad2", "2");
            KeyCodes.Add("NumPad3", "3");
            KeyCodes.Add("NumPad4", "4");
            KeyCodes.Add("NumPad5", "5");
            KeyCodes.Add("NumPad6", "6");
            KeyCodes.Add("NumPad7", "7");
            KeyCodes.Add("NumPad8", "8");
            KeyCodes.Add("NumPad9", "9");
            KeyCodes.Add("Divide", "DivisionBtnTag");
            KeyCodes.Add("Subtract", "SubstractionBtnTag");
            KeyCodes.Add("Multiply", "MultiplicationBtnTag");
            KeyCodes.Add("Add", "AdditionBtnTag");
            KeyCodes.Add("Decimal", "CommaBtnTag");
            KeyCodes.Add("Return", "ResultBtnTag");
            KeyCodes.Add("Back", "EraseBtnTag");
            KeyCodes.Add("Delete", "CBtnTag");
        }

        #region Main logic functions

        /// <summary>
        /// Handles the number button logic
        /// </summary>
        /// <param name="ButtonTag">Button tag that raised the event</param>
        private void NumberButtonProceed(string ButtonTag)
        { 
            // If last operation was printing result or an error
            if (m_eLastOperation == Operation.result || m_eLastOperation == Operation.error)
            {
                // Clear the screen by printing the buttons tag
                txtDisplay.Text = ButtonTag;
                
                // Set the helper variable to true
                b_NextValueWritten = true;

                // Set the last operation selected to none
                m_eLastOperation = Operation.none;
            }
            else
            // Otherwise, it means that last operation was some kind of math operation
            {
                // If the display shows 0 it means that it's the initial value
                if (txtDisplay.Text == "0")
                {
                    // Clear the screen by setting it to the button tag
                    txtDisplay.Text = ButtonTag;

                    // Set the helper variable
                    b_NextValueWritten = true;
                }
                else
                {
                    // If display monitor and memory monitor are the same and next value is not written by the user...
                    if (txtDisplayMemory.Text == txtDisplay.Text && !b_NextValueWritten)
                    {
                        // We need to clear the screen by setting it to teh button tag
                        txtDisplay.Text = ButtonTag;
                        
                        // Set the helper variable
                        b_NextValueWritten = true;
                    }
                    else
                    // When next value is written by the user
                    {
                        // We add number to the screen
                        txtDisplay.Text += ButtonTag;

                        // And set the helper variable just in case
                        b_NextValueWritten = true;
                    }
                }              
            }
        }

        /// <summary>
        /// Handles the operation button logic
        /// </summary>
        /// <param name="ButtonTag">Button tag that raised the event</param>
        private void OperationButtonProceed(string ButtonTag)
        {
            // Brush converter used to change save button colour to show the user that the value is stored
            var converter = new System.Windows.Media.BrushConverter();

            // Depending on the button tag we do the logic
            switch (ButtonTag)
            {
                #region Memory Buttons

                // Memory add button
                case "MPBtnTag":

                    // If screen contains vaild number to save
                    if (m_eLastOperation != Operation.error)
                    {                        
                        // We dont want to save numbers like 000324.2300000 so we clear them
                        ClearUnnecessaryZeros();

                        // If there is nothing saved already or the value to be saved is not a 0...
                        if (double.Parse(txtDisplay.Text) != 0 && strMemory == String.Empty)
                        {
                            // We save the value
                            strMemory = txtDisplay.Text;

                            // And set the button background do some nice green colour
                            MrcButton.Background = (Brush)converter.ConvertFromString("#FF4EF03E");
                        }
                    }
                    break;
                
                // Clears the saved number
                case "MMBtnTag":

                    // If the memory is not empty or the last operation is not an error...
                    if (strMemory != String.Empty && m_eLastOperation != Operation.error)
                    {
                        // Clear the memory
                        strMemory = String.Empty;
                        
                        // And restore default button background colour
                        MrcButton.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
                    }
                    break;

                // Uses the stored value
                case "MrcBtnTag":

                    // If the memory is not empty and last operation is not an error
                    if (strMemory != String.Empty && m_eLastOperation != Operation.error)
                    {
                        // Use the stored value
                        txtDisplay.Text = strMemory;

                        // Set the helper variable
                        b_NextValueWritten = true;
                    }
                    break;

                #endregion

                #region Erasing Buttons
                // Erase Button 
                case "EraseBtnTag":
                    // If there is something to delete and last operation is not an error or not printed result
                    if (txtDisplay.Text != "0" && m_eLastOperation != Operation.error && m_eLastOperation != Operation.result)
                    {
                        // Remove the last character
                        txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1);

                        // We dont want to leave blank string on the screen
                        if (txtDisplay.Text.Length == 0)
                            // Print '0'
                            txtDisplay.Text = "0";
                    }
                    break;

                // C button clears the whole screen
                case "CBtnTag":
                    
                    // Regardless of the screen value we clear it
                    txtDisplay.Text = "0";

                    // If last operation was an error...
                    if (m_eLastOperation == Operation.error)
                        // Set the operation to none
                        m_eLastOperation = Operation.none;
                    break;
                
                // CE button resets the whole calculator
                case "CEBtnTag":

                    // Clear the display
                    txtDisplay.Text = "0";

                    // Clear the memory
                    txtDisplayMemory.Text = String.Empty;

                    // Clear last operation
                    txtDisplayOperation.Text = String.Empty;

                    // Clear the operation monitor
                    m_eLastOperation = Operation.none;
                    break;

                #endregion

                #region Other Function Buttons

                // Changes the numbers sign
                case "NegateBtnTag":

                    // If last operation was not an error...
                    if (m_eLastOperation != Operation.error)
                    {
                        // Toogle the bumbers sign
                        txtDisplay.Text = (double.Parse(txtDisplay.Text) * (-1)).ToString();

                        // Set the helper variable
                        b_NextValueWritten = true;
                    }
                    break;
                
                // Adds the dot
                case "CommaBtnTag":

                    // If the last operation was not an error and not printing result...
                    if (m_eLastOperation != Operation.error && m_eLastOperation != Operation.result)
                    {
                        // If the display dosnt already contain a dot...
                        if (!txtDisplay.Text.Contains(','))
                            // Add it
                            txtDisplay.Text += ",";
                    }
                    break;

                #endregion

                #region Result Button
                // Prints the result
                case "ResultBtnTag":

                    // If the last operation was none of the following: error, printing result, none
                    if (m_eLastOperation != Operation.error && m_eLastOperation != Operation.result && m_eLastOperation != Operation.none)
                    {
                        // Now we do the math
                        switch(m_eLastOperation)
                        {
                            // NOTE: there is no need check if we can parse the string because it is impossible for some dummy data 
                            //       to get there

                            // Addition 
                            case Operation.addition:
                                
                                // Parse string and do the math
                                txtDisplay.Text = (double.Parse(txtDisplay.Text) + double.Parse(txtDisplayMemory.Text)).ToString();
                                break;

                            // Substraction 
                            case Operation.substraction:

                                // Parse string and do the math
                                txtDisplay.Text = (double.Parse(txtDisplayMemory.Text) - double.Parse(txtDisplay.Text)).ToString();
                                break;
                            
                            // Multipliaction
                            case Operation.multiplication:

                                // Parse string and do the math
                                txtDisplay.Text = (double.Parse(txtDisplay.Text) * double.Parse(txtDisplayMemory.Text)).ToString();
                                break;

                            // Division
                            case Operation.division:
                                
                                // If the user wants us to divide by 0...
                                if (double.Parse(txtDisplay.Text) == 0)
                                {
                                    // Set the error type
                                    m_eError = Error.divisionby0;

                                    // Run the error handling method
                                    ErrorProceed();

                                    // Then leave
                                    return;
                                }
                                
                                // If we can do this division
                                txtDisplay.Text = (double.Parse(txtDisplayMemory.Text) / double.Parse(txtDisplay.Text)).ToString();
                                break;

                            // Square root
                            case Operation.squareroot:

                                // The number in the display
                                double number;

                                // Try to parse...
                                if (!double.TryParse(txtDisplay.Text, out number))
                                {
                                    // Set error and return
                                    m_eError = Error.badroot;
                                    ErrorProceed();
                                    return;
                                }

                                // If the result is an error
                                if (Math.Sqrt(number).ToString() == "NaN") // Don't know if it's a better way to chceck it
                                {
                                    // Set the error 
                                    m_eError = Error.badroot;

                                    // Run the error handling method
                                    ErrorProceed();
                                }
                                else
                                    // If the result is corret, print it
                                    txtDisplay.Text = Math.Sqrt(number).ToString();
                                break;
                        }
                        // Clear the memory screen
                        txtDisplayMemory.Text = String.Empty;

                        // Clear the operation screen
                        txtDisplayOperation.Text = String.Empty;

                        // Set the status to the printing result
                        m_eLastOperation = Operation.result;
                    }
                    break;

                #endregion

                #region Math Buttons

                // Addition button
                case "AdditionBtnTag":

                    // If the last operation was not an error and [next value written by the user or last operation 
                    // is not the same as the one we want to do now]...
                    if(m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.addition) )
                    {

                        // If [the last operation is not printing result and not printing result] and next value is written by the user...
                        if((m_eLastOperation != Operation.result && m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            // Calls itself to perform previous operation
                            OperationButtonProceed("ResultBtnTag");
                        }

                        // Unless we got no error in the previous operation
                        if (m_eLastOperation != Operation.error)
                        {
                            // Call the method that cleans the screen and sets program status
                            CleanUp(Operation.addition, "+");
                        }                        
                    }
                    break;
                
                // Substraction
                case "SubstractionBtnTag":

                    // If the last operation was not an error and [next value written by the user or last operation 
                    // is not the same as the one we want to do now]...
                    if (m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.substraction))
                    {

                        // If [the last operation is not printing result and not printing result] and next value is written by the user...
                        if ((m_eLastOperation != Operation.result || m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            // Calls itself to perform previous operation
                            OperationButtonProceed("ResultBtnTag");
                        }

                        // Unless we got no error in the previous operation
                        if (m_eLastOperation != Operation.error)
                        {

                            // Call the method that cleans the screen and sets program status
                            CleanUp(Operation.substraction, "-");
                        }
                    }
                    break;

                // Division
                case "DivisionBtnTag":

                    // If the last operation was not an error and [next value written by the user or last operation 
                    // is not the same as the one we want to do now]...
                    if (m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.division))
                    {

                        // If [the last operation is not printing result and not printing result] and next value is written by the user...
                        if ((m_eLastOperation != Operation.result || m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            // Calls itself to perform previous operation
                            OperationButtonProceed("ResultBtnTag");
                        }

                        // Unless we got no error in the previous operation
                        if (m_eLastOperation != Operation.error)
                        {
                            // Call the method that cleans the screen and sets program status
                            CleanUp(Operation.division, "/");
                        }
                    }
                    break;

                // Multiplication
                case "MultiplicationBtnTag":

                    // If the last operation was not an error and [next value written by the user or last operation 
                    // is not the same as the one we want to do now]...
                    if (m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.multiplication))
                    {

                        // If [the last operation is not printing result and not printing result] and next value is written by the user...
                        if ((m_eLastOperation != Operation.result || m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            // Calls itself to perform previous operation
                            OperationButtonProceed("ResultBtnTag");
                        }

                        // Unless we got no error in the previous operation
                        if (m_eLastOperation != Operation.error)
                        {
                            // Call the method that cleans the screen and sets program status
                            CleanUp(Operation.multiplication, "*");
                        }
                    }
                    break;

                // SquareRoot
                case "SquareRootBtnTag":

                    // If the last operation was not an error and [next value written by the user or last operation 
                    // is not the same as the one we want to do now]...
                    if (m_eLastOperation != Operation.error)
                    {
                        // If [the last operation is not printing result and not printing result] and next value is written by the user...
                        if (m_eLastOperation != Operation.result || m_eLastOperation != Operation.none)
                        {
                            // Calls itself to perform previous operation
                            OperationButtonProceed("ResultBtnTag");
                        }
                        
                        // Set the operation to square root...
                        m_eLastOperation = Operation.squareroot;

                        // And than calls itself again to print the result
                        OperationButtonProceed("ResultBtnTag");
                    }
                    break;

                // Progm should never get here, bu just in case
                default:
                    MessageBox.Show("Fatal Error", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

                #endregion
            }
        }

        #endregion 

        /// <summary>
        /// Cleans up and sets program status
        /// </summary>
        /// <param name="operation">Operation type</param>
        /// <param name="operationSign">Sign of the operation</param>
        private void CleanUp(Operation operation, string operationSign)
        {
            // Clear useless zeros before putting this value to the memory monitor
            ClearUnnecessaryZeros();

            // Set the memory monitor value
            txtDisplayMemory.Text = txtDisplay.Text;

            // Set the helper variable to false, because we leave the number in the main monitor
            b_NextValueWritten = false;
           
            // Set appropriate symbol to the operation monitor
            txtDisplayOperation.Text = operationSign;

            // Set the appropriate program status
            m_eLastOperation = operation;
        }

        /// <summary>
        /// Cleans the unnecessary stuff (e.g. 001.2300 -> 1.23)
        /// </summary>
        private void ClearUnnecessaryZeros()
        {
            txtDisplay.Text = double.Parse(txtDisplay.Text).ToString();
        }

        /// <summary>
        /// Handles all errors
        /// </summary>
        private void ErrorProceed()
        {
            switch (m_eError)
            {
                // Division by 0 error
                case Error.divisionby0:

                    // Show user the information
                    txtDisplay.Text = "Cannot divide by 0";
                    break;
                
                // Root from a negative number error
                case Error.badroot:

                    // Show user the information
                    txtDisplay.Text = "Result does not exist";
                    break;

                // Should never get here,
                default:
                    MessageBox.Show("Unknown error occurred!\nPlease contact developer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            // Clear the memory monitor
            txtDisplayMemory.Text = String.Empty;

            // Clear the operation monitor
            txtDisplayOperation.Text = String.Empty;

            // Set the last operation to error
            m_eLastOperation = Operation.error;

            // Set the last error to none
            m_eError = Error.none;
        }
        
        #endregion
    }
}