namespace Falsus.Providers.Person.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NationalityProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = NationalityProvider.CountryArgumentName,
                ArgumentType = typeof(CountryModel),
                ArgumentCount = 1
            };

            NationalityProvider provider = new NationalityProvider();

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
            var expected = "QA";

            ProviderResult providerResult = CreateProvider();
            NationalityProvider provider = providerResult.Provider;

            // Act
            NationalityModel value = provider.GetRangedRowValue(new NationalityModel()
            {
                CountryAlpha2 = "PR",
                Demonym = "Puerto Rican"
            }, new NationalityModel()
            {
                CountryAlpha2 = "RO",
                Demonym = "Romanian"
            },
            Array.Empty<NationalityModel>());

            // Assert
            Assert.AreEqual(expected, value.CountryAlpha2);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "Portuguese";
            NationalityModel expectedTyped = new NationalityModel()
            {
                CountryAlpha2 = "PT",
                Demonym = "Portuguese"
            };

            ProviderResult providerResult = CreateProvider();
            NationalityProvider provider = providerResult.Provider;

            // Act
            NationalityModel actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expectedTyped, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 4257869;
            string expected = "EE";

            ProviderResult providerResult = CreateProvider(1, seed);
            NationalityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            NationalityModel value = provider.GetRowValue(context, Array.Empty<NationalityModel>());

            // Assert
            Assert.AreEqual(expected, value.CountryAlpha2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<NationalityModel> property = new DataGeneratorProperty<NationalityModel>("Nationality")
                .WithArgument(NationalityProvider.CountryArgumentName, countryProperty);

            NationalityProvider provider = new NationalityProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", new CountryModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<NationalityModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<NationalityModel> property = new DataGeneratorProperty<NationalityModel>("Nationality")
                .WithArgument(NationalityProvider.CountryArgumentName, countryProperty);

            NationalityProvider provider = new NationalityProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Country", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<NationalityModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            NationalityModel[] excludedObjects = GetAllNationalities();
            ProviderResult providerResult = CreateProvider();
            NationalityProvider provider = providerResult.Provider;
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
            List<NationalityModel> generatedValues = new List<NationalityModel>();

            DataGeneratorProperty<CountryModel> countryProperty = new DataGeneratorProperty<CountryModel>("Country");
            DataGeneratorProperty<NationalityModel> property = new DataGeneratorProperty<NationalityModel>("Nationality")
                .WithArgument(NationalityProvider.CountryArgumentName, countryProperty);

            foreach (var model in models)
            {
                NationalityProvider provider = new NationalityProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Country", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                try
                {
                    // Act
                    generatedValues.Add(provider.GetRowValue(context, generatedValues.ToArray()));
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
            NationalityProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            NationalityModel value = provider.GetRowValue(context, Array.Empty<NationalityModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;
            List<NationalityModel> generatedValues = new List<NationalityModel>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            NationalityProvider provider = providerResult.Provider;
            DataGeneratorProperty<NationalityModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<NationalityModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            string expected = "Portuguese";
            NationalityModel expectedTyped = new NationalityModel()
            {
                CountryAlpha2 = "PT",
                Demonym = "Portuguese"
            };

            ProviderResult providerResult = CreateProvider();
            NationalityProvider provider = providerResult.Provider;

            // Act;
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private NationalityModel[] GetAllNationalities()
        {
            return ResourceReader.ReadContentsFromFile<NationalityModel[]>("Falsus.Providers.Person.UnitTests.Datasets.Nationalities.json");
        }

        private CountryModel[] GetAllCountries()
        {
            return ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Person.UnitTests.Datasets.Countries.json");

        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            NationalityProvider provider = new NationalityProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<NationalityModel> property = new DataGeneratorProperty<NationalityModel>("Nationality")
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
            public NationalityProvider Provider;
            public DataGeneratorProperty<NationalityModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
