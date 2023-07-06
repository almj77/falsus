namespace Falsus.Providers.Person.Models
{
    /// <summary>
    /// This class contains the details for First Name generation.
    /// </summary>
    internal class FirstNameModel
    {
        /// <summary>
        /// Gets or sets the Two letter code of the language to which
        /// the <see cref="GivenName"/> belongs to.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the two letter language code.
        /// </value>
        public string LanguageTwoLetterCode { get; set; }

        /// <summary>
        /// Gets or sets the given name associated with the specified
        /// <see cref="LanguageTwoLetterCode"/> and <see cref="Gender"/>.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the given name.
        /// </value>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the gender identifier of the <see cref="GivenName"/>.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the gender identifier.
        /// </value>
        public string Gender { get; set; }
    }
}
