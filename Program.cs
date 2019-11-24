using System;
using System.IO;

namespace CurrencyConverter {
    internal static class Program {
        
        private const string INPUT_FILE_NAME = "input.txt";
        private const string OUTPUT_FILE_NAME = "output.txt";
        
        public static void Main(string[] args) {
            
            BnbCurrencyData bnbData = new BnbCurrencyData();

            string inputText = File.ReadAllText(INPUT_FILE_NAME);
            CurrencyConversionText conversionText = new CurrencyConversionText(inputText, bnbData.Currencies);
            
            string outputCurrency = ReadCurrencyParameter("Specify the output currency : ");
            
            foreach (CurrencyConversionText.MonetaryValue monetaryValue in conversionText) {
                if (monetaryValue.Currency == outputCurrency) {
                    continue;;
                }

                float newValue = bnbData.Convert(monetaryValue.Currency, outputCurrency, monetaryValue.Value);
                
                monetaryValue.Set(newValue, outputCurrency);
            }

            File.WriteAllText(OUTPUT_FILE_NAME, conversionText.Text);
            
            Console.WriteLine("Result has been saved successfully.");
        }
        
        private static string ReadCurrencyParameter(string prompt) {
            Console.Write(prompt);
            return Console.ReadLine().Trim().ToUpper();
        }
    }
}