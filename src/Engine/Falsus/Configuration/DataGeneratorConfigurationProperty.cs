namespace Falsus.Configuration
{
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Defines the structure for a serializable
    /// property configuration.
    /// </summary>
    public class DataGeneratorConfigurationProperty
    {
        /// <summary>
        /// Gets or sets the unique identifier of this property.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the unique identifier of this property.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified name of the provider attached to this property.
        /// </summary>
        /// <value>
        /// A string containing the fully qualified class name.
        /// </value>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this property allows duplicate values
        /// to be generated.
        /// </summary>
        /// <value>
        /// True if this property does not allow duplicate values, otherwise false.
        /// </value>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this property allows null values.
        /// </summary>
        /// <value>
        /// True if this property does not allow null values, otherwise false.
        /// </value>
        public bool IsNotNull { get; set; }

        /// <summary>
        /// Gets or sets a dictionary containing the arguments for this property.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> index by argument name that contains
        /// the identifiers of the properties representing the
        /// arguments binded to this property.
        /// </value>
        public Dictionary<string, string[]> Arguments { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified name of the type of the value that will be generated for this property.
        /// </summary>
        /// <value>
        /// A string containing the fully qualified class name.
        /// </value>
        public string ValueType { get; set; }

        /// <summary>
        /// Gets or sets a json representation of the provider configuration.
        /// </summary>
        /// <value>
        /// A string containing the JSON serialization of the provider
        /// configuration class.
        /// </value>
        public string ProviderConfiguration { get; set; }

        /// <summary>
        /// Gets or sets an array of named tuples containing the weight and the object value.
        /// </summary>
        /// <value>
        /// An array of named tuples.
        /// </value>
        public (float Weight, string Value)[] Weights { get; set; }

        /// <summary>
        /// Gets or sets the specification for the weighted ranges for this property.
        /// </summary>
        /// <value>
        /// An array of the generic <see cref="WeightedRange{T}"/> class.
        /// </value>
        public WeightedRange<string>[] WeightedRanges { get; set; }
    }
}
