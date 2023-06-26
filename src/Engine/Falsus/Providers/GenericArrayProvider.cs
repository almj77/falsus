namespace Falsus.Providers
{
    using System;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Defines the abstract class for an array based data provider supporting all methods defined by the
    /// the <see cref="DataGeneratorProvider{T}"/> base class.
    /// </summary>
    /// <typeparam name="T">The type of data returned by this provider.</typeparam>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public abstract class GenericArrayProvider<T> : DataGeneratorProvider<T>
        where T : IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericArrayProvider{T}"/> class.
        /// </summary>
        public GenericArrayProvider()
            : base()
        {
        }

        /// <summary>
        /// Generates a random instance of <typeparamref name="T"/>
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <typeparamref name="T"/>s that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <typeparamref name="T"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override T GetRangedRowValue(T minValue, T maxValue, T[] excludedObjects)
        {
            T[] values = this.GetValues();

            if (typeof(T) is IComparable<T> || typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
            {
                values = values.Where(u => ((IComparable<T>)u).CompareTo(minValue) > 0 && ((IComparable<T>)u).CompareTo(maxValue) < 0).ToArray();
            }

            if (excludedObjects.Any())
            {
                values = values.Except(excludedObjects).ToArray();
            }

            int minRandomValue = 0;
            int maxRandomValue = values.Length;

            if (!values.Any())
            {
                return default(T);
            }

            if (values.Length == 1)
            {
                return values.First();
            }

            T value = values[this.Randomizer.Next(minRandomValue, maxRandomValue)];

            return value;
        }

        /// <summary>
        /// Generates a random instance of <typeparamref name="T"/>
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <typeparamref name="T"/>s that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <typeparamref name="T"/>.
        /// </returns>
        public override T GetRowValue(DataGeneratorContext context, T[] excludedObjects)
        {
            T[] values = this.GetValues(context);

            if (excludedObjects.Any())
            {
                values = values.Except(excludedObjects).ToArray();
            }

            if (!values.Any())
            {
                throw new InvalidOperationException($"{this.GetType().Name} cannot generate another unique value.");
            }

            if (values.Length == 1)
            {
                return values.First();
            }

            int minRandomValue = 0;
            int maxRandomValue = values.Length;

            T value = values[this.Randomizer.Next(minRandomValue, maxRandomValue)];

            return value;
        }

        /// <summary>
        /// Generates a random instance of the object type supported by this provider
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
        /// An array of <typeparamref name="T"/>s that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="object"/>.
        /// </returns>
        public override T GetRowValue(DataGeneratorContext context, WeightedRange<T>[] excludedRanges, T[] excludedObjects)
        {
            T[] values = this.GetValues();

            if (excludedRanges != null && excludedRanges.Any())
            {
                foreach (WeightedRange<T> range in excludedRanges)
                {
                    values = values.Where(u => ((IComparable<T>)u).CompareTo(range.MinValue) < 0 || ((IComparable<T>)u).CompareTo(range.MaxValue) > 0).ToArray();
                }
            }

            if (excludedObjects.Any())
            {
                values = values.Except(excludedObjects).ToArray();
            }

            int minRandomValue = 0;
            int maxRandomValue = values.Length;

            if (!values.Any())
            {
                return default(T);
            }

            if (values.Length == 1)
            {
                return values.First();
            }

            T value = values[this.Randomizer.Next(minRandomValue, maxRandomValue)];

            return value;
        }

        /// <summary>
        /// Gets all instances of <typeparamref name="T"/> based on the generation context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of <typeparamref name="T"/>s.
        /// </returns>
        /// <remarks>
        /// This method should return all possible values for the <paramref name="context"/>,
        /// excluded objects will be removed afterwards.
        /// </remarks>
        protected abstract T[] GetValues(DataGeneratorContext context);

        /// <summary>
        /// Gets all instances of <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// An array of <typeparamref name="T"/>s.
        /// </returns>
        protected abstract T[] GetValues();
    }
}
