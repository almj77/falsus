namespace Falsus.Shared.Models
{
    using System;

    /// <summary>
    /// This class represents a semantic software version.
    /// </summary>
    public class SemanticVersionModel : IComparable, IComparable<SemanticVersionModel>, IEquatable<SemanticVersionModel>
    {
        /// <summary>
        /// Defines the character that represents the "Alpha" stage (lowercase a).
        /// </summary>
        public const string Alpha = "a";

        /// <summary>
        /// Defines the character that represents the "Beta" stage (lowercase b).
        /// </summary>
        public const string Beta = "b";

        /// <summary>
        /// Defines the character that represents the "Release candidate" stage (lowercase rc).
        /// </summary>
        public const string ReleaseCandidate = "rc";

        private string version;

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions such as in the form of "Major", such as "1" or "2".
        /// </remarks>
        public SemanticVersionModel(int major)
        {
            this.Major = major;
            this.version = major.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions in the form of "Major.Minor", such as "1.1" or "1.2".
        /// </remarks>
        public SemanticVersionModel(int major, int minor)
        {
            this.Major = major;
            this.Minor = minor;
            this.version = string.Concat(major, '.', minor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="stage">The release stage.
        /// Must be either "a", or "b" or "rc" in order to be interchangeable with the numeric conterparts: 0, 1 and 2 respectively.
        /// </param>
        /// <param name="stageNumber">The stage version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions in the form of "Major-Stage.StageNumber", such as "1-a.1" or "1-b.2".
        /// </remarks>
        public SemanticVersionModel(int major, string stage, int stageNumber)
        {
            ValidateStage(stage);
            this.Major = major;
            this.Stage = stage;
            this.StageNumber = stageNumber;
            this.version = string.Concat(major, '-', stage, '.', stageNumber);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="patch">The patch version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions in the form of "Major.Minor.Patch", such as "1.1.1" or "1.2.3".
        /// </remarks>
        public SemanticVersionModel(int major, int minor, int patch)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.version = string.Concat(major, '.', minor, '.', patch);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="stage">The release stage.
        /// Must be either "a", or "b" or "rc" in order to be interchangeable with the numeric conterparts: 0, 1 and 2 respectively.
        /// </param>
        /// <param name="stageNumber">The stage version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions in the form of "Major.Minor-Stage.StageNumber", such as "1.1-a.1" or "1.2-rc.1".
        /// </remarks>
        public SemanticVersionModel(int major, int minor, string stage, int stageNumber)
        {
            ValidateStage(stage);
            this.Major = major;
            this.Minor = minor;
            this.Stage = stage;
            this.StageNumber = stageNumber;
            this.version = string.Concat(major, '.', minor, '-', stage, '.', stageNumber);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="patch">The patch version number.</param>
        /// <param name="stageNumber">The stage version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions in the form of "Major.Minor.Patch.StageNumber", such as "1.1.1.1" or "1.2.1.1".
        /// </remarks>
        public SemanticVersionModel(int major, int minor, int patch, int stageNumber)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.StageNumber = stageNumber;
            this.version = string.Concat(major, '.', minor, '.', patch, '.', stageNumber);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionModel"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="patch">The patch version number.</param>
        /// <param name="stage">The release stage.
        /// Must be either "a", or "b" or "rc" in order to be interchangeable with the numeric conterparts: 0, 1 and 2 respectively.
        /// </param>
        /// <param name="stageNumber">The stage version number.</param>
        /// <remarks>
        /// Creates a <see cref="SemanticVersionModel"/> that represents versions in the form of "Major.Minor.Patch-Stage.StageNumber", such as "1.1.1-b.1" or "1.2.1-rc.2".
        /// </remarks>
        public SemanticVersionModel(int major, int minor, int patch, string stage, int stageNumber)
        {
            ValidateStage(stage);
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.Stage = stage;
            this.StageNumber = stageNumber;
            this.version = string.Concat(major, '.', minor, '.', patch, '-', stage, '.', stageNumber);
        }

        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the major version.
        /// </value>
        public int? Major { get; private set; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the minor version.
        /// </value>
        public int? Minor { get; private set; }

        /// <summary>
        /// Gets the patch version.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the patch version.
        /// </value>
        public int? Patch { get; private set; }

        /// <summary>
        /// Gets the release stage.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the release stage.
        /// </value>
        /// <remarks>
        /// Must be either "a", or "b" or "rc" in order to be interchangeable with the numeric conterparts: 0, 1 and 2 respectively.
        /// </remarks>
        public string Stage { get; private set; }

        /// <summary>
        /// Gets the release stage number.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> representing the release stage number.
        /// </value>
        public int? StageNumber { get; private set; }

        /// <summary>
        /// Indicates whether object <paramref name="a"/> is equal to object <paramref name="b"/> of the same type.
        /// </summary>
        /// <param name="a">An instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <param name="b">Another instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <returns>True if the objects are equal; otherwise, false.</returns>
        public static bool operator ==(SemanticVersionModel a, SemanticVersionModel b)
        {
            if (a is null)
            {
                return b is null;
            }

            if (b is null)
            {
                return a is null;
            }

            return a.CompareTo(b) == 0;
        }

        /// <summary>
        /// Indicates whether object <paramref name="a"/> is different from object <paramref name="b"/> of the same type.
        /// </summary>
        /// <param name="a">An instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <param name="b">Another instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <returns>True if the objects are different; otherwise, false.</returns>
        public static bool operator !=(SemanticVersionModel a, SemanticVersionModel b)
        {
            if (a is null)
            {
                return !(b is null);
            }

            if (b is null)
            {
                return !(a is null);
            }

            return a.CompareTo(b) != 0;
        }

        /// <summary>
        /// Indicates whether object <paramref name="a"/> is greater than object <paramref name="b"/> of the same type.
        /// </summary>
        /// <param name="a">An instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <param name="b">Another instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <returns>True if object <paramref name="a"/> is greater than <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator >(SemanticVersionModel a, SemanticVersionModel b)
        {
            return a.CompareTo(b) > 0;
        }

        /// <summary>
        /// Indicates whether object <paramref name="a"/> is lesser than object <paramref name="b"/> of the same type.
        /// </summary>
        /// <param name="a">An instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <param name="b">Another instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <returns>True if object <paramref name="a"/> is lesser than <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator <(SemanticVersionModel a, SemanticVersionModel b)
        {
            return a.CompareTo(b) < 0;
        }

        /// <summary>
        /// Indicates whether object <paramref name="a"/> is greater or equal to object <paramref name="b"/> of the same type.
        /// </summary>
        /// <param name="a">An instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <param name="b">Another instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <returns>True if object <paramref name="a"/> is greater or equal to <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator >=(SemanticVersionModel a, SemanticVersionModel b)
        {
            return a.CompareTo(b) >= 0;
        }

        /// <summary>
        /// Indicates whether object <paramref name="a"/> is lesser or equal to object <paramref name="b"/> of the same type.
        /// </summary>
        /// <param name="a">An instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <param name="b">Another instance of <see cref="SemanticVersionModel"/> to be compared.</param>
        /// <returns>True if object <paramref name="a"/> is lesser or equal to <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator <=(SemanticVersionModel a, SemanticVersionModel b)
        {
            return a.CompareTo(b) <= 0;
        }

        /// <summary>
        /// Compares this instance with a specified <see cref="SemanticVersionModel"/> and
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified <see cref="SemanticVersionModel"/>.
        /// </summary>
        /// <param name="other">The other <see cref="SemanticVersionModel"/> to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(SemanticVersionModel other)
        {
            int result = 0;

            if (other == null)
            {
                return 1;
            }

            result = (this.Major ?? 0).CompareTo(other.Major ?? 0);

            if (result == 0)
            {
                result = (this.Minor ?? 0).CompareTo(other.Minor ?? 0);
            }

            if (result == 0 && this.StageNumber.HasValue && other.StageNumber.HasValue)
            {
                if ((!string.IsNullOrEmpty(this.Stage) && this.Patch > 0) ||
                    (!string.IsNullOrEmpty(other.Stage) && other.Patch > 0))
                {
                    result = (this.Patch ?? 0).CompareTo(other.Patch ?? 0);
                }

                if (result == 0)
                {
                    if (!string.IsNullOrEmpty(this.Stage) && string.IsNullOrEmpty(other.Stage))
                    {
                        result = GetStageIndex(this.Stage).CompareTo(other.Patch.Value);
                    }
                    else if (string.IsNullOrEmpty(this.Stage) && !string.IsNullOrEmpty(other.Stage))
                    {
                        result = this.Patch.Value.CompareTo(GetStageIndex(other.Stage));
                    }
                    else if (!string.IsNullOrEmpty(this.Stage) && !string.IsNullOrEmpty(other.Stage))
                    {
                        result = this.Stage.CompareTo(other.Stage);
                    }

                    if (result == 0)
                    {
                        result = (this.StageNumber ?? 0).CompareTo(other.StageNumber ?? 0);
                    }
                }
            }
            else if (result == 0)
            {
                result = (this.Patch ?? 0).CompareTo(other.Patch ?? 0);

                if (result == 0)
                {
                    int thisStage = 0;
                    int otherStage = 0;

                    if (!string.IsNullOrEmpty(this.Stage))
                    {
                        thisStage = GetStageIndex(this.Stage);
                    }

                    if (!string.IsNullOrEmpty(other.Stage))
                    {
                        otherStage = GetStageIndex(other.Stage);
                    }

                    result = thisStage.CompareTo(otherStage);
                    if (result == 0)
                    {
                        result = (this.StageNumber ?? 0).CompareTo(other.StageNumber ?? 0);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Compares this instance with an object of the same type
        /// returns an integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the specified object.
        /// </summary>
        /// <param name="obj">The other object to be compared with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or
        /// appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            SemanticVersionModel otherVersion = obj as SemanticVersionModel;
            if (otherVersion != null)
            {
                return this.CompareTo(otherVersion);
            }
            else
            {
                throw new ArgumentException("Object is not a SemanticVersionModel");
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            SemanticVersionModel otherVersion = obj as SemanticVersionModel;
            if (otherVersion != null)
            {
                return this.Equals(otherVersion);
            }
            else
            {
                throw new ArgumentException("Object is not a SemanticVersionModel");
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(SemanticVersionModel other)
        {
            return this.ToString().Equals(other.ToString());
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <remarks>
        /// The returned string will be on the following format "Major.Minor.Patch-Stage.StageNumber".
        /// The components without value will be omitted.
        /// </remarks>
        public override string ToString()
        {
            return this.version;
        }

        /// <summary>
        /// Converts the string representation of a semantic version to its <see cref="SemanticVersionModel"/>.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="model">
        /// When this method returns, contains the result of successfully parsing <paramref name="value"/> or an undefined value on failure.
        /// </param>
        /// <returns>
        /// True if <paramref name="value"/> was successfully parsed; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Semantic versions should be represented in the following format: "Major.Minor.Patch-Stage.StageNumber".
        /// Please note that not all components are required, e.g. sending in "1.2" will populate the Major and Minor components
        /// while sending "1.2-a.1" will populate Major, Minor, Stage and StageNumber components.
        /// </remarks>
        public static bool TryParse(string value, out SemanticVersionModel model)
        {
            model = default;
            int? majorVersion = default;
            int? minorVersion = default;
            int? patchVersion = default;
            string stage = default;
            int? stageNumber = default;

            string mainVersion = string.Empty;
            string stageVersion = string.Empty;

            if (value.Contains("-"))
            {
                string[] versionTokens = value.Split('-');
                mainVersion = versionTokens[0];
                stageVersion = versionTokens[1];
            }
            else
            {
                mainVersion = value;
            }

            if (!string.IsNullOrEmpty(mainVersion))
            {
                string[] versionTokens = mainVersion.Split('.');
                for (int i = 0; i < versionTokens.Length; i++)
                {
                    if (int.TryParse(versionTokens[i], out int val))
                    {
                        switch (i)
                        {
                            case 0:
                                majorVersion = val;
                                break;
                            case 1:
                                minorVersion = val;
                                break;
                            case 2:
                                patchVersion = val;
                                break;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (!string.IsNullOrEmpty(stageVersion))
            {
                string[] versionTokens = stageVersion.Split('.');
                if (versionTokens.Length != 2)
                {
                    return false;
                }

                stage = versionTokens[0];

                if (int.TryParse(versionTokens[1], out int val))
                {
                    stageNumber = val;
                }
                else
                {
                    return false;
                }
            }

            if (majorVersion.HasValue)
            {
                if (minorVersion.HasValue)
                {
                    if (patchVersion.HasValue)
                    {
                        if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                        {
                            model = new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value, stage, stageNumber.Value);
                            return true;
                        }

                        if (stageNumber.HasValue)
                        {
                            model = new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value, stageNumber.Value);
                            return true;
                        }

                        model = new SemanticVersionModel(majorVersion.Value, minorVersion.Value, patchVersion.Value);
                        return true;
                    }

                    if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                    {
                        model = new SemanticVersionModel(majorVersion.Value, minorVersion.Value, stage, stageNumber.Value);
                        return true;
                    }

                    model = new SemanticVersionModel(majorVersion.Value, minorVersion.Value);
                    return true;
                }

                if (!string.IsNullOrEmpty(stage) && stageNumber.HasValue)
                {
                    model = new SemanticVersionModel(majorVersion.Value, stage, stageNumber.Value);
                    return true;
                }

                model = new SemanticVersionModel(majorVersion.Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the hash for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Validates if the provided string represents a valid release stage.
        /// </summary>
        /// <param name="stage">The string to be validated.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <paramref name="stage"/> is not one of the following: null or empty, "a", "b" or "rc".
        /// </exception>
        private static void ValidateStage(string stage)
        {
            if (string.IsNullOrEmpty(stage))
            {
                return;
            }

            if (stage != Alpha && stage != Beta && stage != ReleaseCandidate)
            {
                throw new InvalidOperationException($"Stage {stage} is not recognized. Recognized stages: {Alpha}, {Beta} and {ReleaseCandidate}.");
            }
        }

        /// <summary>
        /// Converts the provided release stage identifier to its numeric representation.
        /// </summary>
        /// <param name="stage">The string to be converted.</param>
        /// <returns>
        /// For "a" returns 0, for "b" returns 1, for "rc" returns 2; otherwise -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="stage"/> is null or empty.
        /// </exception>
        private static int GetStageIndex(string stage)
        {
            if (string.IsNullOrEmpty(stage))
            {
                throw new ArgumentNullException(nameof(stage));
            }

            switch (stage)
            {
                case Alpha:
                    return 0;
                case Beta:
                    return 1;
                case ReleaseCandidate:
                    return 2;
                default:
                    break;
            }

            return -1;
        }
    }
}
