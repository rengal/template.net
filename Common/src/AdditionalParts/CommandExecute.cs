using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Data
{
    [DataClass("CommandExecute")]
    public class CommandExecute
    {
        private string name;
        private List<ParameterExecute> parameters;

        public CommandExecute()
        {
        }

        public CommandExecute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<ParameterExecute> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public override string ToString()
        {
            string paramsString = Parameters != null && Parameters.Count > 0 ? string.Join(", ", Parameters) : string.Empty;
            string result = name;

            if (paramsString != string.Empty)
                result += $" ({paramsString})";

            return result;
        }
    }
}
