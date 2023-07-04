namespace Falsus.Providers.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="CoordinatesModel"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class CoordinatesProvider : DataGeneratorProvider<CoordinatesModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// A dictionary of <see cref="CoordinatesModel"/> instances indexed
        /// by the <see cref="CountryModel.Alpha2"/> property.
        /// </summary>
        private static Dictionary<string, List<CoordinatesModel>> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinatesProvider"/> class.
        /// </summary>
        public CoordinatesProvider()
            : base()
        {
            cache = new Dictionary<string, List<CoordinatesModel>>();
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
        /// Generates a random <see cref="CoordinatesModel"/> instance
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CoordinatesModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CoordinatesModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="CoordinatesModel"/>
        /// values is not supported.
        /// </exception>
        public override CoordinatesModel GetRangedRowValue(CoordinatesModel minValue, CoordinatesModel maxValue, CoordinatesModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CoordinatesProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="CoordinatesModel"/> instance
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
        /// An array of <see cref="CoordinatesModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CoordinatesModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="CoordinatesModel"/>
        /// values is not supported.
        /// </exception>
        public override CoordinatesModel GetRowValue(DataGeneratorContext context, WeightedRange<CoordinatesModel>[] excludedRanges, CoordinatesModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CoordinatesProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets an instance of <see cref="CoordinatesModel"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="CoordinatesModel"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="CoordinatesModel"/> instance is
        /// defined by a comma separated string concatenation of the
        /// <see cref="CoordinatesModel.Latitude"/> and <see cref="CoordinatesModel.Longitude"/> properties.
        /// The unique id can be fetched by invoking the
        /// <see cref="CoordinatesProvider.GetValueId(CoordinatesModel)"/> method.
        /// </remarks>
        public override CoordinatesModel GetRowValue(string id)
        {
            if (!string.IsNullOrEmpty(id) && id.Contains(','))
            {
                string[] parts = id.Split(',');
                return new CoordinatesModel(parts[0], parts[1]);
            }

            return null;
        }

        /// <summary>
        /// Generates a random <see cref="CoordinatesModel"/> instance
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="CoordinatesModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="CoordinatesModel"/>.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the value for a <see cref="CoordinatesModel"/> of the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        public override CoordinatesModel GetRowValue(DataGeneratorContext context, CoordinatesModel[] excludedObjects)
        {
            List<CoordinatesModel> coordinates = new List<CoordinatesModel>();

            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (countryModel == null || string.IsNullOrEmpty(countryModel.Alpha2))
                {
                    throw new InvalidOperationException($"{nameof(CoordinatesProvider)} cannot generate another unique value for null country.");
                }

                if (cache.ContainsKey(countryModel.Alpha2))
                {
                    coordinates = cache[countryModel.Alpha2];
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(CoordinatesProvider)} cannot generate another unique value for {countryModel.Alpha2} country.");
                }
            }
            else
            {
                coordinates = cache.SelectMany(u => u.Value).ToList();
            }

            if (excludedObjects.Any())
            {
                coordinates = coordinates.Where(u => !excludedObjects.Contains(new CoordinatesModel(u.Latitude, u.Longitude))).ToList();
            }

            if (!coordinates.Any())
            {
                throw new InvalidOperationException($"{nameof(CoordinatesProvider)} cannot generate another unique value for null country.");
            }

            int minRandomValue = 0;
            int maxRandomValue = coordinates.Count;

            return coordinates[this.Randomizer.Next(minRandomValue, maxRandomValue)];
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="CoordinatesModel"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="CoordinatesModel"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="CoordinatesModel"/>.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="CoordinatesModel"/> instance is
        /// defined by a comma separated string concatenation of the
        /// <see cref="CoordinatesModel.Latitude"/> and <see cref="CoordinatesModel.Longitude"/> properties.
        /// </remarks>
        public override string GetValueId(CoordinatesModel value)
        {
            return string.Format("{0},{1}", value.Latitude, value.Longitude);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{CoordinatesModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<CoordinatesModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            string[] resourcePaths = ResourceReader.GetEmbeddedResourcePaths("Falsus.Providers.Location.Datasets.Streets");
            foreach (string resource in resourcePaths)
            {
                string country = resource.Substring(51, 2);

                if (!cache.ContainsKey(country))
                {
                    cache.Add(country, new List<CoordinatesModel>());
                }

                CoordinatesModel[] models = ResourceReader.ReadContentsFromFile<CoordinatesModel[]>(resource);
                cache[country].AddRange(models);
            }
        }
    }
}