namespace Falsus.Shared.Avatar
{
    using System;
    using Falsus.Shared.Avatar.Enums;

    /// <summary>
    /// Describes an Avatar.
    /// </summary>
    /// <remarks>
    /// For more details see Avataar documentation https://getavataaars.com/.
    /// </remarks>
    /// <seealso cref="IEquatable{T}"/>
    /// <seealso cref="IComparable{T}"/>
    public class AvatarModel : IEquatable<AvatarModel>, IComparable<AvatarModel>
    {
        /// <summary>
        /// Gets or sets the avatar accessory.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarAccessory"/> values that indicates the accessory of the avatar.
        /// </value>
        public AvatarAccessory Accessory { get; set; }

        /// <summary>
        /// Gets or sets the color of the avatar clothes.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarClotheColor"/> values that indicates the color of the clothes.
        /// </value>
        /// <remarks>
        /// Clothe color is only applied when chosen along with one of the following <see cref="AvatarClotheType"/>:
        /// <list type="bullet">
        /// <item><see cref="AvatarClotheType.CollarSweater"/></item>
        /// <item><see cref="AvatarClotheType.GraphicShirt"/></item>
        /// <item><see cref="AvatarClotheType.Hoodie"/></item>
        /// <item><see cref="AvatarClotheType.Overall"/></item>
        /// <item><see cref="AvatarClotheType.ShirtCrewNeck"/></item>
        /// <item><see cref="AvatarClotheType.ShirtScoopNeck"/></item>
        /// <item><see cref="AvatarClotheType.ShirtVNeck"/></item>
        /// </list>
        /// </remarks>
        public AvatarClotheColor ClotheColor { get; set; }

        /// <summary>
        /// Gets or sets the type of the avatar clothes.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarClotheType"/> values that indicates the type of the clothes.
        /// </value>
        public AvatarClotheType ClotheType { get; set; }

        /// <summary>
        /// Gets or sets the graphic of the avatar clothes.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarClotheGraphicType"/> values that indicates the graphic to draw on the clothes.
        /// </value>
        /// <remarks>
        /// Clothe Graphic is only applied when chosen along with <see cref="AvatarClotheType.GraphicShirt"/>.
        /// </remarks>
        public AvatarClotheGraphicType ClotheGraphicType { get; set; }

        /// <summary>
        /// Gets or sets the type of the avatar eyebrows.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarEyebrowType"/> values that indicates the type of the eyebrows.
        /// </value>
        public AvatarEyebrowType EyebrowType { get; set; }

        /// <summary>
        /// Gets or sets the type of the avatar eyes.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarEyeType"/> values that indicates the type of the eyes.
        /// </value>
        public AvatarEyeType EyeType { get; set; }

        /// <summary>
        /// Gets or sets the color of the avatar facial hair.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarFacialHairColor"/> values that indicates the color of the facial hair.
        /// </value>
        /// <remarks>
        /// Facial hair color is only applied when chosen along any of the possible values of the <see cref="AvatarFacialHairType"/>
        /// except the <see cref="AvatarFacialHairType.Blank"/>.
        /// </remarks>
        public AvatarFacialHairColor FacialHairColor { get; set; }

        /// <summary>
        /// Gets or sets the type of the avatar facial hair.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarFacialHairType"/> values that indicates the type of facial hair.
        /// </value>
        public AvatarFacialHairType FacialHairType { get; set; }

        /// <summary>
        /// Gets or sets the color of the avatar hair.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarHairColor"/> values that indicates the color of the hair.
        /// </value>
        /// <remarks>
        /// Hair color is only applied when chosen along with one of the following <see cref="AvatarTop"/>:
        /// <list type="bullet">
        /// <item><see cref="AvatarTop.LongHairBigHair"/></item>
        /// <item><see cref="AvatarTop.LongHairBob"/></item>
        /// <item><see cref="AvatarTop.LongHairBun"/></item>
        /// <item><see cref="AvatarTop.LongHairCurly"/></item>
        /// <item><see cref="AvatarTop.LongHairCurvy"/></item>
        /// <item><see cref="AvatarTop.LongHairDreads"/></item>
        /// <item><see cref="AvatarTop.LongHairFro"/></item>
        /// <item><see cref="AvatarTop.LongHairFroBand"/></item>
        /// <item><see cref="AvatarTop.LongHairMiaWallace"/></item>
        /// <item><see cref="AvatarTop.LongHairNotTooLong"/></item>
        /// <item><see cref="AvatarTop.LongHairStraight"/></item>
        /// <item><see cref="AvatarTop.LongHairStraight2"/></item>
        /// <item><see cref="AvatarTop.LongHairStraightStrand"/></item>
        /// <item><see cref="AvatarTop.ShortHairDreads01"/></item>
        /// <item><see cref="AvatarTop.ShortHairDreads02"/></item>
        /// <item><see cref="AvatarTop.ShortHairFrizzle"/></item>
        /// <item><see cref="AvatarTop.ShortHairShaggyMullet"/></item>
        /// <item><see cref="AvatarTop.ShortHairShortCurly"/></item>
        /// <item><see cref="AvatarTop.ShortHairShortFlat"/></item>
        /// <item><see cref="AvatarTop.ShortHairShortRound"/></item>
        /// <item><see cref="AvatarTop.ShortHairShortWaved"/></item>
        /// <item><see cref="AvatarTop.ShortHairSides"/></item>
        /// <item><see cref="AvatarTop.ShortHairTheCaesar"/></item>
        /// <item><see cref="AvatarTop.ShortHairTheCaesarSidePart"/></item>
        /// </list>
        /// </remarks>
        public AvatarHairColor HairColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the avatar hat.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarHatColor"/> values that indicates the color of the hat.
        /// </value>
        /// <remarks>
        /// Hat color is only applied when chosen along with one of the following <see cref="AvatarTop"/>:
        /// <list type="bullet">
        /// <item><see cref="AvatarTop.Hijab"/></item>
        /// <item><see cref="AvatarTop.Turban"/></item>
        /// <item><see cref="AvatarTop.WinterHat1"/></item>
        /// <item><see cref="AvatarTop.WinterHat2"/></item>
        /// <item><see cref="AvatarTop.WinterHat3"/></item>
        /// <item><see cref="AvatarTop.WinterHat4"/></item>
        /// </list>
        /// </remarks>
        public AvatarHatColor HatColor { get; set; }

