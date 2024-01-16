namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a category to be used as part of a company name.
    /// </summary>
    internal class CompanyCategoryModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the company category.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a company category.
        /// </value>
        public string Category { get; set; }
    }
}
