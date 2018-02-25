using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool containsNegativeNumbers = false;
        string negativeNumbers = "";
        int maxLimit = 1000;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            string inputNumbers = inputTextBox.Text.Trim();

            negativeNumbers = "";
            containsNegativeNumbers = false;

            int calculateResult = Add(inputNumbers);

            if (!containsNegativeNumbers)
                outputTextBox.Text = calculateResult == -1 ? "Input was not in correct format" : calculateResult.ToString();
            else
                outputTextBox.Text = "Negatives are not allowed. Input contained the following negative numbers: " + negativeNumbers; 
        }

        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;

            List<string> delimiterCharacters = new List<string>();
            delimiterCharacters.Add(",");
            delimiterCharacters.Add("\n");

            if (numbers.Substring(0,1) == "/")
            {
                string delimiterCharactersString = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));

                while (delimiterCharactersString.Contains("[") && delimiterCharactersString.Contains("]"))
                {
                    if (delimiterCharactersString.Contains("["))
                    {
                        delimiterCharacters.Add(delimiterCharactersString.Substring(delimiterCharactersString.IndexOf("[") + 1, delimiterCharactersString.IndexOf("]") - delimiterCharactersString.IndexOf("[") -1));
                        delimiterCharactersString = delimiterCharactersString.Remove(0, delimiterCharactersString.IndexOf("]") - delimiterCharactersString.IndexOf("[") + 1);
                    }
                }

                if (delimiterCharactersString.Contains(Environment.NewLine))
                    delimiterCharactersString = delimiterCharactersString.Remove(delimiterCharactersString.IndexOf(Environment.NewLine));
                
                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(delimiterCharacters.ToArray<string>(), StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if(inputNumberValues[i].Contains("-"))
                {
                    negativeNumbers = (negativeNumbers.Length == 0) ? negativeNumbers = inputNumberValues[i] : negativeNumbers += ", " + inputNumberValues[i];
                    if (!containsNegativeNumbers)
                        containsNegativeNumbers = true;
                }

                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    if (returnValue > maxLimit)
                        return -1;

                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    if (returnValue > maxLimit)
                        continue;

                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }
    }
}


