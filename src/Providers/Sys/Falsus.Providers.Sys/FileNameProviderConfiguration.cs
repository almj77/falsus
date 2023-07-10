namespace Falsus.Providers.Sys
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="FileNameProvider"/> data provider.
    /// </summary>
    public class FileNameProviderConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileNameProviderConfiguration"/> class.
        /// </summary>
        public FileNameProviderConfiguration()
        {
            this.MinWordCount = 1;
            this.MaxWordCount = 3;
            this.MinExtensionCount = 1;
            this.MaxExtensionCount = 1;
        }

        /// <summary>
        /// Gets or sets the minimum number of words that should
        /// compose the file name.
        /// </summary>
        /// <value>
        /// An integer representing the min. amount of words.
        /// </value>
        /// <remarks>
        /// Default value is 1.
        /// </remarks>
        public int MinWordCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of words that should
        /// composite the file name.
        /// </summary>
        /// <value>
        /// An integer representing the max. amount of words.
        /// </value>
        /// <remarks>
        /// Default value is 3.
        /// </remarks>
        public int MaxWordCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of extensions that should
        /// compose the file name.
        /// </summary>
        /// <value>
        /// An integer representing the min. amount of extensions.
        /// </value>
        /// <remarks>
        /// Default value is 1.
        /// </remarks>
        public int MinExtensionCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of extensions that should
        /// compose the file name.
        /// </summary>
        /// <value>
        /// An integer representing the max. amount of extensions.
        /// </value>
        /// <remarks>
        /// Default value is 1.
        /// </remarks>
        public int MaxExtensionCount { get; set; }
    }
}
