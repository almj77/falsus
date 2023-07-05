namespace Falsus.Providers.Person.Models
{
    /// <summary>
    /// This class contains the details for Last Name generation.
    /// </summary>
    internal class LastNameModel
    {
        /// <summary>
        /// Gets or sets the Two letter code of the language to which
        /// the <see cref="Surname"/> belongs to.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the two letter language code.
        /// </value>
        public string LanguageTwoLetterCode { get; set; }

        /// <summary>
        /// Gets or sets the given name associated with the specified
        /// <see cref="LanguageTwoLetterCode"/>.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the given name.
        /// </value>
        public string Surname { get; set; }
    }
}
