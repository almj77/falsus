namespace Falsus.Providers.Number
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.Extensions;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="double"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class DoubleProvider : DataGeneratorProvider<double>
    {
        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleProvider"/> class.
        /// </summary>
        public DoubleProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random <see cref="double"/> value that is greater than the value
        /// of <paramref name="minValue"/> and lower than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="double"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="double"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override double GetRangedRowValue(double minValue, double maxValue, double[] excludedObjects)
        {
            double value = this.Randomizer.NextDouble(minValue, maxValue);
            int attempts = 0;
            while (excludedObjects.Contains(value))
            {
                value = this.Randomizer.NextDouble(minValue, maxValue);
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(DoubleProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Generates a random <see cref="double"/> value
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
        /// An array of <see cref="double"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="double"/> value that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        public override double GetRowValue(DataGeneratorContext context, WeightedRange<double>[] excludedRanges, double[] excludedObjects)
        {
            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(double MinValue, double MaxValue)> ranges = new List<(double MinValue, double MaxValue)>();
                WeightedRange<double>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > double.MinValue)
                    {
                        ranges.Add((double.MinValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < double.MaxValue)
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, double.MaxValue));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(DoubleProvider)} was unable to find a range to generate values.");
                }

                (double MinValue, double MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];

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
                            throw new InvalidOperationException($"{nameof(DoubleProvider)} was unable to find a range to generate values.");
                        }

                        randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                    }
                }

                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        /// <summary>
        /// Gets a <see cref="double"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="double"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The <paramref name="id"/> should contain a valid string representation of a <see cref="double"/>.
        /// </remarks>
        public override double GetRowValue(string id)
        {
            return double.Parse(id);
        }

        /// <summary>
        /// Generates a random <see cref="double"/> value based on the context
        /// and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="double"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="double"/> value.
        /// </returns>
        public override double GetRowValue(DataGeneratorContext context, double[] excludedObjects)
        {
            double value = this.Randomizer.NextDouble(double.MinValue, double.MaxValue);
            if (excludedObjects.Any())
            {
                int attempts = 0;
                while (excludedObjects.Contains(value))
                {
                    value = this.Randomizer.NextDouble(double.MinValue, double.MaxValue);
                    if (attempts == MaxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(DoubleProvider)} cannot generate another unique value.");
                    }

                    attempts++;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="double"/>.
        /// </summary>
        /// <param name="value">The <see cref="double"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Returns the string representation of a <see cref="double"/>.
        /// </remarks>
        public override string GetValueId(double value)
        {
            return value.ToString();
        }
    }
}