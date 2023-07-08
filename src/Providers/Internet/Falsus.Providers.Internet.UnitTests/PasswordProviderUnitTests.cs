namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PasswordProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act 
            CreateProvider(new PasswordProviderConfiguration());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithNoMaxLengthThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = 0
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithNegativeMinLengthThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = -10,
                MaxLength = 10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithNegativeMaxLengthThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = 10,
                MaxLength = -10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithMinLengthGreaterThanMaxLengthThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = 20,
                MaxLength = 10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                Length = 10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRangedRowValue("a", "z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                Length = 10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "pretend.this.is.a.password";

            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                Length = 10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithLengthReturnsExpectedValues()
        {
            // Arrange
            int expectedLength = 10;

            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                Length = 10
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string password = provider.GetRowValue(context, Array.Empty<string>());

            int actual = password.Length;

            // Assert
            Assert.AreEqual(expectedLength, actual);
        }

        [TestMethod]
        public void GetRowValueWithMinAndMaxLengthReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                MinLength = 10,
                MaxLength = 64
            };

            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = expected.MinLength,
                MaxLength = expected.MaxLength
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string password = provider.GetRowValue(context, Array.Empty<string>());

            int actual = password.Length;

            // Assert
            Assert.IsTrue(actual >= expected.MinLength && actual <= expected.MaxLength);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 46982235;
            string expected = "QxdUq76E9F9zwphdE5FpOGOK2fcanlp9";

            // Arrange
            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = 5,
                MaxLength = 50
            }, 1, seed);
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000000
            };

            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                MinLength = 24,
                MaxLength = 64,
                IncludeSpecialChars = true
            }, expected.RowCount);
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expected.RowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "pretend.this.is.a.password";

            ProviderResult providerResult = CreateProvider(new PasswordProviderConfiguration()
            {
                Length = 5
            });
            PasswordProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(PasswordProviderConfiguration configuration, int rowCount = 1, int? seed = default)
        {
            PasswordProvider provider = new PasswordProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Password")
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
            public PasswordProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
