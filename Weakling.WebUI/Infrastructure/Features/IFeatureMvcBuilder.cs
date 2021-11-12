using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weakling.WebUI.Infrastructure.Features
{
    /// <summary>
    /// Builder interface used to ensure core methods are invoked before optional ones.
    /// </summary>
    public interface IFeatureMvcBuilder : IMvcBuilder
    {
    }
}
