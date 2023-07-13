namespace Falsus.Providers.Person.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JobSkillProviderUnitTests
    {
        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            var expected = new
            {
                ArgumentName = JobSkillProvider.JobArgumentName,
                ArgumentType = typeof(JobModel),
                ArgumentCount = 1
            };

            JobSkillProvider provider = new JobSkillProvider();

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
            JobSkillProvider provider = providerResult.Provider;

            // Act
            provider.GetRangedRowValue(Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueForRangedThrowsException()
        {
            // Arrange
            ProviderResult providerResult = CreateProvider();
            JobSkillProvider provider = providerResult.Provider;
            DataGeneratorContext context = providerResult.Context;

            // Act
            provider.GetRowValue(context, Array.Empty<WeightedRange<string[]>>(), Array.Empty<string[]>());
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValue()
        {
            // Arrange
            string[] expectedTyped = new string[3] { "Skill 1", "Skill 2, Skill 3", "Skill 4" };
            var expected = string.Join("|", expectedTyped);

            ProviderResult providerResult = CreateProvider();
            JobSkillProvider provider = providerResult.Provider;

            // Act;
            string[] actualTyped = provider.GetRowValue(expected);
            string actual = string.Join("|", actualTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueReturnsExpectedValues()
        {
            // Arrange
            DataGeneratorProperty<JobModel> jobProperty = new DataGeneratorProperty<JobModel>("JobName");
            DataGeneratorProperty<string[]> property = new DataGeneratorProperty<string[]>("Skills")
                .WithArgument(JobSkillProvider.JobArgumentName, jobProperty);

            JobSkillProvider provider = new JobSkillProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1);

            Dictionary<string, object> row = new Dictionary<string, object>();
            row.Add("JobName", new JobModel()
            {
                Id = "c2c7e1fb595982718dcb6656a7065642",
                Title = "Accounting Analyst",
                ParentJobId = "3eb8be26f38d699304907a67cf4e8cd8"
            });
            DataGeneratorContext context = new DataGeneratorContext(row, 0, 1, property, property.Arguments);

            // Act
            string[] value = provider.GetRowValue(context, Array.Empty<string[]>());

            // Assert
            Assert.IsTrue(value.Length > 0);
        }

        [TestMethod]
        public void GetRowValueGenerates10KValues()
        {
            // Arrange
            int expectedRowCount = 10000;
            List<string[]> generatedValues = new List<string[]>();

            ProviderResult providerResult = CreateProvider(expectedRowCount);
            JobSkillProvider provider = providerResult.Provider;
            DataGeneratorProperty<string[]> property = providerResult.Property;

            for (int i = 0; i < expectedRowCount; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), i, expectedRowCount, property, property.Arguments);
                // Act            
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string[]>()));
            }

            int actualRowCount = generatedValues.Count(u => u != null);

            // Assert
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            string[] expectedTyped = new string[2] { "Skill 1", "Skill 2" };
            string expected = string.Join("|", expectedTyped);

            ProviderResult providerResult = CreateProvider();
            JobSkillProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetValueId(expectedTyped);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private ProviderResult CreateProvider(int rowCount = 1, int? seed = default)
        {
            JobSkillProvider provider = new JobSkillProvider();

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string[]> property = new DataGeneratorProperty<string[]>("Skills")
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
            public JobSkillProvider Provider;
            public DataGeneratorProperty<string[]> Property;
            public DataGeneratorContext Context;
        }
    }
}
