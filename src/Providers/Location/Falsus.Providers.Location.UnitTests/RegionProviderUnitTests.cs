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
    public class RegionProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = RegionProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            RegionProvider provider = new RegionProvider();

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
            RegionProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(new RegionModel(), new RegionModel(), Array.Empty<RegionModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            RegionProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<RegionModel>>(), Array.Empty<RegionModel>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            RegionModel expectedTyped = new RegionModel()
            {
                Code = "PT-13",
                CountryAlpha2 = "PT",
                Name = "Porto"
            };

            var expected = new
            {
                expectedTyped.Code,
                expectedTyped.CountryAlpha2,
                expectedTyped.Name
            };

            ProviderResult providerResult = CreateProvider();
            RegionProvider provider = providerResult.Provider;

            // Act
            RegionModel actualTyped = provider.GetRowValue(string.Concat(expectedTyped.CountryAlpha2, "|", expectedTyped.Code));

            var actual = new
            {
                actualTyped.Code,
                actualTyped.CountryAlpha2,
                actualTyped.Name
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 44782;
            RegionModel expectedTyped = new RegionModel()
            {
                CountryAlpha2 = "VN",
                Code = "VN-63",
                Name = "Hà Nam"
            };

            var expected = new
            {
                expectedTyped.Code,
                expectedTyped.CountryAlpha2,
                expectedTyped.Name
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            RegionProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            RegionModel actualTyped = provider.GetRowValue(context, Array.Empty<RegionModel>());

            var actual = new
            {
                actualTyped.Code,
                actualTyped.CountryAlpha2,
                actualTyped.Name
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
            DataGeneratorProperty<RegionModel> property = new DataGeneratorProperty<RegionModel>("Region")
                .WithArgument(RegionProvider.CountryArgumentName, countryProperty);

            RegionProvider provider = new RegionProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<RegionModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<RegionModel> property = new DataGeneratorProperty<RegionModel>("Region")
                .WithArgument(RegionProvider.CountryArgumentName, countryProperty);

            RegionProvider provider = new RegionProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<RegionModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            RegionProvider provider = providerResult.Provider;
            DataGeneratorProperty<RegionModel> property = providerResult.Property;

            // Act
            RegionModel[] excludedObjects = GetAllRegions();
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            int expectedRowCount = models.Length;
            List<RegionModel> generatedValues = new List<RegionModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<RegionModel> property = new DataGeneratorProperty<RegionModel>("Region")
                .WithArgument(RegionProvider.CountryArgumentName, countryProperty);

            foreach (var model in models)
            {
                RegionProvider provider = new RegionProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<RegionModel>()));
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
            RegionProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            RegionModel value = provider.GetRowValue(context, Array.Empty<RegionModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<RegionModel> generatedValues = new List<RegionModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            RegionProvider provider = providerResult.Provider;
            DataGeneratorProperty<RegionModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<RegionModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            RegionModel expectedTyped = new RegionModel()
            {
                CountryAlpha2 = "PT",
                Code = "PT-13",
                Name = "Porto"
            };
            string expected = string.Concat(expectedTyped.CountryAlpha2, "|", expectedTyped.Code);

            ProviderResult providerResult = CreateProvider();
            RegionProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private RegionModel[] GetAllRegions()
        {
            return ResourceReader.ReadContentsFromFile<RegionModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Regions.json");
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            RegionProvider provider = new RegionProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<RegionModel> property = new DataGeneratorProperty<RegionModel>("Region")
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
            public RegionProvider Provider;
            public DataGeneratorProperty<RegionModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
