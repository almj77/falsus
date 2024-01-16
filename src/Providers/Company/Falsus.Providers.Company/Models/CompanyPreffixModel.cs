namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a preffix to be used as part of a company name.
    /// </summary>
    internal class CompanyPreffixModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the company name preffix.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a company name preffix.
        /// </value>
        public string Preffix { get; set; }
    }
}
