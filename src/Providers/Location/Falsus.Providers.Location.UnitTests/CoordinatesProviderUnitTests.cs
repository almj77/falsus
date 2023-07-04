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
    public class CoordinatesProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = CoordinatesProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            CoordinatesProvider provider = new CoordinatesProvider();

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
            CoordinatesProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(
                new CoordinatesModel(string.Empty, string.Empty),
                new CoordinatesModel(string.Empty, string.Empty),
                Array.Empty<CoordinatesModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CoordinatesProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<CoordinatesModel>>(), Array.Empty<CoordinatesModel>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            var expected = new
            {
                Latitude = "123",
                Longitude = "321"
            };

            ProviderResult providerResult = CreateProvider();
            CoordinatesProvider provider = providerResult.Provider;

            // Act
            CoordinatesModel value = provider.GetRowValue(string.Concat(expected.Latitude, ",", expected.Longitude));

            var actual = new
            {
                value.Latitude,
                value.Longitude
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 5127;
            var expected = new
            {
                Latitude = "-10.9756592",
                Longitude = "-37.0696606"
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            CoordinatesProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            CoordinatesModel value = provider.GetRowValue(context, Array.Empty<CoordinatesModel>());

            var actual = new
            {
                value.Latitude,
                value.Longitude
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
            DataGeneratorProperty<CoordinatesModel> property = new DataGeneratorProperty<CoordinatesModel>("Coordinates")
                .WithArgument(CoordinatesProvider.CountryArgumentName, countryProperty);

            CoordinatesProvider provider = new CoordinatesProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<CoordinatesModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CoordinatesModel> property = new DataGeneratorProperty<CoordinatesModel>("Coordinates")
                .WithArgument(CoordinatesProvider.CountryArgumentName, countryProperty);

            CoordinatesProvider provider = new CoordinatesProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<CoordinatesModel>());
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            models = models.Where(u => u.Alpha2 != "UM" && u.Alpha2 != "GS" && u.Alpha2 != "TK").ToArray();
            int expectedRowCount = models.Length;
            List<CoordinatesModel> generatedValues = new List<CoordinatesModel>();
            List<string> failedCodes = new List<string>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<CoordinatesModel> property = new DataGeneratorProperty<CoordinatesModel>("Coordinates")
                .WithArgument(CoordinatesProvider.CountryArgumentName, countryProperty);

            CoordinatesProvider provider = new CoordinatesProvider();
            provider.InitializeRandomizer();
            provider.Load(property, models.Length);

            foreach (var model in models)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, generatedValues.ToArray()));
                }
                catch (InvalidOperationException)
                {
                    failedCodes.Add(model.Alpha2);
                }
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            if (failedCodes.Any())
            {
                Assert.Fail("Failed to generate new value for {0}.", string.Join(", ", failedCodes));
            }
            else
            {
                Assert.AreEqual(expectedRowCount, actualRowCount);
            }
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            CoordinatesProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            CoordinatesModel value = provider.GetRowValue(context, Array.Empty<CoordinatesModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGenerates1kValues()
        {
            // Arrange
            int expectedRowCount = 1000;

            // Act
            List<CoordinatesModel> generatedValues = new List<CoordinatesModel>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            CoordinatesProvider provider = providerResult.Provider;
            DataGeneratorProperty<CoordinatesModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<CoordinatesModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            CoordinatesModel expectedTyped = new CoordinatesModel("ABC", "DEF");
            string expected = "ABC,DEF";

            ProviderResult providerResult = CreateProvider();
            CoordinatesProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            CoordinatesProvider provider = new CoordinatesProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<CoordinatesModel> property = new DataGeneratorProperty<CoordinatesModel>("Coordinates")
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
            public CoordinatesProvider Provider;
            public DataGeneratorProperty<CoordinatesModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
