namespace Falsus.Providers.Sys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Providers.Text;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="string"/> values representing file names.
    /// </summary>
    /// <seealso cref="GenericArrayProvider{T}"/>
    public class FileNameProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// The name of the input argument that contains the
        /// <see cref="string"/> to be used as the file name.
        /// </summary>
        public const string FileNameArgumentName = "filename";

        /// <summary>
        /// The name of the input argument that contains the
        /// <see cref="string"/> to be used as the extension.
        /// </summary>
        public const string ExtensionArgumentName = "extension";

        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private FileNameProviderConfiguration configuration;

        /// <summary>
        /// The provider of <see cref="FileTypeModel"/> instances.
        /// </summary>
        private FileTypeProvider fileTypeProvider;

        /// <summary>
        /// The provider of <see cref="string"/> representing words.
        /// </summary>
        private WordProvider wordProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNameProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <remarks>
        /// The <paramref name="configuration"/> must not be null and the
        /// <see cref="FileNameProviderConfiguration.ConnectionString"/> cannot be empty.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="configuration"/> instance is null.
        /// </exception>
        public FileNameProvider(FileNameProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;

            this.fileTypeProvider = new FileTypeProvider();
            WordProviderConfiguration wordProviderConfiguration = new WordProviderConfiguration()
            {
                ExcludedWordTypes = new WordType[1]
                {
                    WordType.Interjection
                }
            };

            this.wordProvider = new WordProvider(wordProviderConfiguration);
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
                { FileNameArgumentName, typeof(string) },
                { ExtensionArgumentName, typeof(FileTypeModel) }
            };
        }

        /// <summary>
        /// Generates a random file name
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random file name greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged file names is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FileNameProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random file name
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
        /// Thrown because the generation of ranged file names is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FileNameProvider)} does not support ranged row values.");
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
        /// Generates a random <see cref="string"/> instance based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="string"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random city name.
        /// </returns>
        /// <remarks>
        /// If the <see cref="string"/> is provided then this method will
        /// return the name of the file name belonging to the specified <see cref="string"/>
        /// argument value.
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            string[] models = Array.Empty<string>();
            string value = null;
            string fileName = null;
            string extension = null;

            if (context.HasArgumentValue(FileNameArgumentName))
            {
                fileName = context.GetArgumentValue<string>(FileNameArgumentName);

                if (string.IsNullOrEmpty(fileName))
                {
                    throw new InvalidOperationException($"{nameof(FileNameProvider)} cannot generate value for null file name.");
                }
            }

            if (context.HasArgumentValue(ExtensionArgumentName))
            {
                extension = context.GetArgumentValue<string>(ExtensionArgumentName);

                if (string.IsNullOrEmpty(extension))
                {
                    throw new InvalidOperationException($"{nameof(FileNameProvider)} cannot generate value for null extension.");
                }
            }

            int attempts = 0;

            while ((value == null || excludedObjects.Contains(value)) && attempts <= MaxAttempts)
            {
                if (fileName == null && extension == null)
                {
                    int wordTokenCount = this.Randomizer.Next(this.configuration.MinWordCount, this.configuration.MaxWordCount);
                    int extensionTokenCount = this.Randomizer.Next(this.configuration.MinExtensionCount, this.configuration.MaxExtensionCount);

                    string[] words = new string[wordTokenCount];
                    string[] extensions = new string[extensionTokenCount];

                    for (int i = 0; i < wordTokenCount; i++)
                    {
                        words[i] = this.wordProvider.GetRowValue(context, Array.Empty<string>());
                    }

                    for (int i = 0; i < extensionTokenCount; i++)
                    {
                        extensions[i] = this.fileTypeProvider.GetRowValue(context, new FileTypeModel[1] { default }).Extension;
                    }

                    fileName = string.Join(" ", words);
                    extension = string.Join(".", extensions);
                }
                else if (fileName == null)
                {
                    int wordTokenCount = this.Randomizer.Next(this.configuration.MinWordCount, this.configuration.MaxWordCount);
                    string[] words = new string[wordTokenCount];

                    List<string>[] excludedWords = new List<string>[wordTokenCount];

                    for (int i = 0; i < wordTokenCount; i++)
                    {
                        excludedWords[i] = new List<string>();
                    }

                    for (int i = 0; i < excludedObjects.Length; i++)
                    {
                        string[] fileNameTokens = excludedObjects[i].Split('_');

                        if (fileNameTokens.Length == wordTokenCount)
                        {
                            for (int x = 0; x < wordTokenCount; x++)
                            {
                                excludedWords[x].Add(fileNameTokens[x]);
                            }
                        }
                    }

                    for (int i = 0; i < wordTokenCount; i++)
                    {
                        words[i] = this.wordProvider.GetRowValue(context, excludedWords[i].ToArray());
                    }

                    fileName = string.Join(" ", words);
                }
                else if (extension == null)
                {
                    int extensionTokenCount = this.Randomizer.Next(this.configuration.MinExtensionCount, this.configuration.MaxExtensionCount);
                    string[] extensions = new string[extensionTokenCount];

                    for (int i = 0; i < extensionTokenCount; i++)
                    {
                        extensions[i] = this.fileTypeProvider.GetRowValue(context, new FileTypeModel[1] { default }).Extension;
                    }

                    extension = string.Join(".", extensions);
                }

                string escapedFileName = fileName.Replace("\r", "_").Replace("\n", "_").Replace(" ", "_");
                string escapedExtension = extension.Replace("\r", "_").Replace("\n", "_").Replace(" ", "_");

                value = string.Concat(escapedFileName, ".", escapedExtension);

                attempts++;
            }

            if (excludedObjects.Any(u => u == value))
            {
                throw new InvalidOperationException($"{nameof(FileNameProvider)} cannot generate another unique value.");
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
        /// /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(string value)
        {
            return value;
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation based on the
        /// provided <see cref="FileNameProviderConfiguration"/>.
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

            this.wordProvider.Load(property, rowCount);
            this.fileTypeProvider.Load(property, rowCount);
        }

        /// <summary>
        /// Method invoked after the Random instance has been initialized.
        /// </summary>
        /// <param name="seed">
        /// A number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        protected override void OnInitializeRandomizer(int? seed = null)
        {
            base.OnInitializeRandomizer(seed);

            if (seed.HasValue)
            {
                this.wordProvider.InitializeRandomizer(seed.Value);
                this.fileTypeProvider.InitializeRandomizer(seed.Value);
            }
            else
            {
                this.wordProvider.InitializeRandomizer();
                this.fileTypeProvider.InitializeRandomizer();
            }
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="FileNameProviderConfiguration.MinWordCount"/> is greater than the <see cref="FileNameProviderConfiguration.MaxWordCount"/>
        /// or when the <see cref="FileNameProviderConfiguration.MinExtensionCount"/> is greater than the <see cref="FileNameProviderConfiguration.MaxExtensionCount"/>
        /// or when the <see cref="FileNameProviderConfiguration.MinWordCount"/> is less than or equal to zero.
        /// </exception>
        private void ValidateConfiguration()
        {
            if (this.configuration.MaxWordCount < this.configuration.MinWordCount)
            {
                throw new InvalidOperationException($"{nameof(FileNameProvider)} cannot have {nameof(this.configuration.MaxWordCount)} lower than the {nameof(this.configuration.MinWordCount)}.");
            }

            if (this.configuration.MaxExtensionCount < this.configuration.MinExtensionCount)
            {
                throw new InvalidOperationException($"{nameof(FileNameProvider)} cannot have {nameof(this.configuration.MaxExtensionCount)} lower than the {nameof(this.configuration.MinExtensionCount)}.");
            }

            if (this.configuration.MinWordCount <= 0)
            {
                throw new InvalidOperationException($"{nameof(FileNameProvider)} - {nameof(this.configuration.MinWordCount)} must be greater than 0.");
            }

            if (this.configuration.MinExtensionCount <= 0)
            {
                throw new InvalidOperationException($"{nameof(FileNameProvider)} - {nameof(this.configuration.MinExtensionCount)} must be greater than 0.");
            }
        }
    }
}
