using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CurrencyConverter {
    
    
    internal class Program {
        
        private const string INPUT_FILE_NAME = "input.txt";
        private const string OUTPUT_FILE_NAME = "output.txt";
        
        public static void Main(string[] args) {

            string inputData = File.ReadAllText(INPUT_FILE_NAME);

//            string inputCurrency = ReadParameter("Specify the input currency : ");
//            string outputCurrency = ReadParameter("And the output currency : ");

            MatchCollection numberMatches =
                Regex.Matches(inputData, @"([+-]?)(?=\d|\.\d)\d*(\.\d*)?([Ee]([+-]?\d+))?", RegexOptions.Multiline);

            File.WriteAllText(OUTPUT_FILE_NAME, inputData);
        }

        private static string ReadParameter(string prompt) {
            Console.Write(prompt);
            return Console.ReadLine().Trim();
        }
    }
}