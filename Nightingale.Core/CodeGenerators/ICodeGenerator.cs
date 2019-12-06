using Nightingale.Core.Models;
using System.Threading.Tasks;

namespace Nightingale.Core.CodeGenerators
{
    /// <summary>
    /// Interface for generating code from workspace items.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Gets the programming language 
        /// that the code generator produces.
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Generates code based on a <see cref="WorkspaceRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="WorkspaceRequest"/> to use.</param>
        /// <returns>A string representing generated code.</returns>
        Task<string> GenerateCodeAsync(WorkspaceRequest request);

        /// <summary>
        /// Generates code based on a <see cref="WorkspaceCollection"/>.
        /// </summary>
        /// <param name="request">The <see cref="WorkspaceCollection"/> to use.</param>
        /// <returns>A string representing generated code.</returns>
        Task<string> GenerateCodeAsync(WorkspaceCollection collection);
    }
}
