namespace Falsus.Providers.Location.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CityProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = CityProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            CityProvider provider = new CityProvider();

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
            CityProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(new CityModel(), new CityModel(), Array.Empty<CityModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<CityModel>>(), Array.Empty<CityModel>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            CityModel expectedTyped = new CityModel()
            {
                CountryAlpha2 = "PT",
                Name = "Porto",
                Region = "Porto"
            };
            var expected = new
            {
                expectedTyped.CountryAlpha2,
                expectedTyped.Name,
                expectedTyped.Region
            };

            ProviderResult providerResult = CreateProvider();
            CityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            CityModel actualTyped = provider.GetRowValue(string.Concat(expectedTyped.CountryAlpha2, "|", expectedTyped.Name));

            var actual = new
            {
                actualTyped.CountryAlpha2,
                actualTyped.Name,
                actualTyped.Region
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 6652;
            CityModel expectedTyped = new CityModel()
            {
                CountryAlpha2 = "US",
                Name = "Jacksonville",
                Region = "Illinois"
            };
            var expected = new
            {
                expectedTyped.CountryAlpha2,
                expectedTyped.Name,
                expectedTyped.Region
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            CityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            CityModel actualTyped = provider.GetRowValue(context, Array.Empty<CityModel>());

            var actual = new
            {
                actualTyped.CountryAlpha2,
                actualTyped.Name,
                actualTyped.Region
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
            DataGeneratorProperty<CityModel> property = new DataGeneratorProperty<CityModel>("City")
                .WithArgument(CityProvider.CountryArgumentName, countryProperty);

            CityProvider provider = new CityProvider();

            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<CityModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CityModel> property = new DataGeneratorProperty<CityModel>("City")
                .WithArgument(CityProvider.CountryArgumentName, countryProperty);

            CityProvider provider = new CityProvider();

            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<CityModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            CityModel[] excludedObjects = GetAllCities();

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            int expectedRowCount = models.Length;
            List<CityModel> generatedValues = new List<CityModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CityModel> property = new DataGeneratorProperty<CityModel>("City")
                .WithArgument(CityProvider.CountryArgumentName, countryProperty);


            foreach (var model in models)
            {
                CityProvider provider = new CityProvider();

                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<CityModel>()));
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
            CityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            CityModel value = provider.GetRowValue(context, Array.Empty<CityModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGenerates10kValues()
        {
            // Arrange
            int expectedRowCount = 10000;

            // Act
            List<CityModel> generatedValues = new List<CityModel>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            CityProvider provider = providerResult.Provider;
            DataGeneratorProperty<CityModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<CityModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            CityModel expectedTyped = new CityModel()
            {
                CountryAlpha2 = "PT",
                Name = "Porto",
                Region = "Porto"
            };
            string expected = string.Concat(expectedTyped.CountryAlpha2, "|", expectedTyped.Name);

            ProviderResult providerResult = CreateProvider();
            CityProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private CityModel[] GetAllCities()
        {
            return ResourceReader.ReadContentsFromFile<CityModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Cities.json");
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            CityProvider provider = new CityProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<CityModel> property = new DataGeneratorProperty<CityModel>("City")
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
            public CityProvider Provider;
            public DataGeneratorProperty<CityModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
