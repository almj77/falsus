namespace Falsus.Providers.Company
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="LegalEntityTypeProvider"/> data provider.
    /// </summary>
    public class LegalEntityTypeProviderConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the <see cref="LegalEntityTypeProvider"/>
        /// should attempt to generate a type of legal entity from another country that
        /// shares the same language of the requested country, if unable to generate
        /// a type of legal entity for the requested country.
        /// </summary>
        /// <value>
        /// True if the provider should fallback to a country with the same language.
        /// </value>
        /// <remarks>
        /// True by default.
        /// </remarks>
        /// <example>
        /// If unable to generate a random legal entity type for Monaco, the provider
        /// will attempt to generate for France because both have french legal entity names.
        /// </example>
        public bool AttemptFallbackByLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the <see cref="LegalEntityTypeProvider"/>
        /// should fallback to a specific country (defined by <see cref="FallbackCountryAlpha2"/>) 
        /// if unable to generate a type of legal entity for the requested coountry.
        /// </summary>
        /// <value>
        /// True if the provider should fallback to a specific country.
        /// </value>
        /// <remarks>
        /// True by default.
        /// </remarks>
        public bool FallbackToCountry { get; set; }

        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code that should be used as fallback
        /// for legal entity type generation.
        /// </summary>
        /// <value>
        /// A <see cref="string" /> containing the Alpha2 ISO Country Code.
        /// </value>
        /// <remarks>
        /// CA by default.
        /// </remarks>
        public string FallbackCountryAlpha2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegalEntityTypeProviderConfiguration"/> class.
        /// </summary>
        public LegalEntityTypeProviderConfiguration()
        {
            this.AttemptFallbackByLanguage = true;
            this.FallbackToCountry = true;
            this.FallbackCountryAlpha2 = "AU";
        }
    }
}
