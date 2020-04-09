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
        private string RomanNum1, RomanNum2, strInputDisplay, strCalculation, strCalcDisplayArabic, RomanResult;
        private int ArabicNum1 = 0, ArabicNum2, ArabicResult;
        private NumeralConversion Conversion;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            Conversion = new NumeralConversion();
            ErrorList = new List<string>();
            ErrorList.Add("Test");
            LvError.ItemsSource = ErrorList;
        }
        public List<string> ErrorList { get; set; }

        /// <summary>
        /// Methods that aren't directly tied to any object handler, but instead serve to support those methods.
        /// </summary>
        #region Support Methods
        //Private Variables specific for support methods.
        private static char[] chrOperators = {'+', '-' ,'*', '/'};
        private Operators CurrentOperator;
        private enum Operators
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        private void GetNumbers()
        {
            //Get the position of the math operator and use that to get both Roman numbers.
            //If no operators are found assume only a number was typed in which would be RomanNum1.
            int intOprtPosition = strCalculation.IndexOfAny(chrOperators);
            if (intOprtPosition == -1)
            {
                RomanNum1 = strCalculation;
                ArabicNum1 = Conversion.RomanToArabic(RomanNum1);
                //InfoTxt.Text = Conversion.ErrorMsg;
                strCalcDisplayArabic = ArabicNum1.ToString();
            }
            else
            {
                RomanNum1 = strCalculation.Substring(0, intOprtPosition);
                RomanNum2 = strCalculation.Substring(intOprtPosition + 1);

                //Conversion from Roman to Arabic.
                ArabicNum1 = Conversion.RomanToArabic(RomanNum1);
                ArabicNum2 = Conversion.RomanToArabic(RomanNum2);
                //InfoTxt.Text = Conversion.ErrorMsg;
                strCalcDisplayArabic = String.Format("{0}{1}{2}",ArabicNum1, strInputDisplay.Substring(intOprtPosition, 1),ArabicNum2);
            }
           
        }

        private void UpdateArabicDisplay()
        {
            if (string.IsNullOrEmpty(strInputDisplay) == false)
            {
                GetNumbers();
                TopDisplayLabel2.Content = strCalcDisplayArabic;
                if (string.IsNullOrEmpty(RomanResult) == false)
                    BtmDisplayLabel2.Content = ArabicResult;
            }
        }

        private void ClearTop()
        {
            TopDisplayLabel.Content = null;
            TopDisplayLabel2.Content = null;
            strInputDisplay = null;
            strCalculation = null;
            strCalcDisplayArabic = null;
        }

        private void ClearAll()
        {
            TopDisplayLabel.Content = null;
            BtmDisplayLabel.Content = null;
            TopDisplayLabel2.Content = null;
            BtmDisplayLabel2.Content = null;
            TopDisplayLabel2.Opacity = 0.0;
            BtmDisplayLabel2.Opacity = 0.0;
            IsArabicVisible = false;
            RomanNum1 = null;
            RomanNum2 = null;
            ArabicNum1 = 0;
            ArabicNum2 = 0;
            strInputDisplay = null;
            strCalculation = null;
            strCalcDisplayArabic = null;
            RomanResult = null;
            ArabicResult = 0;
            IsResultCalculated = false;
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
            TopDisplayLabel.Content = strInputDisplay;
        }
        private void AddToCalcAndDisplay(string strCalc, string strDisplay)
        {
            strCalculation += strCalc;
            strInputDisplay += strDisplay;
            TopDisplayLabel.Content = strInputDisplay;
        }

        private void UpdateErrorList()
        {
            foreach (string error in Conversion.ErrorList)
            {
                ErrorList.Add(error);
            }
        }
    
        #endregion

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
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            ClearTop();
        }

        private void Button_Numbers(object sender, RoutedEventArgs e)
        {
            Button sendButton = e.Source as Button;
            if (IsResultCalculated)
            {
                ClearTop();
                IsResultCalculated = false;
            }
            string BtnContent = sendButton.Content.ToString();
            AddToCalcAndDisplay(BtnContent);
        }

        private void Button_Operators(object sender, RoutedEventArgs e)
        {
            Button sendButton = e.Source as Button;
            string BtnContent = sendButton.Content.ToString();
            string BtnName = sendButton.Name.ToString();
            string strForCalc = "";
            if (IsResultCalculated)
            {
                ClearTop();
                AddToCalcAndDisplay(RomanResult);
                IsResultCalculated = false;
            }

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

            AddToCalcAndDisplay(strForCalc, BtnContent);
        }

        private void Button_Undo(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(strCalculation) == false && string.IsNullOrEmpty(strInputDisplay) == false)
            {
                int TempLDisplay = (strInputDisplay.Length);
                strInputDisplay = strInputDisplay.Remove(TempLDisplay - 1);
                strCalculation = strCalculation.Remove(TempLDisplay - 1);
                TopDisplayLabel.Content = strInputDisplay;
            }
        }
        //Bool variables for these methods to keep track of Arabic Displays being visible and if a calculation was done.
        private bool IsArabicVisible = false, IsResultCalculated = false;
        private void Button_RomanArabic(object sender, RoutedEventArgs e)
        {
            UpdateArabicDisplay();
            if (IsArabicVisible)
            {
                TopDisplayLabel2.Opacity = 0.0;
                BtmDisplayLabel2.Opacity = 0.0;
                IsArabicVisible = false;
            }
            else
            {
                TopDisplayLabel2.Opacity = 1.0;
                BtmDisplayLabel2.Opacity = 1.0;
                IsArabicVisible = true;
            }
            UpdateErrorList();
        }

        private void Button_Result(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(strCalculation) == false)
            {
                GetNumbers();
                Calculation();
                UpdateArabicDisplay();
                BtmDisplayLabel.Content = RomanResult;
                IsResultCalculated = true;
            }
        }
        #endregion
    }
}
