namespace Falsus.Providers
{
    using System;

    /// <summary>
    /// Defines the structure of an argument for a <see cref="DataGeneratorProvider{T}"/>.
    /// </summary>
    public class DataGeneratorProviderArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorProviderArgument"/> class.
        /// </summary>
        /// <param name="argumentType">
        /// The <see cref="Type"/> of the argument value.
        /// </param>
        /// <param name="propertyName">
        /// The name of the <see cref="GeneratorProperties.DataGeneratorProperty{T}"/> responsible for providing the value.
        /// </param>
        /// <param name="argumentName">
        /// The name of the argument that the <see cref="DataGeneratorProvider{T}"/> is expecting.
        /// </param>
        public DataGeneratorProviderArgument(Type argumentType, string propertyName, string argumentName)
        {
            this.ArgumentType = argumentType;
            this.PropertyName = propertyName;
            this.ArgumentName = argumentName;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the argument value.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the argument value.
        /// </value>
        public Type ArgumentType { get; private set; }

        /// <summary>
        /// Gets the name of the <see cref="GeneratorProperties.DataGeneratorProperty{T}"/> responsible for providing the value.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the name of the argument that the <see cref="DataGeneratorProvider{T}"/> is expecting.
        /// </summary>
        /// <value>
        /// The name of the argument.
        /// </value>
        public string ArgumentName { get; private set; }
    }
}
