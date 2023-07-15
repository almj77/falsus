namespace Falsus.Providers.Person
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of job skills.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class JobSkillProvider : DataGeneratorProvider<string[]>
    {
        /// <summary>
        /// The name of the input argument that contains the <see cref="string"/>
        /// value containing the name of the job to use on the value generation process.
        /// </summary>
        public const string JobArgumentName = "job";

        /// <summary>
        /// A dictionary of <see cref="string"/> collections containing job skills indexed
        /// by the job name property.
        /// </summary>
        private static Dictionary<string, List<string>> jobToSkills;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSkillProvider"/> class.
        /// </summary>
        public JobSkillProvider()
            : base()
        {
            jobToSkills = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Gets the definition of the input arguments supported by this provider.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the name of the
        /// argument and the expected type of the argument value.
        /// </returns>
        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>()
            {
                { JobArgumentName, typeof(JobModel) }
            };
        }

        /// <summary>
        /// Generates a random collection of <see cref="string"/> skills
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> arrays that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="string"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged job skills is not supported.
        /// </exception>
        public override string[] GetRangedRowValue(string[] minValue, string[] maxValue, string[][] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(JobSkillProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random collection of <see cref="string"/> skills
        /// based on the context and excluded ranges.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedRanges">
        /// An array of <see cref="WeightedRange{T}"/>s defining the ranges
        /// that cannot be returned by the provider.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> arrays that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="string"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged job skills is not supported.
        /// </exception>
        public override string[] GetRowValue(DataGeneratorContext context, WeightedRange<string[]>[] excludedRanges, string[][] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(JobSkillProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets an array of <see cref="string"/> values based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="string"/> value with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="id"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="id"/> argument.
        /// The unique id can be fetched by invoking the
        /// <see cref="JobSkillProvider.GetValueId(string[])"/> method.
        /// </remarks>
        public override string[] GetRowValue(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return id.Split('|');
            }

            return null;
        }

        /// <summary>
        /// Generates a random skill collection based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// A matrix of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A collection of random skills.
        /// </returns>
        /// <remarks>
        /// If the <see cref="JobArgumentName"/> is provided then this method will
        /// return the name of the skills for the specified job.
        /// </remarks>
        public override string[] GetRowValue(DataGeneratorContext context, string[][] excludedObjects)
        {
            if (excludedObjects.Any())
            {
                throw new NotSupportedException($"{nameof(JobSkillProvider)} does not support unique values.");
            }

            if (context.HasArgumentValue(JobArgumentName))
            {
                JobModel jobModel = context.GetArgumentValue<JobModel>(JobArgumentName);
                if (jobModel == null || string.IsNullOrEmpty(jobModel.Id))
                {
                    throw new InvalidOperationException($"{nameof(JobSkillProvider)} cannot find skills for null job.");
                }
                else if (!jobToSkills.ContainsKey(jobModel.Id))
                {
                    throw new InvalidOperationException($"{nameof(JobSkillProvider)} cannot find skills for '{jobModel.Id}' job.");
                }

                return jobToSkills[jobModel.Id].ToArray();
            }

            return jobToSkills.Values.ElementAt(this.Randomizer.Next(0, jobToSkills.Count)).ToArray();
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="string"/> array.
        /// </summary>
        /// <param name="value">The array of <see cref="string"/> values.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// The unique identifier is a concatenation of all skills.
        /// </returns>
        public override string GetValueId(string[] value)
        {
            return string.Join("|", value);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{T}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<string[]> property, int rowCount)
        {
            base.Load(property, rowCount);
            string[] resourcePaths = ResourceReader.GetEmbeddedResourcePaths("Falsus.Providers.Person.Datasets.JobSkills");

            foreach (string resource in resourcePaths)
            {
                Tuple<string, string>[] items = ResourceReader.ReadContentsFromFile<Tuple<string, string>[]>(resource);
                foreach (var item in items)
                {
                    if (!jobToSkills.ContainsKey(item.Item1))
                    {
                        jobToSkills.Add(item.Item1, new List<string>());
                    }

                    jobToSkills[item.Item1].Add(item.Item2);
                }
            }
        }
    }
}
