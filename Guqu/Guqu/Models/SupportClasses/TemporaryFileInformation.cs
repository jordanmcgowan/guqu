using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models.SupportClasses
{
    class TemporaryFileInformation
    {
        private DateTime timeCreated;
        private string absoluteFilePath;

        public TemporaryFileInformation(DateTime timeCreated, string absoluteFilePath)
        {
            this.timeCreated = timeCreated;
            this.absoluteFilePath = absoluteFilePath;
        }

        public string AbsoluteFilePath
        {
            get
            {
                return absoluteFilePath;
            }
        }
        public DateTime TimeCreated
        {
            get
            {
                return timeCreated;
            }
        }
    }
}
