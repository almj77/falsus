namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a buzz adjective to be used as part of a company name.
    /// </summary>
    internal class CompanyBuzzAdjectiveModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the buzz adjective.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a buzz adjective.
        /// </value>
        public string BuzzAdjective { get; set; }
    }
}
