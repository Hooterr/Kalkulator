using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

//autor: Maksymilian Lach

namespace Kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    enum Operation
    {
        none = 0,
        error,
        addition,
        substraction,
        multiplication,
        division,
        squareroot,
        result,
    }
    enum Error
    {
        none = 0,
        divisionby0,
        badroot
    }
    public partial class MainWindow : Window
    {

        public MainWindow() // construcors
        {
            InitializeComponent();
            AddKeyCodes();
            txtDisplay.Text = "0";
        }

        //Variables
        private Error m_eError = Error.none;
        private Operation m_eLastOperation = Operation.none;
        private Dictionary<string, string> KeyCodes = new Dictionary<string, string>();
        private String strMemory = String.Empty;
        private bool b_NextValueWritten = false;
        private static System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        private static System.Reflection.AssemblyName assemblyName = assembly.GetName();
        private static Version version = assemblyName.Version;
        private String StrVersion = version.ToString();

        //Events
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            string ButtonTag = string.Empty;
            Button oButton = (Button)sender;
            ButtonTag = oButton.Tag.ToString();
            NumberButtonProceed(ButtonTag);
            return;
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            string ButtonTag = string.Empty;
            try
            {
                Button oButton = (Button)sender;
                ButtonTag = oButton.Tag.ToString();
            }
            catch
            {
               var oButton = (CustomButton)sender;
                ButtonTag = oButton.Tag.ToString();
            }
            OperationButtonProceed(ButtonTag);
            return;
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            string Tag = string.Empty;
            if (KeyCodes.TryGetValue(e.Key.ToString(), out Tag))
            {
                if (KeyCodes.Keys.ToList().IndexOf(e.Key.ToString()) <= 19)
                    NumberButtonProceed(Tag);
                else
                    OperationButtonProceed(Tag);
            }      
        }

        //Methods
        private void AddKeyCodes()
        {
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

        private void NumberButtonProceed(string ButtonTag)
        { 
            if (m_eLastOperation == Operation.result || m_eLastOperation == Operation.error)
            {
                txtDisplay.Text = ButtonTag;
                b_NextValueWritten = true;
                m_eLastOperation = Operation.none;
            }
            else
            {
                if (txtDisplay.Text == "0")
                {
                    txtDisplay.Text = ButtonTag;
                    b_NextValueWritten = true;
                }
                else
                {
                    if (txtDisplayMemory.Text == txtDisplay.Text)
                    {
                        txtDisplay.Text = ButtonTag;
                        b_NextValueWritten = true;
                    }
                    else
                    {
                        txtDisplay.Text += ButtonTag;
                        b_NextValueWritten = true;
                    }
                }              
            }
        }

        private void OperationButtonProceed(string ButtonTag)
        {
            var converter = new System.Windows.Media.BrushConverter();
            switch (ButtonTag)
            {
                case "MPBtnTag":
                    if (m_eLastOperation != Operation.error)
                    {                        
                        ClearUnnecessaryZeros();
                        if (double.Parse(txtDisplay.Text) != 0 && strMemory == String.Empty)
                        {
                            strMemory = txtDisplay.Text;
                            MrcButton.Background = (Brush)converter.ConvertFromString("#FF4EF03E");
                        }
                    }
                    break;

                case "MMBtnTag":
                    if (strMemory != String.Empty && m_eLastOperation != Operation.error)
                    {
                        strMemory = String.Empty;
                        MrcButton.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
                    }
                    break;

                case "MrcBtnTag":
                    if (strMemory != String.Empty && m_eLastOperation != Operation.error)
                    {
                        txtDisplay.Text = strMemory;
                        b_NextValueWritten = true;
                    }
                    break;
                case "EraseBtnTag":
                    if (txtDisplay.Text != "0" && m_eLastOperation != Operation.error && m_eLastOperation != Operation.result)
                    {
                        txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1);
                        if (txtDisplay.Text.Length == 0)
                            txtDisplay.Text = "0";
                    }
                    break;

                case "CBtnTag":
                    txtDisplay.Text = "0";
                    if (m_eLastOperation == Operation.error)
                        m_eLastOperation = Operation.none;
                    break;

                case "CEBtnTag":
                    txtDisplay.Text = "0";
                    txtDisplayMemory.Text = String.Empty;
                    txtDisplayOperation.Text = String.Empty;
                    m_eLastOperation = Operation.none;
                    break;

                case "NegateBtnTag":
                    if (m_eLastOperation != Operation.error)
                    {
                         txtDisplay.Text = (double.Parse(txtDisplay.Text) * (-1)).ToString();
                        b_NextValueWritten = true;
                    }
                    break;

                case "CommaBtnTag":
                    if (m_eLastOperation != Operation.error && m_eLastOperation != Operation.result)
                    {
                        if (!txtDisplay.Text.Contains(','))
                            txtDisplay.Text += ",";
                    }
                    break;

                case "ResultBtnTag":
                    if (m_eLastOperation != Operation.error && m_eLastOperation != Operation.result && m_eLastOperation != Operation.none)
                    {
                        switch(m_eLastOperation)
                        {
                            case Operation.addition:
                                txtDisplay.Text = (double.Parse(txtDisplay.Text) + double.Parse(txtDisplayMemory.Text)).ToString();
                                break;

                            case Operation.substraction:
                                txtDisplay.Text = (double.Parse(txtDisplayMemory.Text) - double.Parse(txtDisplay.Text)).ToString();
                                break;

                            case Operation.multiplication:
                                txtDisplay.Text = (double.Parse(txtDisplay.Text) * double.Parse(txtDisplayMemory.Text)).ToString();
                                break;

                            case Operation.division:
                                if (double.Parse(txtDisplay.Text) == 0)
                                {
                                    m_eError = Error.divisionby0;
                                    ErrorProceed();
                                    return;
                                }
                                txtDisplay.Text = (double.Parse(txtDisplayMemory.Text) / double.Parse(txtDisplay.Text)).ToString();
                                break;

                            case Operation.squareroot:
                                if (double.Parse(txtDisplay.Text) < 0)
                                {
                                    m_eError = Error.badroot;
                                    ErrorProceed();
                                    return;
                                }
                                txtDisplay.Text = Math.Sqrt(double.Parse(txtDisplay.Text)).ToString();
                                break;
                        }
                        txtDisplayMemory.Text = String.Empty;
                        txtDisplayOperation.Text = String.Empty;
                        m_eLastOperation = Operation.result;
                    }
                    break;

                case "AdditionBtnTag":
                    if(m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.addition))
                    {
                        if((m_eLastOperation != Operation.result && m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            OperationButtonProceed("ResultBtnTag");
                        }
                        if (m_eLastOperation != Operation.error)
                        {
                            ClearUnnecessaryZeros();
                            txtDisplayMemory.Text = txtDisplay.Text;
                            txtDisplayOperation.Text = "+";
                            b_NextValueWritten = false;
                            m_eLastOperation = Operation.addition;
                        }                        
                    }
                    break;

                case "SubstractionBtnTag":
                    if (m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.substraction))
                    {
                        if ((m_eLastOperation != Operation.result || m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            OperationButtonProceed("ResultBtnTag");
                        }
                        if (m_eLastOperation != Operation.error)
                        {
                            ClearUnnecessaryZeros();
                            txtDisplayMemory.Text = txtDisplay.Text;
                            txtDisplayOperation.Text = "-";
                            b_NextValueWritten = false;
                            m_eLastOperation = Operation.substraction;
                        }
                    }
                    break;

                case "DivisionBtnTag":
                    if (m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.division))
                    {
                        if ((m_eLastOperation != Operation.result || m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            OperationButtonProceed("ResultBtnTag");
                        }
                        if (m_eLastOperation != Operation.error)
                        {
                            ClearUnnecessaryZeros();
                            txtDisplayMemory.Text = txtDisplay.Text;
                            txtDisplayOperation.Text = "/";
                            b_NextValueWritten = false;
                            m_eLastOperation = Operation.division;
                        }
                    }
                    break;
                case "MultiplicationBtnTag":
                    if (m_eLastOperation != Operation.error && (b_NextValueWritten || m_eLastOperation != Operation.multiplication))
                    {
                        if ((m_eLastOperation != Operation.result || m_eLastOperation != Operation.none) && b_NextValueWritten)
                        {
                            OperationButtonProceed("ResultBtnTag");
                        }
                        if (m_eLastOperation != Operation.error)
                        {
                            ClearUnnecessaryZeros();
                            txtDisplayMemory.Text = txtDisplay.Text;
                            txtDisplayOperation.Text = "*";
                            b_NextValueWritten = false; 
                            m_eLastOperation = Operation.multiplication;
                        }
                    }
                    break;

                case "SquareRootBtnTag":
                    if (m_eLastOperation != Operation.error)
                    {
                        if (m_eLastOperation != Operation.result || m_eLastOperation != Operation.none)
                        {
                            OperationButtonProceed("ResultBtnTag");
                        }
                        m_eLastOperation = Operation.squareroot;
                        OperationButtonProceed("ResultBtnTag");
                    }
                    break;
                default:
                    MessageBox.Show("Fatal Error", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

            }
        }

        private void ClearUnnecessaryZeros()
        {
            txtDisplay.Text = double.Parse(txtDisplay.Text).ToString();
        }

        private void ErrorProceed()
        {
            switch (m_eError)
            {
                case Error.divisionby0:
                    txtDisplay.Text = "Cannot divide by 0";

                    break;
                case Error.badroot:
                    txtDisplay.Text = "Result does not exist";
                    break;
                default:
                    MessageBox.Show("Unknow error occurred!\nPlease contact developer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            txtDisplayMemory.Text = String.Empty;
            txtDisplayOperation.Text = String.Empty;
            m_eLastOperation = Operation.error;
            m_eError = Error.none;
        }

        private void Mouse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow subWindow = new HelpWindow(StrVersion);
            subWindow.Show();
        }
    }
}