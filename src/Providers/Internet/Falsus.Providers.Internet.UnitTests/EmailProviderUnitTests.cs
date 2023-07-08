namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;

    [TestClass]
    public class EmailProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new EmailProvider(null);
        }

        [TestMethod]
        public void ConstructorWithEmptyTextPropertiesReturnsRandomValue()
        {
            // Arrange
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email");
            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" }
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(value.Contains("hotmail.com"));
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = EmailProvider.TextArgumentsName,
                ArgumentType = typeof(string),
                ArgumentCount = 1
            };

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration());

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
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorWithEmptyDomainsThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration());
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });

            provider.InitializeRandomizer();
            provider.Load(property, 1);

            // Act
            provider.GetRangedRowValue("a", "z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });

            provider.InitializeRandomizer();
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, new Dictionary<string, IDataGeneratorProperty[]>());

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "myfirstname@hotmail.com";

            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });

            provider.InitializeRandomizer();
            provider.Load(property, 1);

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 633285;
            string expected = "0aXvV5@hotmail.com";

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email");
            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer(seed);
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithOnePropertyAndDomainReturnsExpectedValues()
        {
            // Arrange
            string expected = "adam@hotmail.com";

            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FirstName", "Adam");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithTwoPropertyAndOneDomainReturnsExpectedValues()
        {
            // Arrange
            string expected = "adam.sandler@hotmail.com";

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FirstName", "Adam");
            row.Add("LastName", "Sandler");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithTwoPropertyAndDomainsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Email = "adam.sandler",
                ContainsDomain = true
            };

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[3] { "hotmail.com", "gmail.com", "outlook.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FirstName", "Adam");
            row.Add("LastName", "Sandler");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            string[] tokens = value.Split('@');
            var actual = new
            {
                Email = tokens[0],
                ContainsDomain = tokens[1] == "hotmail.com" || tokens[1] == "gmail.com" || tokens[1] == "outlook.com"
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSpacesReturnsExpectedValues()
        {
            // Arrange
            string expected = "adam.michael.sandler@hotmail.com";

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FirstName", "Adam Michael");
            row.Add("LastName", "Sandler");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithOddCharsReturnsExpectedValues()
        {
            // Arrange
            string expected = "jose.rocha@hotmail.com";

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FirstName", "José");
            row.Add("LastName", "Rocha");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithMultipleOddCharsReturnsExpectedValues()
        {
            // Arrange
            string expected = "jose.rocha@hotmail.com";

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("FirstName", "José");
            row.Add("LastName", "Rocha~|");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

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
                RowCount = 1000000,
                ContainsTokens = true
            };

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            List<string> generatedValues = new List<string>();
            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1000000);

            Dictionary<string, object>[] rows = new Dictionary<string, object>[1000000];
            for (int i = 0; i < 1000000; i++)
            {
                rows[i] = new Dictionary<string, object>();
                rows[i].Add("FirstName", string.Concat("F", i));
                rows[i].Add("LastName", string.Concat("L", i));
            }

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(rows[i], i, expected.RowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            bool containsTokens = true;
            for (int i = 0; i < 1000000; i++)
            {
                if (!generatedValues[i].Contains(rows[i]["FirstName"].ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || !generatedValues[i].Contains(rows[i]["LastName"].ToString(), StringComparison.InvariantCultureIgnoreCase)
                    || !generatedValues[i].Contains("hotmail.com"))
                {
                    containsTokens = false;
                    break;
                }
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u)),
                ContainsTokens = containsTokens
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string expected = "ray.chestnut";

            DataGeneratorProperty<string> lastNameProperty = new DataGeneratorProperty<string>("LastName");
            DataGeneratorProperty<string> firstNameProperty = new DataGeneratorProperty<string>("FirstName");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Email")
                .WithArgument(EmailProvider.TextArgumentsName, firstNameProperty)
                .WithArgument(EmailProvider.TextArgumentsName, lastNameProperty);

            EmailProvider provider = new EmailProvider(new EmailProviderConfiguration()
            {
                Domains = new string[1] { "hotmail.com" },
            });
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
