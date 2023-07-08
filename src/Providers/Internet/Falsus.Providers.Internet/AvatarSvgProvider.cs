namespace Falsus.Providers.Internet
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar;
    using Falsus.Shared.Avatar.Enums;

    /// <summary>
    /// Represents a provider of <see cref="string"/> values
    /// containing and SVG representation of an <see cref="AvatarModel"/> instance.
    /// </summary>
    /// <remarks>
    /// This provider works as a proxy for the <see cref="AvatarProvider"/>
    /// converting the <see cref="AvatarModel"/> instance to an SVG string.
    /// </remarks>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class AvatarSvgProvider : DataGeneratorProvider<string>
    {
        /// <summary>
        /// A string containing the placeholder for the avatar circle
        /// when the <see cref="AvatarStyle.Circle"/> value is specified.
        /// When the <see cref="AvatarStyle.Transparent"/> value is specified
        /// then the placeholder will be replaced by an empty string.
        /// </summary>
        private const string AvatarCirclePlaceholder = "{Circle}";

        /// <summary>
        /// A string containing the placeholder for the avatar mouth.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarMouthType"/> value.
        /// </remarks>
        private const string AvatarMouthPlaceholder = "{Mouth}";

        /// <summary>
        /// A string containing the placeholder for the avatar nose.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of default avatar nose.
        /// </remarks>
        private const string AvatarNosePlaceholder = "{Nose}";

        /// <summary>
        /// A string containing the placeholder for the avatar eyes.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarEyeType"/> value.
        /// </remarks>
        private const string AvatarEyesPlaceholder = "{Eyes}";

        /// <summary>
        /// A string containing the placeholder for the avatar eyebrows.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarEyebrowType"/> value.
        /// </remarks>
        private const string AvatarEyebrowPlaceholder = "{Eyebrow}";

        /// <summary>
        /// A string containing the placehodler for the avatar hair color.
        /// </summary>
        /// <remarks>
        /// If present, the placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarHairColor"/> value.
        /// </remarks>
        private const string HairColorPlaceholder = "{hair-color}";

        /// <summary>
        /// A string containing the placeholder for the avatar facial hair type.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarFacialHairType"/> value.
        /// </remarks>
        private const string FacialHairPlaceholder = "{FacialHair}";

        /// <summary>
        /// A string containing the placeholder for the avatar facial hair color.
        /// </summary>
        /// <remarks>
        /// If present, the placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarFacialHairColor"/> value.
        /// </remarks>
        private const string FacialHairColorPlaceholder = "{facial-hair-color}";

        /// <summary>
        /// A string containing the placeholder for the avatar accessory.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarAccessory"/> value.
        /// </remarks>
        private const string AccessoriesPlaceholder = "{Accessories}";

        /// <summary>
        /// A string containing the placeholder for the hat color.
        /// </summary>
        /// <remarks>
        /// If present, the placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarHatColor"/> value.
        /// </remarks>
        private const string HatColorPlaceholder = "{hat-color}";

        /// <summary>
        /// A string containing the placeholder for the avatar top.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarTop"/> value.
        /// </remarks>
        private const string TopPlaceholder = "{Top}";

        /// <summary>
        /// A string containing the placeholder for the avatar clothe color.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarClotheColor"/> value.
        /// </remarks>
        private const string ClothingColorPlaceholder = "{clothing-color}";

        /// <summary>
        /// A string containing the placeholder for the avatar clothe graphic.
        /// </summary>
        /// <remarks>
        /// If present, the placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarClotheGraphicType"/> value.
        /// </remarks>
        private const string ClothingGraphicPlaceholder = "{clothing-graphic}";

        /// <summary>
        /// A string containing the placeholder for the avatar clothe type.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarClotheType"/> value.
        /// </remarks>
        private const string ClothePlaceholder = "{Clothe}";

        /// <summary>
        /// A string containing the placeholder for the avatar skin color.
        /// </summary>
        /// <remarks>
        /// The placeholder will be replaced by the SVG representation
        /// of the selected <see cref="AvatarSkinColor"/> value.
        /// </remarks>
        private const string SkinColorPlaceholder = "{skin-color}";

        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private AvatarSvgProviderConfiguration configuration;

        /// <summary>
        /// The provider of <see cref="AvatarModel"/> instances.
        /// </summary>
        private AvatarProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarSvgProvider"/> class.
        /// </summary>
        public AvatarSvgProvider()
            : base()
        {
            this.provider = new AvatarProvider();
            this.configuration = new AvatarSvgProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarSvgProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public AvatarSvgProvider(AvatarSvgProviderConfiguration configuration)
            : this()
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random <see cref="string"/> containing an SVG representation
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
        /// and less than <paramref name="maxValue"/>, converted to an SVG representation.
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
            throw new NotSupportedException($"{nameof(AvatarSvgProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="string"/> containing an SVG representation
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
        /// A random <see cref="string"/> containing an SVG representation
        /// of an <see cref="AvatarModel"/> instance that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="AvatarModel"/>
        /// values is not supported.
        /// </exception>
        public override string GetRowValue(DataGeneratorContext context, WeightedRange<string>[] excludedRanges, string[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(AvatarSvgProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="string"/> containing an SVG representation
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
        /// A <see cref="string"/> containing an SVG representation of a random instance of <see cref="AvatarModel"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="excludedObjects"/> are not being taken into account.
        /// </remarks>
        public override string GetRowValue(DataGeneratorContext context, string[] excludedObjects)
        {
            AvatarModel model = this.provider.GetRowValue(context, Array.Empty<AvatarModel>());

            Assembly assembly = typeof(AvatarSvgProvider).GetTypeInfo().Assembly;

            string svg = this.ReadManifestData(assembly, "Base.svg");
            if (this.configuration.Style == AvatarStyle.Circle)
            {
                svg = svg.Replace(AvatarCirclePlaceholder, this.ReadManifestData(assembly, "BaseCircle.svg"));
            }
            else
            {
                svg = svg.Replace(AvatarCirclePlaceholder, string.Empty);
            }

            svg = svg.Replace(AvatarMouthPlaceholder, this.ReadManifestData(assembly, string.Concat("Face.Mouth.", model.MouthType.ToString(), ".svg")));
            svg = svg.Replace(AvatarNosePlaceholder, this.ReadManifestData(assembly, "Face.Nose.Default.svg"));
            svg = svg.Replace(AvatarEyesPlaceholder, this.ReadManifestData(assembly, string.Concat("Face.Eyes.", model.EyeType.ToString(), ".svg")));
            svg = svg.Replace(AvatarEyebrowPlaceholder, this.ReadManifestData(assembly, string.Concat("Face.Eyebrow.", model.EyebrowType.ToString(), ".svg")));

            string topSvg = this.ReadManifestData(assembly, string.Concat("Top.", model.Top.ToString(), ".svg"));
            if (topSvg.Contains(HairColorPlaceholder))
            {
                topSvg = topSvg.Replace(HairColorPlaceholder, this.ConvertColor(model.HairColor));
            }

            if (topSvg.Contains(FacialHairPlaceholder))
            {
                if (model.FacialHairType != AvatarFacialHairType.Blank)
                {
                    string facialHairSvg = this.ReadManifestData(assembly, string.Concat("Top.FacialHair.", model.FacialHairType.ToString(), ".svg"));
                    if (facialHairSvg.Contains(FacialHairColorPlaceholder))
                    {
                        facialHairSvg = facialHairSvg.Replace(FacialHairColorPlaceholder, this.ConvertColor(model.FacialHairColor));
                    }

                    topSvg = topSvg.Replace(FacialHairPlaceholder, facialHairSvg);
                }
                else
                {
                    topSvg = topSvg.Replace(FacialHairPlaceholder, string.Empty);
                }
            }

            if (topSvg.Contains(AccessoriesPlaceholder))
            {
                if (model.Accessory != AvatarAccessory.Blank)
                {
                    topSvg = topSvg.Replace(AccessoriesPlaceholder, this.ReadManifestData(assembly, string.Concat("Top.Accessories.", model.Accessory.ToString(), ".svg")));
                }
                else
                {
                    topSvg = topSvg.Replace(AccessoriesPlaceholder, string.Empty);
                }
            }

            if (topSvg.Contains(HatColorPlaceholder))
            {
                topSvg = topSvg.Replace(HatColorPlaceholder, this.ConvertColor(model.HatColor));
            }

            svg = svg.Replace(TopPlaceholder, topSvg);

            string clotheSvg = this.ReadManifestData(assembly, string.Concat("Clothes.", model.ClotheType.ToString(), ".svg"));
            if (clotheSvg.Contains(ClothingColorPlaceholder))
            {
                clotheSvg = clotheSvg.Replace(ClothingColorPlaceholder, this.ConvertColor(model.ClotheColor));
            }

            if (clotheSvg.Contains(ClothingGraphicPlaceholder))
            {
                clotheSvg = clotheSvg.Replace(ClothingGraphicPlaceholder, this.ReadManifestData(assembly, string.Concat("clothes.Graphics.", model.ClotheGraphicType.ToString(), ".svg")));
            }

            svg = svg.Replace(ClothePlaceholder, clotheSvg);
            svg = svg.Replace(SkinColorPlaceholder, this.ConvertColor(model.SkinColor));

            return svg;
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
        /// provided <see cref="AvatarSvgProviderConfiguration"/>.
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

        /// <summary>
        /// Converts a value of <see cref="AvatarHairColor"/> into an
        /// hexadecimal representation of the color.
        /// </summary>
        /// <param name="color">The <see cref="AvatarHairColor"/> value.</param>
        /// <returns>
        /// A string representing the hexadecimal value for the <paramref name="color"/> value.
        /// </returns>
        private string ConvertColor(AvatarHairColor color)
        {
            switch (color)
            {
                case AvatarHairColor.Auburn:
                    return "#A55728";
                case AvatarHairColor.Black:
                    return "#2C1B18";
                case AvatarHairColor.Blonde:
                    return "#B58143";
                case AvatarHairColor.BlondeGolden:
                    return "#D6B370";
                case AvatarHairColor.Brown:
                    return "#724133";
                case AvatarHairColor.PastelPink:
                    return "#F59797";
                case AvatarHairColor.Platinum:
                    return "#ECDCBF";
                case AvatarHairColor.Red:
                    return "#C93305";
                case AvatarHairColor.SilverGray:
                    return "#E8E1E1";
                case AvatarHairColor.BrownDark:
                default:
                    return "#4A312C";
            }
        }

        /// <summary>
        /// Converts a value of <see cref="AvatarFacialHairColor"/> into an
        /// hexadecimal representation of the color.
        /// </summary>
        /// <param name="color">The <see cref="AvatarFacialHairColor"/> value.</param>
        /// <returns>
        /// A string representing the hexadecimal value for the <paramref name="color"/> value.
        /// </returns>
        private string ConvertColor(AvatarFacialHairColor color)
        {
            switch (color)
            {
                case AvatarFacialHairColor.Auburn:
                    return "#A55728";
                case AvatarFacialHairColor.Black:
                    return "#2C1B18";
                case AvatarFacialHairColor.Blonde:
                    return "#B58143";
                case AvatarFacialHairColor.BlondeGolden:
                    return "#D6B370";
                case AvatarFacialHairColor.Brown:
                    return "#724133";
                case AvatarFacialHairColor.Platinum:
                    return "#ECDCBF";
                case AvatarFacialHairColor.Red:
                    return "#C93305";
                case AvatarFacialHairColor.BrownDark:
                default:
                    return "#4A312C";
            }
        }

        /// <summary>
        /// Converts a value of <see cref="AvatarClotheColor"/> into an
        /// hexadecimal representation of the color.
        /// </summary>
        /// <param name="color">The <see cref="AvatarClotheColor"/> value.</param>
        /// <returns>
        /// A string representing the hexadecimal value for the <paramref name="color"/> value.
        /// </returns>
        private string ConvertColor(AvatarClotheColor color)
        {
            switch (color)
            {
                case AvatarClotheColor.Black:
                    return "#262E33";
                case AvatarClotheColor.Blue01:
                    return "#65C9FF";
                case AvatarClotheColor.Blue02:
                    return "#5199E4";
                case AvatarClotheColor.Blue03:
                    return "#25557C";
                case AvatarClotheColor.Gray02:
                    return "#929598";
                case AvatarClotheColor.Heather:
                    return "#3C4F5C";
                case AvatarClotheColor.PastelBlue:
                    return "#B1E2FF";
                case AvatarClotheColor.PastelGreen:
                    return "#A7FFC4";
                case AvatarClotheColor.PastelOrange:
                    return "#FFDEB5";
                case AvatarClotheColor.PastelRed:
                    return "#FFAFB9";
                case AvatarClotheColor.PastelYellow:
                    return "#FFFFB1";
                case AvatarClotheColor.Pink:
                    return "#FF488E";
                case AvatarClotheColor.Red:
                    return "#FF5C5C";
                case AvatarClotheColor.White:
                    return "#FFFFFF";
                case AvatarClotheColor.Gray01:
                default:
                    return "#E6E6E6";
            }
        }

        /// <summary>
        /// Converts a value of <see cref="AvatarHatColor"/> into an
        /// hexadecimal representation of the color.
        /// </summary>
        /// <param name="color">The <see cref="AvatarHatColor"/> value.</param>
        /// <returns>
        /// A string representing the hexadecimal value for the <paramref name="color"/> value.
        /// </returns>
        private string ConvertColor(AvatarHatColor color)
        {
            switch (color)
            {
                case AvatarHatColor.Black:
                    return "#262E33";
                case AvatarHatColor.Blue01:
                    return "#65C9FF";
                case AvatarHatColor.Blue02:
                    return "#5199E4";
                case AvatarHatColor.Gray01:
                    return "#E6E6E6";
                case AvatarHatColor.Gray02:
                    return "#929598";
                case AvatarHatColor.Heather:
                    return "#3C4F5C";
                case AvatarHatColor.PastelBlue:
                    return "#B1E2FF";
                case AvatarHatColor.PastelGreen:
                    return "#A7FFC4";
                case AvatarHatColor.PastelOrange:
                    return "#FFDEB5";
                case AvatarHatColor.PastelRed:
                    return "#FFAFB9";
                case AvatarHatColor.PastelYellow:
                    return "#FFFFB1";
                case AvatarHatColor.Pink:
                    return "#FF488E";
                case AvatarHatColor.Red:
                    return "#FF5C5C";
                case AvatarHatColor.White:
                    return "#FFFFFF";
                case AvatarHatColor.Blue03:
                default:
                    return "#25557C";
            }
        }

        /// <summary>
        /// Converts a value of <see cref="AvatarSkinColor"/> into an
        /// hexadecimal representation of the color.
        /// </summary>
        /// <param name="color">The <see cref="AvatarSkinColor"/> value.</param>
        /// <returns>
        /// A string representing the hexadecimal value for the <paramref name="color"/> value.
        /// </returns>
        private string ConvertColor(AvatarSkinColor color)
        {
            switch (color)
            {
                case AvatarSkinColor.Tanned:
                    return "#FD9841";
                case AvatarSkinColor.Yellow:
                    return "#F8D25C";
                case AvatarSkinColor.Pale:
                    return "#FFDBB4";
                case AvatarSkinColor.Brown:
                    return "#D08B5B";
                case AvatarSkinColor.DarkBrown:
                    return "#AE5D29";
                case AvatarSkinColor.Black:
                    return "#614335";
                case AvatarSkinColor.Light:
                default:
                    return "#EDB98A";
            }
        }

        /// <summary>
        /// Reads the contents of a file embedded in the DLL.
        /// </summary>
        /// <param name="assembly">The assembly to read the file from.</param>
        /// <param name="embeddedFileName">The name of the file to read.</param>
        /// <returns>A <see cref="string"/> containing the contents of the file.</returns>
        /// <remarks>
        /// This method will read the contents of the first file whose name ends with <paramref name="embeddedFileName"/> value.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the manifest resource stream is null.</exception>
        private string ReadManifestData(Assembly assembly, string embeddedFileName)
        {
            var resourceName = assembly.GetManifestResourceNames().First(s => s.EndsWith(embeddedFileName, StringComparison.InvariantCultureIgnoreCase));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
