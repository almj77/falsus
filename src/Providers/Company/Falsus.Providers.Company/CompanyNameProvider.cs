namespace Falsus.Providers.Company
{
    using Falsus.Providers.Company.Models;
    using System.Collections.Generic;
    using Falsus.Shared.Models;
    using Falsus.GeneratorProperties;
    using System.Linq;
    using Falsus.Shared.Helpers;
    using System;
    using System.Text.RegularExpressions;
    using Falsus.Providers.Person;
    using Falsus.Providers.Location;

    /// <summary>
    /// Represents a provider of company names.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class CompanyNameProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private CompanyNameProviderConfiguration configuration;

        /// <summary>
        /// Represents a provider of first/given names.
        /// </summary>
        private FirstNameProvider firstNameProvider;

        /// <summary>
        /// Defines a data generator property to be attached to the <see cref="firstNameProvider"/>.
        /// </summary>
        private DataGeneratorProperty<string> firstNameProperty;

        /// <summary>
        /// Represents a provider of last/family names.
        /// </summary>
        private LastNameProvider lastNameProvider;

        /// <summary>
        /// Defines a data generator property to be attached to the <see cref="lastNameProvider"/>.
        /// </summary>
        private DataGeneratorProperty<string> lastNameProperty;

        /// <summary>
        /// Represents a provider of legal entity types.
        /// </summary>
        private LegalEntityTypeProvider legalEntityTypeProvider;

        /// <summary>
        /// Defines a data generator property to be attached to the <see cref="legalEntityTypeProvider"/>.
        /// </summary>
        private DataGeneratorProperty<LegalEntityTypeModel> legalEntityTypeProperty;

        /// <summary>
        /// Represents a provider of City names.
        /// </summary>
        private CityProvider cityProvider;

        /// <summary>
        /// Defines a data generator property to be attached to the <see cref="cityProvider"/>.
        /// </summary>
        private DataGeneratorProperty<CityModel> cityProperty;

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a company preffix.
        /// </summary>
        private const string KeywordCompanyPrefix = "{{company_prefix}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a person first name.
        /// </summary>
        private const string KeywordPersonFirstName = "{{person_first_name}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a person last name.
        /// </summary>
        private const string KeywordPersonLastName = "{{person_last_name}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a company suffix.
        /// </summary>
        private const string KeywordCompanySuffix = "{{company_suffix}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a noun.
        /// </summary>
        private const string KeywordCompanyNoun = "{{company_noun}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with an adjective.
        /// </summary>
        private const string KeywordCompanyAdjective = "{{company_adjective}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a company type.
        /// </summary>
        private const string KeywordCompanyType = "{{company_type}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a company category.
        /// </summary>
        private const string KeywordCompanyCategory = "{{company_category}}";

        /// <summary>
        /// A string containing the pattern keyword that is to be replaced with a city name.
        /// </summary>
        private const string KeywordCityName = "{{city_name}}";

        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// The name of the input argument that contains a <see cref="string"/> 
        /// containing the two letter language code to be used on the value generation process.
        /// </summary>
        public const string LanguageArgumentName = "language";

        /// <summary>
        /// A dictionary of <see cref="CompanyAdjectiveModel"/> instances indexed by
        /// the <see cref="CompanyAdjectiveModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyAdjectiveModel[]> adjectivesByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyBuzzAdjectiveModel"/> instances indexed by
        /// the <see cref="CompanyBuzzAdjectiveModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyBuzzAdjectiveModel[]> buzzAdjectivesByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyBuzzNounModel"/> instances indexed by
        /// the <see cref="CompanyBuzzNounModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyBuzzNounModel[]> buzzNounsByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyBuzzVerbModel"/> instances indexed by
        /// the <see cref="CompanyBuzzVerbModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyBuzzVerbModel[]> buzzVerbsByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyCategoryModel"/> instances indexed by
        /// the <see cref="CompanyCategoryModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyCategoryModel[]> categoriesByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyDescriptorModel"/> instances indexed by
        /// the <see cref="CompanyDescriptorModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyDescriptorModel[]> descriptorsByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyNamePatternModel"/> instances indexed by
        /// the <see cref="CompanyNamePatternModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyNamePatternModel[]> namePatternsByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyNounModel"/> instances indexed by
        /// the <see cref="CompanyNounModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyNounModel[]> nounsByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyPreffixModel"/> instances indexed by
        /// the <see cref="CompanyPreffixModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyPreffixModel[]> preffixesByLanguage;

        /// <summary>
        /// A dictionary of <see cref="CompanyTypeModel"/> instances indexed by
        /// the <see cref="CompanyTypeModel.TwoLetterLanguageCode"/> property.
        /// </summary>
        private static Dictionary<string, CompanyTypeModel[]> typesByLanguage;

        /// <summary>
        /// A dictionary of <see cref="LegalEntityTypeModel"/> instances indexed by
        /// the <see cref="LegalEntityTypeModel.CountryAlpha2"/> property.
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
        /// Initializes a new instance of the <see cref="CompanyNameProvider"/> class.
        /// </summary>
        public CompanyNameProvider()
            : base()
        {
            legalEntityTypesByCountry = new Dictionary<string, LegalEntityTypeModel[]>();
            this.configuration = new CompanyNameProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyNameProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public CompanyNameProvider(CompanyNameProviderConfiguration configuration)
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
                { CountryArgumentName, typeof(CountryModel) },
                { LanguageArgumentName, typeof(string) }
            };
        }

        /// <summary>
        /// Generates a random company name
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="string"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged company names is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(CompanyNameProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random company name
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
        /// An array of <see cref="string"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="string"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged company names is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LegalEntityTypeProvider)} does not support ranged row values.");
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
        /// Generates a random company name based on the context and excluded objects.
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
        /// A random company name.
        /// </returns>
        /// <remarks>
        /// Priority is given to the language argument name if both are provided.
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the name of company in a language used by the specified <see cref="CountryModel"/>
        /// argument value.
        /// If the <see cref="LanguageArgumentName"/> is provided then this method will
        /// return the name of company in the specified language Alpha2 ISO Code.
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            CompanyNamePatternModel[] models = Array.Empty<CompanyNamePatternModel>();
            string[] selectedLanguageAlpha2 = Array.Empty<string>();
            string[] selectedCountryAlpha2 = Array.Empty<string>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            string value = null;

            if (context.HasArgumentValue(LanguageArgumentName))
            {
                string languageAlpha2 = context.GetArgumentValue<string>(LanguageArgumentName);
                if (string.IsNullOrEmpty(languageAlpha2) && !this.configuration.FallbackToLanguage)
                {
                    throw new InvalidOperationException($"{nameof(CompanyNameProvider)} cannot generate another unique value for null language.");
                }
                else if (!string.IsNullOrEmpty(languageAlpha2) && !namePatternsByLanguage.ContainsKey(languageAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(CompanyNameProvider)} cannot generate another unique value for the '{languageAlpha2}' language.");
                }

                models = namePatternsByLanguage[languageAlpha2].ToArray();

                selectedLanguageAlpha2 = new string[1] { languageAlpha2 };
                selectedCountryAlpha2 = countriesByLanguage[languageAlpha2].Select(u => u.Alpha2CountryCode).ToArray();
            }
            else if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);

                if (countryModel == null || string.IsNullOrEmpty(countryModel.Alpha2))
                {
                    if (this.configuration.FallbackToLanguage)
                    {
                        models = namePatternsByLanguage[this.configuration.FallbackLanguageAlpha2].ToArray();

                        selectedLanguageAlpha2 = new string[1] { this.configuration.FallbackLanguageAlpha2 };
                        selectedCountryAlpha2 = countriesByLanguage[this.configuration.FallbackLanguageAlpha2].Select(u => u.Alpha2CountryCode).ToArray();

                    }
                    else if (this.configuration.AttemptFallbackByLanguage && languagesByCountry.ContainsKey(countryModel.Alpha2))
                    {
                        List<CompanyNamePatternModel> patterns = new List<CompanyNamePatternModel>();
                        foreach (var language in languagesByCountry[countryModel.Alpha2])
                        {
                            if (namePatternsByLanguage.ContainsKey(language.TwoLetterLanguageCode))
                            {
                                patterns.AddRange(namePatternsByLanguage[language.TwoLetterLanguageCode]);
                            }
                        }

                        models = patterns.ToArray();

                        selectedLanguageAlpha2 = patterns.Select(u => u.TwoLetterLanguageCode).Distinct().ToArray();
                        selectedCountryAlpha2 = countriesByLanguage[countryModel.Alpha2].Select(u => u.Alpha2CountryCode).ToArray();
                    }
                    else
                    {
                        throw new InvalidOperationException($"{nameof(CompanyNameProvider)} cannot generate another unique value for null country.");
                    }
                }
                else if (!languagesByCountry.ContainsKey(countryModel.Alpha2))
                {
                    if (this.configuration.FallbackToLanguage)
                    {
                        models = namePatternsByLanguage[this.configuration.FallbackLanguageAlpha2].ToArray();
                        selectedLanguageAlpha2 = new string[1] { this.configuration.FallbackLanguageAlpha2 };
                        selectedCountryAlpha2 = countriesByLanguage[this.configuration.FallbackLanguageAlpha2].Select(u => u.Alpha2CountryCode).ToArray();
                    }
                    else
                    {
                        throw new InvalidOperationException($"{nameof(CompanyNameProvider)} cannot generate another unique value for '{countryModel.Alpha2}' country.");
                    }
                }
                else
                {
                    LanguageToCountryModel[] languages = languagesByCountry[countryModel.Alpha2];
                    List<CompanyNamePatternModel> languagePatterns = new List<CompanyNamePatternModel>();

                    foreach (var language in languages)
                    {
                        if (namePatternsByLanguage.ContainsKey(language.TwoLetterLanguageCode))
                        {
                            languagePatterns.AddRange(namePatternsByLanguage[language.TwoLetterLanguageCode]);
                        }
                    }

                    models = languagePatterns.ToArray();
                    selectedLanguageAlpha2 = languages.Select(u => u.TwoLetterLanguageCode).ToArray();
                    selectedCountryAlpha2 = languages.Select(u => u.Alpha2CountryCode).Distinct().ToArray();

                    if (!models.Any() && this.configuration.FallbackToLanguage)
                    {
                        models = namePatternsByLanguage[this.configuration.FallbackLanguageAlpha2].ToArray();

                        selectedLanguageAlpha2 = new string[1] { this.configuration.FallbackLanguageAlpha2 };
                        selectedCountryAlpha2 = countriesByLanguage[this.configuration.FallbackLanguageAlpha2].Select(u => u.Alpha2CountryCode).ToArray();
                    }
                }
            }
            else
            {
                models = namePatternsByLanguage.Values.SelectMany(u => u).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(CompanyNameProvider)} cannot generate another unique value.");
            }

            minRandomValue = 0;
            maxRandomValue = models.Length;

            CompanyNamePatternModel valueTyped = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            if (!selectedCountryAlpha2.Any())
            {
                selectedCountryAlpha2 = countriesByLanguage[valueTyped.TwoLetterLanguageCode]
                    .Select(u => u.Alpha2CountryCode)
                    .Distinct()
                    .ToArray();
            }

            if (!selectedLanguageAlpha2.Any())
            {
                selectedLanguageAlpha2 = new string[1] { valueTyped.TwoLetterLanguageCode };
            }

            value = this.ReplacePatternKeywords(
                valueTyped.Pattern, 
                selectedLanguageAlpha2, 
                selectedCountryAlpha2,
                context.RequestedRowCount);

            while (excludedObjects.Contains(value))
            {
                value = this.ReplacePatternKeywords(
                    models[this.Randomizer.Next(minRandomValue, maxRandomValue)].Pattern,
                    selectedLanguageAlpha2,
                    selectedCountryAlpha2,
                    context.RequestedRowCount);
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
        /// Replaces the known keywords on the specified pattern by relying on the
        /// language and country arguments to generate random replacement values.
        /// </summary>
        /// <param name="pattern">The company name pattern to be replaced.</param>
        /// <param name="languages">The languages that can be used to find replacement values.</param>
        /// <param name="countries">The countries that can be referenced to find replacement values.</param>
        /// <param name="rowCount">The expected number of rows to generate.</param>
        /// <returns>A company name that matches the provided pattern.</returns>
        private string ReplacePatternKeywords(string pattern, string[] languages, string[] countries, int rowCount)
        {
            string value = pattern;
            Regex regex = new Regex("{{\\w+}}");
            MatchCollection matches = regex.Matches(pattern);

            if (matches.Count > 0)
            {
                string language = languages[0];
                string country = countries[0];

                if (languages.Length > 1)
                {
                    language = languages[this.Randomizer.Next(0, languages.Length)];
                }

                if (countries.Length > 1)
                {
                    country = countries[this.Randomizer.Next(0, countries.Length)];
                }

                foreach (Match match in matches)
                {
                    switch (match.Value)
                    {
                        case KeywordCompanyPrefix:
                            CompanyPreffixModel[] preffixes = preffixesByLanguage[language.ToLowerInvariant()];
                            string preffix = preffixes[this.Randomizer.Next(0, preffixes.Length)].Preffix;

                            value = regex.Replace(value, preffix, 1);
                            break;
                        case KeywordPersonFirstName:
                            if (this.firstNameProvider == null)
                            {
                                this.firstNameProvider = new FirstNameProvider();
                                this.firstNameProvider.InitializeRandomizer(this.Randomizer.Next());

                                NationalityProvider nationalityProvider = new NationalityProvider();

                                DataGeneratorProperty<NationalityModel> nationalityProperty =
                                    new DataGeneratorProperty<NationalityModel>("Nationality")
                                    .FromProvider(nationalityProvider);

                                this.firstNameProperty = new DataGeneratorProperty<string>("FirstName")
                                    .FromProvider(this.firstNameProvider)
                                    .WithArgument<NationalityModel>(
                                        FirstNameProvider.NationalityArgumentName,
                                        nationalityProperty);

                                this.firstNameProvider.Load(this.firstNameProperty, rowCount);
                            }

                            Dictionary<string, object> firstNameContextData = new Dictionary<string, object>
                            {
                                { "Nationality", new NationalityModel() { CountryAlpha2 = country } }
                            };

                            DataGeneratorContext firstNameContext = new DataGeneratorContext(firstNameContextData, 0, 1, this.firstNameProperty, this.firstNameProperty.Arguments);
                            string firstName = this.firstNameProvider.GetRowValue(firstNameContext, Array.Empty<string>());

                            value = regex.Replace(value, firstName, 1);
                            break;
                        case KeywordPersonLastName:
                            if (this.lastNameProvider == null)
                            {
                                this.lastNameProvider = new LastNameProvider();
                                this.lastNameProvider.InitializeRandomizer(this.Randomizer.Next());

                                NationalityProvider nationalityProvider = new NationalityProvider();

                                DataGeneratorProperty<NationalityModel> nationalityProperty =
                                    new DataGeneratorProperty<NationalityModel>("Nationality")
                                    .FromProvider(nationalityProvider);

                                this.lastNameProperty = new DataGeneratorProperty<string>("LastName")
                                    .FromProvider(this.lastNameProvider)
                                    .WithArgument<NationalityModel>(
                                        LastNameProvider.NationalityArgumentName,
                                        nationalityProperty);

                                this.lastNameProvider.Load(this.lastNameProperty, rowCount);
                            }

                            Dictionary<string, object> lastNameContextData = new Dictionary<string, object>
                            {
                                { "Nationality", new NationalityModel() { CountryAlpha2 = country } }
                            };

                            DataGeneratorContext lastNameContext = new DataGeneratorContext(lastNameContextData, 0, 1, this.lastNameProperty, this.lastNameProperty.Arguments);

                            string lastName = this.lastNameProvider.GetRowValue(lastNameContext, Array.Empty<string>());

                            value = regex.Replace(value, lastName, 1);
                            break;
                        case KeywordCompanySuffix:
                            if (this.legalEntityTypeProvider == null)
                            {
                                this.legalEntityTypeProvider = new LegalEntityTypeProvider();
                                this.legalEntityTypeProvider.InitializeRandomizer(this.Randomizer.Next());

                                CountryProvider countryProvider = new CountryProvider();

                                DataGeneratorProperty<CountryModel> countryProperty =
                                    new DataGeneratorProperty<CountryModel>("Country")
                                    .FromProvider(countryProvider);

                                this.legalEntityTypeProperty = new DataGeneratorProperty<LegalEntityTypeModel>("LegalEntityType")
                                    .FromProvider(this.legalEntityTypeProvider)
                                    .WithArgument<CountryModel>(
                                        LegalEntityTypeProvider.CountryArgumentName,
                                        countryProperty);

                                this.legalEntityTypeProvider.Load(this.lastNameProperty, rowCount);
                            }

                            Dictionary<string, object> legalEntityTypeContextData = new Dictionary<string, object>
                            {
                                { "Country", new CountryModel() { Alpha2 = country } }
                            };

                            DataGeneratorContext legalEntityTypeContext = new DataGeneratorContext(legalEntityTypeContextData, 0, 1, this.legalEntityTypeProperty, this.legalEntityTypeProperty.Arguments);
                            LegalEntityTypeModel legalEntityType = this.legalEntityTypeProvider.GetRowValue(legalEntityTypeContext, Array.Empty<LegalEntityTypeModel>());

                            value = regex.Replace(value, legalEntityType.Abbreviation, 1);
                            break;
                        case KeywordCompanyNoun:
                            CompanyNounModel[] nouns = nounsByLanguage[language.ToLower()];
                            string noun = nouns[this.Randomizer.Next(0, nouns.Length)].Noun;

                            value = regex.Replace(value, noun, 1);
                            break;
                        case KeywordCompanyAdjective:
                            CompanyAdjectiveModel[] adjectives = adjectivesByLanguage[language.ToLower()];
                            string adjective = adjectives[this.Randomizer.Next(0, adjectives.Length)].Adjective;

                            value = regex.Replace(value, adjective, 1);
                            break;
                        case KeywordCompanyType:
                            CompanyTypeModel[] companyTypes = typesByLanguage[language.ToLower()];
                            string companyType = companyTypes[this.Randomizer.Next(0, companyTypes.Length)].CompanyType;

                            value = regex.Replace(value, companyType, 1);
                            break;
                        case KeywordCompanyCategory:
                            CompanyCategoryModel[] categories = categoriesByLanguage[language.ToLower()];
                            string category = categories[this.Randomizer.Next(0, categories.Length)].Category;

                            value = regex.Replace(value, category, 1);
                            break;
                        case KeywordCityName:
                            if (this.cityProvider == null)
                            {
                                this.cityProvider = new CityProvider();
                                this.cityProvider.InitializeRandomizer(this.Randomizer.Next());

                                CountryProvider countryProvider = new CountryProvider();

                                DataGeneratorProperty<CountryModel> countryProperty =
                                    new DataGeneratorProperty<CountryModel>("Country")
                                    .FromProvider(countryProvider);

                                this.cityProperty = new DataGeneratorProperty<CityModel>("City")
                                    .FromProvider(this.cityProvider)
                                    .WithArgument<CountryModel>(
                                        CityProvider.CountryArgumentName,
                                        countryProperty);

                                this.cityProvider.Load(this.lastNameProperty, rowCount);
                            }

                            Dictionary<string, object> cityContextData = new Dictionary<string, object>
                            {
                                { "Country", new CountryModel() { Alpha2 = country } }
                            };

                            DataGeneratorContext cityContext = new DataGeneratorContext(cityContextData, 0, 1, this.cityProperty, this.cityProperty.Arguments);
                            string city = this.cityProvider.GetRowValue(cityContext, Array.Empty<CityModel>()).Name;

                            value = regex.Replace(value, city, 1);
                            break;
                    }
                }
            }

            return value;
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
        public override void Load(DataGeneratorProperty<string> property, int rowCount)
        {
            base.Load(property, rowCount);

            CompanyAdjectiveModel[] companyAdjectiveValues = ResourceReader.ReadContentsFromFile<CompanyAdjectiveModel[]>("Falsus.Providers.Company.Datasets.Adjectives.json");
            CompanyBuzzAdjectiveModel[] companyBuzzAdjectiveValues = ResourceReader.ReadContentsFromFile<CompanyBuzzAdjectiveModel[]>("Falsus.Providers.Company.Datasets.BuzzAdjectives.json");
            CompanyBuzzNounModel[] companyBuzzNounValues = ResourceReader.ReadContentsFromFile<CompanyBuzzNounModel[]>("Falsus.Providers.Company.Datasets.BuzzNouns.json");
            CompanyBuzzVerbModel[] companyBuzzVerbValues = ResourceReader.ReadContentsFromFile<CompanyBuzzVerbModel[]>("Falsus.Providers.Company.Datasets.BuzzVerbs.json");
            CompanyCategoryModel[] companyCategoryValues = ResourceReader.ReadContentsFromFile<CompanyCategoryModel[]>("Falsus.Providers.Company.Datasets.Categories.json");
            LanguageToCountryModel[] languageToCountryValues = ResourceReader.ReadContentsFromFile<LanguageToCountryModel[]>("Falsus.Providers.Company.Datasets.CountryLanguages.json");
            CompanyDescriptorModel[] companyDescriptorValues = ResourceReader.ReadContentsFromFile<CompanyDescriptorModel[]>("Falsus.Providers.Company.Datasets.Descriptors.json");
            LegalEntityTypeModel[] legalEntityTypeValues = ResourceReader.ReadContentsFromFile<LegalEntityTypeModel[]>("Falsus.Providers.Company.Datasets.LegalEntityTypes.json");
            CompanyNamePatternModel[] companyNamePatternValues = ResourceReader.ReadContentsFromFile<CompanyNamePatternModel[]>("Falsus.Providers.Company.Datasets.NamePatterns.json");
            CompanyNounModel[] companyNounValues = ResourceReader.ReadContentsFromFile<CompanyNounModel[]>("Falsus.Providers.Company.Datasets.Nouns.json");
            CompanyPreffixModel[] companyPreffixValues = ResourceReader.ReadContentsFromFile<CompanyPreffixModel[]>("Falsus.Providers.Company.Datasets.Preffixes.json");
            CompanyTypeModel[] companyTypeValues = ResourceReader.ReadContentsFromFile<CompanyTypeModel[]>("Falsus.Providers.Company.Datasets.Types.json");

            adjectivesByLanguage = companyAdjectiveValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            buzzAdjectivesByLanguage = companyBuzzAdjectiveValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            buzzNounsByLanguage = companyBuzzNounValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            buzzVerbsByLanguage = companyBuzzVerbValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            categoriesByLanguage = companyCategoryValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            countriesByLanguage = languageToCountryValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            languagesByCountry = languageToCountryValues.GroupBy(u => u.Alpha2CountryCode).ToDictionary(u => u.Key, x => x.ToArray());
            descriptorsByLanguage = companyDescriptorValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            legalEntityTypesByCountry = legalEntityTypeValues.GroupBy(u => u.CountryAlpha2).ToDictionary(u => u.Key, x => x.ToArray());
            namePatternsByLanguage = companyNamePatternValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            nounsByLanguage = companyNounValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            preffixesByLanguage = companyPreffixValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());
            typesByLanguage = companyTypeValues.GroupBy(u => u.TwoLetterLanguageCode).ToDictionary(u => u.Key, x => x.ToArray());

            this.ValidateConfiguration();
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item>
        /// The <see cref="CompanyNameProviderConfiguration.FallbackToLanguage"/> is true
        /// and <see cref="CompanyNameProviderConfiguration.FallbackLanguageAlpha2"/> is null or empty.
        /// </item>
        /// <item>
        /// The <see cref="CompanyNameProviderConfiguration.FallbackToLanguage"/> is true
        /// and <see cref="CompanyNameProviderConfiguration.FallbackLanguageAlpha2"/> is not on 
        /// present on the languages dataset.
        /// </item>
        /// <item>
        /// The <see cref="CompanyNameProviderConfiguration.FallbackToLanguage"/> is true
        /// and <see cref="CompanyNameProviderConfiguration.FallbackLanguageAlpha2"/> is not on 
        /// present on the company name pattern dataset.
        /// </item>
        /// </list>
        /// </exception>
        private void ValidateConfiguration()
        {

            if (this.configuration.FallbackToLanguage)
            {
                if (string.IsNullOrEmpty(this.configuration.FallbackLanguageAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(CompanyNameProvider)} fallback language cannot be null or empty.");
                }

                if (!countriesByLanguage.ContainsKey(this.configuration.FallbackLanguageAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(CompanyNameProvider)} fallback language cannot be found in language dataset.");
                }

                if (!namePatternsByLanguage.ContainsKey(this.configuration.FallbackLanguageAlpha2))
                {
                    throw new InvalidOperationException($"{nameof(CompanyNameProvider)} fallback language cannot be found in company name pattern dataset.");
                }
            }
        }
    }
}
