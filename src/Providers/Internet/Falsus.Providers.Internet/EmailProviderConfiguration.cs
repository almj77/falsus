namespace Falsus.Providers.Internet
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="EmailProvider"/> data provider.
    /// </summary>
    public class EmailProviderConfiguration
    {
        /// <summary>
        /// Gets or sets an array of <see cref="string"/> values
        /// representing the email domains.
        /// </summary>
        /// <value>
        /// An array of <see cref="string"/> values.
        /// </value>
        public string[] Domains { get; set; }
    }
}
