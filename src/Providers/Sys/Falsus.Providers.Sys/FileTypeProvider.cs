namespace Falsus.Providers.Sys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Helpers;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of <see cref="FileTypeModel"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class FileTypeProvider : DataGeneratorProvider<FileTypeModel>
    {
        /// <summary>
        /// The name of the input argument that contains an instance of
        /// <see cref="MimeTypeModel"/> to use on the value generation process.
        /// </summary>
        public const string MimeTypeArgumentName = "mimetype";

        /// <summary>
        /// A dictionary of <see cref="FileTypeModel"/> instances indexed
        /// by a <see cref="Tuple{T1, T2}"/> containing the <see cref="FileTypeModel.Category"/> and
        /// <see cref="FileTypeModel.Extension"/> property.
        /// </summary>
        private static Dictionary<Tuple<string, string>, FileTypeModel> fileTypeByCategoryAndExtension;

        /// <summary>
        /// A dictionary of <see cref="FileTypeModel"/> instances indexed
        /// by the <see cref="MimeTypeModel.MimeType"/> property.
        /// </summary>
        private static Dictionary<string, FileTypeModel[]> fileTypeByMimeType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTypeProvider"/> class.
        /// </summary>
        public FileTypeProvider()
            : base()
        {
            fileTypeByCategoryAndExtension = new Dictionary<Tuple<string, string>, FileTypeModel>();
            fileTypeByMimeType = new Dictionary<string, FileTypeModel[]>();
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
                { MimeTypeArgumentName, typeof(MimeTypeModel) }
            };
        }

        /// <summary>
        /// Generates a random file type
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="FileTypeModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random file type greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged file types is not supported.
        /// </exception>
        public override FileTypeModel GetRangedRowValue(FileTypeModel minValue, FileTypeModel maxValue, FileTypeModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FileTypeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random file type
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
        /// An array of <see cref="FileTypeModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="FileTypeModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged file types is not supported.
        /// </exception>
        public override FileTypeModel GetRowValue(DataGeneratorContext context, WeightedRange<FileTypeModel>[] excludedRanges, FileTypeModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(FileTypeProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Gets a <see cref="string"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="string"/> value with the specified unique identifier.
        /// </returns>
        public override FileTypeModel GetRowValue(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            string[] tokens = id.Split('|');
            if (tokens.Length == 2)
            {
                Tuple<string, string> key = new Tuple<string, string>(tokens[0], tokens[1]);
                if (fileTypeByCategoryAndExtension.ContainsKey(key))
                {
                    return fileTypeByCategoryAndExtension[key];
                }
            }

            return null;
        }

        /// <summary>
        /// Generates a random <see cref="FileTypeModel"/> instance based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="FileTypeModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random city name.
        /// </returns>
        /// <remarks>
        /// If the <see cref="FileTypeModel"/> is provided then this method will
        /// return the name of the file type belonging to the specified <see cref="FileTypeModel"/>
        /// argument value.
        /// </remarks>
        public override FileTypeModel GetRowValue(DataGeneratorContext context, FileTypeModel[] excludedObjects)
        {
            FileTypeModel[] models = Array.Empty<FileTypeModel>();
            int minRandomValue = 0;
            int maxRandomValue = 0;
            FileTypeModel value = null;

            if (context.HasArgumentValue(MimeTypeArgumentName))
            {
                MimeTypeModel mimeTypeModel = context.GetArgumentValue<MimeTypeModel>(MimeTypeArgumentName);

                if (mimeTypeModel == null)
                {
                    throw new InvalidOperationException($"{nameof(FileTypeProvider)} cannot generate another unique value for null MIME type.");
                }

                if (!string.IsNullOrEmpty(mimeTypeModel.Extension) && fileTypeByMimeType.ContainsKey(mimeTypeModel.MimeType))
                {
                    models = fileTypeByMimeType[mimeTypeModel.MimeType];
                    if (excludedObjects.Any())
                    {
                        models = models.Except(excludedObjects).ToArray();
                    }

                    if (!models.Any())
                    {
                        throw new InvalidOperationException($"{nameof(FileTypeProvider)} cannot generate another unique value.");
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
                else
                {
                    throw new InvalidOperationException($"{nameof(FileTypeProvider)} cannot generate another unique value for {mimeTypeModel.MimeType}.");
                }
            }

            models = fileTypeByCategoryAndExtension.Values.ToArray();
            if (excludedObjects.Any())
            {
                models = models.Except(excludedObjects).ToArray();
            }

            if (!models.Any())
            {
                throw new InvalidOperationException($"{nameof(FileTypeProvider)} cannot generate another unique value.");
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
        /// Gets a unique identifier for the provided <see cref="FileTypeModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="FileTypeModel"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        public override string GetValueId(FileTypeModel value)
        {
            if (value == null)
            {
                return null;
            }

            return string.Concat(value.Category, "|", value.Extension);
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation>.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{FileTypeModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<FileTypeModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            Dictionary<string, List<FileTypeModel>> modelsByMimeType = new Dictionary<string, List<FileTypeModel>>();
            FileTypeModel[] models = ResourceReader.ReadContentsFromFile<FileTypeModel[]>("Falsus.Providers.Sys.Datasets.FileTypes.json");
            MimeTypeModel[] mimetypeModels = ResourceReader.ReadContentsFromFile<MimeTypeModel[]>("Falsus.Providers.Sys.Datasets.MimeTypes.json");

            Dictionary<Tuple<string, string>, MimeTypeModel[]> mimeTypesByCategoryExtension = mimetypeModels
                .GroupBy(u => new Tuple<string, string>(u.Category, u.Extension))
                .ToDictionary(u => u.Key, u => u.ToArray());

            foreach (FileTypeModel model in models)
            {
                Tuple<string, string> key = new Tuple<string, string>(model.Category, model.Extension);
                if (mimeTypesByCategoryExtension.ContainsKey(key))
                {
                    MimeTypeModel[] mimeTypes = mimeTypesByCategoryExtension[key];
                    model.IsCommon = mimeTypes.Any(u => u.IsCommon.HasValue && u.IsCommon.Value);

                    foreach (MimeTypeModel mimeType in mimeTypes)
                    {
                        if (!modelsByMimeType.ContainsKey(mimeType.MimeType))
                        {
                            modelsByMimeType.Add(mimeType.MimeType, new List<FileTypeModel>());
                        }

                        modelsByMimeType[mimeType.MimeType].Add(model);

                        if (mimeType.Aliases != null && mimeType.Aliases.Any())
                        {
                            foreach (string alias in mimeType.Aliases)
                            {
                                if (!modelsByMimeType.ContainsKey(alias))
                                {
                                    modelsByMimeType.Add(alias, new List<FileTypeModel>());
                                }

                                modelsByMimeType[alias].Add(model);
                            }
                        }
                    }
                }

                if (!fileTypeByCategoryAndExtension.ContainsKey(key))
                {
                    fileTypeByCategoryAndExtension.Add(key, model);
                }
            }

            fileTypeByMimeType = modelsByMimeType.ToDictionary(k => k.Key, v => v.Value.ToArray());
        }
    }
}
