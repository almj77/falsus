﻿namespace Falsus.Providers.Location.UnitTests
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
    public class StreetNameProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = StreetNameProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            StreetNameProvider provider = new StreetNameProvider();

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
            StreetNameProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue("A", "Z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            StreetNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            var expected = "First street";

            ProviderResult providerResult = CreateProvider();
            StreetNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 95574;
            string expected = "Acceso a Calete";

            ProviderResult providerResult = CreateProvider(1, seed);
            StreetNameProvider provider = providerResult.Provider;
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
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Street")
                .WithArgument(StreetNameProvider.CountryArgumentName, countryProperty);

            StreetNameProvider provider = new StreetNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
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
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Street")
                .WithArgument(StreetNameProvider.CountryArgumentName, countryProperty);

            StreetNameProvider provider = new StreetNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllCountries()
        {
            // Arrange
            CountryModel[] models = GetAllCountries();
            models = models.Where(u => u.Alpha2 != "UM" && u.Alpha2 != "GS" && u.Alpha2 != "TK").ToArray();
            int expectedRowCount = models.Length;
            List<string> generatedValues = new List<string>();
            List<string> failedCodes = new List<string>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Street")
                .WithArgument(StreetNameProvider.CountryArgumentName, countryProperty);

            StreetNameProvider provider = new StreetNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            foreach (var model in models)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
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
            StreetNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGenerates1kValues()
        {
            // Arrange
            int expectedRowCount = 1000;

            List<string> generatedValues = new List<string>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            StreetNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
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
            // Arrange
            string expected = "Second street";

            ProviderResult providerResult = CreateProvider();
            StreetNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.UnitTests.Datasets.Countries.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            StreetNameProvider provider = new StreetNameProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Street")
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
            public StreetNameProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
