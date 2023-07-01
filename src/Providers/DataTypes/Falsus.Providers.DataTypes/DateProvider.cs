namespace Falsus.Providers.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="DateTime"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class DateProvider : DataGeneratorProvider<DateTime>
    {
        /// <summary>
        /// The maximum number of values that can be generated.
        /// </summary>
        private readonly int datasetLength;

        /// <summary>
        /// The maximum number of times the provider will attempt
        /// to generate an unique value before throwing an exception.
        /// </summary>
        private readonly int maxAttempts;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateProvider"/> class.
        /// </summary>
        public DateProvider()
            : base()
        {
            this.datasetLength = Convert.ToInt32(Math.Floor((DateTime.MaxValue - DateTime.MinValue).TotalDays));
            this.maxAttempts = Convert.ToInt32(this.datasetLength * 0.1);
        }

        /// <summary>
        /// Generates a random <see cref="DateTime"/> instance
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="DateTime"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="DateTime"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override DateTime GetRangedRowValue(DateTime minValue, DateTime maxValue, DateTime[] excludedObjects)
        {
            int dayDiff = Convert.ToInt32(Math.Floor((maxValue - minValue).TotalDays));
            DateTime value = minValue.AddDays(this.Randomizer.Next(0, dayDiff)).Date;

            int attempts = 0;
            while (excludedObjects.Contains(value))
            {
                value = minValue.AddDays(this.Randomizer.Next(0, dayDiff)).Date;
                if (attempts == this.maxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(DateProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Generates a random <see cref="DateTime"/> instance
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
        /// An array of <see cref="DateTime"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="DateTime"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        public override DateTime GetRowValue(DataGeneratorContext context, WeightedRange<DateTime>[] excludedRanges, DateTime[] excludedObjects)
        {
            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(DateTime MinValue, DateTime MaxValue)> ranges = new List<(DateTime MinValue, DateTime MaxValue)>();
                WeightedRange<DateTime>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > DateTime.MinValue)
                    {
                        ranges.Add((DateTime.MinValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < DateTime.MaxValue)
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, DateTime.MaxValue));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(DateProvider)} was unable to find a range to generate values.");
                }

                (DateTime MinValue, DateTime MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];

                while (ranges.Count > 1 && excludedObjects.Any() && excludedObjects.Count(u => u > randomRange.MinValue && u < randomRange.MaxValue) >= (randomRange.MaxValue - randomRange.MinValue).TotalDays - 1)
                {
                    ranges.Remove(randomRange);
                    randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(DateProvider)} was unable to find a range to generate values.");
                }

                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="DateTime"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="DateTime"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The provider expects the <paramref name="id"/> argument to be the
        /// string representation of the <see cref="DateTime.Ticks"/> property.
        /// The unique id can be fetched by invoking the
        /// <see cref="DateProvider.GetValueId(DateTime)"/> method.
        /// </remarks>
        public override DateTime GetRowValue(string id)
        {
            return new DateTime(long.Parse(id));
        }

        /// <summary>
        /// Generates a random <see cref="DateTime"/> instance
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="DateTime"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="DateTime"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="DateTime"/> instance generated by this provider
        /// will always have the time component set as 00:00:00.
        /// </remarks>
        public override DateTime GetRowValue(DataGeneratorContext context, DateTime[] excludedObjects)
        {
            if (excludedObjects.Length == this.datasetLength)
            {
                throw new InvalidOperationException($"{nameof(DateProvider)} cannot generate another unique value.");
            }

            DateTime value = this.GetRangedRowValue(DateTime.MinValue, DateTime.MaxValue, excludedObjects);
            if (excludedObjects.Any())
            {
                int attemptCount = 0;
                while (excludedObjects.Contains(value))
                {
                    value = this.GetRangedRowValue(DateTime.MinValue, DateTime.MaxValue, excludedObjects);
                    attemptCount++;

                    if (attemptCount == this.maxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(DateProvider)} was unable to generate unique value after {attemptCount} attempts.");
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="DateTime"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="DateTime"/>.
        /// </returns>
        public override string GetValueId(DateTime value)
        {
            return value.Ticks.ToString();
        }
    }
}
