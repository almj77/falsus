namespace Falsus.Providers.DataTypes.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.DataTypes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DateProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            DateTime minDate = DateTime.UtcNow.AddDays(-10);
            DateTime maxDate = DateTime.UtcNow.AddDays(10);

            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;

            // Act
            DateTime value = provider.GetRangedRowValue(minDate, maxDate, Array.Empty<DateTime>());

            // Assert
            Assert.IsTrue(value >= minDate && value < maxDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRangedRowValueWithOverflownThrowsException()
        {
            // Arrange
            DateTime minDate = new DateTime(2020, 1, 1);
            DateTime maxDate = new DateTime(2020, 1, 10);
            DateTime[] excludedObjects = new DateTime[10] {
                new DateTime(2020,1,1),
                new DateTime(2020,1,2),
                new DateTime(2020,1,3),
                new DateTime(2020,1,4),
                new DateTime(2020,1,5),
                new DateTime(2020,1,6),
                new DateTime(2020,1,7),
                new DateTime(2020,1,8),
                new DateTime(2020,1,9),
                new DateTime(2020,1,10)
            };

            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(minDate, maxDate, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            DateTime expected = new DateTime(2020, 9, 12);

            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;

            // Act
            DateTime actual = provider.GetRowValue(expected.Ticks.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            DateTime? value = provider.GetRowValue(context, Array.Empty<DateTime>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 44235;
            var expected = new DateTime(2100, 12, 15);

            ProviderResult providerResult = CreateProvider(1, seed);
            DateProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            DateTime? actual = provider.GetRowValue(context, Array.Empty<DateTime>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;
            DataGeneratorProperty<DateTime> property = providerResult.Property;

            List<DateTime> excludedObjects = new List<DateTime>();
            int dayDiff = Convert.ToInt32(Math.Floor((DateTime.MaxValue - DateTime.MinValue).TotalDays));
            for (int i = 0; i < dayDiff; i++)
            {
                excludedObjects.Add(DateTime.MinValue.AddDays(i));
            }

            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueForRangedReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            DateTime? value = provider.GetRowValue(context, Array.Empty<WeightedRange<DateTime>>(), Array.Empty<DateTime>());

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

            List<DateTime?> generatedValues = new List<DateTime?>();
            WeightedRange<DateTime>[] excludedRanges = new WeightedRange<DateTime>[1]
            {
                new WeightedRange<DateTime>()
                {
                    MinValue = new DateTime(2020,1,1),
                    MaxValue = DateTime.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            DateProvider provider = providerResult.Provider;
            DataGeneratorProperty<DateTime> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expected.RowCount,
                    property, property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, excludedRanges, Array.Empty<DateTime>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value < new DateTime(2020, 1, 1))
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

            List<DateTime?> generatedValues = new List<DateTime?>();
            WeightedRange<DateTime>[] excludedRanges = new WeightedRange<DateTime>[1]
            {
                new WeightedRange<DateTime>()
                {
                    MinValue = new DateTime(2020,1,1),
                    MaxValue = DateTime.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            DateProvider provider = providerResult.Provider;
            DataGeneratorProperty<DateTime> property = providerResult.Property;

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
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value < new DateTime(2020, 1, 1))
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

            List<DateTime?> generatedValues = new List<DateTime?>();
            WeightedRange<DateTime>[] excludedRanges = new WeightedRange<DateTime>[2]
            {
                new WeightedRange<DateTime>()
                {
                    MinValue = new DateTime(2020,1,1),
                    MaxValue = new DateTime(2020,12,31),
                    Weight = 0.25f
                },
                new WeightedRange<DateTime>()
                {
                    MinValue = new DateTime(2025,1,1),
                    MaxValue = new DateTime(2025,12,31),
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            DateProvider provider = providerResult.Provider;
            DataGeneratorProperty<DateTime> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expected.RowCount,
                    property,
                    property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                UniquevalueCount = generatedValues.Select(u => u.Value).Distinct().Count(),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue
                    && (u.Value < new DateTime(2020, 1, 1) || (u.Value >= new DateTime(2020, 12, 31) && u.Value < new DateTime(2025, 1, 1))
                    || u.Value >= new DateTime(2025, 12, 31)))
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

            List<DateTime?> generatedValues = new List<DateTime?>();
            WeightedRange<DateTime>[] excludedRanges = new WeightedRange<DateTime>[2]
            {
                new WeightedRange<DateTime>()
                {
                    MinValue = new DateTime(2020,1,1),
                    MaxValue = DateTime.MaxValue,
                    Weight = 0.25f
                },
                new WeightedRange<DateTime>()
                {
                    MinValue = DateTime.MinValue,
                    MaxValue = new DateTime(2020,1,1),
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            DateProvider provider = providerResult.Provider;
            DataGeneratorProperty<DateTime> property = providerResult.Property;

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

            List<DateTime?> generatedValues = new List<DateTime?>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            DateProvider provider = providerResult.Provider;
            DataGeneratorProperty<DateTime> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<DateTime>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            DateTime now = DateTime.UtcNow;
            string expected = now.Ticks.ToString();

            ProviderResult providerResult = CreateProvider();
            DateProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(now);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            DateProvider provider = new DateProvider();
            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<DateTime> property = new DataGeneratorProperty<DateTime>("Date")
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
            public DateProvider Provider;
            public DataGeneratorProperty<DateTime> Property;
            public DataGeneratorContext Context;
        }
    }
}
