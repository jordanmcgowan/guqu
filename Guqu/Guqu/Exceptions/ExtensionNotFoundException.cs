using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Exceptions
{
    class ExtensionNotFoundException: Exception
    {
        private string givenExtension;
        public ExtensionNotFoundException(string givenExtension)
        {
            this.givenExtension = givenExtension;
        }
        public string getAttemptedExtension()
        {
            return givenExtension;
        }
    }
}
