namespace Falsus.Providers.Company.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    [TestClass]
    public class CompanyNameProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new CompanyNameProvider(null);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName1 = CompanyNameProvider.CountryArgumentName,
                ArgumentType1 = typeof(CountryModel),
                ArgumentName2 = CompanyNameProvider.LanguageArgumentName,
                ArgumentType2 = typeof(string),
                ArgumentCount = 2
            };

            CompanyNameProvider provider = new CompanyNameProvider();

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            var actual = new
            {
                ArgumentName1 = arguments.Keys.First(),
                ArgumentType1 = arguments.Values.First(),
                ArgumentName2 = arguments.Keys.ElementAt(1),
                ArgumentType2 = arguments.Values.ElementAt(1),
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
            CompanyNameProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(string.Empty, string.Empty, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CompanyNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithEmptyFallbackLanguageThrowsException()
        {
            // Arrange
            CompanyNameProviderConfiguration config = new CompanyNameProviderConfiguration()
            {
                AttemptFallbackByLanguage = true,
                FallbackToLanguage = true,
                FallbackLanguageAlpha2 = null
            };

            ProviderResult providerResult = CreateProvider(configuration: config);
            CompanyNameProvider provider = providerResult.Provider;

            // Act
            provider.GetRowValue("PT|CRL");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidFallbackLanguageThrowsException()
        {
            // Arrange
            CompanyNameProviderConfiguration config = new CompanyNameProviderConfiguration()
            {
                AttemptFallbackByLanguage = true,
                FallbackToLanguage = true,
                FallbackLanguageAlpha2 = "YY"
            };

            ProviderResult providerResult = CreateProvider(configuration: config);
            CompanyNameProvider provider = providerResult.Provider;

            // Act
            provider.GetRowValue("PT|CRL");
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            var expected = "John Doe Inc.";

            ProviderResult providerResult = CreateProvider();
            CompanyNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue("John Doe Inc.");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 553779;
            string expected = "ФОП Natasha";

            ProviderResult providerResult = CreateProvider(1, seed);
            CompanyNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("CompanyName")
                .WithArgument(CompanyNameProvider.CountryArgumentName, countryProperty);

            CompanyNameProviderConfiguration configuration = new CompanyNameProviderConfiguration()
            {
                AttemptFallbackByLanguage = false,
                FallbackToLanguage = false,
            };

            CompanyNameProvider provider = new CompanyNameProvider(configuration);
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("CompanyName")
                .WithArgument(CompanyNameProvider.CountryArgumentName, countryProperty);

            CompanyNameProviderConfiguration config = new CompanyNameProviderConfiguration()
            {
                AttemptFallbackByLanguage = false,
                FallbackToLanguage = false,
            };

            CompanyNameProvider provider = new CompanyNameProvider(config);
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueWithLanguageFallbackCanGenerateValue()
        {
            // Arrange
            string expected = "Ferguson, Bartrop and Kirkland";
            CountryModel countryModel = new CountryModel()
            {
                Alpha2 = "AS"
            };

            CompanyNameProviderConfiguration config = new CompanyNameProviderConfiguration()
            {
                AttemptFallbackByLanguage = true,
                FallbackToLanguage = false,
                FallbackLanguageAlpha2 = string.Empty,
            };

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("country");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("CompanyName")
                .WithArgument(CompanyNameProvider.CountryArgumentName, countryProperty);

            CompanyNameProvider provider = new CompanyNameProvider(config);
            provider.InitializeRandomizer(22591);
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("country", countryModel);
            DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            int expectedRowCount = models.Length;
            List<string> generatedValues = new List<string>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("CompanyName")
                .WithArgument(CompanyNameProvider.CountryArgumentName, countryProperty);

            foreach (var model in models)
            {
                CompanyNameProvider provider = new CompanyNameProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
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
            CompanyNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<string> generatedValues = new List<string>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            CompanyNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            string expected = "Jane Doe Corp.";

            ProviderResult providerResult = CreateProvider();
            CompanyNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Company.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(
            int rowCount = 1,
            int? seed = default,
            CompanyNameProviderConfiguration configuration = default)
        {
            if (configuration == null)
            {
                configuration = new CompanyNameProviderConfiguration();
            }

            CompanyNameProvider provider = new CompanyNameProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("CompanyName")
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
            public CompanyNameProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
