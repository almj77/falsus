namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a MIME type.
    /// </summary>
    public class MimeTypeModel : IEquatable<MimeTypeModel>, IComparable<MimeTypeModel>
    {
        /// <summary>
        /// Gets or sets the MIME Type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the MIME type.
        /// </value>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the extension of the file type used along with
        /// this MIME type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the file extensions.
        /// </value>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the name of the MIME type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the name of the MIME type.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the MIME type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the category of the MIME type.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the MIME type that replaces the current MIME type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the MIME type.
        /// </value>
        public string DeprecatedBy { get; set; }

        /// <summary>
        /// Gets or sets an array of MIME Types that are aliases of the current MIME type.
        /// </summary>
        /// <value>
        /// An array of <see cref="string"/> containing the MIME type aliases for this instance.
        /// </value>
        public string[] Aliases { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this
        /// MIME type is commonly used.
        /// </summary>
        /// <value>
        /// True if the MIME type is commonly used, otherwise false.
        /// </value>
        public bool? IsCommon { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="MimeTypeModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="MimeTypeModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="MimeTypeModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(MimeTypeModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Concat(this.MimeType, "|", this.Extension).CompareTo(string.Concat(other.MimeType, "|", other.Extension));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(MimeTypeModel other)
        {
            return other != null && this.MimeType == other.MimeType && this.Extension == other.Extension;
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

            return this.Equals(obj as MimeTypeModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return string.Concat(this.MimeType, "|", this.Extension).GetHashCode();
        }
    }
}
