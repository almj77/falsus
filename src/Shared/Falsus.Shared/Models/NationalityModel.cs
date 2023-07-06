namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a nationality.
    /// </summary>
    public class NationalityModel : IEquatable<NationalityModel>, IComparable<NationalityModel>
    {
        /// <summary>
        /// Gets or sets the Alpha2 ISO Country Code.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the Alpha2 ISO Country Code.
        /// </value>
        public string CountryAlpha2 { get; set; }

        /// <summary>
        /// Gets or sets the nationality demonym.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the demonym.
        /// </value>
        public string Demonym { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="NationalityModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="NationalityModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="NationalityModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(NationalityModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Demonym.CompareTo(other.Demonym);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(NationalityModel other)
        {
            return other != null && this.CountryAlpha2 == other.CountryAlpha2;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as NationalityModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Demonym.GetHashCode();
        }
    }
}
