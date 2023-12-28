namespace Falsus.Providers.Company
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Company.Models;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of legal entity types.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class LegalEntityTypeProvider : DataGeneratorProvider<LegalEntityTypeModel>
    {
        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private LegalEntityTypeProviderConfiguration configuration;

        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// A dictionary of <see cref="LegalEntityTypeModel"/> instances indexed by
        /// the <see cref="CountryModel.Alpha2"/> property.
        /// </summary>
        private static Dictionary<string, LegalEntityTypeModel[]> legalEntityTypesByCountry;

        /// <summary>
        /// A dictionary of <see cref="LanguageToCountryModel"/> instances indexed by
        /// the <see cref="LanguageToCountryModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, LanguageToCountryModel[]> countriesByLanguage;

        /// <summary>
        /// A dictionary of <see cref="LanguageToCountryModel"/> instances indexed by
        /// the <see cref="LanguageToCountryModel.Alpha2CountryCode"/> property.
        /// </summary>
        private static Dictionary<string, LanguageToCountryModel[]> languagesByCountry;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegalEntityTypeProvider"/> class.
        /// </summary>
        public LegalEntityTypeProvider()
            : base()
        {
            legalEntityTypesByCountry = new Dictionary<string, LegalEntityTypeModel[]>();
            this.configuration = new LegalEntityTypeProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegalEntityTypeProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public LegalEntityTypeProvider(LegalEntityTypeProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
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
        /// Generates a random legal entity type
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="LegalEntityTypeModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="LegalEntityTypeModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged legal entity types is not supported.
        /// </exception>
        public override LegalEntityTypeModel GetRangedRowValue(LegalEntityTypeModel minValue, LegalEntityTypeModel maxValue, LegalEntityTypeModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LegalEntityTypeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random legal entity type
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
        /// An array of <see cref="LegalEntityTypeModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="LegalEntityTypeModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged legal entity types is not supported.
        /// </exception>
        public override LegalEntityTypeModel GetRowValue(DataGeneratorContext context, WeightedRange<LegalEntityTypeModel>[] excludedRanges, LegalEntityTypeModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LegalEntityTypeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="LegalEntityTypeModel"/> instance based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="LegalEntityTypeModel"/> value with the specified unique identifier.
        /// </returns>
        public override LegalEntityTypeModel GetRowValue(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            string[] tokens = id.Split('|');
            if (tokens.Length == 2)
            {
                return legalEntityTypesByCountry[tokens[0]]
                    .FirstOrDefault(u => string.IsNullOrEmpty(u.TwoLetterLanguageCode) && u.Abbreviation == tokens[1]);
            }
            else if (tokens.Length == 3)
            {
                return legalEntityTypesByCountry[tokens[0]]
                    .FirstOrDefault(u => u.TwoLetterLanguageCode == tokens[1] && u.Abbreviation == tokens[2]);
            }

            return null;
        }

        /// <summary>
        /// Generates a random legal entity type based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="LegalEntityTypeModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random type of legal entity.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return a type of legal entity that belogns to the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        public override LegalEntityTypeModel GetRowValue(DataGeneratorContext context, LegalEntityTypeModel[] excludedObjects)
        {
            LegalEntityTypeModel[] models = Array.Empty<LegalEntityTypeModel>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            LegalEntityTypeModel value = null;

            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (countryModel == null)
                {
                    throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} cannot generate another unique value for null country.");
                }

                if (!string.IsNullOrEmpty(countryModel.Alpha2) && legalEntityTypesByCountry.ContainsKey(countryModel.Alpha2))
                {
                    return GetRowValue(excludedObjects, countryModel);
                }
                else if (!string.IsNullOrEmpty(countryModel.Alpha2) && (this.configuration.AttemptFallbackByLanguage || this.configuration.FallbackToCountry))
                {
                    if (this.configuration.AttemptFallbackByLanguage)
                    {
                        if (languagesByCountry.ContainsKey(countryModel.Alpha2))
                        {
                            string[] currentCountryLanguages = languagesByCountry[countryModel.Alpha2]
                                .Select(u => u.TwoLetterLanguageCode)
                                .ToArray();

                            List<string> newCountries = new List<string>();
                            foreach (string language in currentCountryLanguages)
                            {
                                if (countriesByLanguage.ContainsKey(language))
                                {
                                    newCountries.AddRange(
                                        countriesByLanguage[language]
                                        .Where(u => u.Alpha2CountryCode != countryModel.Alpha2 
                                                && legalEntityTypesByCountry.ContainsKey(u.Alpha2CountryCode))
                                        .Select(u => u.Alpha2CountryCode));
                                }
                            }

                            if (newCountries.Any())
                            {
                                foreach (string country in newCountries)
                                {
                                    CountryModel fallbackCountryModel = new CountryModel()
                                    {
                                        Alpha2 = country
                                    };

                                    try
                                    {
                                        return this.GetRowValue(excludedObjects, fallbackCountryModel);
                                    }
                                    catch (InvalidOperationException) { }
                                }
                            }
                        }
                    }

                    if (this.configuration.FallbackToCountry)
                    {
                        CountryModel fallbackCountryModel = new CountryModel()
                        {
                            Alpha2 = this.configuration.FallbackCountryAlpha2
                        };

                        return this.GetRowValue(excludedObjects, fallbackCountryModel);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} cannot generate another unique value for {countryModel.Alpha2}.");
                }
            }

            models = legalEntityTypesByCountry.SelectMany(u => u.Value).ToArray();
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} cannot generate another unique value.");
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
        /// Gets a unique identifier for the provided <see cref="LegalEntityTypeModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="LegalEntityTypeModel"/> instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(LegalEntityTypeModel value)
        {
            if (value == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(value.TwoLetterLanguageCode))
            {
                return string.Concat(value.CountryAlpha2, "|", value.Abbreviation);
            }

            return string.Concat(value.CountryAlpha2, "|", value.TwoLetterLanguageCode, "|", value.Abbreviation);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{LegalEntityTypeModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<LegalEntityTypeModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            LegalEntityTypeModel[] legalEntityTypeValues = ResourceReader.ReadContentsFromFile<LegalEntityTypeModel[]>("Falsus.Providers.Company.Datasets.LegalEntityTypes.json");
            legalEntityTypesByCountry = legalEntityTypeValues.GroupBy(u => u.CountryAlpha2).ToDictionary(u => u.Key, x => x.ToArray());

            LanguageToCountryModel[] languagesToCountry = ResourceReader.ReadContentsFromFile<LanguageToCountryModel[]>("Falsus.Providers.Company.Datasets.CountryLanguages.json");
            countriesByLanguage = languagesToCountry.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            languagesByCountry = languagesToCountry.GroupBy(u => u.Alpha2CountryCode).ToDictionary(u => u.Key, x => x.ToArray());

            this.ValidateConfiguration();
        }

        /// <summary>
        /// Generates a random legal entity type based on the provided arguments.
        /// </summary>
        /// <param name="excludedObjects">
        /// An array of <see cref="LegalEntityTypeModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <param name="countryModel">
        /// The country to use in the legal entity type generation.
        /// </param>
        /// <returns>
        /// A random type of legal entity.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if impossible to generate another unique value.
        /// </exception>
        private LegalEntityTypeModel GetRowValue(LegalEntityTypeModel[] excludedObjects, CountryModel countryModel)
        {
            LegalEntityTypeModel[] models = legalEntityTypesByCountry[countryModel.Alpha2];
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} cannot generate another unique value.");
            }

            int minRandomValue = 0;
            int maxRandomValue = models.Length;

            LegalEntityTypeModel value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            while (excludedObjects.Contains(value))
            {
                value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            }

            return value;
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item>
        /// The <see cref="LegalEntityTypeProviderConfiguration.FallbackToCountry"/> is true
        /// and <see cref="LegalEntityTypeProviderConfiguration.FallbackCountryAlpha2"/> is null or empty.
        /// </item>
        /// <item>
        /// The <see cref="LegalEntityTypeProviderConfiguration.FallbackToCountry"/> is true
        /// and <see cref="LegalEntityTypeProviderConfiguration.FallbackCountryAlpha2"/> is not on 
        /// present on the languages dataset.
        /// </item>
        /// <item>
        /// The <see cref="LegalEntityTypeProviderConfiguration.FallbackToCountry"/> is true
        /// and <see cref="LegalEntityTypeProviderConfiguration.FallbackCountryAlpha2"/> is not on 
        /// present on the legal entities dataset.
        /// </item>
        /// </list>
        /// </exception>
        private void ValidateConfiguration()
        {

            if (this.configuration.FallbackToCountry)
            {
                if (string.IsNullOrEmpty(this.configuration.FallbackCountryAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} fallback country cannot be null or empty.");
                }

                if (!languagesByCountry.ContainsKey(this.configuration.FallbackCountryAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} fallback country cannot be found in language dataset.");
                }

                if (!legalEntityTypesByCountry.ContainsKey(this.configuration.FallbackCountryAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(LegalEntityTypeProvider)} fallback country cannot be found in dataset.");
                }
            }
        }
    }
}
