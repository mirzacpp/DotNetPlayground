using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studens.AspNetCore.Mvc.Middleware.RegisteredServices
{
    public class PostConfigure : IPostConfigureOptions<RegisteredServicesOptions>
    {
        void IPostConfigureOptions<RegisteredServicesOptions>.PostConfigure(string name, RegisteredServicesOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
