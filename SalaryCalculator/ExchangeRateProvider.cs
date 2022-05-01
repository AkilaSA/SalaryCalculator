using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    public interface IExchangeRateProvider
    {
        CurrencyCode ValueCurrencyCode { get; }
        Task<List<IExchangeRate>> GetExchangeRates(CancellationToken cancellationToken = default);
        Task<IExchangeRate> GetExchangeRate(CurrencyCode baseCurrencyCode, CancellationToken cancellationToken = default);
        bool IsSupportedBaseCurrency(CurrencyCode currencyCode);
    }
    public interface IExchangeRateProvider<T> where T : IExchangeRate
    {
        CurrencyCode ValueCurrencyCode { get; }
        Task<List<T>> GetExchangeRates(CancellationToken cancellationToken = default);
        Task<T> GetExchangeRate(CurrencyCode baseCurrencyCode, CancellationToken cancellationToken = default);
        bool IsSupportedBaseCurrency(CurrencyCode currencyCode);
    }

    public abstract partial class ExchangeRateProvider<T> : IExchangeRateProvider<T>, IExchangeRateProvider where T : IExchangeRate
    {
        public abstract CurrencyCode ValueCurrencyCode { get; }
        public abstract Task<T> GetExchangeRate(CurrencyCode baseCurrencyCode, CancellationToken cancellationToken = default);
        public abstract Task<List<T>> GetExchangeRates(CancellationToken cancellationToken = default);
        public abstract bool IsSupportedBaseCurrency(CurrencyCode currencyCode);

        async Task<IExchangeRate> IExchangeRateProvider.GetExchangeRate(CurrencyCode baseCurrencyCode, CancellationToken cancellationToken)
        {
            return await GetExchangeRate(baseCurrencyCode, cancellationToken);
        }

        async Task<List<IExchangeRate>> IExchangeRateProvider.GetExchangeRates(CancellationToken cancellationToken)
        {
            return (await GetExchangeRates(cancellationToken)).Select(x => x as IExchangeRate).ToList();
        }
    }

    public interface IExchangeRate
    {
        public CurrencyCode BaseCurrencyCode { get; }
        public CurrencyCode ValueCurrencyCode { get; }
        //public decimal ODBuying { get; }
        public decimal Buying { get; }
        public decimal Selling { get; }
        public DateTimeOffset AsOf { get; }
    }
}