/*   String Calculator   
 1. Create a simple string calculator with a method int Add(string numbers)
    1. the method can take 0 , 1 or 2 numbers and will return their sum ( for an empty string it will return 0
    2. start with the simplest test case for an empty string and move to 1 and two numbers 
    3. remember to solve things as simply as possibly so that you force yourself to write tests you did not think about
    4. Remember to refactor after each passing test


     private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            if (numbers.IndexOf(',') > 0)
            {
                string[] inputNumberValues = new string[2];
                inputNumberValues[0] = numbers.Substring(0, numbers.IndexOf(','));
                inputNumberValues[1] = numbers.Substring(numbers.IndexOf(',') +1);

                int firstNumber = 0;
                int secondNumber = 0;

                int.TryParse(inputNumberValues[0], out firstNumber);
                int.TryParse(inputNumberValues[1], out secondNumber);

                if (firstNumber >= 0 && secondNumber >= 0)
                {
                    return firstNumber + secondNumber;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                int returnValue = -1;
                int.TryParse(numbers, out returnValue);
                return returnValue;
            }
        }

    2. Allow the Add method to handle an unknown amount of numbers
     
        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(new char[] { ',' });

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }
     
     3. Allow the Add method to handle new lines between numbers (instead of commas)
          1. the following input is ok:  “1\n2,3”  (will equal 6)
          2. the following input is NOT ok:  “1,\n” (not need to prove it - just clarifying)

        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(new char[] { ',', '\n' });

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }

    4. Support different delimiters
            1. to change a delimiter, the beginning of the string will contain a separate line that looks like this:   “//[delimiter]\n[numbers…]” for example “//;\n1;2” should return three where the default delimiter is ‘;’ .
            2. the first line is optional. all existing scenarios should still be supported

        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;
            string delimiterCharacter = ",";

            if (numbers.Substring(0,1) == "/")
            {
                delimiterCharacter = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));
                if (delimiterCharacter.Contains(Environment.NewLine))
                    delimiterCharacter = delimiterCharacter.Remove(delimiterCharacter.IndexOf(Environment.NewLine));

                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(new string[] { ",", "\n", delimiterCharacter}, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }

    5. Calling Add with a negative number will throw an exception “negatives not allowed” - and the negative that was passed.if there are multiple negatives, show all of them in the exception message

        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;
            string delimiterCharacter = ",";
           

            if (numbers.Substring(0,1) == "/")
            {
                delimiterCharacter = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));
                if (delimiterCharacter.Contains(Environment.NewLine))
                    delimiterCharacter = delimiterCharacter.Remove(delimiterCharacter.IndexOf(Environment.NewLine));

                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(new string[] { ",", "\n", delimiterCharacter}, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if(inputNumberValues[i].Contains("-"))
                {
                    negativeNumbers = (negativeNumbers.Length == 0) ? negativeNumbers = inputNumberValues[i] : negativeNumbers += ", " + inputNumberValues[i];
                    if (!containsNegativeNumbers)
                        containsNegativeNumbers = true;
                }

                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }

    6. Numbers bigger than 1000 should be ignored, so adding 2 + 1001  = 2

        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;
            string delimiterCharacter = ",";
           

            if (numbers.Substring(0,1) == "/")
            {
                delimiterCharacter = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));
                if (delimiterCharacter.Contains(Environment.NewLine))
                    delimiterCharacter = delimiterCharacter.Remove(delimiterCharacter.IndexOf(Environment.NewLine));

                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(new string[] { ",", "\n", delimiterCharacter}, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if(inputNumberValues[i].Contains("-"))
                {
                    negativeNumbers = (negativeNumbers.Length == 0) ? negativeNumbers = inputNumberValues[i] : negativeNumbers += ", " + inputNumberValues[i];
                    if (!containsNegativeNumbers)
                        containsNegativeNumbers = true;
                }

                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    if (returnValue > maxLimit)
                        return -1;

                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    if (returnValue > maxLimit)
                        continue;

                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }

    7. Delimiters can be of any length with the following format:  “//[delimiter]\n” for example: “//[***]\n1***2***3” should return 6
        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;
            string delimiterCharacter = ",";
           

            if (numbers.Substring(0,1) == "/")
            {
                delimiterCharacter = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));
                if (delimiterCharacter.Contains(Environment.NewLine))
                    delimiterCharacter = delimiterCharacter.Remove(delimiterCharacter.IndexOf(Environment.NewLine));

                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(new string[] { ",", "\n", delimiterCharacter}, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if(inputNumberValues[i].Contains("-"))
                {
                    negativeNumbers = (negativeNumbers.Length == 0) ? negativeNumbers = inputNumberValues[i] : negativeNumbers += ", " + inputNumberValues[i];
                    if (!containsNegativeNumbers)
                        containsNegativeNumbers = true;
                }

                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    if (returnValue > maxLimit)
                        return -1;

                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    if (returnValue > maxLimit)
                        continue;

                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }

    8. Allow multiple delimiters like this:  “//[delim1][delim2]\n” for example “//[*][%]\n1*2%3” should return 6.

        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;

            List<string> delimiterCharacters = new List<string>();
            delimiterCharacters.Add(",");
            delimiterCharacters.Add("\n");

            if (numbers.Substring(0,1) == "/")
            {
                string delimiterCharactersString = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));

                while (delimiterCharactersString.Contains("[") && delimiterCharactersString.Contains("]"))
                {
                    if (delimiterCharactersString.Contains("["))
                    {
                        delimiterCharacters.Add(delimiterCharactersString.Substring(delimiterCharactersString.IndexOf("[") + 1, delimiterCharactersString.IndexOf("]") - delimiterCharactersString.IndexOf("[") -1));
                        delimiterCharactersString = delimiterCharactersString.Remove(0, delimiterCharactersString.IndexOf("]") - delimiterCharactersString.IndexOf("[") + 1);
                    }
                }

                if (delimiterCharactersString.Contains(Environment.NewLine))
                    delimiterCharactersString = delimiterCharactersString.Remove(delimiterCharactersString.IndexOf(Environment.NewLine));
                
                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(delimiterCharacters.ToArray<string>(), StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if(inputNumberValues[i].Contains("-"))
                {
                    negativeNumbers = (negativeNumbers.Length == 0) ? negativeNumbers = inputNumberValues[i] : negativeNumbers += ", " + inputNumberValues[i];
                    if (!containsNegativeNumbers)
                        containsNegativeNumbers = true;
                }

                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    if (returnValue > maxLimit)
                        return -1;

                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    if (returnValue > maxLimit)
                        continue;

                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }
    
    9. make sure you can also handle multiple delimiters with length longer than one char
        
        private int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            int returnValue = -1;
            int calculatedAmount = 0;

            List<string> delimiterCharacters = new List<string>();
            delimiterCharacters.Add(",");
            delimiterCharacters.Add("\n");

            if (numbers.Substring(0,1) == "/")
            {
                string delimiterCharactersString = numbers.Substring(numbers.IndexOf('/') + 1, numbers.IndexOf('\n'));

                while (delimiterCharactersString.Contains("[") && delimiterCharactersString.Contains("]"))
                {
                    if (delimiterCharactersString.Contains("["))
                    {
                        delimiterCharacters.Add(delimiterCharactersString.Substring(delimiterCharactersString.IndexOf("[") + 1, delimiterCharactersString.IndexOf("]") - delimiterCharactersString.IndexOf("[") -1));
                        delimiterCharactersString = delimiterCharactersString.Remove(0, delimiterCharactersString.IndexOf("]") - delimiterCharactersString.IndexOf("[") + 1);
                    }
                }

                if (delimiterCharactersString.Contains(Environment.NewLine))
                    delimiterCharactersString = delimiterCharactersString.Remove(delimiterCharactersString.IndexOf(Environment.NewLine));
                
                numbers = numbers.Remove(0, numbers.IndexOf("\n"));
            }

            string[] inputNumberValues = new string[] { };
            inputNumberValues = numbers.Split(delimiterCharacters.ToArray<string>(), StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < inputNumberValues.Length; i++)
            {
                if(inputNumberValues[i].Contains("-"))
                {
                    negativeNumbers = (negativeNumbers.Length == 0) ? negativeNumbers = inputNumberValues[i] : negativeNumbers += ", " + inputNumberValues[i];
                    if (!containsNegativeNumbers)
                        containsNegativeNumbers = true;
                }

                if (inputNumberValues[i] == "\r" || inputNumberValues[i] == "\r\n")
                {
                    return -1;
                }

                if (inputNumberValues.Length == 1)
                {
                    int.TryParse(inputNumberValues[0], out returnValue);
                    if (returnValue > maxLimit)
                        return -1;

                    return returnValue;
                }
                else
                {
                    int.TryParse(inputNumberValues[i], out returnValue);
                    if (returnValue > maxLimit)
                        continue;

                    calculatedAmount += returnValue;
                }
            }

            return calculatedAmount;
        }
 */
