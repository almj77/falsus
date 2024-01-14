namespace Falsus.Providers.Company
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="CompanyNameProvider"/> data provider.
    /// </summary>
    public class CompanyNameProviderConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the <see cref="CompanyNameProvider"/>
        /// should attempt to generate a name from another country that
        /// shares the same language of the requested country, if unable to generate
        /// a company name for the requested country.
        /// </summary>
        /// <value>
        /// True if the provider should fallback to a country with the same language.
        /// </value>
        /// <remarks>
        /// True by default.
        /// Only used if provider is configured to generate company names based
        /// on the Country argument.
        /// </remarks>
        /// <example>
        /// If unable to generate a random company name for Monaco, the provider
        /// will attempt to generate for France because both have french names.
        /// </example>
        public bool AttemptFallbackByLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the <see cref="CompanyNameProvider"/>
        /// should fallback to a specific language (defined by <see cref="FallbackCountryAlpha2"/>) 
        /// if unable to generate a company name for the requested coountry.
        /// </summary>
        /// <value>
        /// True if the provider should fallback to a specific language.
        /// </value>
        /// <remarks>
        /// True by default.
        /// </remarks>
        public bool FallbackToLanguage { get; set; }

        /// <summary>
        /// Gets or sets the Alpha2 ISO Language Code that should be used as fallback
        /// for company name generation.
        /// </summary>
        /// <value>
        /// A <see cref="string" /> containing the Alpha2 ISO Language Code.
        /// </value>
        /// <remarks>
        /// English by default.
        /// </remarks>
        public string FallbackLanguageAlpha2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyNameProviderConfiguration"/> class.
        /// </summary>
        public CompanyNameProviderConfiguration()
        {
            this.AttemptFallbackByLanguage = true;
            this.FallbackToLanguage = true;
            this.FallbackLanguageAlpha2 = "EN";
        }
    }
}
