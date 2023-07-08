namespace Falsus.Providers.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Location.Models;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="ContinentModel"/> values.
    /// </summary>
    /// <seealso cref="GenericArrayProvider{T}"/>
    public class ContinentProvider : GenericArrayProvider<ContinentModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// A dictionary of <see cref="ContinentModel"/> instances indexed by
        /// the <see cref="ContinentModel.Alpha2"/> property.
        /// </summary>
        private static Dictionary<string, ContinentModel> continentsByAlpha2;

        /// <summary>
        /// A dictionary of <see cref="CountryToContinentModel"/> instances indexed
        /// by the <see cref="CountryToContinentModel.CountryAlpha2"/> property.
        /// </summary>
        private static Dictionary<string, CountryToContinentModel> continentByCountry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinentProvider"/> class.
        /// </summary>
        public ContinentProvider()
            : base()
        {
            continentsByAlpha2 = new Dictionary<string, ContinentModel>();
            continentByCountry = new Dictionary<string, CountryToContinentModel>();
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
                { CountryArgumentName, typeof(CountryModel) }
            };
        }

        /// <summary>
        /// Gets an instance of <see cref="ContinentModel"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="ContinentModel"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="ContinentModel"/> instance is
        /// defined by the <see cref="ContinentModel.Alpha2"/> property.
        /// The unique id can be fetched by invoking the
        /// <see cref="ContinentProvider.GetValueId(ContinentModel)"/> method.
        /// </remarks>
        public override ContinentModel GetRowValue(string id)
        {
            if (continentsByAlpha2.ContainsKey(id))
            {
                return continentsByAlpha2[id];
            }

            return null;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="ContinentModel"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="ContinentModel"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="ContinentModel"/>.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="ContinentModel"/> instance is
        /// defined by the <see cref="ContinentModel.Alpha2"/> property.
        /// </remarks>
        public override string GetValueId(ContinentModel value)
        {
            return value.Alpha2;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{ContinentModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<ContinentModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            ContinentModel[] continents = ResourceReader.ReadContentsFromFile<ContinentModel[]>("Falsus.Providers.Location.Datasets.Continents.json");
            continentsByAlpha2 = continents.ToDictionary(u => u.Alpha2);

            CountryToContinentModel[] countriesToContinent = ResourceReader.ReadContentsFromFile<CountryToContinentModel[]>("Falsus.Providers.Location.Datasets.ContinentToCountry.json");
            continentByCountry = countriesToContinent.ToDictionary(u => u.CountryAlpha2);
        }

        /// <summary>
        /// Gets an array of <see cref="ContinentModel"/> instances
        /// representing possible values based on the context information.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of <see cref="ContinentModel"/> instances.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the value for a <see cref="ContinentModel"/> of the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        protected override ContinentModel[] GetValues(DataGeneratorContext context)
        {
            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);
                if (countryModel != null && !string.IsNullOrEmpty(countryModel.Alpha2))
                {
                    if (continentByCountry.ContainsKey(countryModel.Alpha2))
                    {
                        string continentAlpha2 = continentByCountry[countryModel.Alpha2].ContinentAlpha2;
                        if (!string.IsNullOrEmpty(continentAlpha2) && continentsByAlpha2.ContainsKey(continentAlpha2))
                        {
                            return new ContinentModel[1] { continentsByAlpha2[continentAlpha2] };
                        }
                    }
                }

                return Array.Empty<ContinentModel>();
            }

            return this.GetValues();
        }

        /// <summary>
        /// Gets an array of <see cref="ContinentModel"/> instances
        /// representing all possible values.
        /// </summary>
        /// <returns>
        /// An array of <see cref="ContinentModel"/> instances.
        /// </returns>
        protected override ContinentModel[] GetValues()
        {
            return continentsByAlpha2.Select(u => u.Value).ToArray();
        }
    }
}
