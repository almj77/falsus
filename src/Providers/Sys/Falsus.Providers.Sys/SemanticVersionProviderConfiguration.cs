namespace Falsus.Providers.Sys
{
    /// <summary>
    /// This class represents the configuration object of the <see cref="SemanticVersionProvider"/> data provider.
    /// </summary>
    public class SemanticVersionProviderConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the "Alpha" release stage
        /// can be used to generate the semantic version.
        /// </summary>
        /// <value>
        /// True if the "Alpha" release stage can be used, otherwise false.
        /// </value>
        public bool IncludeAlphaStage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the "Beta" release stage
        /// can be used to generate the semantic version.
        /// </summary>
        /// <value>
        /// True if the "Beta" release stage can be used, otherwise false.
        /// </value>
        public bool IncludeBetaStage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the "Release candidate" release stage
        /// can be used to generate the semantic version.
        /// </summary>
        /// <value>
        /// True if the "Release candidate" release stage can be used, otherwise false.
        /// </value>
        public bool IncludeReleaseCandidateStage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the numeric status format
        /// should be used to generate the semantic relase stage component of the semantic version.
        /// </summary>
        /// <value>
        /// True if the numeric status format should be used, otherwise false.
        /// </value>
        /// <remarks>
        /// When set to true, instead of generating semantic versions such as "X.X.X-a.X", "X.X.X-b.X" or "X.X.X-rc.X"
        /// it will generate semantic versions as follows "X.X.0.X", "X.X.1.X" or "X.X.2.X".
        /// </remarks>
        public bool UseNumStatusFormat { get; set; }

        /// <summary>
        /// Gets or sets the minimum inclusive value for the Major component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the minimum Major version.
        /// </value>
        public int? MinMajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the Major component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the maximum Major version.
        /// </value>
        public int? MaxMajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum inclusive value for the Minor component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the minimum Minor version.
        /// </value>
        public int? MinMinorVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the Minor component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the maximum Minor version.
        /// </value>
        public int? MaxMinorVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum inclusive value for the Patch component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the minimum Patch version.
        /// </value>
        public int? MinPatchVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the Patch component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the maximum Patch version.
        /// </value>
        public int? MaxPatchVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum inclusive value for the release stage component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the minimum release stage version.
        /// </value>
        public string MinStageVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum inclusive value for the release stage component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the maximum release stage version.
        /// </value>
        public string MaxStageVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum inclusive value for the stage number component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the minimum stage number.
        /// </value>
        public int? MinStageNumber { get; set; }

        /// <summary>
        /// Gets or sets the maximum inclusive value for the stage number component of the version.
        /// </summary>
        /// <value>
        /// A nullable <see cref="int"/> representing the maximum stage number.
        /// </value>
        public int? MaxStageNumber { get; set; }
    }
}
