namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a geographic region.
    /// </summary>
    public class RegionModel : IEquatable<RegionModel>, IComparable<RegionModel>
    {
        /// <summary>
        /// Gets or sets the unique code of the region.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the region unique code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value containing the name of the region.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region category.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value containing the category of the region.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="RegionModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="RegionModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="RegionModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(RegionModel other)
        {
            if (other == null)
            {
                return -1;
            }

            string myValue = string.Concat(this.CountryAlpha2, "|", this.Code);
            string otherValue = string.Concat(other.CountryAlpha2, "|", other.Code);

            return myValue.CompareTo(otherValue);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(RegionModel other)
        {
            return other != null && this.CountryAlpha2 == other.CountryAlpha2 && this.Code == other.Code;
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

            return this.Equals(obj as RegionModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(this.CountryAlpha2, "|", this.Code).GetHashCode();
        }
    }
}
