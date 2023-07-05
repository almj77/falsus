namespace Falsus.Providers.Person.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GenderProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            string minValue = "A";
            string maxValue = "J";

            ProviderResult providerResult = CreateProvider();
            GenderProvider provider = providerResult.Provider;

            string value = provider.GetRangedRowValue(minValue, maxValue, Array.Empty<string>());

            // Assert
            Assert.IsTrue(value == GenderProvider.FemaleGenderIdentifier);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "M";

            ProviderResult providerResult = CreateProvider();
            GenderProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            GenderProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(value == GenderProvider.MaleGenderIdentifier || value == GenderProvider.FemaleGenderIdentifier);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 89675;
            string expected = "M";

            ProviderResult providerResult = CreateProvider(1, seed);
            GenderProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            GenderProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => u == GenderProvider.MaleGenderIdentifier || u == GenderProvider.FemaleGenderIdentifier);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            GenderProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            string[] excludedObjects = new string[2] { GenderProvider.MaleGenderIdentifier, GenderProvider.FemaleGenderIdentifier };

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "F";

            ProviderResult providerResult = CreateProvider();
            GenderProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            GenderProvider provider = new GenderProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Gender")
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
            public GenderProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
