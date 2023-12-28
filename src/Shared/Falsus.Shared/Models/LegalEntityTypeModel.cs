namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a type of legal entity.
    /// </summary>
    public class LegalEntityTypeModel : IEquatable<LegalEntityTypeModel>, IComparable<LegalEntityTypeModel>
    {
        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the two letter language code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the language code.
        /// </value>
        public string TwoLetterLanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the legal entity type abbreviation.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the legal entity type abbreviation.
        /// </value>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets the name of the legal entity type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the legal entity type name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="LegalEntityTypeModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="LegalEntityTypeModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="LegalEntityTypeModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(LegalEntityTypeModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Abbreviation.ToLowerInvariant().CompareTo(other.Abbreviation.ToLowerInvariant());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(LegalEntityTypeModel other)
        {
            return other != null
                && this.CountryAlpha2 == other.CountryAlpha2
                && this.TwoLetterLanguageCode == other.TwoLetterLanguageCode
                && this.Name == other.Name
                && this.Abbreviation == other.Abbreviation;
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

            return this.Equals(obj as LegalEntityTypeModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            if (!string.IsNullOrEmpty(TwoLetterLanguageCode))
            {
                return string.Concat(this.CountryAlpha2, "|", this.TwoLetterLanguageCode, "|", this.Abbreviation).GetHashCode();
            }

            return string.Concat(this.CountryAlpha2, "|", this.Abbreviation).GetHashCode();
        }
    }
}
