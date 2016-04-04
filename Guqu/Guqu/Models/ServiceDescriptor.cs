using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    class ServiceDescriptor
    {
        private CommonDescriptor cd;
        public ServiceDescriptor(CommonDescriptor cd)
        {
            this.cd = cd;
        }
        public ServiceDescriptor(CommonDescriptor cd, Dictionary<string, string> requestHeaders)
        {
            this.cd = cd;
        }
        public Dictionary<string, string> getRequestHeaders()
        {
            //todo: itr2
            return null;
        }
        public CommonDescriptor getCommonDescriptor()
        {
            return cd;
        }
    }
}
