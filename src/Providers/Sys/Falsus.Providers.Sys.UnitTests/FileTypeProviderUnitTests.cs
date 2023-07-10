namespace Falsus.Providers.Sys.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileTypeProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = FileTypeProvider.MimeTypeArgumentName,
                ArgumentType = typeof(MimeTypeModel),
                ArgumentCount = 1
            };

            FileTypeProvider provider = new FileTypeProvider();

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            var actual = new
            {
                ArgumentName = arguments.Keys.First(),
                ArgumentType = arguments.Values.First(),
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
            FileTypeProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(new FileTypeModel(), new FileTypeModel(), Array.Empty<FileTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FileTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<FileTypeModel>>(), Array.Empty<FileTypeModel>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            FileTypeModel expectedTyped = new FileTypeModel()
            {
                Category = "Image",
                Extension = "svg",
                Icon = "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",
                IsCommon = true,
                Name = "Scalable Vector Graphic"
            };

            var expected = new
            {
                expectedTyped.Category,
                expectedTyped.Extension,
                expectedTyped.Icon,
                expectedTyped.IsCommon,
                expectedTyped.Name
            };

            ProviderResult providerResult = CreateProvider();
            FileTypeProvider provider = providerResult.Provider;

            // Act
            FileTypeModel actualTyped = provider.GetRowValue(string.Concat(expectedTyped.Category, "|", expectedTyped.Extension));

            var actual = new
            {
                actualTyped.Category,
                actualTyped.Extension,
                actualTyped.Icon,
                actualTyped.IsCommon,
                actualTyped.Name
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 44782;
            FileTypeModel expectedTyped = new FileTypeModel()
            {
                Category = "Video",
                Extension = "mov",
                Icon = "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",
                IsCommon = true,
                Name = "Apple QuickTime Movie"
            };

            var expected = new
            {
                expectedTyped.Category,
                expectedTyped.Extension,
                expectedTyped.Icon,
                expectedTyped.IsCommon,
                expectedTyped.Name
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            FileTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            FileTypeModel actualTyped = provider.GetRowValue(context, Array.Empty<FileTypeModel>());

            var actual = new
            {
                actualTyped.Category,
                actualTyped.Extension,
                actualTyped.Icon,
                actualTyped.IsCommon,
                actualTyped.Name
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidMimeTypeThrowsError()
        {
            // Arrange
            DataGeneratorProperty<MimeTypeModel> mimeTypeProperty = new DataGeneratorProperty<MimeTypeModel>("MimeType");
            DataGeneratorProperty<FileTypeModel> property = new DataGeneratorProperty<FileTypeModel>("FileType")
                .WithArgument(FileTypeProvider.MimeTypeArgumentName, mimeTypeProperty);

            FileTypeProvider provider = new FileTypeProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("MimeType", new MimeTypeModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<FileTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullCountryThrowsError()
        {
            // Arrange
            DataGeneratorProperty<MimeTypeModel> mimeTypeProperty = new DataGeneratorProperty<MimeTypeModel>("MimeType");
            DataGeneratorProperty<FileTypeModel> property = new DataGeneratorProperty<FileTypeModel>("FileType")
                .WithArgument(FileTypeProvider.MimeTypeArgumentName, mimeTypeProperty);

            FileTypeProvider provider = new FileTypeProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("MimeType", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<FileTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FileTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<FileTypeModel> property = providerResult.Property;

            // Act
            FileTypeModel[] excludedObjects = GetAllFileTypes();
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueCanGenerateForAllMimeTypes()
        {
            // Arrange
            MimeTypeModel[] models = GetAllMimeTypes();
            int expectedRowCount = models.Length;
            List<FileTypeModel> generatedValues = new List<FileTypeModel>();

            DataGeneratorProperty<MimeTypeModel> mimeTypeProperty = new DataGeneratorProperty<MimeTypeModel>("MimeType");
            DataGeneratorProperty<FileTypeModel> property = new DataGeneratorProperty<FileTypeModel>("FileType")
                .WithArgument(FileTypeProvider.MimeTypeArgumentName, mimeTypeProperty);

            foreach (var model in models)
            {
                FileTypeProvider provider = new FileTypeProvider();
                provider.InitializeRandomizer();
                provider.Load(property, 1);

                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("MimeType", model);
                DataGeneratorContext context = new DataGeneratorContext(row, 1, 1, property, property.Arguments);

                // Act
                try
                {
                    generatedValues.Add(provider.GetRowValue(context, Array.Empty<FileTypeModel>()));
                }
                catch (InvalidOperationException)
                {
                    Assert.Fail("Failed to generate new value for {0}.", model.MimeType);
                }
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FileTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            FileTypeModel value = provider.GetRowValue(context, Array.Empty<FileTypeModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<FileTypeModel> generatedValues = new List<FileTypeModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            FileTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<FileTypeModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<FileTypeModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            FileTypeModel expectedTyped = new FileTypeModel()
            {
                Category = "",
                Extension = "",
                Icon = "",
                IsCommon = true,
                Name = ""
            };

            string expected = string.Concat(expectedTyped.Category, "|", expectedTyped.Extension);

            ProviderResult providerResult = CreateProvider();
            FileTypeProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private FileTypeModel[] GetAllFileTypes()
        {
            return ResourceReader.ReadContentsFromFile<FileTypeModel[]>("Falsus.Providers.Sys.UnitTests.Datasets.FileTypes.json");
        }

        private MimeTypeModel[] GetAllMimeTypes()
        {
            List<MimeTypeModel> models = ResourceReader.ReadContentsFromFile<List<MimeTypeModel>>("Falsus.Providers.Sys.UnitTests.Datasets.MimeTypes.json");
            List<MimeTypeModel> newModels = new List<MimeTypeModel>();

            foreach (MimeTypeModel model in models.Where(u => u.Aliases != null && u.Aliases.Any()))
            {
                foreach (string alias in model.Aliases)
                {
                    if (!models.Any(u => u.MimeType == alias) && !newModels.Any(u => u.MimeType == alias))
                    {
                        newModels.Add(new MimeTypeModel()
                        {
                            Aliases = model.Aliases,
                            Category = model.Category,
                            DeprecatedBy = model.DeprecatedBy,
                            Extension = model.Extension,
                            MimeType = alias,
                            Name = model.Name,
                            IsCommon = model.IsCommon
                        });
                    }
                }
            }

            models.AddRange(newModels);

            return models.ToArray();
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            FileTypeProvider provider = new FileTypeProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<FileTypeModel> property = new DataGeneratorProperty<FileTypeModel>("FileType")
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
            public FileTypeProvider Provider;
            public DataGeneratorProperty<FileTypeModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
