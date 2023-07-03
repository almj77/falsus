namespace Falsus.Providers.Text.Models
{
    /// <summary>
    /// This class represents a word in a given language.
    /// </summary>
    internal class WordModel
    {
        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing one word.
        /// </value>
        public string Word { get; set; }

        /// <summary>
        /// Gets or sets the two letter ISO code of the language.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing a two letter ISO language code.
        /// </value>
        public string LanguageTwoLetterCode { get; set; }

        /// <summary>
        /// Gets or sets the type of word.
        /// </summary>
        /// <value>
        /// A value of <see cref="WordType"/>.
        /// </value>
        public WordType WordType { get; set; }
    }
}
