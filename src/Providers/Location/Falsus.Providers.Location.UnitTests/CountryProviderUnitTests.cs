namespace Falsus.Providers.Location.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CountryProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentCount = 0
            };

            CountryProvider provider = new CountryProvider();

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            var actual = new
            {
                ArgumentCount = arguments.Count
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            var expected = "QA";

            ProviderResult providerResult = CreateProvider();
            CountryProvider provider = providerResult.Provider;

            // Act
            CountryModel value = provider.GetRangedRowValue(new CountryModel()
            {
                Alpha2 = "PY",
            }, new CountryModel()
            {
                Alpha2 = "RO",

            },
            Array.Empty<CountryModel>());

            // Assert
            Assert.AreEqual(expected, value.Alpha2);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CountryProvider provider = providerResult.Provider;

            // Act
            CountryModel actual = provider.GetRowValue("PT");

            // Assert
            Assert.IsTrue(actual != null && actual.Alpha2 == "PT");
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CountryProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            CountryModel value = provider.GetRowValue(context, Array.Empty<CountryModel>());

            // Assert
            Assert.IsTrue(value != null && !string.IsNullOrEmpty(value.Alpha2));
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 36472;
            string expected = "NL";

            ProviderResult providerResult = CreateProvider(1, seed);
            CountryProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            CountryModel actual = provider.GetRowValue(context, Array.Empty<CountryModel>());

            // Assert
            Assert.AreEqual(expected, actual.Alpha2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            CountryModel[] excludedObjects = GetAllCountries();

            ProviderResult providerResult = CreateProvider();
            CountryProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<CountryModel> generatedValues = new List<CountryModel>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            CountryProvider provider = providerResult.Provider;
            DataGeneratorProperty<CountryModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<CountryModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            CountryModel expected = new CountryModel()
            {
                Alpha2 = "PT"
            };

            ProviderResult providerResult = CreateProvider();
            CountryProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected.Alpha2, actual);
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            CountryProvider provider = new CountryProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<CountryModel> property = new DataGeneratorProperty<CountryModel>("Country")
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
            public CountryProvider Provider;
            public DataGeneratorProperty<CountryModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
