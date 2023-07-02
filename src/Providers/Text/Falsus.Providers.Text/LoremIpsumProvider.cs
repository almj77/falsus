namespace Falsus.Providers.Text
{
    using System;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using NLipsum.Core;

    /// <summary>
    /// Represents a provider of LoremIpsum text fragments.
    /// </summary>
    /// <remarks>
    /// This provider is a wrapper of the NLipsum.Core library.
    /// </remarks>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class LoremIpsumProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private LoremIpsumProviderConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoremIpsumProvider"/> class.
        /// </summary>
        public LoremIpsumProvider()
            : base()
        {
            this.configuration = new LoremIpsumProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoremIpsumProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="configuration"/> instance is null.
        /// </exception>
        public LoremIpsumProvider(LoremIpsumProviderConfiguration configuration)
            : this()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random text fragment that is greater than the value
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
        /// Thrown because the generation of ranged text fragments is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LoremIpsumProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random text fragment
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
        /// Thrown because the generation of ranged text fragments is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(LoremIpsumProvider)} does not support ranged row values.");
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
        /// Generates a random text fragment based on the context and excluded objects.
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
        /// A random text fragment.
        /// </returns>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string value = this.GetValue();
            int attempts = 0;

            while (excludedObjects.Contains(value))
            {
                value = this.GetValue();
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(LoremIpsumProvider)} cannot generate another unique value.");
                }

                attempts++;
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

        /// <summary>
        /// Instructs the data provider to prepare the data for generation based on the
        /// provided <see cref="LoremIpsumProviderConfiguration"/>.
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
        public override void Load(DataGeneratorProperty<string> property, int rowCount)
        {
            base.Load(property, rowCount);

            this.ValidateConfiguration();
        }

        /// <summary>
        /// Gets a value from the <see cref="LipsumGenerator"/> based on the specified fragment type.
        /// </summary>
        /// <returns>
        /// A text fragment.
        /// </returns>
        private string GetValue()
        {
            LipsumGenerator generator = new LipsumGenerator();

            switch (this.configuration.FragmentType)
            {
                case LoremIpsumFragmentType.Paragraph:
                    return string.Join(Environment.NewLine, generator.GenerateParagraphs(this.configuration.FragmentCount));
                case LoremIpsumFragmentType.Sentence:
                    return string.Join(" ", generator.GenerateSentences(this.configuration.FragmentCount));
                case LoremIpsumFragmentType.Word:
                    return string.Join(" ", generator.GenerateWords(this.configuration.FragmentCount));
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="LoremIpsumProviderConfiguration.FragmentCount"/> is less than zero.
        /// </exception>
        private void ValidateConfiguration()
        {
            if (this.configuration.FragmentCount <= 0)
            {
                throw new InvalidOperationException($"{nameof(LoremIpsumProvider)} fragment count cannot be zero or less.");
            }
        }
    }
}
