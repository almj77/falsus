namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a buzz verb to be used as part of a company name.
    /// </summary>
    internal class CompanyBuzzVerbModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the buzz verb.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a buzz verb.
        /// </value>
        public string BuzzVerb { get; set; }
    }
}