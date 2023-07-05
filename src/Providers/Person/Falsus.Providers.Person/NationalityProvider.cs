namespace Falsus.Providers.Person
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="NationalityModel"/> instances.
    /// </summary>
    /// <seealso cref="GenericArrayProvider{T}"/>
    public class NationalityProvider : GenericArrayProvider<NationalityModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="CountryModel"/> to use on the value generation process.
        /// </summary>
        public const string CountryArgumentName = "country";

        /// <summary>
        /// An array of <see cref="NationalityModel"/> instances containing
        /// all possible values.
        /// </summary>
        private static NationalityModel[] nationalities;

        /// <summary>
        /// Initializes a new instance of the <see cref="NationalityProvider"/> class.
        /// </summary>
        public NationalityProvider()
            : base()
        {
            nationalities = Array.Empty<NationalityModel>();
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
        /// Gets an instance of <see cref="NationalityModel"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="NationalityModel"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="NationalityModel"/> instance is
        /// defined by the <see cref="NationalityModel.Demonym"/> property.
        /// The unique id can be fetched by invoking the
        /// <see cref="NationalityProvider.GetValueId(NationalityModel)"/> method.
        /// </remarks>
        public override NationalityModel GetRowValue(string id)
        {
            return nationalities.FirstOrDefault(u => u.Demonym == id);
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="NationalityModel"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="NationalityModel"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="NationalityModel"/>.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="NationalityModel"/> instance is
        /// defined by the <see cref="NationalityModel.Demonym"/> property.
        /// </remarks>
        public override string GetValueId(NationalityModel value)
        {
            return value.Demonym;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{NationalityModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<NationalityModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            nationalities = ResourceReader.ReadContentsFromFile<NationalityModel[]>("Falsus.Providers.Person.Datasets.Nationalities.json");
        }

        /// <summary>
        /// Gets an array of <see cref="NationalityModel"/> instances
        /// representing possible values based on the context information.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of <see cref="NationalityModel"/> instances.
        /// </returns>
        /// <remarks>
        /// If the <see cref="CountryArgumentName"/> is provided then this method will
        /// return the value for a <see cref="NationalityModel"/> of the specified <see cref="CountryModel"/>
        /// argument value.
        /// </remarks>
        protected override NationalityModel[] GetValues(DataGeneratorContext context)
        {
            if (context.HasArgumentValue(CountryArgumentName))
            {
                CountryModel countryModel = context.GetArgumentValue<CountryModel>(CountryArgumentName);
                if (countryModel != null && !string.IsNullOrEmpty(countryModel.Alpha2))
                {
                    NationalityModel nationalityModel = nationalities.FirstOrDefault(u => u.CountryAlpha2 == countryModel.Alpha2);
                    if (nationalityModel != null)
                    {
                        return new NationalityModel[1] { nationalityModel };
                    }
                }

                return Array.Empty<NationalityModel>();
            }

            return this.GetValues();
        }

        /// <summary>
        /// Gets an array of <see cref="NationalityModel"/> instances
        /// representing all possible values.
        /// </summary>
        /// <returns>
        /// An array of <see cref="NationalityModel"/> instances.
        /// </returns>
        protected override NationalityModel[] GetValues()
        {
            return nationalities;
        }
    }
}
