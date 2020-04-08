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

namespace ItViteaRomeinseRekenmachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private Variables
        private string RomanNum1 = null, RomanNum2, strInputDisplay, RomanResult;
        private int ArabicNum1 = 0, ArabicNum2, ArabicResult;
        private bool IsSecondNumber = false;
        private NumeralConversion Conversion;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            Conversion = new NumeralConversion();
        }

        /// <summary>
        /// Methods that aren't directly tied to any object handler, but instead serve to support those methods.
        /// </summary>
        #region Support Methods
        private void UpdateNumbers()
        {
            ArabicNum1 = Conversion.RomanToArabic(RomanNum1);
            ArabicNum2 = Conversion.RomanToArabic(RomanNum2);
        }

        private void ClearAll()
        {
            InputLabel.Content = null;
            OutputLabel.Content = null;
            RomanNum1 = null;
            RomanNum2 = null;
            ArabicNum1 = 0;
            ArabicNum2 = 0;
            strInputDisplay = null;
            IsSecondNumber = false;
        }

        private void Calculation()
        {
            switch (CurrentOperator)
            {
                case Operators.Add:
                    ArabicResult = ArabicNum1 + ArabicNum2;
                    break;
                case Operators.Subtract:
                    ArabicResult = ArabicNum1 - ArabicNum2;
                    break;
                case Operators.Multiply:
                    ArabicResult = ArabicNum1 * ArabicNum2;
                    break;
                case Operators.Divide:
                    ArabicResult = ArabicNum1 / ArabicNum2;
                    break;
                default:
                    ArabicResult = ArabicNum1;
                    break;
            }
            RomanResult = Conversion.ArabicToRoman(ArabicResult);
        }
        #endregion
        private Operators CurrentOperator;

        private enum Operators
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        #region Button Methods
        /*private void Btn_Convert_Click(object sender, RoutedEventArgs e)
        {
            string strInput = TextBxInput.Text.ToUpper();
            TextBxOutput.Text = RomanToArabic(strInput).ToString();
        }
        
        private void Btn_ConvertToRoman_Click(object sender, RoutedEventArgs e)
        {
            int intInput = Convert.ToInt32(TextBxOutput.Text);
            TextBxInput.Text = ArabicToRoman(intInput);
        }*/

        private void Button_ClearAll(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void Button_Numbers(object sender, RoutedEventArgs e)
        {
            Button sendButton = e.Source as Button;
            string BtnContent = sendButton.Content.ToString();

            if (IsSecondNumber)
                RomanNum2 += BtnContent;
            else
                RomanNum1 += BtnContent;
            
            strInputDisplay += BtnContent;
            InputLabel.Content = strInputDisplay;
        }

        private void Button_Operators(object sender, RoutedEventArgs e)
        {
            Button sendButton = e.Source as Button;
            string BtnContent = sendButton.Content.ToString();
            CurrentOperator = (Operators)Enum.Parse(typeof(Operators), sendButton.Name.ToString(), true);
            IsSecondNumber = true;

            strInputDisplay += String.Format("{0}", BtnContent);
            InputLabel.Content = strInputDisplay;
        }

        private void Button_Result(object sender, RoutedEventArgs e)
        {
            UpdateNumbers();
            Calculation();
            OutputLabel.Content = RomanResult;
        }
        #endregion

    }
}
