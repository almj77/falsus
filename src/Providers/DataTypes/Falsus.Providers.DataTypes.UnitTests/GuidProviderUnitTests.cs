namespace Falsus.Providers.DataTypes.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.DataTypes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuidProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            GuidProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(Guid.NewGuid(), Guid.NewGuid(), Array.Empty<Guid>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            GuidProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            Guid value = provider.GetRowValue(context, Array.Empty<WeightedRange<Guid>>(), Array.Empty<Guid>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            Guid expected = Guid.NewGuid();

            ProviderResult providerResult = CreateProvider();
            GuidProvider provider = providerResult.Provider;

            // Act
            Guid actual = provider.GetRowValue(expected.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            GuidProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            Guid? value = provider.GetRowValue(context, Array.Empty<Guid>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 33215;
            string expected = "4ef89bf8-774e-6448-14d7-22b0f45656d3";

            ProviderResult providerResult = CreateProvider(1, seed);
            GuidProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            Guid actual = provider.GetRowValue(context, Array.Empty<Guid>());

            // Assert
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<Guid?> generatedValues = new List<Guid?>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            GuidProvider provider = providerResult.Provider;
            DataGeneratorProperty<Guid> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<Guid>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            string expected = value.ToString();

            ProviderResult providerResult = CreateProvider();
            GuidProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(value);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            GuidProvider provider = new GuidProvider();
            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<Guid> property = new DataGeneratorProperty<Guid>("Guid")
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
            public GuidProvider Provider;
            public DataGeneratorProperty<Guid> Property;
            public DataGeneratorContext Context;
        }
    }
}
