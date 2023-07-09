namespace Falsus.Providers.Sys.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileNameProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMinWordCountLowerThanZeroThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName");

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MaxWordCount = 3,
                MinWordCount = -1
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMinWordCountEqualToZeroThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName");

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MaxWordCount = 3,
                MinWordCount = 0
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMinExtensionCountLowerThanZeroThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName");

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MaxExtensionCount = 3,
                MinExtensionCount = -1
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMinExtensionCountEqualToZeroThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName");

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MaxExtensionCount = 3,
                MinExtensionCount = 0
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMaxWordCountLowerThanMinWordCountThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName");

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MaxWordCount = 1,
                MinWordCount = 3
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMaxExtensionCountLowerThanMinExtensionCountThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName");

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MaxExtensionCount = 1,
                MinExtensionCount = 3
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Argument1Name = FileNameProvider.FileNameArgumentName,
                Argument1Type = typeof(string),
                Argument2Name = FileNameProvider.ExtensionArgumentName,
                Argument2Type = typeof(FileTypeModel),
                ArgumentCount = 2
            };

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration());

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            var actual = new
            {
                Argument1Name = arguments.Keys.First(),
                Argument1Type = arguments.Values.First(),
                Argument2Name = arguments.Keys.ElementAt(1),
                Argument2Type = arguments.Values.ElementAt(1),
                ArgumentCount = arguments.Count
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FileNameProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(default, default, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FileNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "oblivion";

            ProviderResult providerResult = CreateProvider();
            FileNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 99871;
            string expected = "snicker.mts";

            ProviderResult providerResult = CreateProvider(1, seed);
            FileNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidFileTypeThrowsError()
        {
            // Arrange
            DataGeneratorProperty<FileTypeModel> fileTypeProperty = new DataGeneratorProperty<FileTypeModel>("FileType");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName")
                .WithArgument(FileNameProvider.ExtensionArgumentName, fileTypeProperty);

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FileType", new FileTypeModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullFileTypeThrowsError()
        {
            // Arrange
            DataGeneratorProperty<FileTypeModel> fileTypeProperty = new DataGeneratorProperty<FileTypeModel>("FileType");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName")
                .WithArgument(FileNameProvider.ExtensionArgumentName, fileTypeProperty);

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FileType", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullFileNameThrowsError()
        {
            // Arrange
            DataGeneratorProperty<string> wordProperty = new DataGeneratorProperty<string>("Word");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName")
                .WithArgument(FileNameProvider.ExtensionArgumentName, wordProperty);

            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Word", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FileNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesValuesForBoundedWordCount()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000,
                AllWithinBounds = true,
            };

            // Act
            List<string> generatedValues = new List<string>();
            ProviderResult providerResult = CreateProvider(expected.RowCount, minWordCount: 2, maxWordCount: 3, minExtensionCount: 1, maxExtensionCount: 1);
            FileNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            bool allWithinBounds = true;

            foreach (var item in generatedValues)
            {
                string[] tokens = item.Split('.').First().Split('_');
                if (tokens.Length < 2 || tokens.Length > 3)
                {
                    allWithinBounds = false;
                    break;
                }

            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u != null),
                AllWithinBounds = allWithinBounds
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueGeneratesValuesForBoundedExtensionCount()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000,
                AllWithinBounds = true,
            };

            // Act
            List<string> generatedValues = new List<string>();
            ProviderResult providerResult = CreateProvider(expected.RowCount, minWordCount: 1, maxWordCount: 1, minExtensionCount: 2, maxExtensionCount: 3);
            FileNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < expected.RowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            bool allWithinBounds = true;

            foreach (var item in generatedValues)
            {
                string[] tokens = item.Split('.').Skip(1).ToArray();
                if (tokens.Length < 2 || tokens.Length > 3)
                {
                    allWithinBounds = false;
                    break;
                }

            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => u != null),
                AllWithinBounds = allWithinBounds
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<string> generatedValues = new List<string>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            FileNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            string expected = "stuff";

            ProviderResult providerResult = CreateProvider();
            FileNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default, int minWordCount = 1, int maxWordCount = 1, int minExtensionCount = 1, int maxExtensionCount = 1)
        {
            FileNameProvider provider = new FileNameProvider(new FileNameProviderConfiguration()
            {
                MinWordCount = minWordCount,
                MaxWordCount = maxWordCount,
                MinExtensionCount = minExtensionCount,
                MaxExtensionCount = maxExtensionCount
            });

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FileName")
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
            public FileNameProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
