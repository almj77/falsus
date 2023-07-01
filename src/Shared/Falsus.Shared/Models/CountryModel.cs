namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a geographic country.
    /// </summary>
    public class CountryModel : IEquatable<CountryModel>, IComparable<CountryModel>
    {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the country name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string Alpha2 { get; set; }

        /// <summary>
        /// Gets or sets the Alpha3 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha3 ISO Country Code.
        /// </value>
        public string Alpha3 { get; set; }

        /// <summary>
        /// Gets or sets the Numeric Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the numeric country code.
        /// </value>
        public string Numeric { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="CountryModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="CountryModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="CountryModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(CountryModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return Alpha2.CompareTo(other.Alpha2);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CountryModel other)
        {
            return other != null && Alpha2 == other.Alpha2;
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

            return Equals(obj as CountryModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return Alpha2.GetHashCode();
        }
    }
}