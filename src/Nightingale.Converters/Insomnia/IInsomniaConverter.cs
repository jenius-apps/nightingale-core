using INS = Insomnia.NET.Models;
using JeniusApps.Nightingale.Data.Models;
using System.Collections.Generic;

namespace JeniusApps.Nightingale.Converters.Insomnia
{
    /// <summary>
    /// Interface for converting an insomnia export file
    /// to a <see cref="Workspace"/>.
    /// </summary>
    public interface IInsomniaConverter
    {
        /// <summary>
        /// Converts a <see cref="INS.ExportDoc"/>
        /// into a list of workspaces.
        /// </summary>
        /// <param name="exportFile">The <see cref="INS.ExportDoc"/> to convert.</param>
        /// <returns>List of workspaces.</returns>
        IList<Workspace> Convert(INS.ExportDoc exportFile);
    }
}
