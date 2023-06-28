namespace Falsus.Providers.Number.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Number;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IntegerProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            int minValue = -50;
            int maxValue = 50;

            ProviderResult providerResult = CreateProvider();
            IntegerProvider provider = providerResult.Provider;

            // Act
            int value = provider.GetRangedRowValue(minValue, maxValue, Array.Empty<int>());

            // Assert
            Assert.IsTrue(value > minValue && value < maxValue);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            int expected = 225;

            ProviderResult providerResult = CreateProvider();
            IntegerProvider provider = providerResult.Provider;

            // Act
            int actual = provider.GetRowValue(expected.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            int? value = provider.GetRowValue(context, Array.Empty<int>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 141;
            int expected = 833680146;

            ProviderResult providerResult = CreateProvider(1, seed);
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            int? actual = provider.GetRowValue(context, Array.Empty<int>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            int? value = provider.GetRowValue(context, Array.Empty<WeightedRange<int>>(), Array.Empty<int>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueForRangedWithExcludedRangeReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000,
                ValuesWithinExpectedRange = true
            };

            List<int?> generatedValues = new List<int?>();
            WeightedRange<int>[] excludedRanges = new WeightedRange<int>[1]
            {
                new WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = int.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorProperty<int> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, Array.Empty<int>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value < 0)
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedWithExcludedRangeAndUniqueReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000,
                UniquevalueCount = 1000,
                ValuesWithinExpectedRange = true
            };

            List<int?> generatedValues = new List<int?>();
            WeightedRange<int>[] excludedRanges = new WeightedRange<int>[1]
            {
                new WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = int.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorProperty<int> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                UniquevalueCount = generatedValues.Select(u => u.Value).Distinct().Count(),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value < 0)
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedWithExcludedRangesAndUniqueReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000,
                UniquevalueCount = 1000,
                ValuesWithinExpectedRange = true
            };

            List<int?> generatedValues = new List<int?>();
            WeightedRange<int>[] excludedRanges = new WeightedRange<int>[3]
            {
                new WeightedRange<int>()
                {
                    MinValue = -100,
                    MaxValue = -200,
                    Weight = 0.25f
                },
                new WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = 500,
                    Weight = 0.25f
                },
                new WeightedRange<int>()
                {
                    MinValue = 5000,
                    MaxValue = 10000,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorProperty<int> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                UniquevalueCount = generatedValues.Select(u => u.Value).Distinct().Count(),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && (u.Value < -100 || (u.Value >= -200 && u.Value < 0) || (u.Value >= 500 && u.Value < 5000) || u.Value >= 10000))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueForRangedWithAllExcludedRangesThrowsException()
        {
            // Arrange
            int expectedRowCount = 1000;

            List<int?> generatedValues = new List<int?>();
            WeightedRange<int>[] excludedRanges = new WeightedRange<int>[2]
            {
                new WeightedRange<int>()
                {
                    MinValue = 0,
                    MaxValue = int.MaxValue,
                    Weight = 0.25f
                },
                new WeightedRange<int>()
                {
                    MinValue = int.MinValue,
                    MaxValue = 0,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorProperty<int> property = providerResult.Property;

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }
        }


        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<int?> generatedValues = new List<int?>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            IntegerProvider provider = providerResult.Provider;
            DataGeneratorProperty<int> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<int>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "725";

            ProviderResult providerResult = CreateProvider();
            IntegerProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(725);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            IntegerProvider provider = new IntegerProvider();
            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<int> property = new DataGeneratorProperty<int>("float")
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
            public IntegerProvider Provider;
            public DataGeneratorProperty<int> Property;
            public DataGeneratorContext Context;
        }
    }
}
