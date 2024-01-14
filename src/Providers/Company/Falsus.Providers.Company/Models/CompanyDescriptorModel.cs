namespace Falsus.Providers.Company.Models
{
    /// <summary>
    /// This class represents the a descriptor to be used as part of a company name.
    /// </summary>
    internal class CompanyDescriptorModel
    {
        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the company descriptor.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a company descriptor.
        /// </value>
        public string Descriptor { get; set; }
    }
}
