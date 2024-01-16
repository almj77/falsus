namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a buzz noun to be used as part of a company name.
    /// </summary>
    internal class CompanyBuzzNounModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the buzz noun.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a buzz noun.
        /// </value>
        public string BuzzNoun { get; set; }
    }
}
