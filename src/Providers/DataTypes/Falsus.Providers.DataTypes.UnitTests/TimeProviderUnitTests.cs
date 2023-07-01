namespace Falsus.Providers.DataTypes.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.DataTypes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TimeProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            TimeSpan minSpan = TimeSpan.FromHours(2);
            TimeSpan maxSpan = TimeSpan.FromHours(3);

            ProviderResult providerResult = CreateProvider();
            TimeProvider provider = providerResult.Provider;

            // Act
            TimeSpan value = provider.GetRangedRowValue(minSpan, maxSpan, Array.Empty<TimeSpan>());

            // Assert
            Assert.IsTrue(value > minSpan && value < maxSpan);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRangedRowValueWithOverflownThrowsException()
        {
            // Arrange
            TimeSpan minSpan = TimeSpan.FromMilliseconds(990);
            TimeSpan maxSpan = TimeSpan.FromMilliseconds(1000);
            TimeSpan[] excludedObjects = new TimeSpan[11] {
                TimeSpan.FromMilliseconds(990),
                TimeSpan.FromMilliseconds(991),
                TimeSpan.FromMilliseconds(992),
                TimeSpan.FromMilliseconds(993),
                TimeSpan.FromMilliseconds(994),
                TimeSpan.FromMilliseconds(995),
                TimeSpan.FromMilliseconds(996),
                TimeSpan.FromMilliseconds(997),
                TimeSpan.FromMilliseconds(998),
                TimeSpan.FromMilliseconds(999),
                TimeSpan.FromMilliseconds(1000)
            };

            ProviderResult providerResult = CreateProvider();
            TimeProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(minSpan, maxSpan, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            TimeSpan expected = TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(25));

            ProviderResult providerResult = CreateProvider();
            TimeProvider provider = providerResult.Provider;

            // Act
            TimeSpan actual = provider.GetRowValue(expected.Ticks.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            TimeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            TimeSpan? value = provider.GetRowValue(context, Array.Empty<TimeSpan>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 986652;
            TimeSpan expected = TimeSpan.FromTicks(611140560000);

            ProviderResult providerResult = CreateProvider(1, seed);
            TimeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            TimeSpan actual = provider.GetRowValue(context, Array.Empty<TimeSpan>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            TimeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            TimeSpan? value = provider.GetRowValue(context, Array.Empty<WeightedRange<TimeSpan>>(), Array.Empty<TimeSpan>());

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

            List<TimeSpan?> generatedValues = new List<TimeSpan?>();
            WeightedRange<TimeSpan>[] excludedRanges = new WeightedRange<TimeSpan>[1]
            {
                new WeightedRange<TimeSpan>()
                {
                    MinValue = TimeSpan.FromMinutes(1),
                    MaxValue = TimeSpan.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            TimeProvider provider = providerResult.Provider;
            DataGeneratorProperty<TimeSpan> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, Array.Empty<TimeSpan>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value < TimeSpan.FromMinutes(1))
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

            List<TimeSpan?> generatedValues = new List<TimeSpan?>();
            WeightedRange<TimeSpan>[] excludedRanges = new WeightedRange<TimeSpan>[1]
            {
                new WeightedRange<TimeSpan>()
                {
                    MinValue = TimeSpan.FromMinutes(1),
                    MaxValue = TimeSpan.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            TimeProvider provider = providerResult.Provider;
            DataGeneratorProperty<TimeSpan> property = providerResult.Property;

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
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value < TimeSpan.FromMinutes(1))
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

            List<TimeSpan?> generatedValues = new List<TimeSpan?>();
            WeightedRange<TimeSpan>[] excludedRanges = new WeightedRange<TimeSpan>[2]
            {
                new WeightedRange<TimeSpan>()
                {
                    MinValue = TimeSpan.FromMinutes(1),
                    MaxValue = TimeSpan.FromMinutes(30),
                    Weight = 0.25f
                },
                new WeightedRange<TimeSpan>()
                {
                    MinValue = TimeSpan.FromHours(2),
                    MaxValue = TimeSpan.FromHours(4),
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            TimeProvider provider = providerResult.Provider;
            DataGeneratorProperty<TimeSpan> property = providerResult.Property;

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
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && (u.Value < TimeSpan.FromMinutes(1) || (u >= TimeSpan.FromMinutes(30) && u < TimeSpan.FromHours(2)) || u >= TimeSpan.FromHours(4)))
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

            List<TimeSpan?> generatedValues = new List<TimeSpan?>();
            WeightedRange<TimeSpan>[] excludedRanges = new WeightedRange<TimeSpan>[2]
            {
                new WeightedRange<TimeSpan>()
                {
                    MinValue = TimeSpan.Zero,
                    MaxValue = TimeSpan.MaxValue,
                    Weight = 0.25f
                },
                new WeightedRange<TimeSpan>()
                {
                    MinValue = TimeSpan.MinValue,
                    MaxValue = TimeSpan.Zero,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            TimeProvider provider = providerResult.Provider;
            DataGeneratorProperty<TimeSpan> property = providerResult.Property;

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
            var expected = new
            {
                RowCount = 1000000,
                AllWithinDay = true,
            };

            List<TimeSpan?> generatedValues = new List<TimeSpan?>();
            ProviderResult providerResult = CreateProvider(expected.RowCount);
            TimeProvider provider = providerResult.Provider;
            DataGeneratorProperty<TimeSpan> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<TimeSpan>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);
            bool allWithinDay = generatedValues.All(u => u > TimeSpan.Zero && u < TimeSpan.FromHours(24));

            var actual = new
            {
                RowCount = actualRowCount,
                AllWithinDay = allWithinDay
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            TimeSpan expectedTimeSpan = TimeSpan.FromHours(3).Add(TimeSpan.FromMinutes(12));
            string expected = expectedTimeSpan.Ticks.ToString();

            ProviderResult providerResult = CreateProvider();
            TimeProvider provider = providerResult.Provider;

            string actual = provider.GetValueId(TimeSpan.FromHours(3).Add(TimeSpan.FromMinutes(12)));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            TimeProvider provider = new TimeProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<TimeSpan> property = new DataGeneratorProperty<TimeSpan>("Time")
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
            public TimeProvider Provider;
            public DataGeneratorProperty<TimeSpan> Property;
            public DataGeneratorContext Context;
        }
    }
}
