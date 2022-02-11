namespace Artikel_Import.src.Backend.Objects
{
    /// <summary>
    /// A <see cref="Mapping"/> contains three different Objects <see cref="Pair"/>, <see
    /// cref="Discount"/> and <see cref="CustomDictionary"/> They all have some functionality in
    /// common. In order to make sure they have those functionalities they are children of a MappingObject.
    /// </summary>
    public abstract class MappingObject
    {
        /// <summary>
        /// Returns a string that makes the object distinct from other objects of its type that
        /// belongs to the <see cref="Mapping"/>.
        /// </summary>
        /// <returns>name</returns>
        public abstract string GetName();

        /// <summary>
        /// Inserts the object into the database.
        /// </summary>
        /// <returns>Report of Success</returns>
        public abstract SqlReport Insert();

        /// <summary>
        /// Removes the object from the database.
        /// </summary>
        /// <returns>Report of Success</returns>
        public abstract SqlReport Remove();

        /// <summary>
        /// Creates a string of object values for mostly debugging purpose.
        /// </summary>
        /// <returns>string representation of the object</returns>
        public override abstract string ToString();
    }
}