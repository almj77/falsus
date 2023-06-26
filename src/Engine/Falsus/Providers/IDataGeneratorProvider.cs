namespace Falsus.Providers
{
    using System;
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Defines the interface for a data provider.
    /// </summary>
    public interface IDataGeneratorProvider
    {
        /// <summary>
        /// Retrieves the list of arguments supported by the data provider.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the
        /// name of the argument as key and the argument type as value.
        /// </returns>
        Dictionary<string, Type> GetSupportedArguments();

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
        /// </remarks>
        void Load(IDataGeneratorProperty property, int rowCount);

        /// <summary>
        /// Gets an instance of the object type supported by this provider based
        /// on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="object"/> with the specified unique identifier.
        /// </returns>
        object GetRowValue(string id);

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
        object GetRowValue(DataGeneratorContext context, object[] excludedObjects);

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
        /// A random instance of <see cref="object"/> that is outside of
        /// the provided <paramref name="excludedRanges"/>.
        /// </returns>
        object GetRowValue(DataGeneratorContext context, WeightedRange[] excludedRanges, object[] excludedObjects);

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
        /// </remarks>
        object GetRangedRowValue(object minValue, object maxValue, object[] excludedObjects);

        /// <summary>
        /// Gets a unique identifier for the provided object.
        /// </summary>
        /// <param name="value">An instance of the object type supported by this provider.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided instance.
        /// </returns>
        string GetValueId(object value);

        /// <summary>
        /// Initializes the pseudo-random number generator with the default
        /// time-dependent seed.
        /// </summary>
        void InitializeRandomizer();

        /// <summary>
        /// Initializes the pseudo-random number generator with the
        /// specified <paramref name="seed"/>.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        void InitializeRandomizer(int seed);
    }

    /// <summary>
    /// Defines the interface for typed data provider.
    /// </summary>
    /// <seealso cref="IDataGeneratorProvider"/>
    /// <typeparam name="T">The type of the value that is generated by this provider.</typeparam>
    public interface IDataGeneratorProvider<T> : IDataGeneratorProvider
    {
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
        /// </remarks>
        void Load(DataGeneratorProperty<T> property, int rowCount);

        /// <summary>
        /// Gets an instance of the object type supported by this provider based on
        /// the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <typeparamref name="T"/> with the specified unique identifier.
        /// </returns>
        new T GetRowValue(string id);

        /// <summary>
        /// Generates a random instance of the object type supported by this provider
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <typeparamref name="T"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <typeparamref name="T"/>.
        /// </returns>
        T GetRowValue(DataGeneratorContext context, T[] excludedObjects);

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
        /// A random instance of <typeparamref name="T"/> that is outside of
        /// the provided <paramref name="excludedRanges"/>.
        /// </returns>
        T GetRowValue(DataGeneratorContext context, WeightedRange<T>[] excludedRanges, T[] excludedObjects);

        /// <summary>
        /// Generates a random instance of the object type supported by this provider
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <typeparamref name="T"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <typeparamref name="T"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        T GetRangedRowValue(T minValue, T maxValue, T[] excludedObjects);

        /// <summary>
        /// Gets a unique identifier for the provided object.
        /// </summary>
        /// <param name="value">An instance of the object type supported by this provider.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided instance.
        /// </returns>
        string GetValueId(T value);
    }
}
