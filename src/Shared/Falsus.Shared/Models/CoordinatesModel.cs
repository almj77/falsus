namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a position on Earth's surface.
    /// </summary>
    public class CoordinatesModel : IEquatable<CoordinatesModel>, IComparable<CoordinatesModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinatesModel"/> class.
        /// </summary>
        /// <param name="latitude">The latitute of a point on Earth's surface.</param>
        /// <param name="longitude">The longitude of a point on Earth's surface.</param>
        public CoordinatesModel(string latitude, string longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Gets the latitude of the point represented by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the latitude value.
        /// </value>
        public string Latitude { get; private set; }

        /// <summary>
        /// Gets the longitude of the point represented by this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the longitude value.
        /// </value>
        public string Longitude { get; private set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="CoordinatesModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="CoordinatesModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="CoordinatesModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(CoordinatesModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Concat(this.Latitude, ",", this.Longitude).CompareTo(string.Concat(other.Latitude, ",", other.Longitude));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CoordinatesModel other)
        {
            return other != null &&
                this.Latitude == other.Latitude &&
                this.Longitude == other.Longitude;
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

            return this.Equals(obj as CoordinatesModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(this.Latitude, ",", this.Longitude).GetHashCode();
        }
    }
}
