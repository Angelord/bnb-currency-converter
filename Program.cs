using System;
using System.IO;
using System.Linq;

namespace CurrencyConverter {
    internal static class Program {
        
        private const string INPUT_FILE_NAME = "input.txt";
        private const string OUTPUT_FILE_NAME = "output.txt";
        
        public static void Main(string[] args) {
            
            BnbCurrencyData bnbData = new BnbCurrencyData();
            bnbData.Get();

            if(!File.Exists(INPUT_FILE_NAME)) {
                Console.WriteLine($"Failed to find '{INPUT_FILE_NAME}' file in the execution path. Aborting.");
                return;
            }
            
            string inputText = File.ReadAllText(INPUT_FILE_NAME);
            CurrencyConversionText conversionText = new CurrencyConversionText(inputText, bnbData.Currencies);

            string outputCurrency = ReadCurrencyParameter(bnbData.Currencies);
            
            foreach (CurrencyConversionText.MonetaryValue monetaryValue in conversionText) {
                decimal newValue = bnbData.Convert(monetaryValue.Currency, outputCurrency, monetaryValue.Value);
                
                monetaryValue.Set(newValue, outputCurrency);
            }

            File.WriteAllText(OUTPUT_FILE_NAME, conversionText.Text);
            
            Console.WriteLine("Result has been saved successfully.");
        }
        
        private static string ReadCurrencyParameter(string[] currencies) {
            string joinedCurrencies = String.Join(", ", currencies);
            Console.WriteLine($"Available Currencies : {joinedCurrencies}");
            Console.WriteLine("Specify the output currency : ");
            
            string input = Console.ReadLine()?.Trim().ToUpper();
            while (!currencies.Contains(input)) {
                Console.WriteLine($"Invalid input! The valid currencies are : {joinedCurrencies}");
                input = Console.ReadLine()?.Trim().ToUpper();
            }

            return input;
        }
    }
}