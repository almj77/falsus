namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the an adjective to be used as part of a company name.
    /// </summary>
    internal class CompanyAdjectiveModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the adjective.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing an adjective.
        /// </value>
        public string Adjective { get; set; }
    }
}
