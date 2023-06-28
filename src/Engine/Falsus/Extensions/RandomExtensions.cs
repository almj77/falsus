namespace Falsus.Extensions
{
    using System;

    /// <summary>
    /// Extension methods for the <see cref="Random"/> class.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random boolean.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        /// <returns>A random <see cref="bool"/>.</returns>
        public static bool NextBoolean(this Random random)
        {
            return random.Next() > (int.MaxValue / 2);
        }

        /// <summary>
        /// Returns a random float.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        /// <returns>A random <see cref="float"/>.</returns>
        public static float NextFloat(this Random random)
        {
            return random.NextFloat(0, float.MaxValue);
        }

        /// <summary>
        /// Returns a random float between 0 and <paramref name="max"/>.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        /// <param name="max">The exclusive upper bound of the returned float.</param>
        /// <returns>A <see cref="float"/> value.</returns>
        public static float NextFloat(this Random random, float max)
        {
            return random.NextFloat(0, max);
        }

        /// <summary>
        /// Returns a random float between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        /// <param name="min">The inclusive lower bound of the returned float.</param>
        /// <param name="max">The exclusive upper bound of the returned float.</param>
        /// <returns>A <see cref="float"/> value.</returns>
        public static float NextFloat(this Random random, float min, float max)
        {
            if (float.IsInfinity(max - min))
            {
                return (float)random.NextDouble();
            }

            double val = (random.NextDouble() * (max - min)) + min;
            return (float)val;
        }

        /// <summary>
        /// Returns a random double between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        /// <param name="min">The inclusive lower bound of the returned double.</param>
        /// <param name="max">The exclusive upper bound of the returned double.</param>
        /// <returns>A <see cref="double"/> value.</returns>
        public static double NextDouble(this Random random, double min, double max)
        {
            if (double.IsInfinity(max - min))
            {
                return random.NextDouble();
            }

            return (random.NextDouble() * (max - min)) + min;
        }

        /// <summary>
        /// Returns a random string of the specified <paramref name="length"/>.
        /// </summary>
        /// <param name="random">The pseudo-random number generator.</param>
        /// <param name="length">The number of characters of the random string.</param>
        /// <param name="includeSpecialChars">True if the generated string can include special characters.</param>
        /// <returns>A <see cref="string"/> value.</returns>
        public static string NextString(this Random random, int length, bool includeSpecialChars)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (includeSpecialChars)
            {
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789|!#$%&/()=?»-:,;:_*-+@£{[]}";
            }

            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
