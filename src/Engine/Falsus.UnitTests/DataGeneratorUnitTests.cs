namespace Falsus.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.Configuration;
    using Falsus.GeneratorProperties;
    using Falsus.Providers;
    using Falsus.UnitTests.Fakes;
    using Falsus.UnitTests.Fakes.Models;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;

    [TestClass]
    public class DataGeneratorUnitTests
    {
        [TestMethod]
        public void GenerateWithSinglePropertyReturnsExpectedResults()
        {
            // Arrange
            var expected = new
            {
                ObjectCount = 100,
                AllInt = true,
                Unique = true
            };

            DataGenerator generator = new DataGenerator();
            generator.WithProperty<int>("number")
                .FromProvider(new FakeIntegerProvider())
                .NotNull();

            // Act
            Dictionary<string, object>[] values = generator.Generate(expected.ObjectCount);

            var actual = new
            {
                ObjectCount = values.Length,
                AllInt = values.SelectMany(u => u.Values).All(u => typeof(int) == u.GetType()),
                Unique = values.SelectMany(u => u.Values).Distinct().Count() == expected.ObjectCount
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateWithSinglePropertyEventsAreFired()
        {
            // Arrange
            var expected = new
            {
                PropertyLoadCalled = true,
                PropertyLoadHasExpectedArgs = true,

                PropertyLoadedCalled = true,
                PropertyLoadedHasExpectedArgs = true,

                RowGeneratedCalledTimes = 100,
                RowGeneratedHasExpectedArgs = true,

                ValueGeneratedCalledTimes = 100,
                ValueGeneratedHasExpectedArgs = true
            };

            DataGenerator generator = new DataGenerator();
            generator.WithProperty<int>("number")
                .FromProvider(new FakeIntegerProvider())
                .NotNull();

            int rowGeneratedCalledTimes = 0;
            int valueGeneratedCalledTimes = 0;
            IDataGeneratorProperty propertyLoadArgs = null;
            IDataGeneratorProperty propertyLoadedArgs = null;
            List<Dictionary<string, object>> rowGeneratedArgs = new List<Dictionary<string, object>>();
            List<(IDataGeneratorProperty Property, object Value)> valueGeneratedArgs = new List<(IDataGeneratorProperty Property, object Value)>();

            generator.PropertyLoad += (sender, property) =>
            {
                propertyLoadArgs = property;
            };

            generator.PropertyLoaded += (sender, property) =>
            {
                propertyLoadedArgs = property;
            };

            generator.RowGenerated += (sender, row) =>
            {
                rowGeneratedCalledTimes++;
                rowGeneratedArgs.Add(row);
            };

            generator.ValueGenerated += (sender, args) =>
            {
                valueGeneratedCalledTimes++;
                valueGeneratedArgs.Add(args);
            };

            // Act
            Dictionary<string, object>[] values = generator.Generate(100);

            var actual = new
            {
                PropertyLoadCalled = propertyLoadArgs != null,
                PropertyLoadHasExpectedArgs = propertyLoadArgs != null && propertyLoadArgs.Id == "number",

                PropertyLoadedCalled = propertyLoadedArgs != null,
                PropertyLoadedHasExpectedArgs = propertyLoadedArgs != null && propertyLoadedArgs.Id == "number",

                RowGeneratedCalledTimes = rowGeneratedCalledTimes,
                RowGeneratedHasExpectedArgs = rowGeneratedArgs.All(u => u["number"].GetType() == typeof(int) && u.Keys.Count == 1),

                ValueGeneratedCalledTimes = valueGeneratedCalledTimes,
                ValueGeneratedHasExpectedArgs = valueGeneratedArgs.All(u => u.Property.Id == "number" && u.Value.GetType() == typeof(int))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateWithMultiplePropertiesEventsAreFired()
        {
            // Arrange
            var expected = new
            {
                Property1LoadCalled = true,
                Property1LoadHasExpectedArgs = true,
                Property2LoadCalled = true,
                Property2LoadHasExpectedArgs = true,

                Property1LoadedCalled = true,
                Property1LoadedHasExpectedArgs = true,
                Property2LoadedCalled = true,
                Property2LoadedHasExpectedArgs = true,

                RowGeneratedCalledTimes = 100,
                RowGeneratedHasExpectedArgs = true,

                ValueGenerated1CalledTimes = 100,
                ValueGenerated1HasExpectedArgs = true,
                ValueGenerated2CalledTimes = 100,
                ValueGenerated2HasExpectedArgs = true
            };

            DataGenerator generator = new DataGenerator();
            generator.WithProperty<int>("number")
                .FromProvider(new FakeIntegerProvider())
                .NotNull();
            generator.WithProperty<Guid>("id")
                .FromProvider(new FakeGuidProvider())
                .NotNull();

            int rowGeneratedCalledTimes = 0;
            int valueGenerated1CalledTimes = 0;
            int valueGenerated2CalledTimes = 0;
            IDataGeneratorProperty property1LoadArgs = null;
            IDataGeneratorProperty property2LoadArgs = null;
            IDataGeneratorProperty property1LoadedArgs = null;
            IDataGeneratorProperty property2LoadedArgs = null;
            List<Dictionary<string, object>> rowGeneratedArgs = new List<Dictionary<string, object>>();
            List<(IDataGeneratorProperty Property, object Value)> value1GeneratedArgs = new List<(IDataGeneratorProperty Property, object Value)>();
            List<(IDataGeneratorProperty Property, object Value)> value2GeneratedArgs = new List<(IDataGeneratorProperty Property, object Value)>();

            generator.PropertyLoad += (sender, property) =>
            {
                if (property.Id == "number")
                {
                    property1LoadArgs = property;
                }
                else if (property.Id == "id")
                {
                    property2LoadArgs = property;
                }
            };

            generator.PropertyLoaded += (sender, property) =>
            {
                if (property.Id == "number")
                {
                    property1LoadedArgs = property;
                }
                else if (property.Id == "id")
                {
                    property2LoadedArgs = property;
                }
            };

            generator.RowGenerated += (sender, row) =>
            {
                rowGeneratedCalledTimes++;
                rowGeneratedArgs.Add(row);
            };

            generator.ValueGenerated += (sender, args) =>
            {
                if (args.Property.Id == "number")
                {
                    valueGenerated1CalledTimes++;
                    value1GeneratedArgs.Add(args);
                }
                else if (args.Property.Id == "id")
                {
                    valueGenerated2CalledTimes++;
                    value2GeneratedArgs.Add(args);
                }
            };

            // Act
            Dictionary<string, object>[] values = generator.Generate(100);

            var actual = new
            {
                Property1LoadCalled = property1LoadArgs != null,
                Property1LoadHasExpectedArgs = property1LoadArgs != null && property1LoadArgs.Id == "number",
                Property2LoadCalled = property2LoadArgs != null,
                Property2LoadHasExpectedArgs = property2LoadArgs != null && property2LoadArgs.Id == "id",

                Property1LoadedCalled = property1LoadedArgs != null,
                Property1LoadedHasExpectedArgs = property1LoadedArgs != null && property1LoadedArgs.Id == "number",
                Property2LoadedCalled = property2LoadedArgs != null,
                Property2LoadedHasExpectedArgs = property2LoadedArgs != null && property2LoadedArgs.Id == "id",

                RowGeneratedCalledTimes = rowGeneratedCalledTimes,
                RowGeneratedHasExpectedArgs = rowGeneratedArgs.All(u => u["number"].GetType() == typeof(int) && u["id"].GetType() == typeof(Guid) && u.Keys.Count == 2),

                ValueGenerated1CalledTimes = valueGenerated1CalledTimes,
                ValueGenerated1HasExpectedArgs = value1GeneratedArgs.All(u => u.Property.Id == "number" && u.Value.GetType() == typeof(int)),
                ValueGenerated2CalledTimes = valueGenerated2CalledTimes,
                ValueGenerated2HasExpectedArgs = value2GeneratedArgs.All(u => u.Property.Id == "id" && u.Value.GetType() == typeof(Guid))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithWeightedValuesWithNullWeightTrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithWeightedProperty<int>("number")
                .FromProvider(new FakeIntegerProvider());

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithWeightedValuesWithEmptyWeightTrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithWeightedProperty<int>("number")
                .FromProvider(new FakeIntegerProvider())
                .WithWeightedValues();

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        public void GenerateWithWeightedReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Country1Count = 25,
                Country2Count = 25,
                OtherCount = 50
            };

            DataGenerator generator = new DataGenerator();
            generator.WithWeightedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedValues(new (float Weight, FakeCountryModel Value)[2] {
                    (0.25f, new FakeCountryModel() { Alpha2 = "US"}),
                    (0.25f, new FakeCountryModel() { Alpha2 = "PT"})
                });

            // Act
            Dictionary<string, object>[] countries = generator.Generate(100);

            var actual = new
            {
                Country1Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).Alpha2 == "US"),
                Country2Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).Alpha2 == "PT"),
                OtherCount = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).Alpha2 != "US" && ((FakeCountryModel)u).Alpha2 != "PT")
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateWithDecimalWeightedReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Country1Count = 19,
                Country2Count = 19,
                OtherCount = 37
            };

            DataGenerator generator = new DataGenerator();
            generator.WithWeightedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedValues(new (float Weight, FakeCountryModel Value)[2] {
                    (0.25f, new FakeCountryModel() { Alpha2 = "US"}),
                    (0.25f, new FakeCountryModel() { Alpha2 = "PT"})
                });

            // Act
            Dictionary<string, object>[] countries = generator.Generate(75);

            var actual = new
            {
                Country1Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).Alpha2 == "US"),
                Country2Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).Alpha2 == "PT"),
                OtherCount = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).Alpha2 != "US" && ((FakeCountryModel)u).Alpha2 != "PT")
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithOverflownWeightedThrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithWeightedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedValues(new (float Weight, FakeCountryModel Value)[2] {
                    (0.25f, new FakeCountryModel() { Alpha2 = "US"}),
                    (1f, new FakeCountryModel() { Alpha2 = "PT"})
                });

            // Act
            generator.Generate(75);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithInvalidEntityWeightedThrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithWeightedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedValues(new (float Weight, FakeCountryModel Value)[2] {
                    (0.25f, new FakeCountryModel() { Alpha2 = "-1"}),
                    (0.25f, new FakeCountryModel() { Alpha2 = "PT"})
                });

            // Act
            generator.Generate(75);
        }

        [TestMethod]
        public void GenerateWithIncompleteRangedReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Range1Count = 25,
                Range2Count = 25,
                OtherCount = 50
            };

            FakeCountryModel range1MinCountry = new FakeCountryModel()
            {
                Alpha2 = "BA"
            };
            FakeCountryModel range1MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "CV"
            };

            FakeCountryModel range2MinCountry = new FakeCountryModel()
            {
                Alpha2 = "ER"
            };
            FakeCountryModel range2MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "RU"
            };

            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedRanges(new WeightedRange<FakeCountryModel>[2] {
                    new WeightedRange<FakeCountryModel>() {
                        MinValue = range1MinCountry,
                        MaxValue = range1MaxCountry,
                        Weight = 0.25f
                    },
                    new WeightedRange<FakeCountryModel>()
                    {
                        MinValue = range2MinCountry,
                        MaxValue = range2MaxCountry,
                        Weight = 0.25f
                    }
                })
                .NotNull();

            // Act
            Dictionary<string, object>[] countries = generator.Generate(100);

            var actual = new
            {
                Range1Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).CompareTo(range1MinCountry) >= 0 && ((FakeCountryModel)u).CompareTo(range1MaxCountry) < 0),
                Range2Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).CompareTo(range2MinCountry) >= 0 && ((FakeCountryModel)u).CompareTo(range2MaxCountry) < 0),
                OtherCount = countries.SelectMany(u => u.Values).Count(u =>
                ((FakeCountryModel)u).CompareTo(range1MinCountry) <= 0 || ((FakeCountryModel)u).CompareTo(range2MaxCountry) >= 0 ||
                (((FakeCountryModel)u).CompareTo(range1MaxCountry) >= 0 && ((FakeCountryModel)u).CompareTo(range2MinCountry) <= 0))
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GenerateWithRangedReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                Range1Count = 50,
                Range2Count = 50
            };

            FakeCountryModel range1MinCountry = new FakeCountryModel()
            {
                Alpha2 = "BA"
            };
            FakeCountryModel range1MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "CV"
            };

            FakeCountryModel range2MinCountry = new FakeCountryModel()
            {
                Alpha2 = "ER"
            };
            FakeCountryModel range2MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "RU"
            };

            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedRanges(new WeightedRange<FakeCountryModel>[2] {
                    new WeightedRange<FakeCountryModel>() {
                        MinValue = range1MinCountry,
                        MaxValue = range1MaxCountry,
                        Weight = 0.50f
                    },
                    new WeightedRange<FakeCountryModel>()
                    {
                        MinValue = range2MinCountry,
                        MaxValue = range2MaxCountry,
                        Weight = 0.50f
                    }
                });

            // Act
            Dictionary<string, object>[] countries = generator.Generate(100);

            var actual = new
            {
                Range1Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).CompareTo(range1MinCountry) >= 0 && ((FakeCountryModel)u).CompareTo(range1MaxCountry) < 0),
                Range2Count = countries.SelectMany(u => u.Values).Count(u => ((FakeCountryModel)u).CompareTo(range2MinCountry) >= 0 && ((FakeCountryModel)u).CompareTo(range2MaxCountry) < 0),
            };

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithNullRangedThrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider());

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateWithNullArgsRangedThrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedRanges(null);

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithEmptyArgsRangedThrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedRanges(Array.Empty<WeightedRange<FakeCountryModel>>());

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithNegativeWeightsRangedThrowsException()
        {
            // Arrange
            FakeCountryModel range1MinCountry = new FakeCountryModel()
            {
                Alpha2 = "BA"
            };
            FakeCountryModel range1MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "CV"
            };

            FakeCountryModel range2MinCountry = new FakeCountryModel()
            {
                Alpha2 = "ER"
            };
            FakeCountryModel range2MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "RU"
            };

            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedRanges(new WeightedRange<FakeCountryModel>[2] {
                    new WeightedRange<FakeCountryModel>() {
                        MinValue = range1MinCountry,
                        MaxValue = range1MaxCountry,
                        Weight = -0.50f
                    },
                    new WeightedRange<FakeCountryModel>()
                    {
                        MinValue = range2MinCountry,
                        MaxValue = range2MaxCountry,
                        Weight = 0.50f
                    }
                });

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithOverflownWeightsRangedThrowsException()
        {
            // Arrange
            FakeCountryModel range1MinCountry = new FakeCountryModel()
            {
                Alpha2 = "BA"
            };
            FakeCountryModel range1MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "CV"
            };

            FakeCountryModel range2MinCountry = new FakeCountryModel()
            {
                Alpha2 = "ER"
            };
            FakeCountryModel range2MaxCountry = new FakeCountryModel()
            {
                Alpha2 = "RU"
            };

            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider())
                .WithWeightedRanges(new WeightedRange<FakeCountryModel>[2] {
                    new WeightedRange<FakeCountryModel>() {
                        MinValue = range1MinCountry,
                        MaxValue = range1MaxCountry,
                        Weight = 1.50f
                    },
                    new WeightedRange<FakeCountryModel>()
                    {
                        MinValue = range2MinCountry,
                        MaxValue = range2MaxCountry,
                        Weight = 0.50f
                    }
                });

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        public void GenerateWithUniqueRangedReturnsExpectedResult()
        {
            // Arrange
            var expected = new
            {
                AllItemsWithinRange = true,
                UniqueItemCount = 25,
                ItemCount = 25
            };

            // Act
            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<int>("number")
                .FromProvider(new FakeIntegerProvider())
                .WithWeightedRanges(new WeightedRange<int>[1] {
                    new WeightedRange<int>() {
                        MinValue = 0,
                        MaxValue = 50,
                        Weight = 1f
                    }
                })
                .Unique();

            // Act
            Dictionary<string, object>[] values = generator.Generate(25);

            var actual = new
            {
                AllItemsWithinRange = values.SelectMany(u => u.Values).All(u => (int)u >= 0 && (int)u < 50),
                UniqueItemCount = values.SelectMany(u => u.Values).Distinct().Count(),
                ItemCount = values.Length
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithInvalidUniqueRangedThrowsException()
        {
            // Act
            DataGenerator generator = new DataGenerator();
            generator.WithRangedProperty<int>("number")
                .FromProvider(new FakeIntegerProvider())
                .WithWeightedRanges(new WeightedRange<int>[1] {
                    new WeightedRange<int>() {
                        MinValue = 0,
                        MaxValue = 50,
                        Weight = 1f
                    }
                })
                .Unique();

            // Act
            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithCircularDependencyLevel1ThrowsException()
        {
            // Act
            DataGenerator generator = new DataGenerator();
            DataGeneratorProperty<FakeCountryModel> countryProperty = generator.WithProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider());
            DataGeneratorProperty<FakeNationalityModel> nationalityProperty = generator.WithProperty<FakeNationalityModel>("nationality")
                .FromProvider(new FakeNationalityProvider())
                .WithArgument<FakeCountryModel>(FakeNationalityProvider.CountryArgumentName, countryProperty);

            countryProperty.WithArgument<FakeNationalityModel>(FakeCountryProvider.NationalityArgumentName, nationalityProperty);

            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithCircularDependencyLevel2ThrowsException()
        {
            // Act
            DataGenerator generator = new DataGenerator();
            DataGeneratorProperty<FakeCountryModel> countryProperty = generator.WithProperty<FakeCountryModel>("country")
                .FromProvider(new FakeCountryProvider());
            DataGeneratorProperty<FakeCountryModel> otherCountryProperty = generator.WithProperty<FakeCountryModel>("country2")
                .FromProvider(new FakeCountryProvider());

            DataGeneratorProperty<FakeNationalityModel> nationalityProperty = generator.WithProperty<FakeNationalityModel>("nationality")
                .FromProvider(new FakeNationalityProvider())
                .WithArgument<FakeCountryModel>(FakeNationalityProvider.CountryArgumentName, otherCountryProperty);

            DataGeneratorProperty<FakeNationalityModel> otherNationalityProperty = generator.WithProperty<FakeNationalityModel>("nationality2")
                .FromProvider(new FakeNationalityProvider())
                .WithArgument<FakeCountryModel>(FakeNationalityProvider.CountryArgumentName, countryProperty);

            countryProperty.WithArgument<FakeNationalityModel>(FakeCountryProvider.NationalityArgumentName, nationalityProperty);
            otherCountryProperty.WithArgument<FakeNationalityModel>(FakeCountryProvider.NationalityArgumentName, otherNationalityProperty);

            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithCircularDependencyLevel3ThrowsException()
        {
            // Arrange
            Mock<DataGeneratorProvider<string>> mockProvider = new Mock<DataGeneratorProvider<string>>(MockBehavior.Strict);
            mockProvider.Setup(u => u.GetSupportedArguments()).Returns(new Dictionary<string, Type>() { { "arg", typeof(string) } });

            // Act
            DataGenerator generator = new DataGenerator();
            DataGeneratorProperty<string> level1Property = generator.WithProperty<string>("level1")
                .FromProvider(mockProvider.Object);

            DataGeneratorProperty<string> level2Property = generator.WithProperty<string>("level2")
                .FromProvider(mockProvider.Object);

            DataGeneratorProperty<string> level3Property = generator.WithProperty<string>("level3")
                .FromProvider(mockProvider.Object);

            level1Property.WithArgument("arg", level2Property);
            level2Property.WithArgument("arg", level3Property);
            level3Property.WithArgument("arg", level1Property);

            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithNotNullEnsuresNonNullValues()
        {
            Mock<DataGeneratorProvider<string>> mockProvider = new Mock<DataGeneratorProvider<string>>(MockBehavior.Strict);
            mockProvider.Setup(u => u.Load(It.IsAny<DataGeneratorProperty<string>>(), It.IsAny<int>()));
            mockProvider.Setup(u => u.GetRowValue(It.IsAny<DataGeneratorContext>(), It.IsAny<string[]>())).Returns(default(string));
            mockProvider.Protected().Setup("OnInitializeRandomizer", ItExpr.IsNull<int?>());

            DataGenerator generator = new DataGenerator();
            DataGeneratorProperty<string> property = generator.WithProperty<string>("prop")
                .FromProvider(mockProvider.Object)
                .NotNull();

            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerateWithUniqueEnsuresUniqueValues()
        {
            Mock<DataGeneratorProvider<string>> mockProvider = new Mock<DataGeneratorProvider<string>>(MockBehavior.Strict);
            mockProvider.Setup(u => u.Load(It.IsAny<DataGeneratorProperty<string>>(), It.IsAny<int>()));
            mockProvider.Setup(u => u.GetRowValue(It.IsAny<DataGeneratorContext>(), It.IsAny<string[]>())).Returns("A");
            mockProvider.Protected().Setup("OnInitializeRandomizer", ItExpr.IsNull<int?>());

            DataGenerator generator = new DataGenerator();
            DataGeneratorProperty<string> property = generator.WithProperty<string>("prop")
                .FromProvider(mockProvider.Object)
                .NotNull()
                .Unique();

            generator.Generate(100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigureWithNullConfigurationThrowsException()
        {
            // Arrange
            DataGenerator generator = new DataGenerator();
            generator.Configure(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureWithNullPropertyThrowsException()
        {
            // Arrange
            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    null
                }
            };

            DataGenerator generator = new DataGenerator();

            // Act
            generator.Configure(configuration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureWithRangedAndWeightedPropertyThrowsException()
        {
            // Arrange
            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "Number",
                        ValueType = typeof(int).FullName,
                        Provider = typeof(FakeIntegerProvider).AssemblyQualifiedName,
                        IsUnique = true,
                        WeightedRanges = new WeightedRange<string>[1]
                        {
                            new WeightedRange<string>() { }
                        },
                        Weights = new (float Weight, string Value)[1]
                        {
                            (1, string.Empty)
                        }
                    }
                }
            };

            DataGenerator generator = new DataGenerator();

            // Act
            generator.Configure(configuration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureWithNullValueTypeThrowsException()
        {
            // Arrange
            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "Number",
                        Provider = typeof(FakeIntegerProvider).AssemblyQualifiedName,
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();

            // Act
            generator.Configure(configuration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureWithInvalidValueTypeThrowsException()
        {
            // Arrange
            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "Number",
                        ValueType = "123",
                        Provider = typeof(FakeIntegerProvider).AssemblyQualifiedName,
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();

            // Act
            generator.Configure(configuration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureWithNullProviderTypeTypeThrowsException()
        {
            // Arrange
            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "Number",
                        ValueType = typeof(int).ToString(),
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();

            // Act
            generator.Configure(configuration);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureWithInvalidProviderTypeTypeThrowsException()
        {
            // Arrange
            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "Number",
                        ValueType = typeof(int).ToString(),
                        Provider = "123",
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();

            // Act
            generator.Configure(configuration);
        }

        [TestMethod]
        public void ConfigureWithOnePropertyReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ColumnCount = 1,
                ColumnId = "Number",
                ValueCount = 100,
                ValueOfIntType = true
            };

            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = expected.ColumnId,
                        ValueType = typeof(int).FullName,
                        Provider = typeof(FakeIntegerProvider).AssemblyQualifiedName,
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();
            generator.Configure(configuration);

            // Act
            Dictionary<string, object>[] values = generator.Generate(expected.ValueCount);

            var actual = new
            {
                ColumnCount = values.Max(u => u.Keys.Count),
                ColumnId = values[0].Keys.ElementAt(0),
                ValueCount = values.Length,
                ValueOfIntType = values.SelectMany(u => u.Values).All(u => u.GetType() == typeof(int))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConfigureWithTwoPropertiesReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                EmailHasArgumentAndDomain = true,
            };

            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[2]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "id",
                        ValueType = typeof(Guid).FullName,
                        Provider = typeof(FakeGuidProvider).AssemblyQualifiedName,
                        IsUnique = true
                    },
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "email",
                        ValueType = typeof(string).FullName,
                        Provider = typeof(FakeEmailProvider).AssemblyQualifiedName,
                        ProviderConfiguration = JsonConvert.SerializeObject(new FakeEmailProviderConfiguration()
                        {
                            Domains = new string[1] { "gmail.com" }
                        }),
                        IsUnique = true,
                        Arguments = new Dictionary<string, string[]>
                        {
                            { FakeEmailProvider.TextArgumentsName, new string[1] { "id" } }
                        }
                    },
                }
            };

            DataGenerator generator = new DataGenerator();
            generator.Configure(configuration);

            // Act
            Dictionary<string, object>[] values = generator.Generate(100);

            var actual = new
            {
                EmailHasArgumentAndDomain = values.All(u => u["email"].ToString().Contains(u["id"].ToString().Replace('-', '.')) && u["email"].ToString().Contains("@gmail.com")),
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConfigureWithProviderConstructorReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ColumnCount = 1,
                ColumnId = "country",
                ValueCount = 100,
                ValueOfExpectedType = true
            };

            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = expected.ColumnId,
                        ValueType = typeof(FakeCountryModel).AssemblyQualifiedName,
                        Provider = typeof(FakeCountryProvider).AssemblyQualifiedName,
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();
            generator.Configure(configuration);

            // Act
            Dictionary<string, object>[] values = generator.Generate(expected.ValueCount);

            var actual = new
            {
                ColumnCount = values.Max(u => u.Keys.Count),
                ColumnId = values[0].Keys.ElementAt(0),
                ValueCount = values.Length,
                ValueOfExpectedType = values.SelectMany(u => u.Values).All(u => u.GetType() == typeof(FakeCountryModel))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConfigureWithDependentPropertiesReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ColumnCount = 2,
                ValueCount = 100,
                ValueOfIntType = true,
                ValueOfGuidType = true
            };

            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[2]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "number",
                        ValueType = typeof(int).FullName,
                        Provider = typeof(FakeIntegerProvider).AssemblyQualifiedName,
                        IsUnique = true
                    },
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = "id",
                        ValueType = typeof(Guid).FullName,
                        Provider = typeof(FakeGuidProvider).AssemblyQualifiedName,
                        IsUnique = true
                    }
                }
            };

            DataGenerator generator = new DataGenerator();
            generator.Configure(configuration);

            // Act
            Dictionary<string, object>[] values = generator.Generate(expected.ValueCount);

            var actual = new
            {
                ColumnCount = values.Max(u => u.Keys.Count),
                ValueCount = values.Length,
                ValueOfIntType = values.Select(u => u["number"]).All(u => u.GetType() == typeof(int)),
                ValueOfGuidType = values.Select(u => u["id"]).All(u => u.GetType() == typeof(Guid))
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConfigureWithOneRangedPropertyReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ColumnCount = 1,
                ColumnId = "Number",
                ValueCount = 100,
                ValueOfIntType = true,
                ValueWithinRange = true
            };

            DataGeneratorConfiguration configuration = new DataGeneratorConfiguration()
            {
                Properties = new DataGeneratorConfigurationProperty[1]
                {
                    new DataGeneratorConfigurationProperty()
                    {
                        Id = expected.ColumnId,
                        ValueType = typeof(int).FullName,
                        Provider = typeof(FakeIntegerProvider).AssemblyQualifiedName,
                        WeightedRanges = new WeightedRange<string>[1]
                        {
                            new WeightedRange<string>()
                            {
                                MinValue = "0",
                                MaxValue = "1000",
                                Weight = 1
                            }
                        }
                    }
                }
            };

            DataGenerator generator = new DataGenerator();
            generator.Configure(configuration);

            // Act
            Dictionary<string, object>[] values = generator.Generate(expected.ValueCount);

            var actual = new
            {
                ColumnCount = values.Max(u => u.Keys.Count),
                ColumnId = values[0].Keys.ElementAt(0),
                ValueCount = values.Length,
                ValueOfIntType = values.SelectMany(u => u.Values).All(u => u.GetType() == typeof(int)),
                ValueWithinRange = values.SelectMany(u => u.Values).All(u => ((int)u) >= 0 && ((int)u) < 1000)
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
