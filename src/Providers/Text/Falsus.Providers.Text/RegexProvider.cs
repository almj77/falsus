namespace Falsus.Providers.Text
{
    using System;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Fare;

    /// <summary>
    /// Represents a provider of <see cref="string"/> values that match
    /// a given regular expression.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class RegexProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private RegexProviderConfiguration configuration;

        /// <summary>
        /// An object that will generate text from a regular expression.
        /// </summary>
        private Xeger regexGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <remarks>
        /// The <paramref name="configuration"/> must not be null and the
        /// <see cref="RegexProviderConfiguration.Pattern"/> cannot be empty.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="configuration"/> instance is null.
        /// </exception>
        public RegexProvider(RegexProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random <see cref="string"/> value that is greater than the value
        /// of <paramref name="minValue"/> and lower than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random <see cref="string"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="string"/>
        /// values is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(RegexProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="string"/> value
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
        /// A random <see cref="string"/> value that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="string"/>
        /// values is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(RegexProvider)} does not support ranged row values.");
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
        /// Generates a random <see cref="string"/> value that matches the configured regular expression
        /// based on the context and excluded objects.
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
        /// A random <see cref="string"/> value.
        /// </returns>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string value = this.regexGenerator.Generate();

            int attempts = 0;
            while (excludedObjects.Contains(value))
            {
                value = this.regexGenerator.Generate();
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(RegexProvider)} cannot generate another unique value.");
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
        /// provided <see cref="RegexProviderConfiguration"/>.
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

            this.regexGenerator = new Xeger(this.configuration.Pattern, this.Randomizer);
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="RegexProviderConfiguration.Pattern"/> is empty.
        /// </exception>
        private void ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(this.configuration.Pattern))
            {
                throw new InvalidOperationException($"{nameof(RegexProvider)} pattern cannot be null.");
            }
        }
    }
}
