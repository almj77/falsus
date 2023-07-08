namespace Falsus.Providers.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of City names.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class CityProvider : DataGeneratorProvider<CityModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// A dictionary of <see cref="CityModel"/> instances indexed by
        /// the <see cref="CurrencyModel.CountryAlpha2"/> property.
        /// </summary>
        private static Dictionary<string, CityModel[]> citiesByCountry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CityProvider"/> class.
        /// </summary>
        public CityProvider()
            : base()
        {
            citiesByCountry = new Dictionary<string, CityModel[]>();
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
        /// Generates a random <see cref="CityModel"/> instance.
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CityModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CityModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged city names is not supported.
        /// </exception>
        public override CityModel GetRangedRowValue(CityModel minValue, CityModel maxValue, CityModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CityProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="CityModel"/> instance
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
        /// An array of <see cref="CityModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CityModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged city names is not supported.
        /// </exception>
        public override CityModel GetRowValue(DataGeneratorContext context, WeightedRange<CityModel>[] excludedRanges, CityModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CityProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="CityModel"/> instance based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="CityModel"/> instance that represents the city with the specified unique identifier.
        /// </returns>
        public override CityModel GetRowValue(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] tokens = id.Split('|');
                if (tokens != null && tokens.Length == 2 && citiesByCountry.ContainsKey(tokens[0]))
                {
                    return citiesByCountry[tokens[0]].FirstOrDefault(u => u.Name == tokens[1]);
                }
            }

            return default;
        }

        /// <summary>
        /// Generates a random <see cref="CityModel"/> instance based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CityModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random city name.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the name of a city belonging to the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        public override CityModel GetRowValue(DataGeneratorContext context, CityModel[] excludedObjects)
        {
            CityModel[] models = Array.Empty<CityModel>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            CityModel value = null;

            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (countryModel == null)
                {
                    throw new InvalidOperationException($"{nameof(CityProvider)} cannot generate another unique value for null country.");
                }

                if (!string.IsNullOrEmpty(countryModel.Alpha2) && citiesByCountry.ContainsKey(countryModel.Alpha2))
                {
                    models = citiesByCountry[countryModel.Alpha2].ToArray();
                    if (excludedObjects.Any())
                    {
                        models = models.Except(excludedObjects).ToArray();
                    }

                    if (!models.Any())
                    {
                        throw new InvalidOperationException($"{nameof(CityProvider)} cannot generate another unique value.");
                    }

                    minRandomValue = 0;
                    maxRandomValue = models.Length;

                    value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                    while (excludedObjects.Contains(value))
                    {
                        value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                    }

                    return value;
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(CityProvider)} cannot generate another unique value for {countryModel.Alpha2}.");
                }
            }

            models = citiesByCountry.SelectMany(u => u.Value).ToArray();
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(CityProvider)} cannot generate another unique value.");
            }

            minRandomValue = 0;
            maxRandomValue = models.Length;

            value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            while (excludedObjects.Contains(value))
            {
                value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="CityModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="CityModel"/> instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        public override string GetValueId(CityModel value)
        {
            return string.Concat(value.CountryAlpha2, "|", value.Name);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{CityModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<CityModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            CityModel[] models = ResourceReader.ReadContentsFromFile<CityModel[]>("Falsus.Providers.Location.Datasets.Cities.json");
            citiesByCountry = models.GroupBy(u => u.CountryAlpha2).ToDictionary(u => u.Key, x => x.ToArray());
        }
    }
}
