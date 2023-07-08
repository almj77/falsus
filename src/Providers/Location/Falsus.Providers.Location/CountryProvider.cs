namespace Falsus.Providers.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="CountryModel"/> values.
    /// </summary>
    /// <seealso cref="GenericArrayProvider{T}"/>
    public class CountryProvider : GenericArrayProvider<CountryModel>
    {
        /// <summary>
        /// An array containing all <see cref="CountryModel"/> instances.
        /// </summary>
        private static CountryModel[] countries;

        /// <summary>
        /// Gets the definition of the input arguments supported by this provider.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the name of the
        /// argument and the expected type of the argument value.
        /// </returns>
        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>();
        }

        /// <summary>
        /// Gets an instance of <see cref="CountryModel"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="CountryModel"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="CountryModel"/> instance is
        /// defined by the <see cref="CountryModel.Alpha2"/> property.
        /// The unique id can be fetched by invoking the
        /// <see cref="CountryProvider.GetValueId(CountryModel)"/> method.
        /// </remarks>
        public override CountryModel GetRowValue(string id)
        {
            return countries.FirstOrDefault(u => u.Alpha2 == id);
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="CountryModel"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="CountryModel"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="CountryModel"/>.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="CountryModel"/> instance is
        /// defined by the <see cref="CountryModel.Alpha2"/> property.
        /// </remarks>
        public override string GetValueId(CountryModel value)
        {
            return value.Alpha2;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{CountryModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<CountryModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            countries = ResourceReader.ReadContentsFromFile<CountryModel[]>("Falsus.Providers.Location.Datasets.Countries.json");
        }

        /// <summary>
        /// Gets an array of <see cref="CountryModel"/> instances
        /// representing possible values based on the context information.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of <see cref="CountryModel"/> instances.
        /// </returns>
        protected override CountryModel[] GetValues(DataGeneratorContext context)
        {
            return this.GetValues();
        }

        /// <summary>
        /// Gets an array of <see cref="CountryModel"/> instances
        /// representing all possible values.
        /// </summary>
        /// <returns>
        /// An array of <see cref="CountryModel"/> instances.
        /// </returns>
        protected override CountryModel[] GetValues()
        {
            return countries;
        }
    }
}
