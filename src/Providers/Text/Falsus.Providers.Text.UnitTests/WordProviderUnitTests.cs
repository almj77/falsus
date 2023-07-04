namespace Falsus.Providers.Text.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Text.Models;
    using Falsus.Shared.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WordProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new WordProvider(null);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            int expected = 0;

            WordProvider provider = new WordProvider(new WordProviderConfiguration());

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            int actual = arguments.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            var expected = "chafe";

            ProviderResult providerResult = CreateProvider();
            WordProvider provider = providerResult.Provider;

            // Act
            string value = provider.GetRangedRowValue("certify", "chaff", Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "bulk";
            ProviderResult providerResult = CreateProvider();
            WordProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(actual) && actual == expected);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            WordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(value));
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 36472;
            string expected = "loads";

            ProviderResult providerResult = CreateProvider(1, seed);
            WordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            string[] excludedObjects = GetAllWords(new WordProviderConfiguration());

            ProviderResult providerResult = CreateProvider();
            WordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValuesWithAllWordTypesExcludedThrowsException()
        {
            // Arrange
            string[] excludedObjects = GetAllWords(new WordProviderConfiguration());

            WordType[] excludedWordTypes = new WordType[6]
            {
                WordType.Adjective,
                WordType.Adverb,
                WordType.Conjunction,
                WordType.Interjection,
                WordType.Preposition,
                WordType.Verb
            };

            ProviderResult providerResult = CreateProvider(1000, excludedWordTypes: excludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetRowValuesWithAllWordTypesExcludedExceptAdjectiveReturnsExpectedValues()
        {
            // Arrange
            WordProviderConfiguration configuration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[5]
                {
                    WordType.Adverb,
                    WordType.Conjunction,
                    WordType.Interjection,
                    WordType.Preposition,
                    WordType.Verb
                }
            };

            int rowCount = 1000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(rowCount, excludedWordTypes: configuration.ExcludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < rowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, rowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            string[] validWords = this.GetAllWords(configuration);

            Assert.IsTrue(generatedValues.All(u => validWords.Contains(u)));
        }

        [TestMethod]
        public void GetRowValuesWithAllWordTypesExcludedExceptAdverbReturnsExpectedValues()
        {
            // Arrange
            WordProviderConfiguration configuration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[5]
                {
                    WordType.Adjective,
                    WordType.Conjunction,
                    WordType.Interjection,
                    WordType.Preposition,
                    WordType.Verb
                }
            };

            int rowCount = 1000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(rowCount, excludedWordTypes: configuration.ExcludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < rowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, rowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            string[] validWords = this.GetAllWords(configuration);

            Assert.IsTrue(generatedValues.All(u => validWords.Contains(u)));
        }

        [TestMethod]
        public void GetRowValuesWithAllWordTypesExcludedExceptConjunctionReturnsExpectedValues()
        {
            // Arrange
            WordProviderConfiguration configuration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[5]
                {
                    WordType.Adjective,
                    WordType.Adverb,
                    WordType.Interjection,
                    WordType.Preposition,
                    WordType.Verb
                }
            };

            int rowCount = 1000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(rowCount, excludedWordTypes: configuration.ExcludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < rowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, rowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            string[] validWords = this.GetAllWords(configuration);

            Assert.IsTrue(generatedValues.All(u => validWords.Contains(u)));
        }

        [TestMethod]
        public void GetRowValuesWithAllWordTypesExcludedExceptInterjectionReturnsExpectedValues()
        {
            // Arrange
            WordProviderConfiguration configuration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[5]
                {
                    WordType.Adjective,
                    WordType.Adverb,
                    WordType.Conjunction,
                    WordType.Preposition,
                    WordType.Verb
                }
            };

            int rowCount = 1000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(rowCount, excludedWordTypes: configuration.ExcludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < rowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, rowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            string[] validWords = this.GetAllWords(configuration);

            Assert.IsTrue(generatedValues.All(u => validWords.Contains(u)));
        }

        [TestMethod]
        public void GetRowValuesWithAllWordTypesExcludedExceptPrepositionReturnsExpectedValues()
        {
            // Arrange
            WordProviderConfiguration configuration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[5]
                {
                    WordType.Adjective,
                    WordType.Adverb,
                    WordType.Conjunction,
                    WordType.Interjection,
                    WordType.Verb
                }
            };

            int rowCount = 1000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(rowCount, excludedWordTypes: configuration.ExcludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < rowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, rowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            string[] validWords = this.GetAllWords(configuration);

            Assert.IsTrue(generatedValues.All(u => validWords.Contains(u)));
        }

        [TestMethod]
        public void GetRowValuesWithAllWordTypesExcludedExceptVerbReturnsExpectedValues()
        {
            // Arrange
            WordProviderConfiguration configuration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[5]
                {
                    WordType.Adjective,
                    WordType.Adverb,
                    WordType.Conjunction,
                    WordType.Interjection,
                    WordType.Preposition
                }
            };

            int rowCount = 1000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(rowCount, excludedWordTypes: configuration.ExcludedWordTypes);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < rowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, rowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            string[] validWords = this.GetAllWords(configuration);

            Assert.IsTrue(generatedValues.All(u => validWords.Contains(u)));
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            WordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            // Act
            for (int i = 0; i < 1000000; i++)
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
            // Arrange
            string expected = "chalk";

            ProviderResult providerResult = CreateProvider();
            WordProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private string[] GetAllWords(WordProviderConfiguration configuration)
        {
            Dictionary<WordType, WordModel[]> wordsByType = ResourceReader.ReadContentsFromFile<Dictionary<WordType, WordModel[]>>("Falsus.Providers.Text.UnitTests.Datasets.Words.json");
            List<WordModel> models = new List<WordModel>();

            foreach (KeyValuePair<WordType, WordModel[]> item in wordsByType)
            {
                if (configuration.ExcludedWordTypes == null || !configuration.ExcludedWordTypes.Contains(item.Key))
                {
                    foreach (WordModel model in item.Value)
                    {
                        model.WordType = item.Key;
                        models.Add(model);
                    }
                }
            }

            return models.Select(u => u.Word).ToArray();
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default, WordType[] excludedWordTypes = default)
        {
            WordProvider provider = new WordProvider(new WordProviderConfiguration()
            {
                ExcludedWordTypes = excludedWordTypes
            });

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Word")
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
            public WordProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
