namespace Falsus.GeneratorProperties
{
    /// <summary>
    /// Defines an interface for a property whose values
    /// should be generated based on weighted ranges.
    /// </summary>
    internal interface IRangedDataGeneratorProperty : IDataGeneratorProperty
    {
        /// <summary>
        /// Gets an array of <see cref="WeightedRange"/> instances.
        /// </summary>
        /// <value>
        /// An array of <see cref="WeightedRange"/> instances.
        /// </value>
        WeightedRange[] WeightedRanges { get; }
    }
}
