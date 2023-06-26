namespace Falsus.GeneratorProperties
{
    using System;

    /// <summary>
    /// Defines a weighted range.
    /// </summary>
    /// <remarks>
    /// Use the generic <see cref="WeightedRange{T}"/> class
    /// whenever possible.
    /// </remarks>
    public class WeightedRange
    {
        /// <summary>
        /// Gets or sets the weight of the range.
        /// </summary>
        /// <value>
        /// A value between 0 and 1.
        /// </value>
        public float Weight { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for the range.
        /// </summary>
        /// <value>
        /// An <see cref="object"/> representing the minium value for this range.
        /// </value>
        /// <remarks>
        /// The <see cref="object"/> must implement <see cref="IComparable"/>.
        /// </remarks>
        public object MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the range.
        /// </summary>
        /// <value>
        /// An <see cref="object"/> representing maximum value for this range.
        /// </value>
        /// <remarks>
        /// The <see cref="object"/> must implement <see cref="IComparable"/>.
        /// </remarks>
        public object MaxValue { get; set; }
    }

    /// <summary>
    /// Defines a weighted range of <typeparamref name="T"/> instances..
    /// </summary>
    /// <typeparam name="T">
    /// The type of the range values.
    /// Must implement <see cref="IComparable{T}"/>.
    /// </typeparam>
    public class WeightedRange<T> : WeightedRange
    {
        /// <summary>
        /// Gets or sets the minimum value for the range.
        /// </summary>
        /// <value>
        /// An instance of <typeparamref name="T"/>representing the minium value for this range.
        /// </value>
        /// <remarks>
        /// The <typeparamref name="T"/> class must implement <see cref="IComparable"/>.
        /// </remarks>
        public new T MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for the range.
        /// </summary>
        /// <value>
        /// An instance of <typeparamref name="T"/> representing maximum value for this range.
        /// </value>
        /// <remarks>
        /// The <typeparamref name="T"/> class must implement <see cref="IComparable"/>.
        /// </remarks>
        public new T MaxValue { get; set; }
    }
}
