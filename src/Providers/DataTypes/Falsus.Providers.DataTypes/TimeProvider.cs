namespace Falsus.Providers.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of <see cref="TimeSpan"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class TimeProvider : DataGeneratorProvider<TimeSpan>
    {
        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeProvider"/> class.
        /// </summary>
        public TimeProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan"/> instance
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="TimeSpan"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="TimeSpan"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override TimeSpan GetRangedRowValue(TimeSpan minValue, TimeSpan maxValue, TimeSpan[] excludedObjects)
        {
            double timeSpanDiff = Math.Floor((maxValue - minValue).TotalMilliseconds);
            TimeSpan value = DateTime.MinValue.Add(minValue).AddMilliseconds(this.Randomizer.NextDouble() * timeSpanDiff).TimeOfDay;
            int attempts = 0;

            while (excludedObjects.Contains(value))
            {
                value = DateTime.MinValue.Add(minValue).AddMilliseconds(this.Randomizer.NextDouble() * timeSpanDiff).TimeOfDay;
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(TimeProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan"/> instance
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
        /// An array of <see cref="TimeSpan"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="TimeSpan"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        public override TimeSpan GetRowValue(DataGeneratorContext context, WeightedRange<TimeSpan>[] excludedRanges, TimeSpan[] excludedObjects)
        {
            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(TimeSpan MinValue, TimeSpan MaxValue)> ranges = new List<(TimeSpan MinValue, TimeSpan MaxValue)>();
                WeightedRange<TimeSpan>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > TimeSpan.Zero)
                    {
                        ranges.Add((TimeSpan.Zero, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < TimeSpan.FromDays(1))
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, TimeSpan.FromDays(1)));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(TimeProvider)} was unable to find a range to generate values.");
                }

                (TimeSpan MinValue, TimeSpan MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];

                while (ranges.Count > 1 && excludedObjects.Any() && excludedObjects.Count(u => u > randomRange.MinValue && u < randomRange.MaxValue) >= (randomRange.MaxValue - randomRange.MinValue).TotalMilliseconds - 1)
                {
                    ranges.Remove(randomRange);
                    randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(TimeProvider)} was unable to find a range to generate values.");
                }

                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="TimeSpan"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="TimeSpan"/> with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The provider expects the <paramref name="id"/> argument to be the
        /// string representation of the <see cref="TimeSpan.Ticks"/> property.
        /// The unique id can be fetched by invoking the
        /// <see cref="TimeProvider.GetValueId(TimeSpan)"/> method.
        /// </remarks>
        public override TimeSpan GetRowValue(string id)
        {
            return new TimeSpan(long.Parse(id));
        }

        /// <summary>
        /// Generates a random <see cref="TimeSpan"/> instance
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="TimeSpan"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="TimeSpan"/>.
        /// </returns>
        public override TimeSpan GetRowValue(DataGeneratorContext context, TimeSpan[] excludedObjects)
        {
            TimeSpan value = this.GetRangedRowValue(TimeSpan.Zero, TimeSpan.FromTicks(TimeSpan.TicksPerDay), excludedObjects);
            if (excludedObjects.Any())
            {
                int attempts = 0;
                while (excludedObjects.Contains(value))
                {
                    value = this.GetRangedRowValue(TimeSpan.Zero, TimeSpan.FromTicks(TimeSpan.TicksPerDay), excludedObjects);
                    if (attempts == MaxAttempts)
                    {
                        throw new InvalidOperationException($"{nameof(TimeProvider)} cannot generate another unique value.");
                    }

                    attempts++;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="TimeSpan"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="TimeSpan"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="TimeSpan"/>.
        /// </returns>
        public override string GetValueId(TimeSpan value)
        {
            return value.Ticks.ToString();
        }
    }
}
