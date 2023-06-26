namespace Falsus.GeneratorProperties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.Providers;

    /// <summary>
    /// Defines a generic property that can be
    /// attached to a data provider.
    /// </summary>
    /// <typeparam name="T">The type of the value that will be generated for this property.</typeparam>
    public class DataGeneratorProperty<T> : IDataGeneratorProperty
    {
        /// <summary>
        /// Gets the dictionary containing the arguments for this property.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> index by argument name that contains
        /// implementations of <see cref="IDataGeneratorProperty"/> representing the
        /// arguments binded to this property.
        /// </value>
        /// <remarks>
        /// This dictionary contains a <see cref="List{T}"/> of <see cref="IDataGeneratorProperty"/>
        /// that is used internally to store the arguments added by the fluent methods.
        /// </remarks>
        private Dictionary<string, List<IDataGeneratorProperty>> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorProperty{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of this property.</param>
        public DataGeneratorProperty(string id)
        {
            this.Id = id;
            this.AllowNull = false;
            this.arguments = new Dictionary<string, List<IDataGeneratorProperty>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorProperty{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of this property.</param>
        /// <param name="isUnique">
        /// Indicates whether or not this property allows duplicate values
        /// to be generated.
        /// </param>
        /// <param name="isNotNull">
        /// Indicates whether or not this property allows null values.
        /// </param>
        /// <param name="provider">
        /// the provider responsible for generating values for this property.
        /// </param>
        /// <param name="arguments">
        /// A dictionary, indexed by argument name, that contains the arguments for this property.
        /// </param>
        public DataGeneratorProperty(string id, bool isUnique, bool isNotNull, IDataGeneratorProvider<T> provider, Dictionary<string, IDataGeneratorProperty[]> arguments)
        {
            this.Id = id;
            this.IsUnique = isUnique;
            this.AllowNull = isNotNull;
            this.Provider = provider;
            this.arguments = arguments.ToDictionary(k => k.Key, v => new List<IDataGeneratorProperty>(v.Value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorProperty{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of this property.</param>
        /// <param name="isUnique">
        /// Indicates whether or not this property allows duplicate values
        /// to be generated.
        /// </param>
        /// <param name="isNotNull">
        /// Indicates whether or not this property allows null values.
        /// </param>
        /// <param name="provider">
        /// the provider responsible for generating values for this property.
        /// </param>
        public DataGeneratorProperty(string id, bool isUnique, bool isNotNull, IDataGeneratorProvider<T> provider)
        {
            this.Id = id;
            this.IsUnique = isUnique;
            this.AllowNull = isNotNull;
            this.Provider = provider;
            this.arguments = new Dictionary<string, List<IDataGeneratorProperty>>();
        }

        /// <summary>
        /// Gets the unique identifier of this property.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the unique identifier of this property.
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the <see cref="IDataGeneratorProvider"/> attached to this property.
        /// </summary>
        /// <value>
        /// An implementation of <see cref="IDataGeneratorProvider"/>.
        /// </value>
        public IDataGeneratorProvider Provider { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not this property allows duplicate values
        /// to be generated.
        /// </summary>
        /// <value>
        /// True if this property does not allow duplicate values, otherwise false.
        /// </value>
        public bool IsUnique { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not this property allows null values.
        /// </summary>
        /// <value>
        /// True if this property does not allow null values, otherwise false.
        /// </value>
        public bool AllowNull { get; private set; }

        /// <summary>
        /// Gets the type of the value that will be generated for this property.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the value that will be generated for this property.
        /// </value>
        public Type ValueType
        {
            get
            {
                return typeof(T);
            }
        }

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
        public Dictionary<string, IDataGeneratorProperty[]> Arguments
        {
            get
            {
                return this.arguments.ToDictionary(k => k.Key, v => v.Value.ToArray());
            }
        }

        /// <summary>
        /// Specifies the provider responsible for generating values for this property.
        /// </summary>
        /// <param name="instance">An implementation of <see cref="IDataGeneratorProvider{T}"/>.</param>
        /// <returns>This instance.</returns>
        public DataGeneratorProperty<T> FromProvider(IDataGeneratorProvider<T> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.Provider = instance;
            return this;
        }

        /// <summary>
        /// Specifies an argument for this property.
        /// </summary>
        /// <param name="providerArgName">The unique name of the argument.</param>
        /// <param name="property">The <see cref="IDataGeneratorProperty"/> that represents the argument value.</param>
        /// <returns>This instance.</returns>
        public IDataGeneratorProperty WithArgument(string providerArgName, IDataGeneratorProperty property)
        {
            if (providerArgName == null)
            {
                throw new ArgumentNullException(nameof(providerArgName));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!this.arguments.ContainsKey(providerArgName))
            {
                this.arguments.Add(providerArgName, new List<IDataGeneratorProperty>());
            }

            this.arguments[providerArgName].Add(property);

            return this;
        }

        /// <summary>
        /// Specifies an argument for this property.
        /// </summary>
        /// <typeparam name="T1">The <see cref="Type"/> of the argument.</typeparam>
        /// <param name="providerArgName">The unique name of the argument.</param>
        /// <param name="property">The <see cref="DataGeneratorProperty{T}"/> that represents the argument value.</param>
        /// <returns>This instance.</returns>
        public DataGeneratorProperty<T> WithArgument<T1>(string providerArgName, DataGeneratorProperty<T1> property)
        {
            if (providerArgName == null)
            {
                throw new ArgumentNullException(nameof(providerArgName));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!this.arguments.ContainsKey(providerArgName))
            {
                this.arguments.Add(providerArgName, new List<IDataGeneratorProperty>());
            }

            this.arguments[providerArgName].Add(property);

            return this;
        }

        /// <summary>
        /// Indicates that this property does not allow duplicate values.
        /// </summary>
        /// <returns>This instance.</returns>
        public DataGeneratorProperty<T> Unique()
        {
            this.IsUnique = true;
            return this;
        }

        /// <summary>
        /// Indicates that this property does not allow null values.
        /// </summary>
        /// <returns>This instance.</returns>
        public DataGeneratorProperty<T> NotNull()
        {
            this.AllowNull = false;
            return this;
        }
    }
}
