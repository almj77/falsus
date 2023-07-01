namespace Falsus.Providers.DataTypes
{
    using System;
    using System.Linq;
    using Falsus.Extensions;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="bool"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class BooleanProvider : DataGeneratorProvider<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanProvider"/> class.
        /// </summary>
        public BooleanProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random <see cref="bool"/> value that is greater than the value
        /// of <paramref name="minValue"/> and lower than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="bool"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="bool"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because there are no ranges between boolean values.
        /// </exception>
        public override bool GetRangedRowValue(bool minValue, bool maxValue, bool[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(BooleanProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="bool"/> value
        /// based on the context and excluded ranges.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedRanges">
        /// An array of <see cref="WeightedRange{T}"/>s defining the ranges
        /// that cannot be returned by the provider.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="bool"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random<see cref="bool"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because there are no ranges between boolean values.
        /// </exception>
        public override bool GetRowValue(DataGeneratorContext context, WeightedRange<bool>[] excludedRanges, bool[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(BooleanProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="bool"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="bool"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The <paramref name="id"/> should contain a valid string representation of a <see cref="bool"/>.
        /// </remarks>
        public override bool GetRowValue(string id)
        {
            return bool.Parse(id);
        }

        /// <summary>
        /// Generates a random <see cref="bool"/> value based on the context
        /// and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="bool"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="bool"/> value.
        /// </returns>
        public override bool GetRowValue(DataGeneratorContext context, bool[] excludedObjects)
        {
            if (excludedObjects.Any())
            {
                if (excludedObjects.Length >= 2)
                {
                    throw new InvalidOperationException($"{nameof(BooleanProvider)} cannot generate another unique value.");
                }
                else
                {
                    return !excludedObjects[0];
                }
            }

            return this.Randomizer.NextBoolean();
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Returns the string representation of a <see cref="bool"/>.
        /// </remarks>
        public override string GetValueId(bool value)
        {
            return value.ToString();
        }
    }
}
