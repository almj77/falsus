namespace Falsus.Providers.Location.UnitTests
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
    public class ContinentProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = ContinentProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            ContinentProvider provider = new ContinentProvider();

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
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "AN";

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<ContinentModel> property = new DataGeneratorProperty<ContinentModel>("Continent")
                .WithArgument(ContinentProvider.CountryArgumentName, countryProperty);

            ContinentProvider provider = new ContinentProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            // Act
            ContinentModel value = provider.GetRangedRowValue(
                new ContinentModel()
                {
                    Alpha2 = "AF"
                },
                new ContinentModel()
                {
                    Alpha2 = "AS"
                },
                Array.Empty<ContinentModel>());

            // Assert
            Assert.AreEqual(expected, value.Alpha2);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "EU";

            ProviderResult providerResult = CreateProvider();
            ContinentProvider provider = providerResult.Provider;

            // Act
            ContinentModel actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual.Alpha2);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 882117;
            string expected = "SA";

            ProviderResult providerResult = CreateProvider(1, seed);
            ContinentProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            ContinentModel actual = provider.GetRowValue(context, Array.Empty<ContinentModel>());

            // Assert
            Assert.AreEqual(expected, actual.Alpha2);
        }

        [TestMethod]
        public void GetRowValueWithInvalidIdReturnsExpectedValue()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            ContinentProvider provider = providerResult.Provider;

            // Act
            ContinentModel actual = provider.GetRowValue("-");

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<ContinentModel> property = new DataGeneratorProperty<ContinentModel>("Continent")
                .WithArgument(ContinentProvider.CountryArgumentName, countryProperty);

            ContinentProvider provider = new ContinentProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<ContinentModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<ContinentModel> property = new DataGeneratorProperty<ContinentModel>("Continent")
                .WithArgument(ContinentProvider.CountryArgumentName, countryProperty);

            ContinentProvider provider = new ContinentProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<ContinentModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ContinentModel[] excludedObjects = GetAllContinents();

            ProviderResult providerResult = CreateProvider();
            ContinentProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            int expectedRowCount = models.Length;
            List<ContinentModel> generatedValues = new List<ContinentModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<ContinentModel> property = new DataGeneratorProperty<ContinentModel>("Continent")
                .WithArgument(ContinentProvider.CountryArgumentName, countryProperty);

            foreach (var model in models)
            {
                ContinentProvider provider = new ContinentProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act              
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<ContinentModel>()));
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
            ProviderResult providerResult = CreateProvider();
            ContinentProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            ContinentModel value = provider.GetRowValue(context, Array.Empty<ContinentModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<ContinentModel> generatedValues = new List<ContinentModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            ContinentProvider provider = providerResult.Provider;
            DataGeneratorProperty<ContinentModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<ContinentModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            ContinentModel expected = new ContinentModel()
            {
                Alpha2 = "EU"
            };

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<ContinentModel> property = new DataGeneratorProperty<ContinentModel>("Continent")
                .WithArgument(ContinentProvider.CountryArgumentName, countryProperty);

            ContinentProvider provider = new ContinentProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected.Alpha2, actual);
        }

        private ContinentModel[] GetAllContinents()
        {
            return ResourceReader.ReadContentsFromFile<ContinentModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Continents.json");
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            ContinentProvider provider = new ContinentProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<ContinentModel> property = new DataGeneratorProperty<ContinentModel>("Continent")
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
            public ContinentProvider Provider;
            public DataGeneratorProperty<ContinentModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
