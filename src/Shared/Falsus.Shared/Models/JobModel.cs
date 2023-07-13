namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a Job/Profession.
    /// </summary>
    public class JobModel : IEquatable<JobModel>, IComparable<JobModel>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the job.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the job unique identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the job.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the job title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent job.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the parent job identifier.
        /// </value>
        public string ParentJobId { get; set; }

        /// <summary>
        /// Compares this instance with a specified <see cref="JobModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="JobModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="JobModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(JobModel other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Title.ToLowerInvariant().CompareTo(other.Title.ToLowerInvariant());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(JobModel other)
        {
            return other != null && this.Id == other.Id;
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

            return this.Equals(obj as JobModel);
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
