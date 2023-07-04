namespace Falsus.Providers.Location
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Location.Models;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Fare;

    /// <summary>
    /// Represents a provider of postal codes.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class PostalCodeProvider : DataGeneratorProvider<string>
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
        /// A dictionary of the postal code regular expressions indexed by
        /// the <see cref="CountryModel.Alpha2"/> property.
        /// </summary>
        private static Dictionary<string, string> patternByCountry;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostalCodeProvider"/> class.
        /// </summary>
        public PostalCodeProvider()
            : base()
        {
            patternByCountry = new Dictionary<string, string>();
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
        /// Generates a random postal code
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="string"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged postal codes is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(PostalCodeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random postal code
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
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="string"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged postal codes is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(PostalCodeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="string"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="string"/> value with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="id"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="id"/> argument.
        /// </remarks>
        public override string GetRowValue(string id)
        {
            return id;
        }

        /// <summary>
        /// Generates a random postal code based on the context and excluded objects.
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
        /// A random postal code.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return a postal code that matches the pattern for the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string pattern = string.Empty;
            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel country = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (country == null || string.IsNullOrEmpty(country.Alpha2))
                {
                    throw new InvalidOperationException($"{nameof(PostalCodeProvider)} cannot generate another unique value for null country.");
                }

                if (patternByCountry.ContainsKey(country.Alpha2))
                {
                    pattern = patternByCountry[country.Alpha2];

                    if (string.IsNullOrEmpty(pattern))
                    {
                        throw new InvalidOperationException($"{nameof(PostalCodeProvider)} could not find pattern for country {country.Alpha2}");
                    }
                }
                else
                {
                    pattern = patternByCountry.Values.ElementAt(this.Randomizer.Next(0, patternByCountry.Count));
                }
            }
            else
            {
                pattern = patternByCountry.Values.ElementAt(this.Randomizer.Next(0, patternByCountry.Count));
            }

            Xeger regexGenerator = new Xeger(pattern, this.Randomizer);
            string value = regexGenerator.Generate();

            int attempts = 0;
            while (excludedObjects.Contains(value))
            {
                value = regexGenerator.Generate();
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(PostalCodeProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(string value)
        {
            return value;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{T}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<string> property, int rowCount)
        {
            base.Load(property, rowCount);

            PostalCodePatternModel[] postalCodePatterns =
                ResourceReader.ReadContentsFromFile<PostalCodePatternModel[]>("Falsus.Providers.Location.Datasets.PostalCodes.json");

            patternByCountry = postalCodePatterns.ToDictionary(k => k.CountryAlpha2, v => v.Pattern);
        }
    }
}
