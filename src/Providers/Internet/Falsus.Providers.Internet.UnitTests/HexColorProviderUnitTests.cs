namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Falsus.GeneratorProperties;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HexColorProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            HexColorProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue("a", "z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            HexColorProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "#FFAABB";

            ProviderResult providerResult = CreateProvider();
            HexColorProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(expected.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            HexColorProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            Regex regex = new Regex("^#([0-9A-F]{3}){1,2}$");
            bool isMatch = regex.IsMatch(value.ToUpper());

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 9261578;
            string expected = "#85D4F0";

            ProviderResult providerResult = CreateProvider(1, seed);
            HexColorProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithOverflownThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            HexColorProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;
            DataGeneratorProperty<string> property = providerResult.Property;

            List<string> colors = new List<string>();
            for (int i = 0; i < Convert.ToInt32(0x1000000); i++)
            {
                colors.Add(string.Format("#{0:X6}", i));
            }

            // Act
            provider.GetRowValue(context, colors.ToArray());
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000000,
                AllValid = true,
            };
            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            HexColorProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            Regex regex = new Regex("^#([0-9A-F]{3}){1,2}$");

            var actual = new
            {
                RowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u)),
                AllValid = generatedValues.All(u => regex.IsMatch(u.ToUpper()))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "#FFAABB";

            ProviderResult providerResult = CreateProvider();
            HexColorProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            HexColorProvider provider = new HexColorProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Hex")
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
            public HexColorProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
