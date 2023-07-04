namespace Falsus.Providers.Location.Models
{
    using System;

    /// <summary>
    /// This class represents the association between coordinates and a street name.
    /// </summary>
    internal class CoordinateToStreetModel : IEquatable<CoordinateToStreetModel>, IComparable<CoordinateToStreetModel>
    {
        /// <summary>
        /// Gets the latitude of the point represented by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the latitude value.
        /// </value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets the longitude of the point represented by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the longitude value.
        /// </value>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the name of the street.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="CoordinateToStreetModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="CoordinateToStreetModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="CoordinateToStreetModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(CoordinateToStreetModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Concat(this.Latitude, ",", this.Longitude, ",", this.Name).CompareTo(string.Concat(other.Latitude, ",", other.Longitude, ",", other.Name));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CoordinateToStreetModel other)
        {
            return other != null && this.Latitude == other.Latitude && this.Longitude == other.Longitude && this.Name == other.Name;
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

            return this.Equals(obj as CoordinateToStreetModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(this.Latitude, ",", this.Longitude, ",", this.Name).GetHashCode();
        }
    }
}
