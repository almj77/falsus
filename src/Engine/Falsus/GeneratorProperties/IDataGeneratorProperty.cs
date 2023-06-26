namespace Falsus.GeneratorProperties
{
    using System;
    using System.Collections.Generic;
    using Falsus.Providers;

    /// <summary>
    /// Defines an interface for a property that can be
    /// attached to a data provider.
    /// </summary>
    public interface IDataGeneratorProperty
    {
        /// <summary>
        /// Gets the unique identifier of this property.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the unique identifier of this property.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets the <see cref="IDataGeneratorProvider"/> attached to this property.
        /// </summary>
        /// <value>
        /// An implementation of <see cref="IDataGeneratorProvider"/>.
        /// </value>
        IDataGeneratorProvider Provider { get; }

        /// <summary>
        /// Gets a value indicating whether or not this property allows duplicate values
        /// to be generated.
        /// </summary>
        /// <value>
        /// True if this property does not allow duplicate values, otherwise false.
        /// </value>
        bool IsUnique { get; }

        /// <summary>
        /// Gets a value indicating whether or not this property allows null values.
        /// </summary>
        /// <value>
        /// True if this property does not allow null values, otherwise false.
        /// </value>
        bool AllowNull { get; }

        /// <summary>
        /// Gets the dictionary containing the arguments for this property.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> index by argument name that contains
        /// implementations of <see cref="IDataGeneratorProperty"/> representing the
        /// arguments binded to this property.
        /// </value>
        /// <remarks>
        /// When generating data, properties defined as arguments for this property
        /// will be generated first, ensuring that when this property is called
        /// the values for argument properties are already defined.
        /// </remarks>
        Dictionary<string, IDataGeneratorProperty[]> Arguments { get; }

        /// <summary>
        /// Gets the type of the value that will be generated for this property.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the value that will be generated for this property.
        /// </value>
        Type ValueType { get; }

        /// <summary>
        /// Specifies an argument for this property.
        /// </summary>
        /// <param name="providerArgName">The unique name of the argument.</param>
        /// <param name="property">The <see cref="IDataGeneratorProperty"/> that represents the argument value.</param>
        /// <returns>This instance.</returns>
        IDataGeneratorProperty WithArgument(string providerArgName, IDataGeneratorProperty property);
    }
}