        /// <summary>
        /// Gets or sets the type of the avatar mouth.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarMouthType"/> values that indicates the type of the mouth.
        /// </value>
        public AvatarMouthType MouthType { get; set; }

        /// <summary>
        /// Gets or sets the avatar skin color.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarSkinColor"/> values that indicates the color of the skin.
        /// </value>
        public AvatarSkinColor SkinColor { get; set; }

        /// <summary>
        /// Gets or sets the hair/top accessory of the avatar.
        /// </summary>
        /// <value>
        /// One of the <see cref="AvatarTop"/> values that indicates the hair/top of the avatar.
        /// </value>
        public AvatarTop Top { get; set; }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position
        /// in the sort order as the other object.
        /// </summary>
        /// <param name="other">An instance of <see cref="AvatarModel"/> to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <term>Meaning</term>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>This instance precedes <paramref name="other"/> in the sort order.</term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>This instance occurs in the same position in the sort order as <paramref name="other"/>.</term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>This instance follows <paramref name="other"/> in the sort order.</term>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(AvatarModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Concat(
                this.Accessory,
                ",",
                this.ClotheColor,
                ",",
                this.ClotheGraphicType,
                ",",
                this.ClotheType,
                ",",
                this.EyebrowType,
                ",",
                this.EyeType,
                ",",
                this.FacialHairColor,
                ",",
                this.FacialHairType,
                ",",
                this.HairColor,
                ",",
                this.HatColor,
                ",",
                this.MouthType,
                ",",
                this.SkinColor,
                ",",
                this.Top)
                .CompareTo(string.Concat(
                    other.Accessory,
                    ",",
                    other.ClotheColor,
                    ",",
                    other.ClotheGraphicType,
                    ",",
                    other.ClotheType,
                    ",",
                    other.EyebrowType,
                    ",",
                    other.EyeType,
                    ",",
                    other.FacialHairColor,
                    ",",
                    other.FacialHairType,
                    ",",
                    other.HairColor,
                    ",",
                    other.HatColor,
                    ",",
                    other.MouthType,
                    ",",
                    other.SkinColor,
                    ",",
                    other.Top));
        }

        /// <summary>
        /// Indicates whether the current <see cref="AvatarModel"/> is equal to another <see cref="AvatarModel"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="AvatarModel"/> to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(AvatarModel other)
        {
            return other != null && this.Accessory == other.Accessory
                && this.ClotheColor == other.ClotheColor
                && this.ClotheGraphicType == other.ClotheGraphicType
                && this.ClotheType == other.ClotheType
                && this.EyebrowType == other.EyebrowType
                && this.EyeType == other.EyeType
                && this.FacialHairColor == other.FacialHairColor
                && this.FacialHairType == other.FacialHairType
                && this.HairColor == other.HairColor
                && this.HatColor == other.HatColor
                && this.MouthType == other.MouthType
                && this.SkinColor == other.SkinColor
                && this.Top == other.Top;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Equals(obj as AvatarModel);
        }

        /// <summary>
        /// Calculates the hash for the current instance.
        /// </summary>
        /// <returns>
        /// An hash value based on the hash of a string
        /// containing the values of all properties, provided
        /// by the default hash function.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(
                this.Accessory,
                ",",
                this.ClotheColor,
                ",",
                this.ClotheGraphicType,
                ",",
                this.ClotheType,
                ",",
                this.EyebrowType,
                ",",
                this.EyeType,
                ",",
                this.FacialHairColor,
                ",",
                this.FacialHairType,
                ",",
                this.HairColor,
                ",",
                this.HatColor,
                ",",
                this.MouthType,
                ",",
                this.SkinColor,
                ",",
                this.Top).GetHashCode();
        }
    }
}
