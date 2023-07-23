namespace Falsus.Providers.Internet.UnitTests
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
    public class MimeTypeProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            new MimeTypeProvider(null);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = MimeTypeProvider.FileExtensionArgumentName,
                ArgumentType = typeof(FileTypeModel),
                ArgumentCount = 1
            };

            MimeTypeProvider provider = new MimeTypeProvider(new MimeTypeProviderConfiguration());

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
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(new MimeTypeModel(), new MimeTypeModel(), Array.Empty<MimeTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<MimeTypeModel>>(), Array.Empty<MimeTypeModel>());
        }

        [TestMethod]
        public void GetRowValueWithNullReturnsNull()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(1, includeDeprecated: true);
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            MimeTypeModel actual = provider.GetRowValue(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetRowValueWithEmptyReturnsNull()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(1, includeDeprecated: true);
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            MimeTypeModel actual = provider.GetRowValue(string.Empty);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetRowValueWithInvalidTokensReturnsNull()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(1, includeDeprecated: true);
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            MimeTypeModel actual = provider.GetRowValue("a|b|c");

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetRowValueWithInvalidKeyReturnsNull()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(1, includeDeprecated: true);
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            MimeTypeModel actual = provider.GetRowValue("a|b");

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            MimeTypeModel expectedTyped = new MimeTypeModel()
            {
                Aliases = new string[]{
                    "application/javascript",
                    "application/x-ecmascript",
                    "application/x-javascript",
                    "text/ecmascript",
                    "text/javascript1.0",
                    "text/javascript1.1",
                    "text/javascript1.2",
                    "text/javascript1.3",
                    "text/javascript1.4",
                    "text/javascript1.5",
                    "text/jscript,text/livescript",
                    "text/x-ecmascript","text/x-javascript"
                },
                Category = "Application",
                DeprecatedBy = "text/javascript",
                Extension = "js",
                MimeType = "application/ecmascript",
                Name = "JavaScript",
                IsCommon = true,
            };

            var expected = new
            {
                Aliases = string.Join(',', expectedTyped.Aliases),
                expectedTyped.Category,
                expectedTyped.DeprecatedBy,
                expectedTyped.Extension,
                expectedTyped.MimeType,
                expectedTyped.Name,
                expectedTyped.IsCommon,
            };

            ProviderResult providerResult = CreateProvider(1, includeDeprecated: true);
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            MimeTypeModel actualTyped = provider.GetRowValue(string.Concat(expectedTyped.MimeType, "|", expectedTyped.Extension));

            var actual = new
            {
                Aliases = string.Join(',', actualTyped.Aliases),
                actualTyped.Category,
                actualTyped.DeprecatedBy,
                actualTyped.Extension,
                actualTyped.MimeType,
                actualTyped.Name,
                actualTyped.IsCommon,
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 7322;
            MimeTypeModel expectedTyped = new MimeTypeModel()
            {
                Category = "Text",
                Extension = "dic",
                MimeType = "text/x-c",
                Name = "C/C++ Source Code File",
                IsCommon = false
            };

            var expected = new
            {
                expectedTyped.Aliases,
                expectedTyped.Category,
                expectedTyped.DeprecatedBy,
                expectedTyped.Extension,
                expectedTyped.MimeType,
                expectedTyped.Name,
                expectedTyped.IsCommon,
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            MimeTypeModel actualTyped = provider.GetRowValue(context, Array.Empty<MimeTypeModel>());

            var actual = new
            {
                actualTyped.Aliases,
                actualTyped.Category,
                actualTyped.DeprecatedBy,
                actualTyped.Extension,
                actualTyped.MimeType,
                actualTyped.Name,
                actualTyped.IsCommon,
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidFileTypeThrowsError()
        {
            // Arrange
            DataGeneratorProperty<FileTypeModel> fileTypeProperty = new DataGeneratorProperty<FileTypeModel>("FileType");
            DataGeneratorProperty<MimeTypeModel> property = new DataGeneratorProperty<MimeTypeModel>("MimeType")
                .WithArgument(MimeTypeProvider.FileExtensionArgumentName, fileTypeProperty);

            MimeTypeProvider provider = new MimeTypeProvider(new MimeTypeProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FileType", new FileTypeModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<MimeTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullFileTypeThrowsError()
        {
            // Arrange
            DataGeneratorProperty<FileTypeModel> fileTypeProperty = new DataGeneratorProperty<FileTypeModel>("FileType");
            DataGeneratorProperty<MimeTypeModel> property = new DataGeneratorProperty<MimeTypeModel>("MimeType")
                .WithArgument(MimeTypeProvider.FileExtensionArgumentName, fileTypeProperty);

            MimeTypeProvider provider = new MimeTypeProvider(new MimeTypeProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FileType", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<MimeTypeModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<MimeTypeModel> property = providerResult.Property;

            // Act
            MimeTypeModel[] excludedObjects = GetAllMimeTypes();
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, excludedObjects.ToArray());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            MimeTypeModel value = provider.GetRowValue(context, Array.Empty<MimeTypeModel>());

            // Assert
            Assert.IsTrue(value != null);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<MimeTypeModel> generatedValues = new List<MimeTypeModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount);
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<MimeTypeModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<MimeTypeModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null && string.IsNullOrEmpty(u.DeprecatedBy));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValuesWithoutUncommon()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<MimeTypeModel> generatedValues = new List<MimeTypeModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount, excludeUncommon: true);
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<MimeTypeModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<MimeTypeModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null && string.IsNullOrEmpty(u.DeprecatedBy));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValuesWithDeprecated()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<MimeTypeModel> generatedValues = new List<MimeTypeModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount, includeDeprecated: true);
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<MimeTypeModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<MimeTypeModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValuesWithUniqueAliases()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<MimeTypeModel> generatedValues = new List<MimeTypeModel>();
            ProviderResult providerResult = CreateProvider(expectedRowCount, treatAliasesAsUnique: true);
            MimeTypeProvider provider = providerResult.Provider;
            DataGeneratorProperty<MimeTypeModel> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<MimeTypeModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            MimeTypeModel expectedTyped = new MimeTypeModel()
            {
                Aliases = null,
                Category = null,
                DeprecatedBy = null,
                Extension = null,
                MimeType = null,
                Name = null
            };
            string expected = string.Concat(expectedTyped.MimeType, "|", expectedTyped.Extension);

            ProviderResult providerResult = CreateProvider();
            MimeTypeProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private MimeTypeModel[] GetAllMimeTypes()
        {
            List<MimeTypeModel> models = ResourceReader.ReadContentsFromFile<List<MimeTypeModel>>("Falsus.Providers.Internet.UnitTests.Datasets.MimeTypes.json");

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
        private ProviderResult CreateProvider(
            int rowCount = 1,
            int? seed = default,
            bool excludeUncommon = false,
            bool includeDeprecated = false,
            bool treatAliasesAsUnique = false)
        {
            MimeTypeProvider provider = new MimeTypeProvider(new MimeTypeProviderConfiguration()
            {
                ExcludeUncommon = excludeUncommon,
                IncludeDeprecated = includeDeprecated,
                TreatAliasesAsUnique = treatAliasesAsUnique
            });

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<MimeTypeModel> property = new DataGeneratorProperty<MimeTypeModel>("MimeType")
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
            public MimeTypeProvider Provider;
            public DataGeneratorProperty<MimeTypeModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
