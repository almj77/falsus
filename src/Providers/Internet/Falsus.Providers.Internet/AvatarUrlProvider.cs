namespace Falsus.Providers.Internet
{
    using System;
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar;

    /// <summary>
    /// Represents a provider of <see cref="string"/> values
    /// containing and URI representation of an <see cref="AvatarModel"/> instance.
    /// </summary>
    /// <remarks>
    /// This provider works as a proxy for the <see cref="AvatarProvider"/>
    /// converting the <see cref="AvatarModel"/> instance to an SVG string.
    /// </remarks>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class AvatarUrlProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarStyle"/> value.
        /// </summary>
        public const string StyleUrlParameter = "avatarStyle";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarTop"/> value.
        /// </summary>
        public const string TopTypeUrlParameter = "topType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarAccessory"/> value.
        /// </summary>
        public const string AccessoriesTypeUrlParameter = "accessoriesType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarHairColor"/> value.
        /// </summary>
        public const string HairColorUrlParameter = "hairColor";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarHatColor"/> value.
        /// </summary>
        public const string HatColorUrlParameter = "hatColor";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarFacialHairType"/> value.
        /// </summary>
        public const string FacialHairTypeUrlParameter = "facialHairType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarFacialHairColor"/> value.
        /// </summary>
        public const string FacialHairColorUrlParameter = "facialHairColor";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarClotheType"/> value.
        /// </summary>
        public const string ClotheTypeUrlParameter = "clotheType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarClotheColor"/> value.
        /// </summary>
        public const string ClotheColorUrlParameter = "clotheColor";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarClotheGraphicType"/> value.
        /// </summary>
        public const string ClotheGraphicTypeUrlParameter = "graphicType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarEyeType"/> value.
        /// </summary>
        public const string EyeTypeUrlParameter = "eyeType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarEyebrowType"/> value.
        /// </summary>
        public const string EyebrowTypeUrlParameter = "eyebrowType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarMouthType"/> value.
        /// </summary>
        public const string MouthTypeUrlParameter = "mouthType";

        /// <summary>
        /// A string containing the query string parameter name for the <see cref="Shared.Avatar.Enums.AvatarSkinColor"/> value.
        /// </summary>
        public const string SkinColorUrlParameter = "skinColor";

        /// <summary>
        /// A string containing the base for the avatar URI.
        /// </summary>
        public const string BaseUrl = "https://avataaars.io/";

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private AvatarUrlProviderConfiguration configuration;

        /// <summary>
        /// The provider of <see cref="AvatarModel"/> instances.
        /// </summary>
        private AvatarProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarUrlProvider"/> class.
        /// </summary>
        public AvatarUrlProvider()
            : base()
        {
            this.provider = new AvatarProvider();
            this.configuration = new AvatarUrlProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarUrlProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public AvatarUrlProvider(AvatarUrlProviderConfiguration configuration)
            : this()
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random <see cref="string"/> containing an URI representation
        /// of an <see cref="AvatarModel"/> instance
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
        /// A random instance of <see cref="AvatarModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>, converted to an URI representation.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="AvatarModel"/>
        /// values is not supported.
        /// </exception>
        public override string GetRangedRowValue(string minValue, string maxValue, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(AvatarUrlProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="string"/> containing an URI representation
        /// of an <see cref="AvatarModel"/> instance
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
        /// A random <see cref="string"/> containing an URI representation
        /// of an <see cref="AvatarModel"/> instance that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="AvatarModel"/>
        /// values is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(AvatarUrlProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="string"/> containing an URI representation
        /// of an <see cref="AvatarModel"/> instance based on the context
        /// and excluded objects.
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
        /// A <see cref="string"/> containing an URI representation of a random instance of <see cref="AvatarModel"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="excludedObjects"/> are not being taken into account.
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            AvatarModel model = this.provider.GetRowValue(context, Array.Empty<AvatarModel>());

            List<string> urlParams = new List<string>();
            urlParams.Add(string.Concat(StyleUrlParameter, "=", this.configuration.Style.ToString()));
            urlParams.Add(string.Concat(AccessoriesTypeUrlParameter, "=", model.Accessory.ToString()));
            urlParams.Add(string.Concat(ClotheColorUrlParameter, "=", model.ClotheColor.ToString()));
            urlParams.Add(string.Concat(ClotheTypeUrlParameter, "=", model.ClotheType.ToString()));
            urlParams.Add(string.Concat(ClotheGraphicTypeUrlParameter, "=", model.ClotheGraphicType.ToString()));
            urlParams.Add(string.Concat(EyebrowTypeUrlParameter, "=", model.EyebrowType.ToString()));
            urlParams.Add(string.Concat(EyeTypeUrlParameter, "=", model.EyeType.ToString()));
            urlParams.Add(string.Concat(FacialHairColorUrlParameter, "=", model.FacialHairColor.ToString()));
            urlParams.Add(string.Concat(FacialHairTypeUrlParameter, "=", model.FacialHairType.ToString()));
            urlParams.Add(string.Concat(HairColorUrlParameter, "=", model.HairColor.ToString()));
            urlParams.Add(string.Concat(HatColorUrlParameter, "=", model.HatColor.ToString()));
            urlParams.Add(string.Concat(MouthTypeUrlParameter, "=", model.MouthType.ToString()));
            urlParams.Add(string.Concat(SkinColorUrlParameter, "=", model.SkinColor.ToString()));
            urlParams.Add(string.Concat(TopTypeUrlParameter, "=", model.Top.ToString()));

            return string.Concat(BaseUrl, string.Join("&", urlParams));
        }

        /// <summary>
        /// Gets an instance of <see cref="string"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="string"/> with the specified unique identifier.
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
        /// Gets a unique identifier for the provided <see cref="string"/> value.
        /// </summary>
        /// <param name="value">The <see cref="string"/>value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="string"/>.
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
        /// provided <see cref="AvatarUrlProviderConfiguration"/>.
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

            DataGeneratorProperty<AvatarModel> providerProperty = new DataGeneratorProperty<AvatarModel>(property.Id, property.IsUnique, property.AllowNull, this.provider, property.Arguments);
            this.provider.Load(providerProperty, rowCount);
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
                this.provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                this.provider.InitializeRandomizer();
            }
        }
    }
}
