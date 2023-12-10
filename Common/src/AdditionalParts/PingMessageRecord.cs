using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public partial class PingMessageRecord
    {
        private const string NullConnectionGroupText = "iikoFranchise";

        public string SourceName
        {
            get { return SourceConnectionGroup == null ? NullConnectionGroupText : SourceConnectionGroup.Name; }
        }

        public string TargetName
        {
            get { return TargetConnectionGroup == null ? NullConnectionGroupText : TargetConnectionGroup.Name; }
        }
    }
}