namespace Falsus.Providers.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of country regions.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class RegionProvider : DataGeneratorProvider<RegionModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// A dictionary of <see cref="RegionModel"/> instances indexed by
        /// the <see cref="CountryModel.Alpha2"/> property.
        /// </summary>
        private static Dictionary<string, RegionModel[]> regionsByCountry;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionProvider"/> class.
        /// </summary>
        public RegionProvider()
            : base()
        {
            regionsByCountry = new Dictionary<string, RegionModel[]>();
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
        /// Generates a random region name
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="RegionModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="RegionModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged region names is not supported.
        /// </exception>
        public override RegionModel GetRangedRowValue(RegionModel minValue, RegionModel maxValue, RegionModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(RegionProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random region name
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
        /// An array of <see cref="RegionModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="RegionModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged region names is not supported.
        /// </exception>
        public override RegionModel GetRowValue(DataGeneratorContext context, WeightedRange<RegionModel>[] excludedRanges, RegionModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(RegionProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="RegionModel"/> instance based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="RegionModel"/> value with the specified unique identifier.
        /// </returns>
        public override RegionModel GetRowValue(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            string[] tokens = id.Split('|');
            if (tokens.Length == 2 && regionsByCountry.ContainsKey(tokens[0]))
            {
                return regionsByCountry[tokens[0]].FirstOrDefault(u => u.Code == tokens[1]);
            }

            return null;
        }

        /// <summary>
        /// Generates a random region name based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random region.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the name of a region belonging to the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        public override RegionModel GetRowValue(DataGeneratorContext context, RegionModel[] excludedObjects)
        {
            RegionModel[] models = Array.Empty<RegionModel>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            RegionModel value = null;

            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (countryModel == null)
                {
                    throw new InvalidOperationException($"{nameof(RegionProvider)} cannot generate another unique value for null country.");
                }

                if (!string.IsNullOrEmpty(countryModel.Alpha2) && regionsByCountry.ContainsKey(countryModel.Alpha2))
                {
                    models = regionsByCountry[countryModel.Alpha2];
                    if (excludedObjects.Any())
                    {
                        models = models.Except(excludedObjects).ToArray();
                    }

                    if (!models.Any())
                    {
                        throw new InvalidOperationException($"{nameof(RegionProvider)} cannot generate another unique value.");
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
                    throw new InvalidOperationException($"{nameof(RegionProvider)} cannot generate another unique value for {countryModel.Alpha2}.");
                }
            }

            models = regionsByCountry.SelectMany(u => u.Value).ToArray();
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(RegionProvider)} cannot generate another unique value.");
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
        /// Gets a unique identifier for the provided <see cref="RegionModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="RegionModel"/> instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(RegionModel value)
        {
            if (value == null)
            {
                return null;
            }

            return string.Concat(value.CountryAlpha2, "|", value.Code);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{RegionModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<RegionModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            RegionModel[] regionValues = ResourceReader.ReadContentsFromFile<RegionModel[]>("Falsus.Providers.Location.Datasets.Regions.json");
            regionsByCountry = regionValues.GroupBy(u => u.CountryAlpha2).ToDictionary(u => u.Key, x => x.ToArray());
        }
    }
}
