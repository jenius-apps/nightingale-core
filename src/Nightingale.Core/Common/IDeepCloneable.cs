namespace Nightingale.Core.Common
{
    /// <summary>
    /// Interface for performing a deep clone
    /// of an object.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone.</typeparam>
    public interface IDeepCloneable<T>
    {
        /// <summary>
        /// Returns a deep copy of the object.
        /// </summary>
        /// <returns>A deep copy of the object.</returns>
        T DeepClone();
    }
}
