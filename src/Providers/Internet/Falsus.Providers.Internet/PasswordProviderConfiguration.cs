namespace Falsus.Providers.Internet
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="PasswordProvider"/> data provider.
    /// </summary>
    public class PasswordProviderConfiguration
    {
        /// <summary>
        /// Gets or sets the length for the password strings.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the length of the password.
        /// </value>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the minimum inclusive length for the password strings.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the minimum
        /// length of the password.
        /// </value>
        public int? MinLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum non-inclusive length for the password strings.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the maximum
        /// length of the password.
        /// </value>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not special characters
        /// can be used to generate the password.
        /// </summary>
        /// <value>
        /// True if special characters can be used, otherwise false.
        /// </value>
        public bool IncludeSpecialChars { get; set; }
    }
}
