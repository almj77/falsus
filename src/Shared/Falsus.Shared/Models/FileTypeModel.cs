namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class contains the details for a type of file.
    /// </summary>
    public class FileTypeModel : IEquatable<FileTypeModel>, IComparable<FileTypeModel>
    {
        /// <summary>
        /// Gets or sets the category of the file type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the category of the file type.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the file extension.
        /// </value>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the name of the file type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the name of the file type.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the base64 encoded PNG image file representing
        /// the icon of the file type.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing base64 encoded PNG image
        /// file representing the icon of the file type.
        /// </value>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this
        /// type of file is commonly used.
        /// </summary>
        /// <value>
        /// True if the type of file is commonly used, otherwise false.
        /// </value>
        public bool? IsCommon { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="FileTypeModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="FileTypeModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="FileTypeModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(FileTypeModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return string.Concat(this.Category, "|", this.Extension).CompareTo(string.Concat(other.Category, "|", other.Extension));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(FileTypeModel other)
        {
            return other != null && this.Category == other.Category && this.Extension == other.Extension;
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

            return this.Equals(obj as FileTypeModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Extension.GetHashCode();
        }
    }
}
