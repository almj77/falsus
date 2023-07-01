namespace Falsus.Providers.DataTypes.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.DataTypes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BooleanProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            BooleanProvider provider = result.Provider;

            // Act
            provider.GetRangedRowValue(true, true, Array.Empty<bool>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            BooleanProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<bool>>(), Array.Empty<bool>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            BooleanProvider provider = result.Provider;

            // Act
            bool actual = provider.GetRowValue(true.ToString());

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            BooleanProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            bool? value = provider.GetRowValue(context, Array.Empty<bool>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            bool? expected = false;
            int seed = 2049;

            ProviderResult result = CreateProvider(1, seed);
            BooleanProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            // Act
            bool? actual = provider.GetRowValue(context, Array.Empty<bool>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult result = CreateProvider();
            BooleanProvider provider = result.Provider;
            DataGeneratorContext context = result.Context;

            bool[] excludedObjects = new bool[2] { true, false };

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<bool?> generatedValues = new List<bool?>();
            ProviderResult result = CreateProvider(expectedRowCount);
            BooleanProvider provider = result.Provider;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expectedRowCount,
                    result.Property,
                    result.Property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, Array.Empty<bool>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = false.ToString();

            ProviderResult result = CreateProvider();
            BooleanProvider provider = result.Provider;

            // Act
            string actual = provider.GetValueId(false);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            BooleanProvider provider = new BooleanProvider();
            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<bool> property = new DataGeneratorProperty<bool>("bool")
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
            public BooleanProvider Provider;
            public DataGeneratorProperty<bool> Property;
            public DataGeneratorContext Context;
        }
    }
}
