namespace Falsus.Providers.Text.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoremIpsumProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new LoremIpsumProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithNoFragmentCountThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration());
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithNegativeFragmentCountThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = -2
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 2
            });
            LoremIpsumProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue("a", "z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 2
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "This is a sample phrase.";

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1,
                FragmentType = LoremIpsumFragmentType.Sentence
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(value));
        }

        [TestMethod]
        public void GetRowValueWithWordReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ContainsSpaces = false,
                StartsWithUppercase = true,
                EndsWithPeriod = false
            };

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1,
                FragmentType = LoremIpsumFragmentType.Word
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            var actual = new
            {
                ContainsSpaces = value.Contains(' '),
                StartsWithUppercase = value.ToLower()[0] == value[0],
                EndsWithPeriod = value.EndsWith('.')
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithTwoWordsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ContainsOneSpace = true,
                StartsWithUppercase = true,
                EndsWithPeriod = false
            };

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 2,
                FragmentType = LoremIpsumFragmentType.Word
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            var actual = new
            {
                ContainsOneSpace = value.Count(u => u == ' ') == 1,
                StartsWithUppercase = value.ToLower()[0] == value[0],
                EndsWithPeriod = value.EndsWith('.')
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSentenceReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ContainsSpaces = true,
                StartsWithUppercase = true,
                EndsWithPeriod = true,
                ContainsOnePeriod = true,
            };

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1,
                FragmentType = LoremIpsumFragmentType.Sentence
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            var actual = new
            {
                ContainsSpaces = value.Count(u => u == ' ') > 1,
                StartsWithUppercase = value.ToUpper()[0] == value[0],
                EndsWithPeriod = value.EndsWith('.'),
                ContainsOnePeriod = value.Count(u => u == '.') == 1
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithTwoSentencesReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ContainsSpaces = true,
                StartsWithUppercase = true,
                EndsWithPeriod = true,
                ContainsPeriods = true,
            };

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 2,
                FragmentType = LoremIpsumFragmentType.Sentence
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            var actual = new
            {
                ContainsSpaces = value.Count(u => u == ' ') > 1,
                StartsWithUppercase = value.ToUpper()[0] == value[0],
                EndsWithPeriod = value.EndsWith('.'),
                ContainsPeriods = value.Count(u => u == '.') >= 1
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithParagraphReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ContainsSpaces = true,
                StartsWithUppercase = true,
                EndsWithPeriod = true,
                ContainsPeriods = true,
            };

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1,
                FragmentType = LoremIpsumFragmentType.Paragraph
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            var actual = new
            {
                ContainsSpaces = value.Count(u => u == ' ') > 1,
                StartsWithUppercase = value.ToUpper()[0] == value[0],
                EndsWithPeriod = value.EndsWith('.'),
                ContainsPeriods = value.Count(u => u == '.') >= 1
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithTwoParagraphsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ContainsSpaces = true,
                StartsWithUppercase = true,
                EndsWithPeriod = true,
                ContainsPeriods = true,
                ContainsLineBreak = true
            };

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 2,
                FragmentType = LoremIpsumFragmentType.Paragraph
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            var actual = new
            {
                ContainsSpaces = value.Count(u => u == ' ') > 1,
                StartsWithUppercase = value.ToUpper()[0] == value[0],
                EndsWithPeriod = value.EndsWith('.'),
                ContainsPeriods = value.Count(u => u == '.') >= 1,
                ContainsLineBreak = value.Contains(Environment.NewLine)
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueGenerates10KValues()
        {
            // Arrange
            int expectedRowCount = 10000;
            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1,
                FragmentType = LoremIpsumFragmentType.Sentence
            }, expectedRowCount);
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "This is a sample phrase.";

            ProviderResult providerResult = CreateProvider(new LoremIpsumProviderConfiguration()
            {
                FragmentCount = 1,
            });
            LoremIpsumProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(LoremIpsumProviderConfiguration configuration, int rowCount = 1, int? seed = default)
        {
            LoremIpsumProvider provider = new LoremIpsumProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("LoremIpsum")
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
            public LoremIpsumProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
