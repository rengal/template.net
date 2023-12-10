using Resto.Framework.Data;

namespace Resto.Data
{
    [DataClass("ParameterExecute")]
    public class ParameterExecute
    {
        private string name;
        private string value;

        public ParameterExecute()
        {
        }

        public ParameterExecute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}