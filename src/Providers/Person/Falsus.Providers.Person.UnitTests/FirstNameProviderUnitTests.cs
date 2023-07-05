namespace Falsus.Providers.Person.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FirstNameProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FirstNameProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue("a", "z", Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FirstNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string>>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Argument1Name = FirstNameProvider.GenderArgumentName,
                Argument1Type = typeof(string),
                Argument2Name = FirstNameProvider.NationalityArgumentName,
                Argument2Type = typeof(NationalityModel),
                ArgumentCount = 2
            };

            FirstNameProvider provider = new FirstNameProvider();

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
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string expected = "Sample Name";

            ProviderResult providerResult = CreateProvider();
            FirstNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidGenderThrowsError()
        {
            // Arrange
            DataGeneratorProperty<string> genderProperty = new DataGeneratorProperty<string>("Gender");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.GenderArgumentName, genderProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Gender", "-");
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithInvalidNationalityThrowsError()
        {
            // Arrange
            DataGeneratorProperty<NationalityModel> nationalityProperty = new DataGeneratorProperty<NationalityModel>("Nationality");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.NationalityArgumentName, nationalityProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Nationality", new NationalityModel());
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithNullNationalityThrowsError()
        {
            // Arrange
            DataGeneratorProperty<NationalityModel> nationalityProperty = new DataGeneratorProperty<NationalityModel>("Nationality");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.NationalityArgumentName, nationalityProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("Nationality", null);
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            provider.GetRowValue(context, Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 57896;
            string expected = "Marta";

            ProviderResult providerResult = CreateProvider(1, seed);
            FirstNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueCanGenerateMiniumPerNationality()
        {
            // Arrange
            int expectedRowCount = 71;
            List<string> failedRecords = new List<string>();

            NationalityModel[] models = GetAllNationalities();

            DataGeneratorProperty<NationalityModel> nationalityProperty = new DataGeneratorProperty<NationalityModel>("Nationality");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.NationalityArgumentName, nationalityProperty);

            foreach (var model in models)
            {
                List<string> generatedValues = new List<string>();

                ProviderResult providerResult = CreateProvider(expectedRowCount);
                FirstNameProvider provider = providerResult.Provider;

                for (int i = 0; i < expectedRowCount; i++)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("Nationality", new NationalityModel()
                    {
                        CountryAlpha2 = model.CountryAlpha2
                    });
                    DataGeneratorContext context = new DataGeneratorContext(row, i, expectedRowCount, nationalityProperty, nationalityProperty.Arguments);
                    try
                    {
                        // Act
                        generatedValues.Add(provider.GetRowValue(context, generatedValues.ToArray()));
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (ex.Message == "FirstNameProvider cannot generate another unique value.")
                        {
                            Assert.Fail("Failed to generate new value for {0}. Generated {1} unique records.", model.CountryAlpha2, generatedValues.Count);
                        }
                    }
                }

                int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

                // Assert
                if (actualRowCount != expectedRowCount)
                {
                    Assert.Fail("{0} does not meet minium requirements.", model.CountryAlpha2);
                }
            }
        }

        [TestMethod]
        public void GetRowValueCanGenerateMiniumMalePerNationality()
        {
            // Arrange
            int expectedRowCount = 35;
            List<string> failedRecords = new List<string>();

            NationalityModel[] models = GetAllNationalities();

            DataGeneratorProperty<NationalityModel> nationalityProperty = new DataGeneratorProperty<NationalityModel>("Nationality");
            DataGeneratorProperty<string> genderProperty = new DataGeneratorProperty<string>("Gender");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.GenderArgumentName, genderProperty)
                .WithArgument(FirstNameProvider.NationalityArgumentName, nationalityProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, expectedRowCount);

            foreach (var model in models)
            {
                List<string> generatedValues = new List<string>();

                for (int i = 0; i < expectedRowCount; i++)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("Gender", "M");
                    row.Add("Nationality", new NationalityModel()
                    {
                        CountryAlpha2 = model.CountryAlpha2
                    });
                    DataGeneratorContext context = new DataGeneratorContext(row, i, expectedRowCount, property, property.Arguments);
                    try
                    {
                        // Act
                        generatedValues.Add(provider.GetRowValue(context, generatedValues.ToArray()));
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (ex.Message == "FirstNameProvider cannot generate another unique value.")
                        {
                            Assert.Fail("Failed to generate new value for {0}. Generated {1} unique records.", model.CountryAlpha2, generatedValues.Count);
                        }
                    }
                }

                int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

                // Assert
                if (actualRowCount != expectedRowCount)
                {
                    Assert.Fail("{0} does not meet minium requirements.", model.CountryAlpha2);
                }
            }
        }

        [TestMethod]
        public void GetRowValueCanGenerateMiniumFemalePerNationality()
        {
            // Arrange
            int expectedRowCount = 35;
            List<string> failedRecords = new List<string>();

            NationalityModel[] models = GetAllNationalities();

            DataGeneratorProperty<NationalityModel> nationalityProperty = new DataGeneratorProperty<NationalityModel>("Nationality");
            DataGeneratorProperty<string> genderProperty = new DataGeneratorProperty<string>("Gender");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.GenderArgumentName, genderProperty)
                .WithArgument(FirstNameProvider.NationalityArgumentName, nationalityProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, expectedRowCount);

            foreach (var model in models)
            {
                List<string> generatedValues = new List<string>();

                for (int i = 0; i < expectedRowCount; i++)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("Gender", "F");
                    row.Add("Nationality", new NationalityModel()
                    {
                        CountryAlpha2 = model.CountryAlpha2
                    });
                    DataGeneratorContext context = new DataGeneratorContext(row, i, expectedRowCount, property, property.Arguments);
                    try
                    {
                        // Act
                        generatedValues.Add(provider.GetRowValue(context, generatedValues.ToArray()));
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (ex.Message == "FirstNameProvider cannot generate another unique value.")
                        {
                            Assert.Fail("Failed to generate new value for {0}. Generated {1} unique records.", model.CountryAlpha2, generatedValues.Count);
                        }
                    }
                }

                int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

                // Assert
                if (actualRowCount != expectedRowCount)
                {
                    Assert.Fail("{0} does not meet minium requirements.", model.CountryAlpha2);
                }
            }
        }

        [TestMethod]
        public void GetRowValueCanGenerateOneMilionMaleRecords()
        {
            // Arrange
            int expectedRowCount = 1000000;
            List<string> generatedValues = new List<string>();

            DataGeneratorProperty<string> genderProperty = new DataGeneratorProperty<string>("Gender");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.GenderArgumentName, genderProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            for (int i = 0; i < 1000000; i++)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Gender", "M");
                DataGeneratorContext context = new DataGeneratorContext(row, i, expectedRowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueCanGenerateOneMilionFemaleRecords()
        {
            // Arrange
            int expectedRowCount = 1000000;

            List<string> generatedValues = new List<string>();

            DataGeneratorProperty<string> genderProperty = new DataGeneratorProperty<string>("Gender");
            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
                .WithArgument(FirstNameProvider.GenderArgumentName, genderProperty);

            FirstNameProvider provider = new FirstNameProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);
            for (int i = 0; i < 1000000; i++)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("Gender", "F");
                DataGeneratorContext context = new DataGeneratorContext(row, i, expectedRowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            FirstNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            string value = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(value));
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;

            // Act
            List<string> generatedValues = new List<string>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            FirstNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<string> property = providerResult.Property;

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            int actualRowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u));

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange;
            string expected = "sample name";

            ProviderResult providerResult = CreateProvider();
            FirstNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private NationalityModel[] GetAllNationalities()
        {
            return ResourceReader.ReadContentsFromFile<NationalityModel[]>("Falsus.Providers.Person.UnitTests.Datasets.Nationalities.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            FirstNameProvider provider = new FirstNameProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("FirstName")
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
            public FirstNameProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
