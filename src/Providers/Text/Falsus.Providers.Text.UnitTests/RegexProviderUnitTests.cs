namespace Falsus.Providers.Text.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Falsus.GeneratorProperties;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RegexProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullConfigurationThrowsException()
        {
            // Act
            new RegexProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithEmptyPatternThrowsException()
        {
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Regex");
            RegexProvider provider = new RegexProvider(new RegexProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider("[a-z]{3}");
            RegexProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue("a", "z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider("[a-z]{3}");
            RegexProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = Guid.NewGuid().ToString();

            ProviderResult providerResult = CreateProvider("[a-z]{3}");
            RegexProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider("[a-z]{3}");
            RegexProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(value));
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 986652;
            string expected = "wlp";

            ProviderResult providerResult = CreateProvider("[a-z]{3}", 1, seed);
            RegexProvider provider = providerResult.Provider;
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
            ProviderResult providerResult = CreateProvider("[a-z]{1}");
            RegexProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            string[] excludedObjects = new string[26]
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
            };

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider("[a-z]{10}", expectedRowCount);
            RegexProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueWithEmailRegexReturnsValidValue()
        {
            // Arrange
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            ProviderResult providerResult = CreateProvider(pattern);
            RegexProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            Regex regex = new Regex(pattern);
            Match match = regex.Match(value);

            // Assert
            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void GetRowValueWithNumberRegexReturnsValidValue()
        {
            // Arrange
            string pattern = @"\d+";

            ProviderResult providerResult = CreateProvider(pattern);
            RegexProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            Regex regex = new Regex(pattern);
            Match match = regex.Match(value);

            int numericValue = -1;
            bool isValidNumber = int.TryParse(value, out numericValue);

            // Assert
            Assert.IsTrue(match.Success && isValidNumber);
        }

        public void GetRowValueWithIpAddressRegexReturnsValidValue()
        {
            string pattern = @"/^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$/";

            ProviderResult providerResult = CreateProvider(pattern);
            RegexProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            Regex regex = new Regex(pattern);
            Match match = regex.Match(value);

            // Assert
            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = Guid.NewGuid().ToString();

            ProviderResult providerResult = CreateProvider("[a-z]{3}");
            RegexProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(string pattern, int rowCount = 1, int? seed = default)
        {
            RegexProvider provider = new RegexProvider(new RegexProviderConfiguration()
            {
                Pattern = pattern
            });

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Regex")
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
            public RegexProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
