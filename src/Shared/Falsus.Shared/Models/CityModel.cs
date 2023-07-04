namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a geographic city.
    /// </summary>
    public class CityModel : IEquatable<CityModel>, IComparable<CityModel>
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the name of the city.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ISO Alpha2 unique identifier of the country.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO country code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the region.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the city region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="CityModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="CityModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="CityModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(CityModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CityModel other)
        {
            return other != null && this.CountryAlpha2 == other.CountryAlpha2 && this.Name == other.Name;
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

            return this.Equals(obj as CityModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(this.CountryAlpha2, "|", this.Name).GetHashCode();
        }
    }
}
