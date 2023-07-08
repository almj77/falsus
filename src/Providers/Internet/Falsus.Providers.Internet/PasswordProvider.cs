namespace Falsus.Providers.Internet
{
    using System;
    using Falsus.Extensions;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of passwords.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class PasswordProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The default length for the random passwords.
        /// </summary>
        private const int DefaultRandomStringLength = 6;

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private PasswordProviderConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordProvider"/> class.
        /// </summary>
        public PasswordProvider()
            : base()
        {
            this.configuration = new PasswordProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public PasswordProvider(PasswordProviderConfiguration configuration)
            : this()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random password that is greater than the
        /// value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
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
        /// Thrown because the generation of ranged street names is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(PasswordProvider)} does not supported ranged row values.");
        }

        /// <summary>
        /// Generates a random password
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
        /// Thrown because the generation of ranged passwords is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(PasswordProvider)} does not support ranged row values.");
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
        /// Generates a random password based on the context and excluded objects.
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
        /// A random password.
        /// </returns>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            if (this.configuration.Length.HasValue)
            {
                return this.Randomizer.NextString(this.configuration.Length.Value, this.configuration.IncludeSpecialChars);
            }

            if (this.configuration.MinLength.HasValue && this.configuration.MaxLength.HasValue)
            {
                int length = this.Randomizer.Next(this.configuration.MinLength.Value, this.configuration.MaxLength.Value);
                return this.Randomizer.NextString(length, this.configuration.IncludeSpecialChars);
            }

            return this.Randomizer.NextString(DefaultRandomStringLength, this.configuration.IncludeSpecialChars);
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
        /// provided <see cref="PasswordProviderConfiguration"/>.
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
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item>The <see cref="configuration"/> instance is null</item>
        /// <item>
        /// The <see cref="PasswordProviderConfiguration.Length"/> or <see cref="PasswordProviderConfiguration.MinLength"/>
        /// or <see cref="PasswordProviderConfiguration.MaxLength"/> have values lower than 0.
        /// </item>
        /// <item>
        /// The value of <see cref="PasswordProviderConfiguration.MinLength"/> is greater than the
        /// value of <see cref="PasswordProviderConfiguration.MaxLength"/>.
        /// </item>
        /// </list>
        /// </exception>
        private void ValidateConfiguration()
        {
            if (this.configuration.Length.HasValue && this.configuration.Length.Value <= 0)
            {
                throw new InvalidOperationException($"{nameof(PasswordProvider)} length cannot be less than zero.");
            }

            if (this.configuration.MinLength.HasValue && this.configuration.MinLength.Value <= 0)
            {
                throw new InvalidOperationException($"{nameof(PasswordProvider)} min. length cannot be less than zero.");
            }

            if (this.configuration.MaxLength.HasValue && this.configuration.MaxLength.Value <= 0)
            {
                throw new InvalidOperationException($"{nameof(PasswordProvider)} max. length cannot be less than zero.");
            }

            if (!this.configuration.Length.HasValue && (!this.configuration.MinLength.HasValue || !this.configuration.MaxLength.HasValue))
            {
                throw new InvalidOperationException($"{nameof(PasswordProvider)} min. and max. length are required.");
            }

            if (this.configuration.MinLength.HasValue && this.configuration.MaxLength.HasValue && this.configuration.MinLength >= this.configuration.MaxLength)
            {
                throw new InvalidOperationException($"{nameof(PasswordProvider)} min. length must be greater than max. length.");
            }
        }
    }
}
