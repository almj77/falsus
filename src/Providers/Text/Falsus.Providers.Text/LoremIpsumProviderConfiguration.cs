namespace Falsus.Providers.Text
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="LoremIpsumProvider"/> data provider.
    /// </summary>
    public class LoremIpsumProviderConfiguration
    {
        /// <summary>
        /// Gets or sets the desired number of fragments to generate.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the number of fragments.
        /// </value>
        public int FragmentCount { get; set; }

        /// <summary>
        /// Gets or sets the type of fragment to generate.
        /// </summary>
        /// <value>
        /// One of the possible values of the <see cref="LoremIpsumFragmentType"/> enum.
        /// </value>
        /// <remarks>
        /// Defaults to <see cref="LoremIpsumFragmentType.Paragraph"/>.
        /// </remarks>
        public LoremIpsumFragmentType FragmentType { get; set; }
    }
}
