using AngleSharp.Html.Parser;

namespace SalaryCalculator
{
    public sealed class SampathExchangeRateProvider : ExchangeRateProvider<SampathExchangeRate>
    {
        private readonly TimeSpan CacheRefreshThreshold = new(0, 1, 0, 0);
        private const string BaseUrl = "https://www.sampath.lk/en/exchange-rates";

        private readonly HttpClient _httpClient;
        private (DateTimeOffset Timestamp, Dictionary<CurrencyCode, SampathExchangeRate> Data) _cache;

        public SampathExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public override CurrencyCode ValueCurrencyCode => CurrencyCode.LKR;
        public override bool IsSupportedBaseCurrency(CurrencyCode currencyCode)
        {
            switch (currencyCode)
            {
                case CurrencyCode.AUD:
                case CurrencyCode.CAD:
                case CurrencyCode.CNY:
                case CurrencyCode.DKK:
                case CurrencyCode.EUR:
                case CurrencyCode.HKD:
                case CurrencyCode.JPY:
                case CurrencyCode.NZD:
                case CurrencyCode.NOK:
                case CurrencyCode.SGD:
                case CurrencyCode.ZAR:
                case CurrencyCode.SEK:
                case CurrencyCode.CHF:
                case CurrencyCode.AED:
                case CurrencyCode.GBP:
                case CurrencyCode.USD:
                    return true;
                default:
                    return false;
            }
        }
        public override async Task<SampathExchangeRate> GetExchangeRate(CurrencyCode baseCurrencyCode, CancellationToken cancellationToken = default)
        {
            if (IsSupportedBaseCurrency(baseCurrencyCode))
            {
                await FetchData(cancellationToken);
                return _cache.Data[baseCurrencyCode];
            }
            else
            {
                throw new NotSupportedException($"Base Currency '{baseCurrencyCode}' is not supported by this provider.");
            }
        }

        public override async Task<List<SampathExchangeRate>> GetExchangeRates(CancellationToken cancellationToken = default)
        {
            await FetchData(cancellationToken);
            return _cache.Data.Values.ToList();
        }

        private async Task FetchData(CancellationToken cancellationToken = default)
        {
            if (_cache.Data != null && DateTimeOffset.UtcNow - _cache.Timestamp < CacheRefreshThreshold)
                return;

            var response = await _httpClient.GetAsync(BaseUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync(cancellationToken);
                var data = await ParseHtml(html, cancellationToken);
                _cache = (DateTimeOffset.UtcNow, data.ToDictionary(x => x.BaseCurrencyCode, y => y));
            }
            else
            {
                throw new Exception("Data fetch error.");
            }
        }

        private static async Task<List<SampathExchangeRate>> ParseHtml(string htmlData, CancellationToken cancellationToken = default)
        {
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(htmlData, cancellationToken);

            var exchRatesTable = document.GetElementsByClassName("exch-rates")?.First();

            if (exchRatesTable?.FirstElementChild is not AngleSharp.Html.Dom.IHtmlTableSectionElement table)
                throw new Exception("Data Error");

            DateTimeOffset now;
            if (exchRatesTable.ParentElement?.FirstElementChild is AngleSharp.Html.Dom.IHtmlParagraphElement paragraph)
            {
                var text = paragraph.TextContent.Replace("Daily Exchange rates as at ", string.Empty);
                var time = DateTime.ParseExact(text, "M/d/yyyy h:m:s tt", null, System.Globalization.DateTimeStyles.None);
                now = new DateTimeOffset(time, new TimeSpan(5, 30, 0)).ToUniversalTime();
            }
            else
            {
                now = DateTimeOffset.UtcNow;
            }

            table.RemoveRowAt(0);

            var rates = new List<SampathExchangeRate>();

            foreach (var row in table.Rows)
            {
                var rate = new SampathExchangeRate
                {
                    BaseCurrencyCode = GetCurrencyCode(row.Cells[0].TextContent),
                    Buying = decimal.Parse(row.Cells[1].TextContent),
                    ODBuying = decimal.Parse(row.Cells[2].TextContent),
                    Selling = decimal.Parse(row.Cells[3].TextContent),
                    AsOf = now
                };
                rates.Add(rate);
            }
            return rates;
        }

        private static CurrencyCode GetCurrencyCode(string currencyText)
        {
            switch (currencyText)
            {
                case "Australian Dollar":
                    {
                        return CurrencyCode.AUD;
                    }
                case "Canadian Dollar":
                    {
                        return CurrencyCode.CAD;
                    }
                case "Chinese Yuan":
                    {
                        return CurrencyCode.CNY;
                    }
                case "Danish Krone":
                    {
                        return CurrencyCode.DKK;
                    }
                case "Euro":
                    {
                        return CurrencyCode.EUR;
                    }
                case "Hong Kong Dollar":
                    {
                        return CurrencyCode.HKD;
                    }
                case "Japanese Yen":
                    {
                        return CurrencyCode.JPY;
                    }
                case "New Zealand Dollar":
                    {
                        return CurrencyCode.NZD;
                    }
                case "Norwegian Krone":
                    {
                        return CurrencyCode.NOK;
                    }
                case "Singapore Dollar":
                    {
                        return CurrencyCode.SGD;
                    }
                case "South African Rand":
                    {
                        return CurrencyCode.ZAR;
                    }
                case "Swedish Krona":
                    {
                        return CurrencyCode.SEK;
                    }
                case "Swiss Franc":
                    {
                        return CurrencyCode.CHF;
                    }
                case "U.A.E Dirham":
                    {
                        return CurrencyCode.AED;
                    }
                case "U.K. Pound":
                    {
                        return CurrencyCode.GBP;
                    }
                case "U.S. Dollar":
                    {
                        return CurrencyCode.USD;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyText), "No matching CurrencyCode");
            }
        }
    }

    public class SampathExchangeRate : IExchangeRate
    {
        public CurrencyCode BaseCurrencyCode { get; set; }
        public CurrencyCode ValueCurrencyCode => CurrencyCode.LKR;
        public string CurrencyName => ExchangeRateProvider<SampathExchangeRate>.CurrencyNames[BaseCurrencyCode];
        public decimal ODBuying { get; set; }
        public decimal Buying { get; set; }
        public decimal Selling { get; set; }
        public DateTimeOffset AsOf { get; set; }
    }
}
