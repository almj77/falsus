namespace Falsus.Providers.Sys
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Models;

    /// <summary>
    /// Represents a provider of Semantic Versions.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class SemanticVersionProvider : DataGeneratorProvider<SemanticVersionModel>
    {
        private const string AlphaStageIdentifier = "a";

        private const string BetaStageIdentifier = "b";

        private const string ReleaseCandidateIdentifier = "rc";

        /// <summary>
        /// The number of attemps to try to generate a unique value
        /// before throwing an exception.
        /// </summary>
        private const int MaxAttempts = 10;

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private SemanticVersionProviderConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionProvider"/> class.
        /// </summary>
        public SemanticVersionProvider()
            : base()
        {
            this.configuration = new SemanticVersionProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public SemanticVersionProvider(SemanticVersionProviderConfiguration configuration)
            : this()
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random semantic version that is greater than the
        /// value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="SemanticVersionModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="SemanticVersionModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        public override SemanticVersionModel GetRangedRowValue(SemanticVersionModel minValue, SemanticVersionModel maxValue, SemanticVersionModel[] excludedObjects)
        {
            if (minValue == null && maxValue == null)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate ranged value with both min. and max. value set to null.");
            }

            SemanticVersionModel value = this.GetRangedRowValueInternal(minValue, maxValue);
            int attempts = 0;
            while (excludedObjects.Contains(value) && attempts <= MaxAttempts)
            {
                value = this.GetRangedRowValueInternal(minValue, maxValue);
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Generates a random semantic version based on the context and excluded ranges.
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
        /// An array of <see cref="SemanticVersionModel"/> values that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="SemanticVersionModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        public override SemanticVersionModel GetRowValue(DataGeneratorContext context, WeightedRange<SemanticVersionModel>[] excludedRanges, SemanticVersionModel[] excludedObjects)
        {
            SemanticVersionModel allowedMinVal = this.GetMinValue();
            SemanticVersionModel allowedMaxVal = this.GetMaxValue();

            if (excludedRanges != null && excludedRanges.Any())
            {
                List<(SemanticVersionModel MinValue, SemanticVersionModel MaxValue)> ranges = new List<(SemanticVersionModel MinValue, SemanticVersionModel MaxValue)>();
                WeightedRange<SemanticVersionModel>[] sortedExcludedRanges = excludedRanges.OrderBy(u => u.MinValue).ToArray();

                for (int i = 0; i < excludedRanges.Length; i++)
                {
                    if (i == 0 && sortedExcludedRanges[i].MinValue > allowedMinVal)
                    {
                        ranges.Add((allowedMinVal, sortedExcludedRanges[i].MinValue));
                    }

                    if (i > 0 && i <= excludedRanges.Length - 1 && sortedExcludedRanges[i - 1].MaxValue < sortedExcludedRanges[i].MinValue)
                    {
                        ranges.Add((sortedExcludedRanges[i - 1].MaxValue, sortedExcludedRanges[i].MinValue));
                    }

                    if (i == excludedRanges.Length - 1 && sortedExcludedRanges[i].MaxValue < allowedMaxVal)
                    {
                        ranges.Add((sortedExcludedRanges[i].MaxValue, allowedMaxVal));
                    }
                }

                if (!ranges.Any())
                {
                    throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} was unable to find a range to generate values.");
                }

                (SemanticVersionModel MinValue, SemanticVersionModel MaxValue) randomRange = ranges[this.Randomizer.Next(0, ranges.Count)];
                return this.GetRangedRowValue(randomRange.MinValue, randomRange.MaxValue, excludedObjects);
            }
            else
            {
                return this.GetRowValue(context, excludedObjects);
            }
        }

        /// <summary>
        /// Gets a <see cref="SemanticVersionModel"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="SemanticVersionModel"/> value with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="SemanticVersionModel"/> instance is
        /// defined by the concatenation of all version numbers.
        /// The unique id can be fetched by invoking the
        /// <see cref="SemanticVersionProvider.GetValueId(SemanticVersionModel)"/>
        /// or the <see cref="SemanticVersionModel.ToString"/> methods.
        /// </remarks>
        public override SemanticVersionModel GetRowValue(string id)
        {
            if (SemanticVersionModel.TryParse(id, out SemanticVersionModel model))
            {
                return model;
            }

            return null;
        }

        /// <summary>
        /// Generates a random semantic version based on the context and excluded objects.
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
        /// A random semantic version.
        /// </returns>
        public override SemanticVersionModel GetRowValue(DataGeneratorContext context, SemanticVersionModel[] excludedObjects)
        {
            SemanticVersionModel value = this.GetRowValueInternal();
            int attempts = 0;
            while (excludedObjects.Contains(value) && attempts <= MaxAttempts)
            {
                value = this.GetRowValueInternal();
                if (attempts == MaxAttempts)
                {
                    throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate another unique value.");
                }

                attempts++;
            }

            return value;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="SemanticVersionModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="SemanticVersionModel"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// The unique identifier of the <see cref="SemanticVersionModel"/> instance is
        /// defined by the concatenation of all version numbers.
        /// The unique id can be fetched by invoking the
        /// <see cref="SemanticVersionModel.ToString"/> method.
        /// </remarks>
        public override string GetValueId(SemanticVersionModel value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation based on the
        /// provided <see cref="SemanticVersionProviderConfiguration"/>.
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
        public override void Load(DataGeneratorProperty<SemanticVersionModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            this.ValidateConfiguration();
        }

        private SemanticVersionModel GetRangedRowValueInternal(SemanticVersionModel minValue, SemanticVersionModel maxValue)
        {
            if (minValue == null && maxValue == null)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate ranged value with both min. and max. value set to null.");
            }

            SemanticVersionModel internalMinValue = new SemanticVersionModel(
                minValue.Major ?? this.configuration.MinMajorVersion ?? 0,
                minValue.Minor ?? this.configuration.MinMinorVersion ?? 0,
                minValue.Patch ?? this.configuration.MinPatchVersion ?? 0,
                minValue.StageNumber ?? this.configuration.MinStageVersion ?? 0);
            SemanticVersionModel internalMaxValue = new SemanticVersionModel(
                maxValue.Major ?? this.configuration.MaxMajorVersion ?? int.MaxValue,
                maxValue.Minor ?? this.configuration.MaxMinorVersion ?? int.MaxValue,
                maxValue.Patch ?? this.configuration.MaxPatchVersion ?? int.MaxValue,
                maxValue.StageNumber ?? this.configuration.MaxStageVersion ?? int.MaxValue);

            int? majorVersion = default;
            int? minorVersion = default;
            int? patchVersion = default;
            string stage = string.Empty;
            int? stageNumber = default;

            // Major Version
            if (internalMinValue.Major.Value <= internalMaxValue.Major.Value)
            {
                majorVersion = this.Randomizer.Next(internalMinValue.Major.Value, internalMaxValue.Major.Value);
            }
            else
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate ranged value because min version '{minValue}' is higher than max version '{maxValue}'.");
            }

            // Minor Version
            if (internalMinValue.Minor.Value <= internalMaxValue.Minor.Value)
            {
                minorVersion = this.Randomizer.Next(internalMinValue.Minor.Value, internalMaxValue.Minor.Value);
            }
            else
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate ranged value because min version '{minValue}' is higher than max version '{maxValue}'.");
            }

            // Patch Version
            if (internalMinValue.Patch.Value <= internalMaxValue.Patch.Value)
            {
                patchVersion = this.Randomizer.Next(internalMinValue.Patch.Value, internalMaxValue.Patch.Value);
            }
            else
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate ranged value because min version '{minValue}' is higher than max version '{maxValue}'.");
            }

            // Stage
            List<string> stages = new List<string>();

            if (!string.IsNullOrEmpty(internalMinValue.Stage) && !string.IsNullOrEmpty(internalMaxValue.Stage))
            {
                if (majorVersion == internalMinValue.Major && minorVersion == internalMinValue.Minor && patchVersion == internalMinValue.Patch &&
                    majorVersion == internalMaxValue.Major && minorVersion == internalMaxValue.Minor && patchVersion == internalMaxValue.Patch)
                {
                    if (internalMinValue.Stage.CompareTo(AlphaStageIdentifier) >= 0 && internalMaxValue.Stage.CompareTo(BetaStageIdentifier) < 0)
                    {
                        stages.Add(AlphaStageIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(BetaStageIdentifier) >= 0 && internalMaxValue.Stage.CompareTo(ReleaseCandidateIdentifier) < 0)
                    {
                        stages.Add(BetaStageIdentifier);
                    }
                }
                else if (majorVersion == internalMinValue.Major && minorVersion == internalMinValue.Minor && patchVersion == internalMinValue.Patch)
                {
                    if (internalMinValue.Stage.CompareTo(AlphaStageIdentifier) >= 0)
                    {
                        stages.Add(AlphaStageIdentifier);
                        stages.Add(BetaStageIdentifier);
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(BetaStageIdentifier) >= 0)
                    {
                        stages.Add(BetaStageIdentifier);
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(ReleaseCandidateIdentifier) >= 0)
                    {
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                }
                else if (majorVersion == internalMaxValue.Major && minorVersion == internalMaxValue.Minor && patchVersion == internalMaxValue.Patch)
                {
                    if (internalMaxValue.Stage.CompareTo(BetaStageIdentifier) < 0)
                    {
                        stages.Add(AlphaStageIdentifier);
                    }

                    if (internalMaxValue.Stage.CompareTo(ReleaseCandidateIdentifier) < 0)
                    {
                        stages.Add(BetaStageIdentifier);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(internalMinValue.Stage))
            {
                if (majorVersion == internalMinValue.Major && minorVersion == internalMinValue.Minor && patchVersion == internalMinValue.Patch)
                {
                    if (internalMinValue.Stage.CompareTo(AlphaStageIdentifier) >= 0)
                    {
                        stages.Add(AlphaStageIdentifier);
                        stages.Add(BetaStageIdentifier);
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(BetaStageIdentifier) >= 0)
                    {
                        stages.Add(BetaStageIdentifier);
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(ReleaseCandidateIdentifier) >= 0)
                    {
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                }
                else
                {
                    stages.Add(AlphaStageIdentifier);
                    stages.Add(BetaStageIdentifier);
                    stages.Add(ReleaseCandidateIdentifier);
                }
            }
            else if (!string.IsNullOrEmpty(internalMaxValue.Stage))
            {
                if (majorVersion == internalMinValue.Major && minorVersion == internalMinValue.Minor && patchVersion == internalMinValue.Patch)
                {
                    if (internalMinValue.Stage.CompareTo(AlphaStageIdentifier) >= 0)
                    {
                        stages.Add(AlphaStageIdentifier);
                        stages.Add(BetaStageIdentifier);
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(BetaStageIdentifier) >= 0)
                    {
                        stages.Add(BetaStageIdentifier);
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                    else if (internalMinValue.Stage.CompareTo(ReleaseCandidateIdentifier) >= 0)
                    {
                        stages.Add(ReleaseCandidateIdentifier);
                    }
                }
                else
                {
                    stages.Add(AlphaStageIdentifier);
                    stages.Add(BetaStageIdentifier);
                    stages.Add(ReleaseCandidateIdentifier);
                }
            }

            if (stages.Count > 0)
            {
                stage = stages[this.Randomizer.Next(0, stages.Count)];
            }

            // Stage Number
            if (internalMinValue.StageNumber.HasValue && internalMaxValue.StageNumber.HasValue)
            {
                if (internalMinValue.StageNumber.Value <= internalMaxValue.StageNumber.Value)
                {
                    stageNumber = this.Randomizer.Next(internalMinValue.StageNumber.Value, internalMaxValue.StageNumber.Value);
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} cannot generate ranged value because min version '{minValue}' is higher than max version '{maxValue}'.");
                }
            }

            if (majorVersion.HasValue)
            {
                if (minorVersion.HasValue)
                {
                    if (patchVersion.HasValue)
                    {
                        if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                        {
                            return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value, stage, stageNumber.Value);
                        }

                        if (stageNumber.HasValue)
                        {
                            return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value, stageNumber.Value);
                        }

                        return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value);
                    }

                    if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                    {
                        return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, stage, stageNumber.Value);
                    }

                    return new SemanticVersionModel(majorVersion.Value, minorVersion.Value);
                }

                if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                {
                    return new SemanticVersionModel(majorVersion.Value, stage, stageNumber.Value);
                }

                return new SemanticVersionModel(majorVersion.Value);
            }

            return null;
        }

        private SemanticVersionModel GetRowValueInternal()
        {
            int? majorVersion;
            int? minorVersion;
            int? patchVersion;
            string stage = string.Empty;
            int? stageNumber = default;

            if (this.configuration.MinMajorVersion.HasValue || this.configuration.MaxMajorVersion.HasValue)
            {
                majorVersion = this.Randomizer.Next(this.configuration.MinMajorVersion ?? 0, this.configuration.MaxMajorVersion ?? int.MaxValue);
            }
            else
            {
                majorVersion = this.Randomizer.Next(0, int.MaxValue);
            }

            if (this.configuration.MinMinorVersion.HasValue || this.configuration.MaxMinorVersion.HasValue)
            {
                minorVersion = this.Randomizer.Next(this.configuration.MinMinorVersion ?? 0, this.configuration.MaxMinorVersion ?? int.MaxValue);
            }
            else
            {
                minorVersion = this.Randomizer.Next(0, int.MaxValue);
            }

            if (this.configuration.IncludeAlphaStage || this.configuration.IncludeBetaStage || this.configuration.IncludeReleaseCandidateStage)
            {
                List<string> stageTokens = new List<string>();

                if (this.configuration.UseNumStatusFormat)
                {
                    patchVersion = this.Randomizer.Next(0, 3);
                }
                else
                {
                    if (this.configuration.MinPatchVersion.HasValue || this.configuration.MaxPatchVersion.HasValue)
                    {
                        patchVersion = this.Randomizer.Next(this.configuration.MinPatchVersion ?? 0, this.configuration.MaxPatchVersion ?? int.MaxValue);
                    }
                    else
                    {
                        patchVersion = this.Randomizer.Next(0, int.MaxValue);
                    }

                    if (this.configuration.IncludeAlphaStage)
                    {
                        stageTokens.Add(AlphaStageIdentifier);
                    }

                    if (this.configuration.IncludeBetaStage)
                    {
                        stageTokens.Add(BetaStageIdentifier);
                    }

                    if (this.configuration.IncludeReleaseCandidateStage)
                    {
                        stageTokens.Add(ReleaseCandidateIdentifier);
                    }
                }

                stage = stageTokens[this.Randomizer.Next(0, stageTokens.Count)];
            }
            else
            {
                if (this.configuration.MinPatchVersion.HasValue || this.configuration.MaxPatchVersion.HasValue)
                {
                    patchVersion = this.Randomizer.Next(this.configuration.MinPatchVersion ?? 0, this.configuration.MaxPatchVersion ?? int.MaxValue);
                }
                else
                {
                    patchVersion = this.Randomizer.Next(0, int.MaxValue);
                }
            }

            if (majorVersion.HasValue)
            {
                if (minorVersion.HasValue)
                {
                    if (patchVersion.HasValue)
                    {
                        if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                        {
                            return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value, stage, stageNumber.Value);
                        }

                        if (stageNumber.HasValue)
                        {
                            return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value, stageNumber.Value);
                        }

                        return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value);
                    }

                    if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                    {
                        return new SemanticVersionModel(majorVersion.Value, minorVersion.Value, stage, stageNumber.Value);
                    }

                    return new SemanticVersionModel(majorVersion.Value, minorVersion.Value);
                }

                if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                {
                    return new SemanticVersionModel(majorVersion.Value, stage, stageNumber.Value);
                }

                return new SemanticVersionModel(majorVersion.Value);
            }

            return null;
        }

        private SemanticVersionModel GetMinValue()
        {
            if (this.configuration.MinMajorVersion.HasValue)
            {
                if (this.configuration.MinMinorVersion.HasValue)
                {
                    if (this.configuration.MinPatchVersion.HasValue)
                    {
                        if (this.configuration.MinStageVersion.HasValue)
                        {
                            return new SemanticVersionModel(
                                this.configuration.MinMajorVersion.Value,
                                this.configuration.MinMinorVersion.Value,
                                this.configuration.MinPatchVersion.Value,
                                this.configuration.MinStageVersion.Value);
                        }

                        return new SemanticVersionModel(
                            this.configuration.MinMajorVersion.Value,
                            this.configuration.MinMinorVersion.Value,
                            this.configuration.MinPatchVersion.Value);
                    }

                    return new SemanticVersionModel(
                        this.configuration.MinMajorVersion.Value,
                        this.configuration.MinMinorVersion.Value);
                }

                return new SemanticVersionModel(this.configuration.MinMajorVersion.Value);
            }

            return new SemanticVersionModel(0);
        }

        private SemanticVersionModel GetMaxValue()
        {
            if (this.configuration.MaxMajorVersion.HasValue)
            {
                if (this.configuration.MaxMinorVersion.HasValue)
                {
                    if (this.configuration.MaxPatchVersion.HasValue)
                    {
                        if (this.configuration.MaxStageVersion.HasValue)
                        {
                            return new SemanticVersionModel(
                                this.configuration.MaxMajorVersion.Value,
                                this.configuration.MaxMinorVersion.Value,
                                this.configuration.MaxPatchVersion.Value,
                                this.configuration.MaxStageVersion.Value);
                        }

                        return new SemanticVersionModel(
                            this.configuration.MaxMajorVersion.Value,
                            this.configuration.MaxMinorVersion.Value,
                            this.configuration.MaxPatchVersion.Value);
                    }

                    return new SemanticVersionModel(
                        this.configuration.MaxMajorVersion.Value,
                        this.configuration.MaxMinorVersion.Value);
                }

                return new SemanticVersionModel(this.configuration.MaxMajorVersion.Value);
            }

            return new SemanticVersionModel(int.MaxValue);
        }

        /// <summary>
        /// Checks if the <see cref="configuration"/> object is assigned and correct.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item>
        /// The <see cref="SemanticVersionProviderConfiguration.MinMajorVersion"/> or <see cref="SemanticVersionProviderConfiguration.MaxMajorVersion"/>
        /// or <see cref="SemanticVersionProviderConfiguration.MinMinorVersion"/> or <see cref="SemanticVersionProviderConfiguration.MaxMinorVersion"/>
        /// or <see cref="SemanticVersionProviderConfiguration.MinPatchVersion"/> or <see cref="SemanticVersionProviderConfiguration.MaxPatchVersion"/>
        /// or <see cref="SemanticVersionProviderConfiguration.MinStageVersion"/> or <see cref="SemanticVersionProviderConfiguration.MaxStageVersion"/>
        /// have values lower than 0.
        /// </item>
        /// <item>
        /// The value of <see cref="SemanticVersionProviderConfiguration.MinMajorVersion"/> is greater than the
        /// value of <see cref="SemanticVersionProviderConfiguration.MaxMajorVersion"/>.
        /// </item>
        /// <item>
        /// The value of <see cref="SemanticVersionProviderConfiguration.MinMinorVersion"/> is greater than the
        /// value of <see cref="SemanticVersionProviderConfiguration.MaxMinorVersion"/>.
        /// </item>
        /// <item>
        /// The value of <see cref="SemanticVersionProviderConfiguration.MinPatchVersion"/> is greater than the
        /// value of <see cref="SemanticVersionProviderConfiguration.MaxPatchVersion"/>.
        /// </item>
        /// <item>
        /// The value of <see cref="SemanticVersionProviderConfiguration.MinStageVersion"/> is greater than the
        /// value of <see cref="SemanticVersionProviderConfiguration.MaxStageVersion"/>.
        /// </item>
        /// </list>
        /// </exception>
        private void ValidateConfiguration()
        {
            if (this.configuration.MinMajorVersion.HasValue && this.configuration.MinMajorVersion.Value < 0)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} Min. major version cannot be lower than zero.");
            }

            if (this.configuration.MinMinorVersion.HasValue && this.configuration.MinMinorVersion.Value < 0)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} Min. minor version cannot be lower than zero.");
            }

            if (this.configuration.MinPatchVersion.HasValue && this.configuration.MinPatchVersion.Value < 0)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} Min. patch version cannot be lower than zero.");
            }

            if (this.configuration.MinMajorVersion.HasValue && this.configuration.MaxMajorVersion.HasValue &&
                this.configuration.MinMajorVersion.Value > this.configuration.MaxMajorVersion.Value)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} Min. major version cannot be higher than Max. major version.");
            }

            if (this.configuration.MinMinorVersion.HasValue && this.configuration.MaxMinorVersion.HasValue &&
                this.configuration.MinMinorVersion.Value > this.configuration.MaxMinorVersion.Value)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} Min. minor version cannot be higher than Max. minor version.");
            }

            if (this.configuration.MinPatchVersion.HasValue && this.configuration.MaxPatchVersion.HasValue &&
                this.configuration.MinPatchVersion.Value > this.configuration.MaxPatchVersion.Value)
            {
                throw new InvalidOperationException($"{nameof(SemanticVersionProvider)} Min. patch version cannot be higher than Max. patch version.");
            }
        }
    }
}
