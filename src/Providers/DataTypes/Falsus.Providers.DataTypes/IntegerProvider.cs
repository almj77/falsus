namespace Falsus.Providers.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="int"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class IntegerProvider : DataGeneratorProvider<int>
    {
        /// <summary>
        /// The maximum number of values that can be generated.
        /// </summary>
        private readonly long datasetLength;

        /// <summary>
        /// The maximum number of times the provider will attempt
        /// to generate an unique value before throwing an exception.
        /// </summary>
        private readonly int maxAttempts;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerProvider"/> class.
        /// </summary>
        public IntegerProvider()
            : base()
        {
            this.datasetLength = Math.BigMul(int.MaxValue, 2);
            this.maxAttempts = Convert.ToInt32(this.datasetLength * 0.001);
        }

        /// <summary>
        /// Generates a random <see cref="int"/> value that is greater than the value
        /// of <paramref name="minValue"/> and lower than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="int"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="int"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override int GetRangedRowValue(int minValue, int maxValue, int[] excludedObjects)
        {
            int value = this.Randomizer.Next(minValue, maxValue);
            if (excludedObjects.Any() && excludedObjects.All(u => u >= minValue && u <= maxValue) && excludedObjects.Length == maxValue - minValue)
            {
                throw new InvalidOperationException($"{nameof(IntegerProvider)} cannot generate another unique value.");
            }

            int attemptCount = 0;
            while (excludedObjects.Contains(value))
            {
                value = this.Randomizer.Next(minValue, maxValue);
                attemptCount++;

                if (attemptCount == this.maxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(IntegerProvider)} was unable to generate unique value after {attemptCount} attempts.");
                }
            }

            return value;
        }

        /// <summary>
        /// Generates a random <see cref="int"/> value based on the context and excluded ranges.
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
        /// An array of <see cref="int"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="int"/> value that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        public override int GetRowValue(DataGeneratorContext context, WeightedRange<int>[] excludedRanges, int[] excludedObjects)
        {
            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(int MinValue, int MaxValue)> ranges = new List<(int MinValue, int MaxValue)>();
                WeightedRange<int>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > int.MinValue)
                    {
                        ranges.Add((int.MinValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < int.MaxValue)
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, int.MaxValue));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(IntegerProvider)} was unable to find a range to generate values.");
                }

                (int MinValue, int MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];

                while (ranges.Count > 1 && excludedObjects.Any() && excludedObjects.Count(u => u > randomRange.MinValue && u < randomRange.MaxValue) >= randomRange.MaxValue - randomRange.MinValue - 1)
                {
                    ranges.Remove(randomRange);
                    randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(IntegerProvider)} was unable to find a range to generate values.");
                }

                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        /// <summary>
        /// Gets an <see cref="int"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="int"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The <paramref name="id"/> should contain a valid string representation of a <see cref="int"/>.
        /// </remarks>
        public override int GetRowValue(string id)
        {
            return int.Parse(id);
        }

        /// <summary>
        /// Generates a random <see cref="int"/> value based on the context
        /// and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="int"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="int"/> value.
        /// </returns>
        public override int GetRowValue(DataGeneratorContext context, int[] excludedObjects)
        {
            if (excludedObjects.Length == this.datasetLength)
            {
                throw new InvalidOperationException($"{nameof(IntegerProvider)} cannot generate another unique value.");
            }

            int value = this.Randomizer.Next(int.MinValue, int.MaxValue);
            if (excludedObjects.Any())
            {
                int attemptCount = 0;
                while (excludedObjects.Contains(value))
                {
                    value = this.Randomizer.Next(int.MinValue, int.MaxValue);
                    attemptCount++;

                    if (attemptCount == this.maxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(IntegerProvider)} was unable to generate unique value after {attemptCount} attempts.");
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="int"/>.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Returns the string representation of an <see cref="int"/>.
        /// </remarks>
        public override string GetValueId(int value)
        {
            return value.ToString();
        }
    }
}
