using System;
using System.Collections.Generic;
using System.Text;

namespace Nightingale.Core.Models
{
    public interface IHydratable
    {
        bool IsHydrated { get; set; }
    }
}
