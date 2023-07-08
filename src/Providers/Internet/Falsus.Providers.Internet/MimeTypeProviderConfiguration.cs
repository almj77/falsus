namespace Falsus.Providers.Internet
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="MimeTypeProvider"/> data provider.
    /// </summary>
    public class MimeTypeProviderConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not deprecated
        /// MIME types should be considered for random value generation.
        /// </summary>
        /// <value>
        /// True if deprecated MIME types should be included, otherwise false.
        /// </value>
        public bool IncludeDeprecated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not uncommon
        /// MIME types should be excluded from the random value generation.
        /// </summary>
        /// <value>
        /// True if uncommon MIME types should be excluded, otherwise false.
        /// </value>
        public bool ExcludeUncommon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not MIME type aliases
        /// should be considered as unique.
        /// </summary>
        /// <value>
        /// True if aliases should be considered unique, otherwise false.
        /// </value>
        /// <remarks>
        /// Settings this value to true will cause, for example,
        /// application/javascript and text/javascript to be treated as two
        /// different MIME types.
        /// </remarks>
        public bool TreatAliasesAsUnique { get; set; }
    }
}
