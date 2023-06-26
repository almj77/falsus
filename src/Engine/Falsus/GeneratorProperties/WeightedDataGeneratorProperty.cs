namespace Falsus.GeneratorProperties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.Providers;

    /// <summary>
    /// Definesa property whose values should be generated based on weights.
    /// </summary>
    /// <typeparam name="T">The type of the value that will be generated for this property.</typeparam>
    /// <seealso cref="DataGeneratorProperty{T}"/>
    /// <seealso cref="IWeightedDataGeneratorProperty"/>
    public class WeightedDataGeneratorProperty<T> : DataGeneratorProperty<T>, IWeightedDataGeneratorProperty
    {
        /// <summary>
        /// Gets an array of named tuples containing the weight and the object value.
        /// </summary>
        /// <value>
        /// An array of named tuples.
        /// </value>
        private (float Weight, object Value)[] genericWeights;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedDataGeneratorProperty{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of this property.</param>
        public WeightedDataGeneratorProperty(string id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedDataGeneratorProperty{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of this property.</param>
        /// <param name="isUnique">
        /// Indicates whether or not this property allows duplicate values
        /// to be generated.
        /// </param>
        /// <param name="isNotNull">
        /// Indicates whether or not this property allows null values.
        /// </param>
        /// <param name="arguments">
        /// A dictionary, indexed by argument name, that contains the arguments for this property.
        /// </param>
        /// <param name="provider">
        /// the provider responsible for generating values for this property.
        /// </param>
        /// <param name="weights">
        /// An array of named tuples containing the weight and the <typeparamref name="T"/> value.
        /// </param>
        public WeightedDataGeneratorProperty(string id, bool isUnique, bool isNotNull, IDataGeneratorProvider<T> provider, Dictionary<string, IDataGeneratorProperty[]> arguments, (float Weight, T Value)[] weights)
            : base(id, isUnique, isNotNull, provider, arguments)
        {
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }

            this.Weights = weights;
            this.genericWeights = this.Weights.Select(u => (u.Weight, (object)u.Value)).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedDataGeneratorProperty{T}"/> class.
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
        /// <param name="weights">
        /// An array of named tuples containing the weight and the <typeparamref name="T"/> value.
        /// </param>
        public WeightedDataGeneratorProperty(string id, bool isUnique, bool isNotNull, IDataGeneratorProvider<T> provider, (float Weight, T Value)[] weights)
            : base(id, isUnique, isNotNull, provider)
        {
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }

            this.Weights = weights;
            this.genericWeights = this.Weights.Select(u => (u.Weight, (object)u.Value)).ToArray();
        }

        /// <summary>
        /// Gets an array of named tuples containing the weight and the <typeparamref name="T"/> value.
        /// </summary>
        /// <value>
        /// An array of named tuples.
        /// </value>
        public (float Weight, T Value)[] Weights { get; private set; }

        /// <summary>
        /// Gets an array of named tuples containing the weight and the object value.
        /// </summary>
        /// <value>
        /// An array of named tuples.
        /// </value>
        (float Weight, object Value)[] IWeightedDataGeneratorProperty.Weights => this.genericWeights;

        /// <summary>
        /// Specifies the provider responsible for generating values for this property.
        /// </summary>
        /// <param name="instance">An implementation of <see cref="IDataGeneratorProvider{T}"/>.</param>
        /// <returns>This instance.</returns>
        public new WeightedDataGeneratorProperty<T> FromProvider(IDataGeneratorProvider<T> instance)
        {
            return base.FromProvider(instance) as WeightedDataGeneratorProperty<T>;
        }

        /// <summary>
        /// Specifies the weighted values for this property.
        /// </summary>
        /// <param name="weights">An array of named tuples containing the weight and the <typeparamref name="T"/> value.</param>
        /// <returns>This instance.</returns>
        public DataGeneratorProperty<T> WithWeightedValues(params (float Weight, T Value)[] weights)
        {
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }

            this.Weights = weights;
            this.genericWeights = this.Weights.Select(u => (u.Weight, (object)u.Value)).ToArray();
            return this;
        }
    }
}
