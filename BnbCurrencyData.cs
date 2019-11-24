using System;
using System.IO;
using System.Net;

namespace CurrencyConverter {
    internal class BnbCurrencyData {

        public string[] Currencies {
            get { return new string[] {"BGN", "YEN"}; }
        }

        public float Convert(string from, string to, float value) {
            return value * 2.0f;
        }

        private string GetHtml() {
            string html = string.Empty;
            string url = @"https://www.bnb.bg/Statistics/StExternalSector/StExchangeRates/StERForeignCurrencies/index.htm?downloadOper=&group1=first&firstDays=20&firstMonths=11&firstYear=2019&search=true&showChart=false&showChartButton=false";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
            return html;
        }
    }
}