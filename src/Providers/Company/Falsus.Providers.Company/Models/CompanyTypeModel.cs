namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a type to be used as part of a company name.
    /// </summary>
    internal class CompanyTypeModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the company type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a type of company.
        /// </value>
        public string CompanyType { get; set; }
    }
}
