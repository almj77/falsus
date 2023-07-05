namespace Falsus.Providers.Person
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Person.Models;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of last/family names.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class LastNameProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="NationalityModel"/> to use on the value generation process.
        /// </summary>
        public const string NationalityArgumentName = "nationality";

        /// <summary>
        /// A dictionary of <see cref="string"/> values (last names) indexed by
        /// the <see cref="CountryModel.Alpha2"/> property.
        /// </summary>
        private static Dictionary<string, string[]> namesByCountry;

        /// <summary>
        /// An array of <see cref="string"/> values containing all available last names.
        /// </summary>
        private static string[] names;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameProvider"/> class.
        /// </summary>
        public LastNameProvider()
            : base()
        {
            namesByCountry = new Dictionary<string, string[]>();
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
                { NationalityArgumentName, typeof(NationalityModel) }
            };
        }

        /// <summary>
        /// Generates a random last/family name
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
        /// Thrown because the generation of ranged last names is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LastNameProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random last/family name
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
        /// Thrown because the generation of ranged last/family names is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LastNameProvider)} does not support ranged row values.");
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
        /// Generates a random last name based on the context and excluded objects.
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
        /// A random last name.
        /// </returns>
        /// <remarks>
        /// If the <see cref="NationalityArgumentName"/> is provided then this method will
        /// return the name of the specified <see cref="NationalityModel"/> value.
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string[] values = this.GetValues(context, excludedObjects);
            int minRandomValue = 0;
            int maxRandomValue = values.Length;

            if (!values.Any())
            {
                throw new InvalidOperationException($"{nameof(LastNameProvider)} cannot generate another unique value.");
            }

            string value = values[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            if (excludedObjects.Any())
            {
                while (excludedObjects.Contains(value))
                {
                    value = values[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                }
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
        /// Instructs the data provider to prepare the data for generation based on the
        /// provided <see cref="LastNameProviderConfiguration"/>.
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

            LastNameModel[] lastNameValues =
                ResourceReader.ReadContentsFromFile<LastNameModel[]>("Falsus.Providers.Person.Datasets.Surnames.json");

            LanguageToCountryModel[] languageToCountryValues =
                ResourceReader.ReadContentsFromFile<LanguageToCountryModel[]>("Falsus.Providers.Person.Datasets.CountryLanguages.json");

            if (property.Arguments.ContainsKey(NationalityArgumentName))
            {
                Dictionary<string, string[]> languagesByCountry = languageToCountryValues
                    .GroupBy(u => u.Alpha2CountryCode)
                    .ToDictionary(k => k.Key, v => v.OrderBy(u => u.Order).Select(u => u.TwoLetterLanguageCode).ToArray());

                namesByCountry = new Dictionary<string, string[]>();
                foreach (KeyValuePair<string, string[]> item in languagesByCountry)
                {
                    string[] names = lastNameValues
                        .Where(u => item.Value
                        .Contains(u.LanguageTwoLetterCode)).Select(u => u.Surname)
                        .ToArray();

                    namesByCountry.Add(item.Key, names);
                }
            }
            else
            {
                names = lastNameValues.Select(u => u.Surname).ToArray();
            }
        }

        /// <summary>
        /// Gets the list of all possible values based on the context and excluded objects.
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
        /// An array of <see cref="string"/> values containing all possible values.
        /// </returns>
        /// <remarks>
        /// If the <see cref="NationalityArgumentName"/> is provided then this method will
        /// return the name of the specified <see cref="NationalityModel"/> value.
        /// </remarks>
        private string[] GetValues(DataGeneratorContext context, string[] excludedObjects)
        {
            string[] values = null;

            if (context.HasArgumentValue(NationalityArgumentName))
            {
                NationalityModel nationality = context.GetArgumentValue<NationalityModel>(NationalityArgumentName);
                if (nationality == null || nationality.CountryAlpha2 == null || !namesByCountry.ContainsKey(nationality.CountryAlpha2))
                {
                    string countryName = (nationality == null || nationality.CountryAlpha2 == null) ? string.Empty : nationality.CountryAlpha2;
                    throw new InvalidOperationException($"{nameof(LastNameProvider)} could not find data for country '{countryName}'.");
                }

                values = namesByCountry[nationality.CountryAlpha2].ToArray();
            }
            else
            {
                values = names;
            }

            if (excludedObjects.Any())
            {
                values = values.Where(u => !excludedObjects.Contains(u)).ToArray();
            }

            return values;
        }
    }
}
