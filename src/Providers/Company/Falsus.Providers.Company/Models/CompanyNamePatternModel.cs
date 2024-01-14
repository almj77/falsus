namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a pattern that defines how a company name should be constructed.
    /// </summary>
    internal class CompanyNamePatternModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the company name pattern.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the company name pattern.
        /// </value>
        public string Pattern { get; set; }
    }
}
