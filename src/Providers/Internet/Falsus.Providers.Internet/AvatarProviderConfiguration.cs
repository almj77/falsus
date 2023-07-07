namespace Falsus.Providers.Internet
{
    using System;
    using Falsus.Shared.Avatar.Enums;

    /// <summary>
    /// This class represents the configuration object of the <see cref="AvatarProvider"/> data provider.
    /// </summary>
    public class AvatarProviderConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarProviderConfiguration"/> class.
        /// </summary>
        public AvatarProviderConfiguration()
        {
            this.ValidAccessories = Array.Empty<AvatarAccessory>();
            this.ValidClotheColors = Array.Empty<AvatarClotheColor>();
            this.ValidClotheTypes = Array.Empty<AvatarClotheType>();
            this.ValidClotheGraphicTypes = Array.Empty<AvatarClotheGraphicType>();
            this.ValidEyebrowTypes = Array.Empty<AvatarEyebrowType>();
            this.ValidEyeTypes = Array.Empty<AvatarEyeType>();
            this.ValidFacialHairColors = Array.Empty<AvatarFacialHairColor>();
            this.ValidFacialHairTypes = Array.Empty<AvatarFacialHairType>();
            this.ValidHairColors = Array.Empty<AvatarHairColor>();
            this.ValidHatColors = Array.Empty<AvatarHatColor>();
            this.ValidMouthTypes = Array.Empty<AvatarMouthType>();
            this.ValidSkinColors = Array.Empty<AvatarSkinColor>();
            this.ValidTops = Array.Empty<AvatarTop>();
        }

        /// <summary>
        /// Gets or sets the type of avatar style.
        /// </summary>
        /// <value>
        /// One of the values specified by the <see cref="AvatarStyle"/> enum.
        /// </value>
        public AvatarStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarAccessory"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarAccessory"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarAccessory"/> enum will be used.
        /// </remarks>
        public AvatarAccessory[] ValidAccessories { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarClotheColor"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarClotheColor"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarClotheColor"/> enum will be used.
        /// </remarks>
        public AvatarClotheColor[] ValidClotheColors { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarClotheType"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarClotheType"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarClotheType"/> enum will be used.
        /// </remarks>
        public AvatarClotheType[] ValidClotheTypes { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarClotheGraphicType"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarClotheGraphicType"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarClotheGraphicType"/> enum will be used.
        /// </remarks>
        public AvatarClotheGraphicType[] ValidClotheGraphicTypes { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarEyebrowType"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarEyebrowType"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarEyebrowType"/> enum will be used.
        /// </remarks>
        public AvatarEyebrowType[] ValidEyebrowTypes { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarEyeType"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarEyeType"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarEyeType"/> enum will be used.
        /// </remarks>
        public AvatarEyeType[] ValidEyeTypes { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarFacialHairColor"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarFacialHairColor"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarFacialHairColor"/> enum will be used.
        /// </remarks>
        public AvatarFacialHairColor[] ValidFacialHairColors { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarFacialHairType"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarFacialHairType"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarFacialHairType"/> enum will be used.
        /// </remarks>
        public AvatarFacialHairType[] ValidFacialHairTypes { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarHairColor"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarHairColor"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarHairColor"/> enum will be used.
        /// </remarks>
        public AvatarHairColor[] ValidHairColors { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarHatColor"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarHatColor"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarHatColor"/> enum will be used.
        /// </remarks>
        public AvatarHatColor[] ValidHatColors { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarMouthType"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarMouthType"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarMouthType"/> enum will be used.
        /// </remarks>
        public AvatarMouthType[] ValidMouthTypes { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarSkinColor"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarSkinColor"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarSkinColor"/> enum will be used.
        /// </remarks>
        public AvatarSkinColor[] ValidSkinColors { get; set; }

        /// <summary>
        /// Gets or sets the array containing the <see cref="AvatarTop"/> values
        /// to use during the data generation process.
        /// </summary>
        /// <value>
        /// An array of <see cref="AvatarTop"/> values.
        /// </value>
        /// <remarks>
        /// If null or empty, all values from the <see cref="AvatarTop"/> enum will be used.
        /// </remarks>
        public AvatarTop[] ValidTops { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarAccessory"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarAccessory"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarAccessory"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarAccessory"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarAccessory[], AvatarAccessory[]> FilterAccessories { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarClotheColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarClotheColor"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarClotheColor"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarClotheColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarClotheColor[], AvatarClotheColor[]> FilterClotheColors { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarClotheType"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarClotheType"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarClotheType"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarClotheType"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarClotheType[], AvatarClotheType[]> FilterClotheTypes { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarClotheGraphicType"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarClotheGraphicType"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarClotheGraphicType"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarClotheGraphicType"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarClotheGraphicType[], AvatarClotheGraphicType[]> FilterClotheGraphicTypes { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarEyebrowType"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarEyebrowType"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarEyebrowType"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarEyebrowType"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarEyebrowType[], AvatarEyebrowType[]> FilterEyebrowTypes { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarEyeType"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarEyeType"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarEyeType"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarEyeType"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarEyeType[], AvatarEyeType[]> FilterEyeTypes { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarFacialHairColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarFacialHairColor"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarFacialHairColor"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarFacialHairColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarFacialHairColor[], AvatarFacialHairColor[]> FilterFacialHairColors { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarFacialHairType"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarFacialHairType"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarFacialHairType"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarFacialHairType"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarFacialHairType[], AvatarFacialHairType[]> FilterFacialHairTypes { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarHairColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarHairColor"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarHairColor"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarHairColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarHairColor[], AvatarHairColor[]> FilterHairColors { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarHatColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarHatColor"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarHatColor"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarHatColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarHatColor[], AvatarHatColor[]> FilterHatColors { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarMouthType"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarMouthType"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarMouthType"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarMouthType"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarMouthType[], AvatarMouthType[]> FilterMouthTypes { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarSkinColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarSkinColor"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarSkinColor"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarSkinColor"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarSkinColor[], AvatarSkinColor[]> FilterSkinColors { get; set; }

        /// <summary>
        /// Gets or sets a function used to filter the collection of <see cref="AvatarTop"/> values
        /// based on the <see cref="DataGeneratorContext"/> during the data generation process.
        /// </summary>
        /// <value>
        /// A function that returns an array of <see cref="AvatarTop"/> and receives the <see cref="DataGeneratorContext"/>
        /// and the current array of <see cref="AvatarTop"/> values.
        /// </value>
        /// <remarks>
        /// This function can be used to apply custom logic to filter tha <see cref="AvatarTop"/> values
        /// based on the <see cref="DataGeneratorContext"/> for the current tow.
        /// </remarks>
        public Func<DataGeneratorContext, AvatarTop[], AvatarTop[]> FilterTops { get; set; }
    }
}
