namespace Falsus.Providers.DataTypes
{
    using System;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="Guid"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class GuidProvider : DataGeneratorProvider<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidProvider"/> class.
        /// </summary>
        public GuidProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random <see cref="Guid"/> instance
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="Guid"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="Guid"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="Guid"/>
        /// values is not supported.
        /// </exception>
        public override Guid GetRangedRowValue(Guid minValue, Guid maxValue, Guid[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(GuidProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="Guid"/> instance
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
        /// An array of <see cref="Guid"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="Guid"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="Guid"/>
        /// values is not supported.
        /// </exception>
        public override Guid GetRowValue(DataGeneratorContext context, WeightedRange<Guid>[] excludedRanges, Guid[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(GuidProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets an instance of <see cref="Guid"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="Guid"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The <paramref name="id"/> should contain a valid string representation of a <see cref="Guid"/>.
        /// </remarks>
        public override Guid GetRowValue(string id)
        {
            return Guid.Parse(id);
        }

        /// <summary>
        /// Generates a random <see cref="Guid"/> instance
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="Guid"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="Guid"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="excludedObjects"/> are not being taken into account since
        /// <see cref="Guid"/> is always unique.
        /// </remarks>
        public override Guid GetRowValue(DataGeneratorContext context, Guid[] excludedObjects)
        {
            byte[] bytes = new byte[16];
            this.Randomizer.NextBytes(bytes);
            return new Guid(bytes);
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="Guid"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="Guid"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="Guid"/>.
        /// </returns>
        public override string GetValueId(Guid value)
        {
            return value.ToString();
        }
    }
}
