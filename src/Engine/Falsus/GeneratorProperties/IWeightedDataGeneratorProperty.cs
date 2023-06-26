namespace Falsus.GeneratorProperties
{
    /// <summary>
    /// Defines an interface for a property whose values
    /// should be generated based on weights.
    /// </summary>
    internal interface IWeightedDataGeneratorProperty : IDataGeneratorProperty
    {
        /// <summary>
        /// Gets an array of named tuples containing the weight and the object value.
        /// </summary>
        /// <value>
        /// An array of named tuples.
        /// </value>
        (float Weight, object Value)[] Weights { get; }
    }
}
