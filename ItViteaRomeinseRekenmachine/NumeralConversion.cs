﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItViteaRomeinseRekenmachine
{
    public class NumeralConversion : INotifyPropertyChanged
    {
        //Private variables.
        private List<string> _ErrorList;

        public NumeralConversion()
        {
            ErrorList = new List<string>();
        }

        //Error list to keep track of all the error messages to be displayed above.
        public List<string> ErrorList
        {
            get {return _ErrorList; }
            set
            {
                _ErrorList = value;
                OnPropertyChanged("ErrorList");
            }
        }

        public static Dictionary<char, int> RomanArabicDict =
        new Dictionary<char, int>
        {
                {'I',   1 },
                {'V',   5 },
                {'X',   10 },
                {'L',   50 },
                {'C',   100 },
                {'D',   500 },
                {'M',   1000 },
        };
        public static Dictionary<int, string> ArabicRomanDict =
            new Dictionary<int, string>
            {
                { 1000,  "M" },
                { 900,   "CM" },
                { 500,   "D" },
                { 400,   "CD" },
                { 100,   "C" },
                { 90,    "XC" },
                { 50,    "L" },
                { 40,    "XL" },
                { 10,    "X" },
                { 9,     "IX" },
                { 5,     "V" },
                { 4,     "IV" },
                { 1,     "I" },
            };

        #region Methods

        public string ArabicToRoman(int intArabic)
        {
            var romanStrBuild = new StringBuilder();
            string strResult = null;
            ErrorList.Clear();

            //Check if number does not excede maximum value or minimum value.
            if (intArabic > 3999)
            {
                ErrorList.Add("Number must be below 3999.");
            }
            else if (intArabic < 0)
            {
                ErrorList.Add("Number must be above 0.");
            }
            else
            {

                //Check if number isn't 0.
                if (intArabic == 0)
                    strResult = "N";
                else
                {
                    foreach (var item in ArabicRomanDict)
                    {
                        while (intArabic >= item.Key)
                        {
                            romanStrBuild.Append(item.Value);
                            intArabic -= item.Key;
                        }
                    }
                    strResult = romanStrBuild.ToString();
                }
            }

            return strResult;
        }

        public int RomanToArabic(string strRoman)
        {
            int RepCount = 1, intIndex = 0, maxDigitValue = 1000, intTotal = 0;
            char chrLast = 'Z';

            //There was no number for zero. Later on N was used to indicate nothing.
            if (strRoman == "N")
                return 0;

            // Rule 4 V,L, and D may only appear once in a roman number sequence.
            //Use split to split strRoman at the letters and count how many splits there are to check if the letter appears more than once.
            if (strRoman.Split('V').Length > 2 || strRoman.Split('L').Length > 2
                || strRoman.Split('D').Length > 2)
            {
                ErrorList.Add("V, L and D may each only be used once in a sequence.");
            }

        
            //Check rule 1. A single letter may be repeated up to 3 times.
            foreach (char numeral in strRoman)
            {
                if (numeral == chrLast)
                {
                    RepCount++;
                    if (RepCount == 4)
                    {
                        ErrorList.Add("A single letter may be repeated up to 3 times.");
                        break;
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
                int iDigit1 = RomanArabicDict[chrNum1];

                //Check for No further numeral or pair may match or exceed the subtracted value. 
                if (iDigit1 > maxDigitValue)
                {
                        ErrorList.Add("No further numeral or pair may match or exceed prior subtracted values.");
                }

                //Second Digit
                if (intIndex < strRoman.Length - 1)
                {
                    char chrNum2 = strRoman[intIndex + 1];
                    int iDigit2 = RomanArabicDict[chrNum2];
                    //Check if second number is greater to see if the first should be subtracted.
                    if (iDigit2 > iDigit1)
                    {
                        //Check if the number subtracted is I, X or C. 
                        if ("IXVC".IndexOf(chrNum1) == -1)
                        {
                            ErrorList.Add("Only I, X or C may be subtracted.");
                        }
                        //Check if the number subtracted is not smaller than a tenth of the number it is subtracted from.
                        if (iDigit2 > (iDigit1 * 10))
                        {
                            ErrorList.Add("The subtracted number must be no smaller than a tenth of the number it is subtracted from.");
                        }

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

        #region INotifyPropertyChanged Members  
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
