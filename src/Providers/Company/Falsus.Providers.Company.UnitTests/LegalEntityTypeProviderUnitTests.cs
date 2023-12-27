namespace Falsus.Providers.Company.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LegalEntityTypeProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new LegalEntityTypeProvider(null);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = LegalEntityTypeProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            LegalEntityTypeProvider provider = new LegalEntityTypeProvider();

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            var actual = new
            {
                ArgumentName = arguments.Keys.First(),
                ArgumentType = arguments.Values.First(),
                ArgumentCount = arguments.Count
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            LegalEntityTypeProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(new LegalEntityTypeModel(), new LegalEntityTypeModel(), Array.Empty<LegalEntityTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            LegalEntityTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<LegalEntityTypeModel>>(), Array.Empty<LegalEntityTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithEmptyFallbackCountryThrowsException()
        {
            // Arrange
            LegalEntityTypeProviderConfiguration config = new LegalEntityTypeProviderConfiguration()
            {
                AttemptFallbackByLanguage = true,
                FallbackToCountry = true,
                FallbackCountryAlpha2 = null
            };

            ProviderResult providerResult = CreateProvider(configuration: config);
            LegalEntityTypeProvider provider = providerResult.Provider;

            // Act
            provider.GetRowValue("PT|CRL");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidFallbackCountryThrowsException()
        {
            // Arrange
            LegalEntityTypeProviderConfiguration config = new LegalEntityTypeProviderConfiguration()
            {
                AttemptFallbackByLanguage = true,
                FallbackToCountry = true,
                FallbackCountryAlpha2 = "YY"
            };

            ProviderResult providerResult = CreateProvider(configuration: config);
            LegalEntityTypeProvider provider = providerResult.Provider;

            // Act
            provider.GetRowValue("PT|CRL");
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            LegalEntityTypeModel expectedTyped = new LegalEntityTypeModel()
            {
                CountryAlpha2 = "CA",
                Name = "Incorporated",
                TwoLetterLanguageCode = "en",
                Abbreviation = "Inc."
            };

            var expected = new
            {
                expectedTyped.CountryAlpha2,
                expectedTyped.Name,
                expectedTyped.TwoLetterLanguageCode,
                expectedTyped.Abbreviation
            };

            ProviderResult providerResult = CreateProvider();
            LegalEntityTypeProvider provider = providerResult.Provider;

            // Act
            LegalEntityTypeModel actualTyped = provider.GetRowValue(string.Concat(expectedTyped.CountryAlpha2, "|", expectedTyped.TwoLetterLanguageCode, "|", expectedTyped.Abbreviation));

            var actual = new
            {
                actualTyped.CountryAlpha2,
                actualTyped.Name,
                actualTyped.TwoLetterLanguageCode,
                actualTyped.Abbreviation
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 125;
            LegalEntityTypeModel expectedTyped = new LegalEntityTypeModel()
            {
                CountryAlpha2 = "AU",
                Name = "Limited liability partnership",
                Abbreviation = "LLP"
            };

            var expected = new
            {
                expectedTyped.CountryAlpha2,
                expectedTyped.Name,
                expectedTyped.TwoLetterLanguageCode,
                expectedTyped.Abbreviation
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            LegalEntityTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            LegalEntityTypeModel actualTyped = provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>());

            var actual = new
            {
                actualTyped.CountryAlpha2,
                actualTyped.Name,
                actualTyped.TwoLetterLanguageCode,
                actualTyped.Abbreviation
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<LegalEntityTypeModel> property = new DataGeneratorProperty<LegalEntityTypeModel>("LegalEntityType")
                .WithArgument(LegalEntityTypeProvider.CountryArgumentName, countryProperty);

            LegalEntityTypeProvider provider = new LegalEntityTypeProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<LegalEntityTypeModel> property = new DataGeneratorProperty<LegalEntityTypeModel>("LegalEntityType")
                .WithArgument(LegalEntityTypeProvider.CountryArgumentName, countryProperty);

            LegalEntityTypeProvider provider = new LegalEntityTypeProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            LegalEntityTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<LegalEntityTypeModel> property = providerResult.Property;

            // Act
            LegalEntityTypeModel[] excludedObjects = GetAllLegalEntityTypes();
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueWithLanguageFallbackCanGenerateValue()
        {
            // Arrange
            CountryModel countryModel = new CountryModel()
            {
                Alpha2 = "GL"
            };

            LegalEntityTypeProviderConfiguration config = new LegalEntityTypeProviderConfiguration()
            {
                AttemptFallbackByLanguage = true,
                FallbackToCountry = false,
                FallbackCountryAlpha2 = string.Empty,
            };

            List<LegalEntityTypeModel> generatedValues = new List<LegalEntityTypeModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<LegalEntityTypeModel> property = new DataGeneratorProperty<LegalEntityTypeModel>("LegalEntityType")
                .WithArgument(LegalEntityTypeProvider.CountryArgumentName, countryProperty);

            LegalEntityTypeProvider provider = new LegalEntityTypeProvider(config);
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", countryModel);
            DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

            // Act
            LegalEntityTypeModel value = provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>());

            // Assert
            Assert.IsTrue(value.CountryAlpha2 == "DK");
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            int expectedRowCount = models.Length;
            List<LegalEntityTypeModel> generatedValues = new List<LegalEntityTypeModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<LegalEntityTypeModel> property = new DataGeneratorProperty<LegalEntityTypeModel>("LegalEntityType")
                .WithArgument(LegalEntityTypeProvider.CountryArgumentName, countryProperty);

            foreach (var model in models)
            {
                LegalEntityTypeProvider provider = new LegalEntityTypeProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>()));
                }
                catch (InvalidOperationException)
                {
                    Assert.Fail("Failed to generate new value for {0}.", model.Alpha2);
                }
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            LegalEntityTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            LegalEntityTypeModel value = provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<LegalEntityTypeModel> generatedValues = new List<LegalEntityTypeModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            LegalEntityTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<LegalEntityTypeModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<LegalEntityTypeModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            LegalEntityTypeModel expectedTyped = new LegalEntityTypeModel()
            {
                CountryAlpha2 = "PT",
                Abbreviation = "CRL",
                Name = "Cooperativa de Responsabilidade Limitada"
            };
            string expected = string.Concat(expectedTyped.CountryAlpha2, "|", expectedTyped.Abbreviation);

            ProviderResult providerResult = CreateProvider();
            LegalEntityTypeProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private LegalEntityTypeModel[] GetAllLegalEntityTypes()
        {
            return ResourceReader.ReadContentsFromFile<LegalEntityTypeModel[]>("Falsus.Providers.Company.UnitTests.Datasets.LegalEntityTypes.json");
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Company.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(
            int rowCount = 1, 
            int? seed = default, 
            LegalEntityTypeProviderConfiguration configuration = default)
        {
            if (configuration == null)
            {
                configuration = new LegalEntityTypeProviderConfiguration();
            }

            LegalEntityTypeProvider provider = new LegalEntityTypeProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<LegalEntityTypeModel> property = new DataGeneratorProperty<LegalEntityTypeModel>("LegalEntityType")
                .FromProvider(provider);

            provider.Load(property, rowCount);

            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            return new ProviderResult()
            {
                Provider = provider,
                Property = property,
                Context = context
            };
        }

        private struct ProviderResult
        {
            public LegalEntityTypeProvider Provider;
            public DataGeneratorProperty<LegalEntityTypeModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
