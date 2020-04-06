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
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Conversion methods
        public static Dictionary<char, int> RomanNumbersDict =
        new Dictionary<char, int>
        {
                {'I',       1 },
                {'V',       5 },
                {'X',       10 },
                {'L',       50 },
                {'C',       100 },
                {'D',       500 },
                {'M',       1000 },
        };

        public int RomanToArabic(string strRoman)
        {
            LabelInfo.Content = null;
            int RepCount = 1, intIndex = 0, maxDigitValue = 1000, intTotal = 0;
            char chrLast = 'Z';
            //Check rule 1
            foreach (char numeral in strRoman)
            {
                if (numeral == chrLast)
                {
                    RepCount++;
                    if (RepCount == 4)
                    {
                        LabelInfo.Content += "Invalid Number: A single letter may be repeated up to 3 times.";
                    }
                }
                else
                {
                    RepCount = 1;
                    chrLast = numeral;
                }
            }
            //While loop to iterate through Roman string.
            while (intIndex < strRoman.Length)
            {
                //First digit.
                char chrNum1 = strRoman[intIndex];
                int iDigit1 = RomanNumbersDict[chrNum1];

                //Check for Rule 3. No further numeral or pair may match or exceed the subtracted value. 
                if (iDigit1 > maxDigitValue)
                    LabelInfo.Content += "Invalid Number: Numbers to the right must be smaller than the numbers that came before them.";

                //Second Digit
                if (intIndex < strRoman.Length - 1)
                {
                    char chrNum2 = strRoman[intIndex + 1];
                    int iDigit2 = RomanNumbersDict[chrNum2];
                    //Check if second number is greater to see if the first should be subtracted.
                    if (iDigit2 > iDigit1)
                    {
                        //Check if the number subtracted is I, X or C. 
                        //And check if the number subtracted is not smaller than a tenth of the number it is subtracted from.
                        if ("IXV".IndexOf(chrNum1) == -1 || iDigit2 > (iDigit1 * 10))
                            LabelInfo.Content += "Invalid Number: Only I, X or C may be subtracted. The subtracted number must be no smaller than a tenth of the number it is subtracted from.";

                        //MaxDigit Value adjusted to check for rule 3. iDigit1 adjusted according to subtraction rules.
                        //IntIndex incremented one to skip next digit. (As to not count digit2 twice.)
                        maxDigitValue = iDigit1 - 1;
                        iDigit1 = iDigit2 - iDigit1;
                        intIndex++;
                    }
                }
                //Add digit value to the total result.
                intTotal += iDigit1;

                //Increment to next Digit.
                intIndex++;
            }

            return intTotal;
        }
        #endregion

        #region Button Methods
        private void Btn_Convert_Click(object sender, RoutedEventArgs e)
        {
            string strInput = TextBxInput.Text.ToUpper();
            TextBxOutput.Text = RomanToArabic(strInput).ToString();
        }
        #endregion
    }
}
