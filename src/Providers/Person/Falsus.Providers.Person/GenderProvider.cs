namespace Falsus.Providers.Person
{
    /// <summary>
    /// Represents a provider of genders.
    /// </summary>
    /// <seealso cref="DataGeneratorProvider{T}"/>
    public class GenderProvider : GenericArrayProvider<string>
    {
        /// <summary>
        /// Gets the <see cref="string"/> containing the identifier
        /// for the Male gender.
        /// </summary>
        public const string MaleGenderIdentifier = "M";

        /// <summary>
        /// Gets the <see cref="string"/> containing the identifier
        /// for the Female gender.
        /// </summary>
        public const string FemaleGenderIdentifier = "F";

        /// <summary>
        /// An array of <see cref="string"/> containing all gender values.
        /// </summary>
        private readonly string[] values;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderProvider"/> class.
        /// </summary>
        public GenderProvider()
            : base()
        {
            this.values = new string[2] { MaleGenderIdentifier, FemaleGenderIdentifier };
        }

        /// <summary>
        /// Gets a <see cref="string"/> value based on the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An unique identifier for the object.</param>
        /// <returns>
        /// A <see cref="string"/> value with the specified unique identifier.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="id"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="id"/> argument.
        /// </remarks>
        public override string GetRowValue(string id)
        {
            return id;
        }

        /// <summary>
        /// Gets a unique identifier for the provided <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value.</param>
        /// <returns>
        /// A <see cref="string"/> containing the unique identifier of the provided value.
        /// </returns>
        /// <remarks>
        /// Since both the <paramref name="value"/> and returning type are <see cref="string"/>
        /// this method returns exactly what is passed on the <paramref name="value"/> argument.
        /// </remarks>
        public override string GetValueId(string value)
        {
            return value;
        }

        /// <summary>
        /// Gets an array of <see cref="string"/> genders
        /// representing possible values based on the context information.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="DataGeneratorContext"/> defining
        /// the current generation context.
        /// </param>
        /// <returns>
        /// An array of <see cref="string"/> values.
        /// </returns>
        protected override string[] GetValues(DataGeneratorContext context)
        {
            return this.GetValues();
        }

        /// <summary>
        /// Gets an array of <see cref="string"/> genders
        /// representing all possible values.
        /// </summary>
        /// <returns>
        /// An array of <see cref="string"/> values.
        /// </returns>
        protected override string[] GetValues()
        {
            return this.values;
        }
    }
}
