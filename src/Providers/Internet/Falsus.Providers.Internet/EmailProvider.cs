namespace Falsus.Providers.Internet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Falsus.Extensions;
    using Falsus.GeneratorProperties;

    /// <summary>
    /// Represents a provider of email addresses.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class EmailProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The name of the input argument that contains the
        /// text tokens to use on the value generation process.
        /// </summary>
        public const string TextArgumentsName = "text";

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private EmailProviderConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <remarks>
        /// The <paramref name="configuration"/> must not be null and the
        /// <see cref="EmailProviderConfiguration.Domains"/> must have
        /// at least one entry.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="configuration"/> instance is null.
        /// </exception>
        public EmailProvider(EmailProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the definition of the input arguments supported by this provider.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the name of the
        /// argument and the expected type of the argument value.
        /// </returns>
        public override Dictionary<string, Type> GetSupportedArguments()
        {
            return new Dictionary<string, Type>()
            {
                { TextArgumentsName, typeof(string) }
            };
        }

        /// <summary>
        /// Generates a random email address that is greater than
        /// the value of <paramref name="minValue"/> and lower
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
            throw new NotSupportedException($"{nameof(EmailProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random email address
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
        /// Thrown because the generation of ranged email addresses is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(EmailProvider)} does not support ranged row values.");
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
        /// Generates a random street name based on the context and excluded objects.
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
        /// A random email.
        /// </returns>
        /// <remarks>
        /// The generated email address will result from the concatenation of
        /// the <see cref="TextArgumentsName"/> argument values (seperated by a dot)
        /// along with a randomly chosen domain from the <see cref="EmailProviderConfiguration.Domains"/> list.
        /// If nothing is provided as the <see cref="TextArgumentsName"/> value, a random string will be
        /// generated.
        /// If the <paramref name="excludedObjects"/> contains the value that was generated
        /// then a number will be appended at end (eg. if john.doe@domain.com is present then
        /// it will become john.doe2@domain.com).
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string value = string.Empty;
            string domain = string.Empty;

            if (context.HasArgumentValue(TextArgumentsName))
            {
                string[] texts = context.GetArgumentValues(TextArgumentsName).Select(u => u.ToString()).ToArray();
                value = string.Join(".", texts).ToLowerInvariant();
                value = System.Text.RegularExpressions.Regex.Replace(value, @"[^\w]+", ".");
                if (value.EndsWith("."))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                // Convert josé to jose
                value = this.RemoveAccents(value);
            }

            if (string.IsNullOrEmpty(value))
            {
                value = this.Randomizer.NextString(6, false);
            }

            domain = this.GetDomain().ToLower();

            int equalCount = excludedObjects.Count(u => u == string.Concat(value, "@", domain));

            if (equalCount > 0)
            {
                return string.Concat(value, equalCount + 1, "@", domain);
            }

            return string.Concat(value, "@", domain);
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
        /// provided <see cref="EmailProviderConfiguration"/>.
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
        /// Gets a random domain from the <see cref="EmailProviderConfiguration.Domains"/>
        /// list.
        /// </summary>
        /// <returns>
        /// A string representing the domain.
        /// </returns>
        /// <remarks>
        /// If <see cref="EmailProviderConfiguration.Domains"/> has only one item
        /// then this method will always return the same value.
        /// </remarks>
        private string GetDomain()
        {
            if (this.configuration.Domains.Length == 1)
            {
                return this.configuration.Domains.First();
            }
            else
            {
                return this.configuration.Domains[this.Randomizer.Next(0, this.configuration.Domains.Length)];
            }
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="EmailProviderConfiguration.Domains"/> doesn't have at
        /// least one entry.
        /// </exception>
        private void ValidateConfiguration()
        {
            if (this.configuration.Domains == null || !this.configuration.Domains.Any())
            {
                throw new InvalidOperationException($"{nameof(EmailProvider)} requires at least one domain.");
            }
        }

        /// <summary>
        /// Removes the accents from the input string.
        /// </summary>
        /// <param name="input">The string to normalize.</param>
        /// <returns>
        /// The normalized string.
        /// </returns>
        /// <remarks>
        /// When provided with 'José' this method will return 'Jose'.
        /// </remarks>
        private string RemoveAccents(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(
                Encoding.ASCII.CodePage,
                new EncoderReplacementFallback(string.Empty),
                new DecoderReplacementFallback(string.Empty));
            byte[] bytes = removal.GetBytes(normalized);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
