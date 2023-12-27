namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the association between a language and a country.
    /// </summary>
    internal class LanguageToCountryModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string Alpha2CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the order of importance of the language in the
        /// specified country.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the order of importance.
        /// Lower values confer an higher importance.
        /// </value>
        public int Order { get; set; }
    }
}
