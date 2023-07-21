namespace Falsus.Providers.Finance.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CurrencyProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = CurrencyProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            CurrencyProvider provider = new CurrencyProvider();

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
            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;

            // Act
            provider.GetRangedRowValue(new CurrencyModel(), new CurrencyModel(), Array.Empty<CurrencyModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<CurrencyModel>>(), Array.Empty<CurrencyModel>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "USD";

            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;

            // Act
            CurrencyModel actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual.Id);
        }

        [TestMethod]
        public void GetRowValueWithInvalidIdReturnsNull()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;

            // Act
            CurrencyModel actual = provider.GetRowValue("-");

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency")
                .WithArgument(CurrencyProvider.CountryArgumentName, countryProperty);

            CurrencyProvider provider = new CurrencyProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<CurrencyModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency")
                .WithArgument(CurrencyProvider.CountryArgumentName, countryProperty);

            CurrencyProvider provider = new CurrencyProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<CurrencyModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            CurrencyModel[] excludedObjects = GetAllCurrencies();

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithCountryAndAllExcludedThrowsException()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency")
                .WithArgument(CurrencyProvider.CountryArgumentName, countryProperty);

            CurrencyProvider provider = new CurrencyProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel() { Alpha2 = "PT" });
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            CurrencyModel[] excludedObjects = GetAllCurrencies();

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueWithCountryAndOneExcludedReturnsExpectedValue()
        {
            // Arrange
            CurrencyModel expectedTyped = new CurrencyModel()
            {
                CountryAlpha2 = "BT",
                CurrencyName = "Indian Rupee",
                Id = "INR",
                Symbol = "₹"
            };

            var expected = new
            {
                expectedTyped.CountryAlpha2,
                expectedTyped.CurrencyName,
                expectedTyped.Id,
                expectedTyped.Symbol
            };

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency")
                .WithArgument(CurrencyProvider.CountryArgumentName, countryProperty);

            CurrencyProvider provider = new CurrencyProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel() { Alpha2 = "BT" });
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            CurrencyModel[] excludedObjects = GetAllCurrencies().Where(u => u.Id != "BTN").ToArray();

            // Act
            CurrencyModel actualTyped = provider.GetRowValue(context, excludedObjects.ToArray());
            var actual = new
            {
                expectedTyped.CountryAlpha2,
                expectedTyped.CurrencyName,
                expectedTyped.Id,
                expectedTyped.Symbol
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            string[] unsupportedCountries = new string[16]
            {
                "BJ", "BF", "CI", "GN", "GW", "ML", "MR", "NE", "PS",
                "SN", "GS", "SS", "SD", "TG", "VE", "ZW"
            };
            models = models.Where(u => !unsupportedCountries.Contains(u.Alpha2)).ToArray();
            int expectedRowCount = models.Length;
            List<CurrencyModel> generatedValues = new List<CurrencyModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency")
                .WithArgument(CurrencyProvider.CountryArgumentName, countryProperty);

            // Act
            foreach (var model in models)
            {
                CurrencyProvider provider = new CurrencyProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);

                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<CurrencyModel>()));
                }
                catch (InvalidOperationException)
                {
                    Assert.Fail("Failed to generate new value for {0}.", model.Alpha2);
                }
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            CurrencyModel value = provider.GetRowValue(context, Array.Empty<CurrencyModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 35897745;
            var expected = "HUF";

            ProviderResult result = CreateProvider(1, seed);
            CurrencyProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            CurrencyModel value = provider.GetRowValue(context, Array.Empty<CurrencyModel>());
            var actual = value.Id;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency");
            List<CurrencyModel> generatedValues = new List<CurrencyModel>();

            CurrencyProvider provider = new CurrencyProvider();

            provider.InitializeRandomizer();
            provider.Load(property, 1);

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<CurrencyModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "EUR";

            ProviderResult result = CreateProvider();
            CurrencyProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            string actual = provider.GetValueId(new CurrencyModel() { Id = expected });

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private CurrencyModel[] GetAllCurrencies()
        {
            return ResourceReader.ReadContentsFromFile<CurrencyModel[]>("Falsus.Providers.Finance.UnitTests.Datasets.Currencies.json");
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Finance.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            CurrencyProvider provider = new CurrencyProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<CurrencyModel> property = new DataGeneratorProperty<CurrencyModel>("Currency")
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
            public CurrencyProvider Provider;
            public DataGeneratorProperty<CurrencyModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
