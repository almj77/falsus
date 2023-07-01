namespace Falsus.Providers.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.Extensions;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="float"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class FloatProvider : DataGeneratorProvider<float>
    {
        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatProvider"/> class.
        /// </summary>
        public FloatProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random <see cref="float"/> value that is greater than the value
        /// of <paramref name="minValue"/> and lower than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="float"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="float"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override float GetRangedRowValue(float minValue, float maxValue, float[] excludedObjects)
        {
            float value = this.Randomizer.NextFloat(minValue, maxValue);
            int attempts = 0;
            while (excludedObjects.Contains(value))
            {
                value = this.Randomizer.NextFloat(minValue, maxValue);
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(FloatProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Generates a random <see cref="float"/> value
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
        /// An array of <see cref="float"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="float"/> value that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        public override float GetRowValue(DataGeneratorContext context, WeightedRange<float>[] excludedRanges, float[] excludedObjects)
        {
            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(float MinValue, float MaxValue)> ranges = new List<(float MinValue, float MaxValue)>();
                WeightedRange<float>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > float.MinValue)
                    {
                        ranges.Add((float.MinValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < float.MaxValue)
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, float.MaxValue));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(FloatProvider)} was unable to find a range to generate values.");
                }

                (float MinValue, float MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];

                while (ranges.Any() && excludedObjects.Any())
                {
                    try
                    {
                        return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
                    }
                    catch (InvalidOperationException)
                    {
                        ranges.Remove(randomRange);

                        if (!ranges.Any())
                        {
                            throw new InvalidOperationException($"{nameof(FloatProvider)} was unable to find a range to generate values.");
                        }

                        randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(FloatProvider)} was unable to find a range to generate values.");
                }

                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        /// <summary>
        /// Gets a <see cref="float"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="float"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The <paramref name="id"/> should contain a valid string representation of a <see cref="float"/>.
        /// </remarks>
        public override float GetRowValue(string id)
        {
            return float.Parse(id);
        }

        /// <summary>
        /// Generates a random <see cref="float"/> value based on the context
        /// and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="float"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="float"/> value.
        /// </returns>
        public override float GetRowValue(DataGeneratorContext context, float[] excludedObjects)
        {
            float value = this.Randomizer.NextFloat(float.MinValue, float.MaxValue);
            if (excludedObjects.Any())
            {
                int attempts = 0;
                while (excludedObjects.Contains(value))
                {
                    value = this.Randomizer.NextFloat(float.MinValue, float.MaxValue);
                    if (attempts == MaxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(FloatProvider)} cannot generate another unique value.");
                    }

                    attempts++;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="float"/>.
        /// </summary>
        /// <param name="value">The <see cref="float"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Returns the string representation of a <see cref="float"/>.
        /// </remarks>
        public override string GetValueId(float value)
        {
            return value.ToString();
        }
    }
}