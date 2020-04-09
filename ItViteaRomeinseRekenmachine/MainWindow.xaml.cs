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
        private string RomanNum1 = null, RomanNum2, strInputDisplay, strCalculation, RomanResult;
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
        //Private Variables specific for support methods.
        private static char[] chrOperators = {'+', '-' ,'*', '/'};

        private void UpdateNumbers()
        {
            ArabicNum1 = Conversion.RomanToArabic(RomanNum1);
            ArabicNum2 = Conversion.RomanToArabic(RomanNum2);
        }
        private void GetNumbers()
        {
            //Get the position of the math operator and use that to get both Roman numbers.
            int intOprtPosition = strCalculation.IndexOfAny(chrOperators);
            RomanNum1 = strCalculation.Substring(0, intOprtPosition);
            RomanNum2 = strCalculation.Substring(intOprtPosition + 1);

            //Conversion from Roman to Arabic.
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
            strCalculation = null;
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

        private void AddToCalcAndDisplay(string str)
        {
            strCalculation += str;
            strInputDisplay += str;
            InputLabel.Content = strInputDisplay;
        }
        private void AddToCalcAndDisplay(string strCalc, string strDisplay)
        {
            strCalculation += strCalc;
            strInputDisplay += strDisplay;
            InputLabel.Content = strInputDisplay;
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

            /* if (IsSecondNumber)
                 RomanNum2 += BtnContent;
             else
                 RomanNum1 += BtnContent;*/

            AddToCalcAndDisplay(BtnContent);
        }

        private void Button_Operators(object sender, RoutedEventArgs e)
        {
            Button sendButton = e.Source as Button;
            string BtnContent = sendButton.Content.ToString();
            string BtnName = sendButton.Name.ToString();
            string strForCalc = "";

            //CurrentOperator = (Operators)Enum.Parse(typeof(Operators), sendButton.Name.ToString(), true);
            switch (BtnName)
            {
                case "Add":
                    CurrentOperator = Operators.Add;
                    strForCalc = "+";
                    break;
                case "Subtract":
                    CurrentOperator = Operators.Subtract;
                    strForCalc = "-";
                    break;
                case "Multiply":
                    CurrentOperator = Operators.Multiply;
                    strForCalc = "*";
                    break;
                case "Divide":
                    CurrentOperator = Operators.Divide;
                    strForCalc = "/";
                    break;
            }
            //IsSecondNumber = true;

            AddToCalcAndDisplay(strForCalc, BtnContent);
            //strInputDisplay += String.Format("{0}", BtnContent);
            //InputLabel.Content = strInputDisplay;
        }

        private void Button_Undo(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(strCalculation) == false && string.IsNullOrEmpty(strInputDisplay) == false)
            {
                int TempLDisplay = (strInputDisplay.Length);
                strInputDisplay = strInputDisplay.Remove(TempLDisplay - 1);
                strCalculation = strCalculation.Remove(TempLDisplay - 1);
                InputLabel.Content = strInputDisplay;
            }
        }

        private void Button_Result(object sender, RoutedEventArgs e)
        {
            GetNumbers();
            Calculation();
            OutputLabel.Content = RomanResult;
        }
        #endregion

    }
}
