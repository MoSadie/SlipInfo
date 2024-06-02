using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Responses
{
    class VersionResponse
    {
        public string version;

        public VersionResponse(Version version)
        {
            this.version = version.ToString();
        }
    }
}
