namespace Falsus.Providers.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Text.Models;
    using Falsus.Shared.Helpers;

    /// <summary>
    /// Represents a provider of <see cref="string"/> values representing words.
    /// </summary>
    /// <seealso cref="GenericArrayProvider{T}"/>
    public class WordProvider : GenericArrayProvider<string>
    {
        /// <summary>
        /// An array containing all words.
        /// </summary>
        /// <remarks>
        /// Words belonging to types present on the <see cref="WordProviderConfiguration.ExcludedWordTypes"/>
        /// are excluded from this list.
        /// </remarks>
        private static string[] words;

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private readonly WordProviderConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="configuration"/> instance is null.
        /// </exception>
        public WordProvider(WordProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
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
        /// provided <see cref="WordProvider"/>.
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

            Dictionary<WordType, WordModel[]> wordsByType = ResourceReader.ReadContentsFromFile<Dictionary<WordType, WordModel[]>>("Falsus.Providers.Text.Datasets.Words.json");
            List<WordModel> models = new List<WordModel>();

            foreach (KeyValuePair<WordType, WordModel[]> item in wordsByType)
            {
                if (this.configuration.ExcludedWordTypes == null || !this.configuration.ExcludedWordTypes.Contains(item.Key))
                {
                    foreach (WordModel model in item.Value)
                    {
                        model.WordType = item.Key;
                        models.Add(model);
                    }
                }
            }

            words = models.Select(u => u.Word).ToArray();
        }

        /// <summary>
        /// Gets an array of <see cref="string"/> instances
        /// representing possible values based on the context information.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of words.
        /// </returns>
        protected override string[] GetValues(DataGeneratorContext context)
        {
            return this.GetValues();
        }

        /// <summary>
        /// Gets an array of <see cref="string"/> instances
        /// representing all possible values.
        /// </summary>
        /// <returns>
        /// An array of <see cref="string"/> instances.
        /// </returns>
        protected override string[] GetValues()
        {
            return words;
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="WordProviderConfiguration.ExcludedWordTypes"/> has duplicate values.
        /// </exception>
        private void ValidateConfiguration()
        {
            if (this.configuration.ExcludedWordTypes != null && this.configuration.ExcludedWordTypes.Any())
            {
                bool hasDuplicates = this.configuration.ExcludedWordTypes
                    .GroupBy(u => u)
                    .Select(u => new { u.Key, Count = u.Count() })
                    .Any(u => u.Count > 1);

                if (hasDuplicates)
                {
                    throw new InvalidOperationException($"{nameof(WordProvider)} cannot be configured with duplicate excluded types.");
                }
            }
        }
    }
}
