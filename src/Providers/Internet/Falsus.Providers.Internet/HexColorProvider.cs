namespace Falsus.Providers.Internet
{
    using System;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of hexadecimal color values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class HexColorProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="HexColorProvider"/> class.
        /// </summary>
        public HexColorProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random hexadecimal color that is greater than the value
        /// of <paramref name="minValue"/> and lower than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="string"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged colors is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(HexColorProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random hexadecimal color
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
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="string"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged hexadecimal colors is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(HexColorProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="string"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="string"/> value with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="id"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="id"/> argument.
        /// </remarks>
        public override string GetRowValue(string id)
        {
            return id;
        }

        /// <summary>
        /// Generates a random hexadecimal color based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random hexadecimal value.
        /// </returns>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string value = string.Format("#{0:X6}", this.Randomizer.Next(0x1000000));
            if (excludedObjects.Any())
            {
                int attempts = 0;
                while (excludedObjects.Contains(value))
                {
                    value = string.Format("#{0:X6}", this.Randomizer.Next(0x1000000));
                    if (attempts == MaxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(HexColorProvider)} cannot generate another unique value.");
                    }

                    attempts++;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(string value)
        {
            return value;
        }
    }
}
