namespace Falsus.Providers.Internet
{
    using System;
    using System.Linq;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar;
    using Falsus.Shared.Avatar.Enums;

    /// <summary>
    /// Represents a provider of <see cref="AvatarModel"/> values.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class AvatarProvider : DataGeneratorProvider<AvatarModel>
    {
        /// <summary>
        /// The configuration of this provider.
        /// </summary>
        private AvatarProviderConfiguration configuration;

        /// <summary>
        /// An array of <see cref="AvatarAccessory"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarAccessory"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidAccessories"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterAccessories"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarAccessory[] accessories;

        /// <summary>
        /// An array of <see cref="AvatarClotheColor"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarClotheColor"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidClotheColors"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterClotheColors"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarClotheColor[] clotheColors;

        /// <summary>
        /// An array of <see cref="AvatarClotheType"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarClotheType"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidClotheTypes"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterClotheTypes"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarClotheType[] clotheTypes;

        /// <summary>
        /// An array of <see cref="AvatarClotheGraphicType"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarClotheGraphicType"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidClotheGraphicTypes"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterClotheGraphicTypes"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarClotheGraphicType[] clotheGraphicTypes;

        /// <summary>
        /// An array of <see cref="AvatarEyebrowType"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarEyebrowType"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidEyebrowTypes"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterEyebrowTypes"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarEyebrowType[] eyebrowTypes;

        /// <summary>
        /// An array of <see cref="AvatarEyeType"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarEyeType"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidEyeTypes"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterEyeTypes"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarEyeType[] eyeTypes;

        /// <summary>
        /// An array of <see cref="AvatarFacialHairColor"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarFacialHairColor"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidFacialHairColors"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterFacialHairColors"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarFacialHairColor[] facialHairColors;

        /// <summary>
        /// An array of <see cref="AvatarFacialHairType"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarFacialHairType"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidFacialHairTypes"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterFacialHairTypes"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarFacialHairType[] facialHairTypes;

        /// <summary>
        /// An array of <see cref="AvatarHairColor"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarHairColor"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidHairColors"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterHairColors"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarHairColor[] hairColors;

        /// <summary>
        /// An array of <see cref="AvatarHatColor"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarHatColor"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidHatColors"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterHatColors"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarHatColor[] hatColors;

        /// <summary>
        /// An array of <see cref="AvatarMouthType"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarMouthType"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidMouthTypes"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterMouthTypes"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarMouthType[] mouthTypes;

        /// <summary>
        /// An array of <see cref="AvatarSkinColor"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarSkinColor"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidSkinColors"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterSkinColors"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarSkinColor[] skinColors;

        /// <summary>
        /// An array of <see cref="AvatarTop"/> from which a
        /// pseudo-random value will be taken and used to populate the
        /// <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <remarks>
        /// Initially it can contain all possible values for the <see cref="AvatarTop"/>
        /// enum, or the values from the <see cref="AvatarProviderConfiguration.ValidTops"/>
        /// if any.
        /// If the <see cref="AvatarProviderConfiguration.FilterTops"/> function is specified
        /// it will be invoked during the data generation process in order to filter this collection.
        /// </remarks>
        private AvatarTop[] tops;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarProvider"/> class.
        /// </summary>
        public AvatarProvider()
            : base()
        {
            this.configuration = new AvatarProviderConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public AvatarProvider(AvatarProviderConfiguration configuration)
            : base()
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a random <see cref="AvatarModel"/> instance
        /// that is greater than the value of <paramref name="minValue"/> and lower
        /// than the value of <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="minValue">The exclusive lower bound of the random object returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random object returned.</param>
        /// <param name="excludedObjects">
        /// An array of <see cref="AvatarModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="AvatarModel"/> greater than <paramref name="minValue"/>
        /// and less than <paramref name="maxValue"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="maxValue"/> must be greater than <paramref name="minValue"/>.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="AvatarModel"/>
        /// values is not supported.
        /// </exception>
        public override AvatarModel GetRangedRowValue(AvatarModel minValue, AvatarModel maxValue, AvatarModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(AvatarProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="AvatarModel"/> instance
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
        /// An array of <see cref="AvatarModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="AvatarModel"/> that is outside of
        /// the specified <paramref name="excludedRanges"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown because the generation of ranged <see cref="AvatarModel"/>
        /// values is not supported.
        /// </exception>
        public override AvatarModel GetRowValue(DataGeneratorContext context, WeightedRange<AvatarModel>[] excludedRanges, AvatarModel[] excludedObjects)
        {
            throw new NotSupportedException($"{nameof(AvatarProvider)} does not support ranged row values.");
        }

        /// <summary>
        /// Generates a random <see cref="AvatarModel"/> instance
        /// based on the context and excluded objects.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <param name="excludedObjects">
        /// An array of <see cref="AvatarModel"/> instances that cannot be returned
        /// by the provider.
        /// </param>
        /// <returns>
        /// A random instance of <see cref="AvatarModel"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="excludedObjects"/> are not being taken into account.
        /// </remarks>
        public override AvatarModel GetRowValue(DataGeneratorContext context, AvatarModel[] excludedObjects)
        {
            AvatarAccessory[] filteredAccessories = this.accessories;
            AvatarClotheColor[] filteredClotheColors = this.clotheColors;
            AvatarClotheType[] filteredClotheTypes = this.clotheTypes;
            AvatarClotheGraphicType[] filteredClotheGraphicTypes = this.clotheGraphicTypes;
            AvatarEyebrowType[] filteredEyebrowTypes = this.eyebrowTypes;
            AvatarEyeType[] filteredEyeTypes = this.eyeTypes;
            AvatarFacialHairColor[] filteredFacialHairColors = this.facialHairColors;
            AvatarFacialHairType[] filteredFacialHairTypes = this.facialHairTypes;
            AvatarHairColor[] filteredHairColors = this.hairColors;
            AvatarHatColor[] filteredHatColors = this.hatColors;
            AvatarMouthType[] filteredMouthTypes = this.mouthTypes;
            AvatarSkinColor[] filteredSkinColors = this.skinColors;
            AvatarTop[] filteredTops = this.tops;

            if (this.configuration.FilterAccessories != null)
            {
                filteredAccessories = this.configuration.FilterAccessories(context, this.accessories);
            }

            if (this.configuration.FilterClotheColors != null)
            {
                filteredClotheColors = this.configuration.FilterClotheColors(context, this.clotheColors);
            }

            if (this.configuration.FilterClotheTypes != null)
            {
                filteredClotheTypes = this.configuration.FilterClotheTypes(context, this.clotheTypes);
            }

            if (this.configuration.FilterClotheGraphicTypes != null)
            {
                filteredClotheGraphicTypes = this.configuration.FilterClotheGraphicTypes(context, this.clotheGraphicTypes);
            }

            if (this.configuration.FilterEyebrowTypes != null)
            {
                filteredEyebrowTypes = this.configuration.FilterEyebrowTypes(context, this.eyebrowTypes);
            }

            if (this.configuration.FilterEyeTypes != null)
            {
                filteredEyeTypes = this.configuration.FilterEyeTypes(context, this.eyeTypes);
            }

            if (this.configuration.FilterFacialHairColors != null)
            {
                filteredFacialHairColors = this.configuration.FilterFacialHairColors(context, this.facialHairColors);
            }

            if (this.configuration.FilterFacialHairTypes != null)
            {
                filteredFacialHairTypes = this.configuration.FilterFacialHairTypes(context, this.facialHairTypes);
            }

            if (this.configuration.FilterHairColors != null)
            {
                filteredHairColors = this.configuration.FilterHairColors(context, this.hairColors);
            }

            if (this.configuration.FilterHatColors != null)
            {
                filteredHatColors = this.configuration.FilterHatColors(context, this.hatColors);
            }

            if (this.configuration.FilterMouthTypes != null)
            {
                filteredMouthTypes = this.configuration.FilterMouthTypes(context, this.mouthTypes);
            }

            if (this.configuration.FilterSkinColors != null)
            {
                filteredSkinColors = this.configuration.FilterSkinColors(context, this.skinColors);
            }

            if (this.configuration.FilterTops != null)
            {
                filteredTops = this.configuration.FilterTops(context, this.tops);
            }

            int randomAccessoriesIndex = this.Randomizer.Next(0, filteredAccessories.Length);
            int randomClotheColorsIndex = this.Randomizer.Next(0, filteredClotheColors.Length);
            int randomClotheTypesIndex = this.Randomizer.Next(0, filteredClotheTypes.Length);
            int randomClotheGraphicTypesIndex = this.Randomizer.Next(0, filteredClotheGraphicTypes.Length);
            int randomEyebrowTypesIndex = this.Randomizer.Next(0, filteredEyebrowTypes.Length);
            int randomEyeTypesIndex = this.Randomizer.Next(0, filteredEyeTypes.Length);
            int randomFacialHairColorsIndex = this.Randomizer.Next(0, filteredFacialHairColors.Length);
            int randomFacialHairTypesIndex = this.Randomizer.Next(0, filteredFacialHairTypes.Length);
            int randomHairColorsIndex = this.Randomizer.Next(0, filteredHairColors.Length);
            int randomHatColorsIndex = this.Randomizer.Next(0, filteredHatColors.Length);
            int randomMouthTypesIndex = this.Randomizer.Next(0, filteredMouthTypes.Length);
            int randomSkinColorsIndex = this.Randomizer.Next(0, filteredSkinColors.Length);
            int randomTopsIndex = this.Randomizer.Next(0, filteredTops.Length);

            AvatarModel model = new AvatarModel()
            {
                Accessory = filteredAccessories[randomAccessoriesIndex],
                ClotheColor = filteredClotheColors[randomClotheColorsIndex],
                ClotheType = filteredClotheTypes[randomClotheTypesIndex],
                ClotheGraphicType = filteredClotheGraphicTypes[randomClotheGraphicTypesIndex],
                EyebrowType = filteredEyebrowTypes[randomEyebrowTypesIndex],
                EyeType = filteredEyeTypes[randomEyeTypesIndex],
                FacialHairColor = filteredFacialHairColors[randomFacialHairColorsIndex],
                FacialHairType = filteredFacialHairTypes[randomFacialHairTypesIndex],
                HairColor = filteredHairColors[randomHairColorsIndex],
                HatColor = filteredHatColors[randomHatColorsIndex],
                MouthType = filteredMouthTypes[randomMouthTypesIndex],
                SkinColor = filteredSkinColors[randomSkinColorsIndex],
                Top = filteredTops[randomTopsIndex]
            };

            return model;
        }

        /// <summary>
        /// Gets an instance of <see cref="AvatarModel"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// An instance of the <see cref="AvatarModel"/> with the specified unique identifier.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// An instance of <see cref="AvatarModel"/> cannot be represented by a <see cref="string"/> identifier
        /// therefore this method is not supported.
        /// </exception>
        public override AvatarModel GetRowValue(string id)
        {
            throw new NotSupportedException($"{nameof(AvatarProvider)} does not support getting avatars by identifier.");
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="AvatarModel"/> instance.
        /// </summary>
        /// <param name="value">An instance of <see cref="AvatarModel"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided <see cref="AvatarModel"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// An instance of <see cref="AvatarModel"/> cannot be represented by a <see cref="string"/> identifier
        /// therefore this method is not supported.
        /// </exception>
        public override string GetValueId(AvatarModel value)
        {
            throw new NotSupportedException($"{nameof(AvatarProvider)} does not support getting avatars by identifier.");
        }

        /// <summary>
        /// Instructs the data provider to prepare the data for generation based on the
        /// provided <see cref="AvatarProviderConfiguration"/>.
        /// </summary>
        /// <param name="property">
        /// An implementation of the generic <see cref="DataGeneratorProperty{AvatarModel}"/>
        /// defining the property that this provider has been attached to.
        /// </param>
        /// <param name="rowCount">The desired number of rows to be generated.</param>
        /// <remarks>
        /// Invoked before any calculation takes place, the goal of this method is
        /// usually preparing or caching data for the generation taking place afterwards.
        /// </remarks>
        public override void Load(DataGeneratorProperty<AvatarModel> property, int rowCount)
        {
            base.Load(property, rowCount);

            if (this.configuration.ValidAccessories != null && this.configuration.ValidAccessories.Any())
            {
                this.accessories = this.configuration.ValidAccessories;
            }
            else
            {
                this.accessories = new AvatarAccessory[]
                {
                    AvatarAccessory.Blank,
                    AvatarAccessory.Kurt,
                    AvatarAccessory.Prescription01,
                    AvatarAccessory.Prescription02,
                    AvatarAccessory.Round,
                    AvatarAccessory.Sunglasses,
                    AvatarAccessory.Wayfarers
                };
            }

            if (this.configuration.ValidClotheColors != null && this.configuration.ValidClotheColors.Any())
            {
                this.clotheColors = this.configuration.ValidClotheColors;
            }
            else
            {
                this.clotheColors = new AvatarClotheColor[]
                {
                    AvatarClotheColor.Black,
                    AvatarClotheColor.Blue01,
                    AvatarClotheColor.Blue02,
                    AvatarClotheColor.Blue03,
                    AvatarClotheColor.Gray01,
                    AvatarClotheColor.Gray02,
                    AvatarClotheColor.Heather,
                    AvatarClotheColor.PastelBlue,
                    AvatarClotheColor.PastelGreen,
                    AvatarClotheColor.PastelOrange,
                    AvatarClotheColor.PastelRed,
                    AvatarClotheColor.PastelYellow,
                    AvatarClotheColor.Pink,
                    AvatarClotheColor.Red,
                    AvatarClotheColor.White
                };
            }

            if (this.configuration.ValidClotheTypes != null && this.configuration.ValidClotheTypes.Any())
            {
                this.clotheTypes = this.configuration.ValidClotheTypes;
            }
            else
            {
                this.clotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.BlazerShirt,
                    AvatarClotheType.BlazerSweater,
                    AvatarClotheType.CollarSweater,
                    AvatarClotheType.GraphicShirt,
                    AvatarClotheType.Hoodie,
                    AvatarClotheType.Overall,
                    AvatarClotheType.ShirtCrewNeck,
                    AvatarClotheType.ShirtScoopNeck,
                    AvatarClotheType.ShirtVNeck
                };
            }

            if (this.configuration.ValidClotheGraphicTypes != null && this.configuration.ValidClotheGraphicTypes.Any())
            {
                this.clotheGraphicTypes = this.configuration.ValidClotheGraphicTypes;
            }
            else
            {
                this.clotheGraphicTypes = new AvatarClotheGraphicType[]
                {
                    AvatarClotheGraphicType.Bat,
                    AvatarClotheGraphicType.Cumbia,
                    AvatarClotheGraphicType.Deer,
                    AvatarClotheGraphicType.Diamond,
                    AvatarClotheGraphicType.Hola,
                    AvatarClotheGraphicType.Pizza,
                    AvatarClotheGraphicType.Resist,
                    AvatarClotheGraphicType.Selena,
                    AvatarClotheGraphicType.Bear,
                    AvatarClotheGraphicType.SkullOutline,
                    AvatarClotheGraphicType.Skull
                };
            }

            if (this.configuration.ValidEyebrowTypes != null && this.configuration.ValidEyebrowTypes.Any())
            {
                this.eyebrowTypes = this.configuration.ValidEyebrowTypes;
            }
            else
            {
                this.eyebrowTypes = new AvatarEyebrowType[]
                {
                    AvatarEyebrowType.Angry,
                    AvatarEyebrowType.AngryNatural,
                    AvatarEyebrowType.Default,
                    AvatarEyebrowType.DefaultNatural,
                    AvatarEyebrowType.FlatNatural,
                    AvatarEyebrowType.RaisedExcited,
                    AvatarEyebrowType.RaisedExcitedNatural,
                    AvatarEyebrowType.SadConcerned,
                    AvatarEyebrowType.SadConcernedNatural,
                    AvatarEyebrowType.UnibrowNatural,
                    AvatarEyebrowType.UpDown,
                    AvatarEyebrowType.UpDownNatural
                };
            }

            if (this.configuration.ValidEyeTypes != null && this.configuration.ValidEyeTypes.Any())
            {
                this.eyeTypes = this.configuration.ValidEyeTypes;
            }
            else
            {
                this.eyeTypes = new AvatarEyeType[]
                {
                    AvatarEyeType.Close,
                    AvatarEyeType.Cry,
                    AvatarEyeType.Default,
                    AvatarEyeType.Dizzy,
                    AvatarEyeType.EyeRoll,
                    AvatarEyeType.Happy,
                    AvatarEyeType.Hearts,
                    AvatarEyeType.Side,
                    AvatarEyeType.Squint,
                    AvatarEyeType.Surprised,
                    AvatarEyeType.Wink,
                    AvatarEyeType.WinkWacky
                };
            }

            if (this.configuration.ValidFacialHairColors != null && this.configuration.ValidFacialHairColors.Any())
            {
                this.facialHairColors = this.configuration.ValidFacialHairColors;
            }
            else
            {
                this.facialHairColors = new AvatarFacialHairColor[]
                {
                    AvatarFacialHairColor.Auburn,
                    AvatarFacialHairColor.Black,
                    AvatarFacialHairColor.Blonde,
                    AvatarFacialHairColor.BlondeGolden,
                    AvatarFacialHairColor.Brown,
                    AvatarFacialHairColor.BrownDark,
                    AvatarFacialHairColor.Platinum,
                    AvatarFacialHairColor.Red
                };
            }

            if (this.configuration.ValidFacialHairTypes != null && this.configuration.ValidFacialHairTypes.Any())
            {
                this.facialHairTypes = this.configuration.ValidFacialHairTypes;
            }
            else
            {
                this.facialHairTypes = new AvatarFacialHairType[]
                {
                    AvatarFacialHairType.Blank,
                    AvatarFacialHairType.BeardMedium,
                    AvatarFacialHairType.BeardLight,
                    AvatarFacialHairType.BeardMajestic,
                    AvatarFacialHairType.MoustacheFancy,
                    AvatarFacialHairType.MoustacheMagnum
                };
            }

            if (this.configuration.ValidHairColors != null && this.configuration.ValidHairColors.Any())
            {
                this.hairColors = this.configuration.ValidHairColors;
            }
            else
            {
                this.hairColors = new AvatarHairColor[]
                {
                    AvatarHairColor.Auburn,
                    AvatarHairColor.Black,
                    AvatarHairColor.Blonde,
                    AvatarHairColor.BlondeGolden,
                    AvatarHairColor.Brown,
                    AvatarHairColor.BrownDark,
                    AvatarHairColor.PastelPink,
                    AvatarHairColor.Platinum,
                    AvatarHairColor.Red,
                    AvatarHairColor.SilverGray
                };
            }

            if (this.configuration.ValidHairColors != null && this.configuration.ValidHatColors.Any())
            {
                this.hatColors = this.configuration.ValidHatColors;
            }
            else
            {
                this.hatColors = new AvatarHatColor[]
                {
                    AvatarHatColor.Black,
                    AvatarHatColor.Blue01,
                    AvatarHatColor.Blue02,
                    AvatarHatColor.Blue03,
                    AvatarHatColor.Gray01,
                    AvatarHatColor.Gray02,
                    AvatarHatColor.Heather,
                    AvatarHatColor.PastelBlue,
                    AvatarHatColor.PastelGreen,
                    AvatarHatColor.PastelOrange,
                    AvatarHatColor.PastelRed,
                    AvatarHatColor.PastelYellow,
                    AvatarHatColor.Pink,
                    AvatarHatColor.Red,
                    AvatarHatColor.White
                };
            }

            if (this.configuration.ValidMouthTypes != null && this.configuration.ValidMouthTypes.Any())
            {
                this.mouthTypes = this.configuration.ValidMouthTypes;
            }
            else
            {
                this.mouthTypes = new AvatarMouthType[]
                {
                    AvatarMouthType.Concerned,
                    AvatarMouthType.Default,
                    AvatarMouthType.Disbelief,
                    AvatarMouthType.Eating,
                    AvatarMouthType.Grimace,
                    AvatarMouthType.Sad,
                    AvatarMouthType.ScreamOpen,
                    AvatarMouthType.Serious,
                    AvatarMouthType.Smile,
                    AvatarMouthType.Tongue,
                    AvatarMouthType.Twinkle,
                    AvatarMouthType.Vomit
                };
            }

            if (this.configuration.ValidSkinColors != null && this.configuration.ValidSkinColors.Any())
            {
                this.skinColors = this.configuration.ValidSkinColors;
            }
            else
            {
                this.skinColors = new AvatarSkinColor[]
                {
                    AvatarSkinColor.Tanned,
                    AvatarSkinColor.Yellow,
                    AvatarSkinColor.Pale,
                    AvatarSkinColor.Light,
                    AvatarSkinColor.Brown,
                    AvatarSkinColor.DarkBrown,
                    AvatarSkinColor.Black
                };
            }

            if (this.configuration.ValidTops != null && this.configuration.ValidTops.Any())
            {
                this.tops = this.configuration.ValidTops;
            }
            else
            {
                this.tops = new AvatarTop[]
                {
                    AvatarTop.NoHair,
                    AvatarTop.Eyepatch,
                    AvatarTop.Hat,
                    AvatarTop.Hijab,
                    AvatarTop.Turban,
                    AvatarTop.WinterHat1,
                    AvatarTop.WinterHat2,
                    AvatarTop.WinterHat3,
                    AvatarTop.WinterHat4,
                    AvatarTop.LongHairBigHair,
                    AvatarTop.LongHairBob,
                    AvatarTop.LongHairBun,
                    AvatarTop.LongHairCurly,
                    AvatarTop.LongHairCurvy,
                    AvatarTop.LongHairDreads,
                    AvatarTop.LongHairFrida,
                    AvatarTop.LongHairFro,
                    AvatarTop.LongHairFroBand,
                    AvatarTop.LongHairNotTooLong,
                    AvatarTop.LongHairShavedSides,
                    AvatarTop.LongHairMiaWallace,
                    AvatarTop.LongHairStraight,
                    AvatarTop.LongHairStraight2,
                    AvatarTop.LongHairStraightStrand,
                    AvatarTop.ShortHairDreads01,
                    AvatarTop.ShortHairDreads02,
                    AvatarTop.ShortHairFrizzle,
                    AvatarTop.ShortHairShaggyMullet,
                    AvatarTop.ShortHairShortCurly,
                    AvatarTop.ShortHairShortFlat,
                    AvatarTop.ShortHairShortRound,
                    AvatarTop.ShortHairShortWaved,
                    AvatarTop.ShortHairSides,
                    AvatarTop.ShortHairTheCaesar,
                    AvatarTop.ShortHairTheCaesarSidePart
                };
            }
        }
    }
}
