using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SalaryCalculator
{
	public sealed class SampathExchangeRateProvider : ExchangeRateProvider<SampathExchangeRate>
	{
		private readonly TimeSpan CacheRefreshThreshold = new(0, 1, 0, 0);
		private const string BaseUrl = "https://www.sampath.lk/api/exchange-rates";

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

			response.EnsureSuccessStatusCode();
			if (response.Content is not null && response.Content.Headers.ContentType?.MediaType == MediaTypeNames.Application.Json)
			{
				var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
				var data = await JsonSerializer.DeserializeAsync<SampathRateResponse>(stream, cancellationToken: cancellationToken);
				if (data is not null && data.IsSuccess && data.ExchangeRates is not null)
				{
					_cache = (DateTimeOffset.UtcNow, data.ExchangeRates.ToDictionary(x => x.BaseCurrencyCode, y => y));
				}
				else
				{
					throw new Exception("No exchange rates returned");
				}
			}
			else
			{
				throw new Exception("HTTP Response was invalid and cannot be deserialised.");
			}

		}
	}

	public class SampathRateResponse
	{
		[JsonPropertyName("success")]
		public bool IsSuccess { get; set; }
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("data")]
		public List<SampathExchangeRate> ExchangeRates { get; set; }
	}


	public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//Debug.Assert(typeToConvert == typeof(DateTime));
			return DateTime.Parse(reader.GetString() ?? string.Empty);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
	public class StringToDecimalConverter : JsonConverter<decimal>
	{
		public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//Debug.Assert(typeToConvert == typeof(decimal));
			return decimal.Parse(reader.GetString() ?? string.Empty);
		}

		public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
	public class StringToIntConverter : JsonConverter<int>
	{
		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			//Debug.Assert(typeToConvert == typeof(int));
			return int.Parse(reader.GetString() ?? string.Empty);
		}

		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}


	public class SampathExchangeRate : IExchangeRate
	{
		[JsonPropertyName("CurrCode"), JsonConverter(typeof(JsonStringEnumConverter))]
		public CurrencyCode BaseCurrencyCode { get; set; }
		[JsonIgnore]
		public CurrencyCode ValueCurrencyCode => CurrencyCode.LKR;
		[JsonIgnore]
		public string CurrencyName => ExchangeRateProvider<SampathExchangeRate>.CurrencyNames[BaseCurrencyCode];
		[JsonPropertyName("ODBUY"), JsonConverter(typeof(StringToDecimalConverter))]
		public decimal ODBuying { get; set; }
		[JsonPropertyName("TTBUY"), JsonConverter(typeof(StringToDecimalConverter))]
		public decimal Buying { get; set; }
		[JsonPropertyName("TTSEL"), JsonConverter(typeof(StringToDecimalConverter))]
		public decimal Selling { get; set; }
		[JsonIgnore]
		public DateTimeOffset AsOf => new(RateWEF, TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time").BaseUtcOffset);

		[JsonConverter(typeof(DateTimeConverterUsingDateTimeParse))]
		public DateTime RateWEF { get; set; }
		[JsonPropertyName("SplRemarks")]
		public string? SpecialRemarks { get; set; }
		[JsonPropertyName("RateType"), JsonConverter(typeof(JsonStringEnumConverter))]
		public RateType Type { get; set; }
		[JsonConverter(typeof(StringToIntConverter))]
		public int Order { get; set; }
		public string? CurrName { get; set; }

		public enum RateType
		{
			EXRT,
			CURRT
		}
	}
}
