using JeniusApps.Nightingale.Data.Models;

namespace JeniusApps.Nightingale.CodeGen
{
    /// <summary>
    /// An interface for a Nightingale
    /// code generator.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Name of code output such as
        /// csharp, curl, javascript, etc.
        /// </summary>
        string OutputName { get; }

        /// <summary>
        /// Outputs code based on the given
        /// <see cref="Item"/>.
        /// </summary>
        /// <param name="item">Either a request or a collection that will be turned to code.</param>
        /// <returns>Code that can be run to execute a request.</returns>
        string GenerateCode(Item item);
    }
}
