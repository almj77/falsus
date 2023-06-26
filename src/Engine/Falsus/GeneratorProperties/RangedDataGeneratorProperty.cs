namespace Falsus.GeneratorProperties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.Providers;

    /// <summary>
    /// Defines a property whose values
    /// should be generated based on weighted ranges.
    /// </summary>
    /// <seealso cref="DataGeneratorProperty{T}"/>
    /// <seealso cref="IRangedDataGeneratorProperty"/>
    /// <typeparam name="T">The type of the value that will be generated for this property.</typeparam>
    public class RangedDataGeneratorProperty<T> : DataGeneratorProperty<T>, IRangedDataGeneratorProperty
        where T : IComparable<T>
    {
        /// <summary>
        /// An array containing the specification of the weighted ranges.
        /// </summary>
        private WeightedRange[] genericRanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedDataGeneratorProperty{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of this property.</param>
        public RangedDataGeneratorProperty(string id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedDataGeneratorProperty{T}"/> class.
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
        /// <param name="weightedRanges">
        /// The the specification for the weighted ranges for this property.
        /// </param>
        public RangedDataGeneratorProperty(string id, bool isUnique, bool isNotNull, IDataGeneratorProvider<T> provider, Dictionary<string, IDataGeneratorProperty[]> arguments, WeightedRange<T>[] weightedRanges)
            : base(id, isUnique, isNotNull, provider, arguments)
        {
            if (weightedRanges == null)
            {
                throw new ArgumentNullException(nameof(weightedRanges));
            }

            this.WeightedRanges = weightedRanges;
            this.genericRanges = this.WeightedRanges.Select(u => new WeightedRange() { MaxValue = u.MaxValue, MinValue = u.MinValue, Weight = u.Weight }).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedDataGeneratorProperty{T}"/> class.
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
        /// <param name="weightedRanges">
        /// The the specification for the weighted ranges for this property.
        /// </param>
        public RangedDataGeneratorProperty(string id, bool isUnique, bool isNotNull, IDataGeneratorProvider<T> provider, WeightedRange<T>[] weightedRanges)
            : base(id, isUnique, isNotNull, provider)
        {
            if (weightedRanges == null)
            {
                throw new ArgumentNullException(nameof(weightedRanges));
            }

            this.WeightedRanges = weightedRanges;
            this.genericRanges = this.WeightedRanges.Select(u => new WeightedRange() { MaxValue = u.MaxValue, MinValue = u.MinValue, Weight = u.Weight }).ToArray();
        }

        /// <summary>
        /// Gets the specification for the weighted ranges for this property.
        /// </summary>
        /// <value>
        /// An array of the generic <see cref="WeightedRange{T}"/> class.
        /// </value>
        public WeightedRange<T>[] WeightedRanges { get; private set; }

        /// <summary>
        /// Gets an array of <see cref="WeightedRange"/> instances.
        /// </summary>
        /// <value>
        /// An array of <see cref="WeightedRange"/> instances.
        /// </value>
        WeightedRange[] IRangedDataGeneratorProperty.WeightedRanges => this.genericRanges;

        /// <summary>
        /// Specifies the provider responsible for generating values for this property.
        /// </summary>
        /// <param name="instance">An implementation of <see cref="IDataGeneratorProvider{T}"/>.</param>
        /// <returns>This instance.</returns>
        public new RangedDataGeneratorProperty<T> FromProvider(IDataGeneratorProvider<T> instance)
        {
            return base.FromProvider(instance) as RangedDataGeneratorProperty<T>;
        }

        /// <summary>
        /// Specifies the weighted ranges for this property.
        /// </summary>
        /// <param name="ranges">An array of <see cref="WeightedRange{T}"/> instances.</param>
        /// <returns>This instance.</returns>
        public RangedDataGeneratorProperty<T> WithWeightedRanges(params WeightedRange<T>[] ranges)
        {
            if (ranges == null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            this.WeightedRanges = ranges;
            this.genericRanges = this.WeightedRanges.Select(u => new WeightedRange() { MaxValue = u.MaxValue, MinValue = u.MinValue, Weight = u.Weight }).ToArray();
            return this;
        }
    }
}
