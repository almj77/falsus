namespace Falsus.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Defines the abstract class for a data provider merging the methods from
    /// the <see cref="IDataGeneratorProvider"/> and <see cref="IDataGeneratorProvider{T}"/> interfaces.
    /// </summary>
    /// <typeparam name="T">The type of data returned by this provider.</typeparam>
    public abstract class DataGeneratorProvider<T> : IDataGeneratorProvider<T>
    {
        /// <summary>
        /// Gets the pseudo-random number generator.
        /// </summary>
        /// <value>
        /// An instance of <see cref="Random"/>.
        /// </value>
        protected Random Randomizer { get; private set; }

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
        public abstract T GetRangedRowValue(T minValue, T maxValue, T[] excludedObjects);

        /// <summary>
        /// Gets an instance of <typeparamref name="T"/> based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <typeparamref name="T"/> with the specified unique identifier.
        /// </returns>
        public abstract T GetRowValue(string id);

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
        public abstract T GetRowValue(DataGeneratorContext context, T[] excludedObjects);

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
        /// A random instance of <typeparamref name="T"/> that is outside of the
        /// specified <paramref name="excludedRanges"/>.
        /// </returns>
        public abstract T GetRowValue(DataGeneratorContext context, WeightedRange<T>[] excludedRanges, T[] excludedObjects);

        /// <summary>
        /// Gets a unique identifier for the provided <typeparamref name="T"/> instance.
        /// </summary>
        /// <param name="value">An instance of <typeparamref name="T"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <paramref name="value"/>.
        /// </returns>
        public abstract string GetValueId(T value);

        /// <summary>
        /// Retrieves the list of arguments supported by the data provider.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the
        /// name of the argument as key and the argument type as value.
        /// </returns>
        /// <remarks>
        /// If not overriden, returns an empty dictionary.
        /// </remarks>
        public virtual Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>();
        }

        /// <summary>
        /// Generates a random instance of the object type supported by this provider
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="object"/>s that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="object"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// This method will return the result of <see cref="DataGeneratorProvider{T}.GetRangedRowValue(T, T, T[])"/>
        /// if the provided <see cref="object"/>s can be casted to <typeparamref name="T"/>.
        /// </remarks>
        public object GetRangedRowValue(object minValue, object maxValue, object[] excludedObjects)
        {
            return this.GetRangedRowValue((T)minValue, (T)maxValue, excludedObjects.Cast<T>().ToArray());
        }

        /// <summary>
        /// Generates a random instance of the object type supported by this provider
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="object"/>s that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="object"/>.
        /// </returns>
        /// <remarks>
        /// This method will return the result of <see cref="DataGeneratorProvider{T}.GetRowValue(DataGeneratorContext, T[])"/>
        /// if the provided <see cref="object"/> array can be casted to an array of <typeparamref name="T"/>.
        /// </remarks>
        public object GetRowValue(DataGeneratorContext context, object[] excludedObjects)
        {
            return this.GetRowValue(context, excludedObjects.Cast<T>().ToArray());
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
        /// An array of <see cref="WeightedRange"/>s defining the ranges
        /// that cannot be returned by the provider.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="object"/>s that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="object"/>.
        /// </returns>
        /// <remarks>
        /// This method will return the result of <see cref="DataGeneratorProvider{T}.GetRowValue(DataGeneratorContext, WeightedRange{T}[], T[])"/>
        /// if the provided <see cref="WeightedRange"/> array can be casted to an array of <see cref="WeightedRange{T}"/>.
        /// </remarks>
        public object GetRowValue(DataGeneratorContext context, WeightedRange[] excludedRanges, object[] excludedObjects)
        {
            return this.GetRowValue(
                context,
                excludedRanges.Select(u => new WeightedRange<T>() { MinValue = (T)u.MinValue, MaxValue = (T)u.MaxValue, Weight = u.Weight }).ToArray(),
                excludedObjects.Cast<T>().ToArray());
        }

        /// <summary>
        /// Gets a unique identifier for the provided object.
        /// </summary>
        /// <param name="value">An instance of the object type supported by this provider.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided instance.
        /// </returns>
        /// <remarks>
        /// This method will return the result of <see cref="DataGeneratorProvider{T}.GetValueId(T)"/>
        /// if the provided <see cref="object"/> can be casted to <typeparamref name="T"/>.
        /// </remarks>
        public string GetValueId(object value)
        {
            return this.GetValueId((T)value);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of <see cref="IDataGeneratorProperty"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// This method will return the result of <see cref="DataGeneratorProvider{T}.Load(DataGeneratorProperty{T}, int)"/>
        /// if the provided <see cref="IDataGeneratorProperty"/> can be casted to a <see cref="DataGeneratorProperty{T}"/>.
        /// </remarks>
        public void Load(IDataGeneratorProperty property, int rowCount)
        {
            this.Load(property as DataGeneratorProperty<T>, rowCount);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{T}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// Does nothing unless overriden.
        /// </remarks>
        public virtual void Load(DataGeneratorProperty<T> property, int rowCount)
        {
            // Intentionally left blank.
        }

        /// <summary>
        /// Initializes the pseudo-random number generator with the default
        /// time-dependent seed.
        /// </summary>
        public void InitializeRandomizer()
        {
            if (this.Randomizer != null)
            {
                throw new InvalidOperationException($"Cannot invoke InitializeRandomizer method more than once.");
            }

            this.Randomizer = new Random();

            this.OnInitializeRandomizer();
        }

        /// <summary>
        /// Initializes the pseudo-random number generator with the
        /// specified <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public void InitializeRandomizer(int seed)
        {
            if (this.Randomizer != null)
            {
                throw new InvalidOperationException($"Cannot invoke InitializeRandomizer method more than once.");
            }

            this.Randomizer = new Random(seed);

            this.OnInitializeRandomizer(seed);
        }

        /// <summary>
        /// Gets an instance of the object type supported by this provider based
        /// on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="object"/> with the specified unique identifier.
        /// </returns>
        object IDataGeneratorProvider.GetRowValue(string id)
        {
            return this.GetRowValue(id);
        }

        /// <summary>
        /// Method invoked after the Random instance has been initialized.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        protected virtual void OnInitializeRandomizer(int? seed = default)
        {
            // Intentionally left blank
        }
    }
}
