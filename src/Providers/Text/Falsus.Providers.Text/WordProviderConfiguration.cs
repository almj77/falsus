namespace Falsus.Providers.Text
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="WordProvider"/> data provider.
    /// </summary>
    public class WordProviderConfiguration
    {
        /// <summary>
        /// Gets or sets an array containing the types of words to exclude.
        /// </summary>
        /// <value>
        /// An array of <see cref="WordType"/> values representing the
        /// types of words to exclude.
        /// </value>
        public WordType[] ExcludedWordTypes { get; set; }
    }
}
