namespace Falsus.Providers.Number.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Number;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FloatProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            float minFloat = 0.25f;
            float maxFloat = 0.5f;

            ProviderResult providerResult = CreateProvider();
            FloatProvider provider = providerResult.Provider;

            // Act
            float value = provider.GetRangedRowValue(minFloat, maxFloat, Array.Empty<float>());

            // Assert
            Assert.IsTrue(value > minFloat && value < maxFloat);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRangedRowValueWithOverflownThrowsException()
        {
            // Arrange
            float minFloat = 3.402823460E+38f;
            float maxFloat = float.MaxValue;
            float[] excludedObjects = new float[7] {
                3.402823460E+38f,
                3.402823461E+38f,
                3.402823462E+38f,
                3.402823463E+38f,
                3.402823464E+38f,
                3.402823465E+38f,
                3.402823466E+38f
            };

            ProviderResult providerResult = CreateProvider();
            FloatProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(minFloat, maxFloat, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            float expected = 694.856f;

            ProviderResult providerResult = CreateProvider();
            FloatProvider provider = providerResult.Provider;

            // Act
            float actual = provider.GetRowValue(expected.ToString());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FloatProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            float? value = provider.GetRowValue(context, Array.Empty<float>());

            // Assert
            Assert.IsTrue(value.HasValue);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 552378;
            float expected = 0.9764444f;

            ProviderResult providerResult = CreateProvider(1, seed);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            float? actual = provider.GetRowValue(context, Array.Empty<float>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FloatProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            float? value = provider.GetRowValue(context, Array.Empty<WeightedRange<float>>(), Array.Empty<float>());

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

            List<float?> generatedValues = new List<float?>();
            WeightedRange<float>[] excludedRanges = new WeightedRange<float>[1]
            {
                new WeightedRange<float>()
                {
                    MinValue = 0,
                    MaxValue = float.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorProperty<float> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, Array.Empty<float>()));
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

            List<float?> generatedValues = new List<float?>();
            WeightedRange<float>[] excludedRanges = new WeightedRange<float>[1]
            {
                new WeightedRange<float>()
                {
                    MinValue = 0,
                    MaxValue = float.MaxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorProperty<float> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, new DataGeneratorProperty<float>("float"), new Dictionary<string, IDataGeneratorProperty[]>());
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

            List<float?> generatedValues = new List<float?>();
            WeightedRange<float>[] excludedRanges = new WeightedRange<float>[2]
            {
                new WeightedRange<float>()
                {
                    MinValue = -2000,
                    MaxValue = -1000,
                    Weight = 0.25f
                },
                new WeightedRange<float>()
                {
                    MinValue = 5000,
                    MaxValue = 9000,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorProperty<float> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, new DataGeneratorProperty<float>("float"), new Dictionary<string, IDataGeneratorProperty[]>());
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u.HasValue),
                UniquevalueCount = generatedValues.Select(u => u.Value).Distinct().Count(),
                ValuesWithinExpectedRange = generatedValues.All(u => u.HasValue && (u.Value < -2000 || (u.Value > -1000 && u.Value < 5000) || (u.Value > 9000)))
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

            List<float?> generatedValues = new List<float?>();
            WeightedRange<float>[] excludedRanges = new WeightedRange<float>[2]
            {
                new WeightedRange<float>()
                {
                    MinValue = 0,
                    MaxValue = float.MaxValue,
                    Weight = 0.25f
                },
                new WeightedRange<float>()
                {
                    MinValue = float.MinValue,
                    MaxValue = 0,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorProperty<float> property = providerResult.Property;

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, new DataGeneratorProperty<float>("float"), new Dictionary<string, IDataGeneratorProperty[]>());
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Select(u => u.Value).ToArray()));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueForRangedWithRangesAndAllValuesExcludedThrowsException()
        {
            // Arrange
            int expectedRowCount = 1000;

            WeightedRange<float>[] excludedRanges = new WeightedRange<float>[1]
            {
                new WeightedRange<float>()
                {
                    MinValue = float.MinValue,
                    MaxValue =  3.40282347E+38f - 0.00000100E+38f,
                    Weight = 0.25f
                }
            };

            List<float?> generatedValues = new List<float?>(new float?[3]{
                3.40282245E+38f,
                3.40282246E+38f,
                3.40282247E+38f
            });

            FloatProvider provider = new FloatProvider();
            DataGeneratorProperty<float> property = new RangedDataGeneratorProperty<float>("float")
                .FromProvider(provider)
                .WithWeightedRanges(new WeightedRange<float>()
                {
                    MinValue = 0,
                    MaxValue = 100,
                    Weight = 1f
                });

            provider.InitializeRandomizer();

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.Where(u => u.HasValue).Select(u => u.Value).ToArray()));
            }
        }

        [TestMethod]
        public void GetRowValueGeneratesTenThousandUniqueValues()
        {
            // Arrange
            int expectedRowCount = 10000;

            List<float?> generatedValues = new List<float?>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorProperty<float> property = providerResult.Property;

            // Act
            for (int i = 0; i < 10000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
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

            List<float?> generatedValues = new List<float?>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            FloatProvider provider = providerResult.Provider;
            DataGeneratorProperty<float> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<float>()));
            }

            int actualRowCount = generatedValues.Count(u => u.HasValue);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = string.Concat("521", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "477");

            ProviderResult providerResult = CreateProvider();
            FloatProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(521.477f);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            FloatProvider provider = new FloatProvider();
            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<float> property = new DataGeneratorProperty<float>("float")
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
            public FloatProvider Provider;
            public DataGeneratorProperty<float> Property;
            public DataGeneratorContext Context;
        }
    }
}
