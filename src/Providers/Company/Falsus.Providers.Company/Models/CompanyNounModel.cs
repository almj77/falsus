namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a noun to be used as part of a company name.
    /// </summary>
    internal class CompanyNounModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the company name noun.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a company name noun.
        /// </value>
        public string Noun { get; set; }
    }
}
