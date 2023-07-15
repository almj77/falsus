namespace Falsus.Providers.Person.UnitTests
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
    public class JobNameProviderUnitTests
    {
        [TestMethod]
        public void GetRangedRowValueReturnsExpectedValue()
        {
            // Arrange
            JobModel expectedTyped = new JobModel()
            {
                Id = "4899a623b8252b4876b9ac51ae36a042",
                Title = "Mystery Shopper",
                ParentJobId = "7a1ea7efbeabb76e088a4c5ce6ff919f"
            };
            var expected = new
            {
                expectedTyped.Id,
                expectedTyped.Title,
                expectedTyped.ParentJobId
            };

            ProviderResult providerResult = CreateProvider();
            JobNameProvider provider = providerResult.Provider;

            JobModel lowerBound = new JobModel()
            {
                Id = "607923d5ed6e19ed8cb02ce78f86b556",
                Title = "Mycology Teacher",
                ParentJobId = "27d2c00408c1cc5452cd2c91ce80cdfd"
            };
            JobModel upperBound = new JobModel()
            {
                Id = "d53d0ba73b068452d329266a90567ca6",
                Title = "Nail Artist",
                ParentJobId = "1a2a0e9f50f751abf5ab786c12d1ec3b"
            };

            // Act
            JobModel actualTyped = provider.GetRangedRowValue(lowerBound, upperBound, Array.Empty<JobModel>());

            var actual = new
            {
                actualTyped.Id,
                actualTyped.Title,
                actualTyped.ParentJobId
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            JobModel expectedTyped = new JobModel()
            {
                Id = "2ca171f3e3d0cdce8e35ffa1c290bbe8",
                ParentJobId = "de4684389978230e3c916f74dbcde1b2",
                Title = "Musician"
            };
            var expected = new
            {
                expectedTyped.Id,
                expectedTyped.ParentJobId,
                expectedTyped.Title
            };

            ProviderResult providerResult = CreateProvider();
            JobNameProvider provider = providerResult.Provider;

            // Act
            JobModel actualTyped = provider.GetRowValue(expectedTyped.Id);

            var actual = new
            {
                actualTyped.Id,
                actualTyped.ParentJobId,
                actualTyped.Title
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            JobNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            JobModel value = provider.GetRowValue(context, Array.Empty<JobModel>());

            // Assert
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 398217;
            JobModel expectedTyped = new JobModel
            {
                Id = "c7097ecaa2c7e316d1e5b2dd62db797c",
                Title = "Fitter/Welder",
                ParentJobId = "49a7ca68242e9a9a818a74d555fb834c"
            };
            var expected = new
            {
                expectedTyped.Id,
                expectedTyped.Title,
                expectedTyped.ParentJobId
            };

            ProviderResult providerResult = CreateProvider(1, seed);
            JobNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            JobModel actualTyped = provider.GetRowValue(context, Array.Empty<JobModel>());

            var actual = new
            {
                actualTyped.Id,
                actualTyped.Title,
                actualTyped.ParentJobId
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRowValueWithAllExcludedThrowsException()
        {
            // Arrange
            JobModel[] excludedObjects = GetAllJobs();

            ProviderResult providerResult = CreateProvider();
            JobNameProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, excludedObjects);
        }

        [TestMethod]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            int expectedRowCount = 1000000;
            List<JobModel> generatedValues = new List<JobModel>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            JobNameProvider provider = providerResult.Provider;
            DataGeneratorProperty<JobModel> property = providerResult.Property;

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<JobModel>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            JobModel expected = new JobModel()
            {
                Id = "25943c458d651a97c3ca196bbe294c93",
                Title = "Business Reporter",
                ParentJobId = "25aa9f6e90db5c3a734b68d57b7fb797"
            };

            ProviderResult providerResult = CreateProvider();
            JobNameProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected.Id, actual);
        }

        private JobModel[] GetAllJobs()
        {
            return ResourceReader.ReadContentsFromFile<JobModel[]>("Falsus.Providers.Person.UnitTests.Datasets.Jobs.json");
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            JobNameProvider provider = new JobNameProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<JobModel> property = new DataGeneratorProperty<JobModel>("JobName")
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
            public JobNameProvider Provider;
            public DataGeneratorProperty<JobModel> Property;
            public DataGeneratorContext Context;
        }
    }
}
