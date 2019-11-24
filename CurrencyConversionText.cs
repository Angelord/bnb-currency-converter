using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CurrencyConverter {
    internal class CurrencyConversionText : IEnumerable<CurrencyConversionText.MonetaryValue> {

        private string _text;
        private readonly List<MonetaryValue> _values = new List<MonetaryValue>();

        public string Text => _text;
        public IReadOnlyList<MonetaryValue> Values => _values;

        public CurrencyConversionText(string text, string[] currencies) {
            _text = text;
            
            MatchCollection matches = Regex.Matches(text, GenerateCurrencyRegex(currencies), RegexOptions.Multiline);

            foreach (Match match in matches) {
                _values.Add(new MonetaryValue(this, match.Value));
            }
        }
        
        private string GenerateCurrencyRegex(string[] currencies) {
            string currencyMatcher = String.Format("({0})", string.Join("|", currencies));
            return @"([+-]?)(?=\d|\.\d)\d*(\.\d*)?([Ee]([+-]?\d+))? " + currencyMatcher;
        }

        public IEnumerator<MonetaryValue> GetEnumerator() { return _values.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        
        public class MonetaryValue {

            private CurrencyConversionText _originalText;
            private string _text;
            private float _value;
            private string _currency;
            
            public string Text { get { return _text; } }
            public float Value { get { return _value; } }
            public string Currency { get { return _currency; } }

            public MonetaryValue(CurrencyConversionText originalText, string text) {
                this._originalText = originalText;
                _text = text;
                _value = float.Parse(text.Substring(0, text.IndexOf(" ", StringComparison.Ordinal)));
                _currency = text.Substring(text.IndexOf(" ", StringComparison.Ordinal) + 1);
            }

            public void Set(float value, string currency) {
                _value = value;
                _currency = currency;
                
                string newText = $"{value} {currency}";
                _originalText._text = _originalText._text.Replace(_text, newText);
                _text = newText;
            }
        }
    }
}