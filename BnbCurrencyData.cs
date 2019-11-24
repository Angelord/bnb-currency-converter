using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace CurrencyConverter {
    
    internal class CurrencyInformation {

        private readonly float _toLevMultiplier;    // Multiplier for converting to levs
        private readonly float _fromLevMultiplier;  // Multiplier for converting from levs

        public CurrencyInformation(float unitsGold, float toLev, float fromLevLev) {
            _toLevMultiplier = toLev / unitsGold;
            _fromLevMultiplier = fromLevLev;
        }
        
        /// <summary> Converts the value in the currency to its value in levs </summary>
        public float ToLev(float valueCurrency) { return valueCurrency * _toLevMultiplier; }
        
        /// <summary> Converts from lev to the currency </summary>
        public float ToCurrency(float valueLevs) { return valueLevs * _fromLevMultiplier; }
    }

    internal class BnbCurrencyData {

        private const string BNB_URL =  @"https://www.bnb.bg/Statistics/StExternalSector/StExchangeRates/StERForeignCurrencies/index.htm";
        private const string CURRENCY_BGN = "BGN";
        
        private string[] _currencies;
        private readonly Dictionary<string, CurrencyInformation> _rates = new Dictionary<string, CurrencyInformation>();

        public string[] Currencies => _currencies;

        public float Convert(string from, string to, float value) {
            CurrencyInformation sourceCurrency = _rates[from];
            CurrencyInformation targetCurrency = _rates[to];

            float valueInLev = sourceCurrency.ToLev(value);
            return targetCurrency.ToCurrency(valueInLev);
        }

        public void Get() {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(GetHtml());

            List<HtmlNode> rows = document.DocumentNode.Descendants().Where(element => {
                return element.Name == "tr" 
                       && !element.HasClass("header_cells") 
                       && element.ChildNodes.Count == 11
                       && element.ChildNodes[3].InnerHtml != "XAU";    //Skip price of gold
            }).ToList();

            foreach (HtmlNode row in rows) {
                string currency = row.ChildNodes[3].InnerHtml;
                float unitsGold = float.Parse(row.ChildNodes[5].InnerHtml);
                float toLev = float.Parse(row.ChildNodes[7].InnerHtml);
                float fromLev = float.Parse(row.ChildNodes[9].InnerHtml);
                
                _rates.Add(currency, new CurrencyInformation(unitsGold, toLev, fromLev));
            }
            
            // Handle conversion to BGN, as it is not included in the table
            _rates.Add(CURRENCY_BGN, new CurrencyInformation(1.0f, 1.0f, 1.0f));

            _currencies = _rates.Keys.ToArray();
        }
        
        private string GetHtml() {
            string html;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BNB_URL);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream ?? throw new HtmlWebException("Failed to retrieve data from BNB!"))) {
                html = reader.ReadToEnd();
            }
            
            return html;
        }
    }
}