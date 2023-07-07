namespace Falsus.Providers.Internet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of MIME types.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class MimeTypeProvider : DataGeneratorProvider<MimeTypeModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="FileTypeModel"/> to use on the value generation process.
        /// </summary>
        public const string FileExtensionArgumentName = "fileextension";

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private readonly MimeTypeProviderConfiguration configuration;

        /// <summary>
        /// A dictionary of <see cref="MimeTypeModel"/> instances indexed
        /// by a <see cref="Tuple{T1, T2}"/> containing the <see cref="MimeTypeModel.MimeType"/> and
        /// <see cref="MimeTypeModel.Extension"/> property.
        /// </summary>
        private Dictionary<Tuple<string, string>, MimeTypeModel> mimeTypeByMimeTypeAndExtension;

        /// <summary>
        /// A dictionary of <see cref="MimeTypeModel"/> instances indexed
        /// by a <see cref="Tuple{T1, T2}"/> containing the <see cref="MimeTypeModel.MimeType"/> and
        /// <see cref="MimeTypeModel.Category"/> property.
        /// </summary>
        private Dictionary<Tuple<string, string>, MimeTypeModel[]> mimeTypesByCategoryAndExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeTypeProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <remarks>
        /// The <paramref name="configuration"/> must not be null and the
        /// <see cref="MimeTypeProviderConfiguration.ConnectionString"/> cannot be empty.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="configuration"/> instance is null.
        /// </exception>
        public MimeTypeProvider(MimeTypeProviderConfiguration configuration)
            : base()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
            this.mimeTypeByMimeTypeAndExtension = new Dictionary<Tuple<string, string>, MimeTypeModel>();
            this.mimeTypesByCategoryAndExtension = new Dictionary<Tuple<string, string>, MimeTypeModel[]>();
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
                { FileExtensionArgumentName, typeof(FileTypeModel) }
            };
        }

        /// <summary>
        /// Generates a random <see cref="MimeTypeModel"/> instance.
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="MimeTypeModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="MimeTypeModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged MIME Types is not supported.
        /// </exception>
        public override MimeTypeModel GetRangedRowValue(MimeTypeModel minValue, MimeTypeModel maxValue, MimeTypeModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(MimeTypeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="MimeTypeModel"/> instance
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
        /// An array of <see cref="MimeTypeModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="MimeTypeModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged MIME Types is not supported.
        /// </exception>
        public override MimeTypeModel GetRowValue(DataGeneratorContext context, WeightedRange<MimeTypeModel>[] excludedRanges, MimeTypeModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(MimeTypeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="MimeTypeModel"/> instance based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="MimeTypeModel"/> instance that represents the MIME type with the specified unique identifier.
        /// </returns>
        public override MimeTypeModel GetRowValue(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            string[] tokens = id.Split('|');
            if (tokens.Length == 2)
            {
                Tuple<string, string> key = new Tuple<string, string>(tokens[0], tokens[1]);
                if (this.mimeTypeByMimeTypeAndExtension.ContainsKey(key))
                {
                    return this.mimeTypeByMimeTypeAndExtension[key];
                }
            }

            return null;
        }

        /// <summary>
        /// Generates a random <see cref="MimeTypeModel"/> instance based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="MimeTypeModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random MIME type.
        /// </returns>
        /// <remarks>
        /// If the <see cref="FileExtensionArgumentName"/> is provided then this method will
        /// return the name of the MIME type belonging to the specified <see cref="FileTypeModel"/>
        /// argument value.
        /// </remarks>
        public override MimeTypeModel GetRowValue(DataGeneratorContext context, MimeTypeModel[] excludedObjects)
        {
            MimeTypeModel[] models = Array.Empty<MimeTypeModel>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            MimeTypeModel value = null;

            if (context.HasArgumentValue(FileExtensionArgumentName))
            {
                FileTypeModel fileTypeModel = context.GetArgumentValue<FileTypeModel>(FileExtensionArgumentName);

                if (fileTypeModel == null)
                {
                    throw new InvalidOperationException($"{nameof(MimeTypeProvider)} cannot generate another unique value for null file extension.");
                }

                if (fileTypeModel.Extension != null)
                {
                    Tuple<string, string> key = new Tuple<string, string>(fileTypeModel.Category, fileTypeModel.Extension);

                    if (this.mimeTypesByCategoryAndExtension.ContainsKey(key))
                    {
                        models = this.mimeTypesByCategoryAndExtension[key];
                        if (excludedObjects.Any())
                        {
                            models = models.Except(excludedObjects).ToArray();
                        }

                        if (!models.Any())
                        {
                            throw new InvalidOperationException($"{nameof(MimeTypeProvider)} cannot generate another unique value.");
                        }

                        minRandomValue = 0;
                        maxRandomValue = models.Length;

                        value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                        while (excludedObjects.Contains(value))
                        {
                            value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
                        }
                    }

                    return value;
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(MimeTypeProvider)} cannot generate another unique value for {fileTypeModel.Category} and {fileTypeModel.Extension}.");
                }
            }

            models = this.mimeTypesByCategoryAndExtension.SelectMany(u => u.Value).ToArray();
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(MimeTypeProvider)} cannot generate another unique value.");
            }

            minRandomValue = 0;
            maxRandomValue = models.Length;

            value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            while (excludedObjects.Contains(value))
            {
                value = models[this.Randomizer.Next(minRandomValue, maxRandomValue)];
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="MimeTypeModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="MimeTypeModel"/> instance.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        public override string GetValueId(MimeTypeModel value)
        {
            return string.Concat(value.MimeType, "|", value.Extension);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation based on the
        /// provided <see cref="MimeTypeProviderConfiguration"/>.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{MimeTypeModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<MimeTypeModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            List<MimeTypeModel> models = ResourceReader.ReadContentsFromFile<List<MimeTypeModel>>("Falsus.Providers.Internet.Datasets.MimeTypes.json");

            if (!this.configuration.IncludeDeprecated)
            {
                models = models.Where(u => string.IsNullOrEmpty(u.DeprecatedBy)).ToList();
            }

            if (this.configuration.ExcludeUncommon)
            {
                models = models.Where(u => u.IsCommon.HasValue && u.IsCommon.Value).ToList();
            }

            if (this.configuration.TreatAliasesAsUnique)
            {
                List<MimeTypeModel> newModels = new List<MimeTypeModel>();

                foreach (MimeTypeModel model in models.Where(u => u.Aliases != null && u.Aliases.Any()))
                {
                    foreach (string alias in model.Aliases)
                    {
                        if (!models.Any(u => u.MimeType == alias) && !newModels.Any(u => u.MimeType == alias))
                        {
                            newModels.Add(new MimeTypeModel()
                            {
                                Aliases = model.Aliases,
                                Category = model.Category,
                                DeprecatedBy = model.DeprecatedBy,
                                Extension = model.Extension,
                                MimeType = alias,
                                Name = model.Name,
                                IsCommon = model.IsCommon
                            });
                        }
                    }
                }

                models.AddRange(newModels);
            }

            this.mimeTypesByCategoryAndExtension = models
                .GroupBy(x => new { x.Category, x.Extension })
                .ToDictionary(k => new Tuple<string, string>(k.Key.Category, k.Key.Extension), v => v.ToArray());

            this.mimeTypeByMimeTypeAndExtension = models.ToDictionary(k => new Tuple<string, string>(k.MimeType, k.Extension), v => v);
        }
    }
}
