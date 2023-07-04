namespace Falsus.Providers.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="CurrencyModel"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class CurrencyProvider : DataGeneratorProvider<CurrencyModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// A dictionary of <see cref="CurrencyModel"/> instances indexed by
        /// the <see cref="CurrencyModel.Id"/> property.
        /// </summary>
        private Dictionary<string, CurrencyModel> currencyById;

        /// <summary>
        /// A dictionary of <see cref="CurrencyModel"/> instances indexed by
        /// the <see cref="CurrencyModel.CountryAlpha2"/> property.
        /// </summary>
        private Dictionary<string, CurrencyModel[]> currenciesByCountry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyProvider"/> class.
        /// </summary>
        public CurrencyProvider()
            : base()
        {
            this.currenciesByCountry = new Dictionary<string, CurrencyModel[]>();
            this.currencyById = new Dictionary<string, CurrencyModel>();
        }

        /// <summary>
        /// Gets the definition of the input arguments supported by this provider.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the name of the
        /// argument and the expected type of the argument value.
        /// </returns>
        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>()
            {
                { CountryArgumentName, typeof(CountryModel) }
            };
        }

        /// <summary>
        /// Generates a random <see cref="CurrencyModel"/> instance
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CurrencyModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CurrencyModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="CurrencyModel"/>
        /// values is not supported.
        /// </exception>
        public override CurrencyModel GetRangedRowValue(CurrencyModel minValue, CurrencyModel maxValue, CurrencyModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CurrencyProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="CurrencyModel"/> instance
        /// based on the context and excluded ranges.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedRanges">
        /// An array of <see cref="WeightedRange{T}"/>s defining the ranges
        /// that cannot be returned by the provider.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CurrencyModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CurrencyModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="CurrencyModel"/>
        /// values is not supported.
        /// </exception>
        public override CurrencyModel GetRowValue(DataGeneratorContext context, WeightedRange<CurrencyModel>[] excludedRanges, CurrencyModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CurrencyProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets an instance of <see cref="CurrencyModel"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="CurrencyModel"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="CurrencyModel"/> instance is
        /// defined by the <see cref="CurrencyModel.Id"/> property.
        /// The unique id can be fetched by invoking the
        /// <see cref="CurrencyProvider.GetValueId(CurrencyModel)"/> method.
        /// </remarks>
        public override CurrencyModel GetRowValue(string id)
        {
            if (this.currencyById.ContainsKey(id))
            {
                return this.currencyById[id];
            }

            return null;
        }

        /// <summary>
        /// Generates a random <see cref="CurrencyModel"/> instance
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CurrencyModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CurrencyModel"/>.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the value for a <see cref="CurrencyModel"/> of the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        public override CurrencyModel GetRowValue(DataGeneratorContext context, CurrencyModel[] excludedObjects)
        {
            CurrencyModel[] models = Array.Empty<CurrencyModel>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            CurrencyModel value = null;

            int attempts = 0;
            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (countryModel == null)
                {
                    throw new InvalidOperationException($"{nameof(CurrencyProvider)} cannot generate another unique value for null country.");
                }

                if (!string.IsNullOrEmpty(countryModel.Alpha2) && this.currenciesByCountry.ContainsKey(countryModel.Alpha2))
                {
                    models = this.currenciesByCountry[countryModel.Alpha2].ToArray();
                    if (excludedObjects.Any())
                    {
                        models = models.Except(excludedObjects).ToArray();
                    }

                    if (!models.Any())
                    {
                        throw new InvalidOperationException($"{nameof(CurrencyProvider)} cannot generate another unique value.");
                    }

                    minRandomValue = 0;
                    maxRandomValue = models.Length;

                    value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                    while (excludedObjects.Contains(value) && attempts <= MaxAttempts)
                    {
                        value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                        if (attempts == MaxAttempts)
                        {
                            throw new InvalidOperationException($"{nameof(CurrencyProvider)} cannot generate another unique value.");
                        }

                        attempts++;
                    }

                    return value;
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(CurrencyProvider)} cannot generate another unique value for {countryModel.Alpha2}.");
                }
            }

            models = this.currenciesByCountry.SelectMany(u => u.Value).ToArray();
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(CurrencyProvider)} cannot generate another unique value.");
            }

            minRandomValue = 0;
            maxRandomValue = models.Length;

            value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            attempts = 0;
            while (excludedObjects.Contains(value) && attempts <= MaxAttempts)
            {
                value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(CurrencyProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="CurrencyModel"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="CurrencyModel"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="CurrencyModel"/>.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="CurrencyModel"/> instance is
        /// defined by the <see cref="CurrencyModel.Id"/> property.
        /// </remarks>
        public override string GetValueId(CurrencyModel value)
        {
            return value.Id;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{CurrencyModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<CurrencyModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            CurrencyModel[] models = ResourceReader.ReadContentsFromFile<CurrencyModel[]>("Falsus.Providers.Finance.Datasets.Currencies.json");

            this.currenciesByCountry = models.GroupBy(u => u.CountryAlpha2).ToDictionary(u => u.Key, x => x.ToArray());
            this.currencyById = models.GroupBy(u => u.Id).ToDictionary(k => k.Key, v => v.FirstOrDefault());
        }
    }
}
