namespace Falsus.Providers.Person
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of job names.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class JobNameProvider : GenericArrayProvider<JobModel>
    {
        /// <summary>
        /// An array of <see cref="string"/> containing all job values.
        /// </summary>
        private static JobModel[] jobs;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobNameProvider"/> class.
        /// </summary>
        public JobNameProvider()
            : base()
        {
            jobs = Array.Empty<JobModel>();
        }

        /// <summary>
        /// Gets a <see cref="JobModel"/> instance based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="JobModel"/> instance representing the Job with the specified unique identifier.
        /// </returns>
        public override JobModel GetRowValue(string id)
        {
            return jobs.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="JobModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="JobModel"/> instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(JobModel value)
        {
            return value.Id;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{JobModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<JobModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            jobs = ResourceReader.ReadContentsFromFile<JobModel[]>("Falsus.Providers.Person.Datasets.Jobs.json");
        }

        /// <summary>
        /// Gets an array of <see cref="JobModel"/> instances
        /// representing possible values based on the context information.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of <see cref="JobModel"/> instances.
        /// </returns>
        protected override JobModel[] GetValues(DataGeneratorContext context)
        {
            return this.GetValues();
        }

        /// <summary>
        /// Gets an array of <see cref="JobModel"/> instances
        /// representing all possible values.
        /// </summary>
        /// <returns>
        /// An array of <see cref="JobModel"/> instances.
        /// </returns>
        protected override JobModel[] GetValues()
        {
            return jobs;
        }
    }
}
