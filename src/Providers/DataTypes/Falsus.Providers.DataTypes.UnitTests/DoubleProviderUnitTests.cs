namespace Falsus.Providers.DataTypes.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.DataTypes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DoubleProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            double minDouble = int.MaxValue;
            double maxDouble = double.MaxValue;

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;

            // Act
            double value = provider.GetRangedRowValue(minDouble, maxDouble, Array.Empty<double>());

            // Assert
            Assert.IsTrue(value > minDouble && value < maxDouble);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRangedRowValueWithOverflownThrowsException()
        {
            // Arrange
            double minDouble = 1.7976931348623150E+308;
            double maxDouble = double.MaxValue;
            double[] excludedObjects = new double[8] {
                1.7976931348623150E+308,
                1.7976931348623151E+308,
                1.7976931348623152E+308,
                1.7976931348623153E+308,
                1.7976931348623154E+308,
                1.7976931348623155E+308,
                1.7976931348623156E+308,
                1.7976931348623157E+308
            };

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(minDouble, maxDouble, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            double expected = 1275;

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;

            // Act
            double actual = provider.GetRowValue(expected.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            double? value = provider.GetRowValue(context, Array.Empty<double>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 669823;
            double expected = 0.21746937754446147;

            ProviderResult providerResult = CreateProvider(1, seed);
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            double? actual = provider.GetRowValue(context, Array.Empty<double>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            double? value = provider.GetRowValue(context, Array.Empty<WeightedRange<double>>(), Array.Empty<double>());

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

            List<double?> generatedValues = new List<double?>();
            WeightedRange<double>[] excludedRanges = new WeightedRange<double>[1]
            {
                new WeightedRange<double>()
                {
                    MinValue = 0,
                    MaxValue = double.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorProperty<double> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expected.RowCount,
                    property,
                    property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Where(u => u.HasValue).Select(u => u.Value).ToArray()));
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

            List<double?> generatedValues = new List<double?>();
            WeightedRange<double>[] excludedRanges = new WeightedRange<double>[1]
            {
                new WeightedRange<double>()
                {
                    MinValue = 0,
                    MaxValue = double.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorProperty<double> property = providerResult.Property;

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

            List<double?> generatedValues = new List<double?>();
            WeightedRange<double>[] excludedRanges = new WeightedRange<double>[2]
            {
                new WeightedRange<double>()
                {
                    MinValue = double.MinValue,
                    MaxValue = 0,
                    Weight = 0.25f
                },
                new WeightedRange<double>()
                {
                    MinValue = 1000,
                    MaxValue = double.MaxValue - 1000,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorProperty<double> property = providerResult.Property;

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
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && u.Value > 0 && u.Value < 1000)
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

            List<double?> generatedValues = new List<double?>();
            WeightedRange<double>[] excludedRanges = new WeightedRange<double>[2]
            {
                new WeightedRange<double>()
                {
                    MinValue = 0,
                    MaxValue = double.MaxValue,
                    Weight = 0.25f
                },
                new WeightedRange<double>()
                {
                    MinValue = double.MinValue,
                    MaxValue = 0,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorProperty<double> property = providerResult.Property;

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expectedRowCount,
                    property,
                    property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueForRangedWithRangesAndAllValuesExcludedThrowsException()
        {
            // Arrange
            int expectedRowCount = 1000;

            List<double?> generatedValues = new List<double?>();
            WeightedRange<double>[] excludedRanges = new WeightedRange<double>[1]
            {
                new WeightedRange<double>()
                {
                    MinValue = double.MinValue,
                    MaxValue = 1.7976931348623150E+308,
                    Weight = 0.25f
                }
            };

            double[] excludedObjects = new double[8] {
                1.7976931348623150E+308,
                1.7976931348623151E+308,
                1.7976931348623152E+308,
                1.7976931348623153E+308,
                1.7976931348623154E+308,
                1.7976931348623155E+308,
                1.7976931348623156E+308,
                1.7976931348623157E+308
            };

            // Act
            DoubleProvider provider = new DoubleProvider();
            DataGeneratorProperty<double> property = new RangedDataGeneratorProperty<double>("double")
                .FromProvider(provider)
                .WithWeightedRanges(new WeightedRange<double>()
                {
                    MinValue = 0,
                    MaxValue = 100,
                    Weight = 1f
                });

            provider.InitializeRandomizer();
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, excludedObjects));
            }
        }

        [TestMethod]
        public void GetRowValueGeneratesTenThousandUniqueValues()
        {
            // Arrange
            int expectedRowCount = 10000;

            List<double?> generatedValues = new List<double?>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorProperty<double> property = providerResult.Property;

            // Act
            for (int i = 0; i < 10000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expectedRowCount,
                    property,
                    property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, generatedValues.Where(u => u.HasValue).Select(u => u.Value).ToArray()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<double?> generatedValues = new List<double?>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            DoubleProvider provider = providerResult.Provider;
            DataGeneratorProperty<double> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(
                    new Dictionary<string, object>(),
                    i,
                    expectedRowCount,
                    property,
                    property.Arguments);

                generatedValues.Add(provider.GetRowValue(context, Array.Empty<double>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = string.Concat("7598", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "4");

            ProviderResult providerResult = CreateProvider();
            DoubleProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(7598.4);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            DoubleProvider provider = new DoubleProvider();
            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<double> property = new DataGeneratorProperty<double>("double")
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
            public DoubleProvider Provider;
            public DataGeneratorProperty<double> Property;
            public DataGeneratorContext Context;
        }
    }
}
