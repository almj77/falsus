namespace Falsus.Configuration
{
    /// <summary>
    /// Defines a serializable structure for the <see cref="DataGenerator"/>
    /// configuration.
    /// </summary>
    public class DataGeneratorConfiguration
    {
        /// <summary>
        /// Gets or sets the number used to calculate a starting value for the pseudo-random number sequence.
        /// </summary>
        /// <value>
        /// A nullable integer to seed the pseudo-random number generator.
        /// </value>
        public int? Seed { get; set; }

        /// <summary>
        /// Gets or sets an array of property configurations used
        /// to configure a <see cref="DataGenerator"/>.
        /// </summary>
        /// <value>
        /// An array of <see cref="DataGeneratorConfigurationProperty"/>.
        /// </value>
        public DataGeneratorConfigurationProperty[] Properties { get; set; }
    }
}
