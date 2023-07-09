namespace Falsus.Providers.Sys.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SemanticVersionProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            SemanticVersionModel minValue = new SemanticVersionModel(1, 1, 0);
            SemanticVersionModel maxValue = new SemanticVersionModel(1, 2, 0);

            ProviderResult providerResult = CreateProvider();
            SemanticVersionProvider provider = providerResult.Provider;

            // Act
            SemanticVersionModel value = provider.GetRangedRowValue(minValue, maxValue, Array.Empty<SemanticVersionModel>());

            // Assert
            Assert.IsTrue(value > minValue && value < maxValue);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "1.20.1";

            ProviderResult providerResult = CreateProvider();
            SemanticVersionProvider provider = providerResult.Provider;

            // Act
            SemanticVersionModel actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            SemanticVersionModel value = provider.GetRowValue(context, Array.Empty<SemanticVersionModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 141;
            SemanticVersionModel expected = new SemanticVersionModel(3, 18, 32);

            SemanticVersionProviderConfiguration providerConfiguration = new SemanticVersionProviderConfiguration()
            {
                MaxMajorVersion = 10,
                MaxMinorVersion = 99,
                MaxPatchVersion = 99
            };

            ProviderResult providerResult = CreateProvider(1, seed, providerConfiguration);
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            SemanticVersionModel actual = provider.GetRowValue(context, Array.Empty<SemanticVersionModel>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueForRangedReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            SemanticVersionModel value = provider.GetRowValue(context, Array.Empty<WeightedRange<SemanticVersionModel>>(), Array.Empty<SemanticVersionModel>());

            // Assert
            Assert.IsTrue(value != null);
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

            SemanticVersionModel minValue = new SemanticVersionModel(1, 0, SemanticVersionModel.Alpha, 2);
            SemanticVersionModel maxValue = new SemanticVersionModel(10, 0, 0);

            List<SemanticVersionModel> generatedValues = new List<SemanticVersionModel>();
            WeightedRange<SemanticVersionModel>[] excludedRanges = new WeightedRange<SemanticVersionModel>[1]
            {
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue,
                    MaxValue = maxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorProperty<SemanticVersionModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, Array.Empty<SemanticVersionModel>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u != null),
                ValuesWithinExpectedRange = generatedValues.All(u => u != null && (u <= minValue || u > maxValue))
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

            SemanticVersionModel minValue = new SemanticVersionModel(1, 0, 0);
            SemanticVersionModel maxValue = new SemanticVersionModel(100, 0, 0);

            List<SemanticVersionModel> generatedValues = new List<SemanticVersionModel>();
            WeightedRange<SemanticVersionModel>[] excludedRanges = new WeightedRange<SemanticVersionModel>[1]
            {
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue,
                    MaxValue = maxValue,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorProperty<SemanticVersionModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.ToArray()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u != null),
                UniquevalueCount = generatedValues.Distinct().Count(),
                ValuesWithinExpectedRange = generatedValues.All(u => u != null && (u <= minValue || u > maxValue))
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

            SemanticVersionModel minValue1 = new SemanticVersionModel(1, 0, 0);
            SemanticVersionModel maxValue1 = new SemanticVersionModel(10, 0, 0);

            SemanticVersionModel minValue2 = new SemanticVersionModel(30, 0, 0);
            SemanticVersionModel maxValue2 = new SemanticVersionModel(40, 0, 0);

            SemanticVersionModel minValue3 = new SemanticVersionModel(60, 0, 0);
            SemanticVersionModel maxValue3 = new SemanticVersionModel(70, 0, 0);

            List<SemanticVersionModel> generatedValues = new List<SemanticVersionModel>();
            WeightedRange<SemanticVersionModel>[] excludedRanges = new WeightedRange<SemanticVersionModel>[3]
            {
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue1,
                    MaxValue = maxValue1,
                    Weight = 0.25f
                },
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue2,
                    MaxValue = maxValue2,
                    Weight = 0.25f
                },
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue3,
                    MaxValue = maxValue3,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expected.RowCount);
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorProperty<SemanticVersionModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.ToArray()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u != null),
                UniquevalueCount = generatedValues.Distinct().Count(),
                ValuesWithinExpectedRange = generatedValues.All(u => u != null && (u <= minValue1 || (u > maxValue1 && u <= minValue2) || (u > maxValue2 && u <= minValue3) || u > maxValue3))
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

            SemanticVersionModel minValue1 = new SemanticVersionModel(0, 0, 0);
            SemanticVersionModel maxValue1 = new SemanticVersionModel(10, 0, 0);

            SemanticVersionModel minValue2 = new SemanticVersionModel(10, 0, 0);
            SemanticVersionModel maxValue2 = new SemanticVersionModel(int.MaxValue, int.MaxValue, int.MaxValue);

            List<SemanticVersionModel> generatedValues = new List<SemanticVersionModel>();
            WeightedRange<SemanticVersionModel>[] excludedRanges = new WeightedRange<SemanticVersionModel>[2]
            {
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue1,
                    MaxValue = maxValue1,
                    Weight = 0.25f
                },
                new WeightedRange<SemanticVersionModel>()
                {
                    MinValue = minValue2,
                    MaxValue = maxValue2,
                    Weight = 0.25f
                }
            };

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorProperty<SemanticVersionModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, excludedRanges, generatedValues.ToArray()));
            }
        }


        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<SemanticVersionModel> generatedValues = new List<SemanticVersionModel>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            SemanticVersionProvider provider = providerResult.Provider;
            DataGeneratorProperty<SemanticVersionModel> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<SemanticVersionModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "2.15.1";

            ProviderResult providerResult = CreateProvider();
            SemanticVersionProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(new SemanticVersionModel(2, 15, 1));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default, SemanticVersionProviderConfiguration configuration = default)
        {
            SemanticVersionProvider provider;

            if (configuration == null)
            {
                provider = new SemanticVersionProvider();
            }
            else
            {
                provider = new SemanticVersionProvider(configuration);
            }

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<SemanticVersionModel> property = new DataGeneratorProperty<SemanticVersionModel>("semanticVersion")
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
            public SemanticVersionProvider Provider;
            public DataGeneratorProperty<SemanticVersionModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
