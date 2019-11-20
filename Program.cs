using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CurrencyConverter {
    
    
    internal class Program {
        
        private const string INPUT_FILE_NAME = "input.txt";
        private const string OUTPUT_FILE_NAME = "output.txt";
        
        /*
         * TODO
         * input/output currency validation
         * getting data from bank website
         */
        public static void Main(string[] args) {

            string inputText = File.ReadAllText(INPUT_FILE_NAME);

            string inputCurrency = ReadParameter("Specify the input currency : ");
            string outputCurrency = ReadParameter("And the output currency : ");

            MatchCollection numberMatches =
                Regex.Matches(inputText, @"([+-]?)(?=\d|\.\d)\d*(\.\d*)?([Ee]([+-]?\d+))?", RegexOptions.Multiline);

            string outputText = inputText;
            float conversionRate = GetConversionRate(inputCurrency, outputCurrency);
            foreach (Match numberMatch in numberMatches) {
                float oldValue = float.Parse(numberMatch.Value);
                string convertedValue = (oldValue * conversionRate).ToString();
                outputText = outputText.Replace(numberMatch.Value, convertedValue);
            }
            
            File.WriteAllText(OUTPUT_FILE_NAME, outputText);
        }

        private static string ReadParameter(string prompt) {
            Console.Write(prompt);
            return Console.ReadLine().Trim();
        }

        private static float GetConversionRate(string from, string to) {
            return 1.0f;
        }
    }
}