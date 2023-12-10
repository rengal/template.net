namespace Resto.Data
{
    public partial class ReplicationConfig : INamed
    {
        public string NameLocal { 
            get { return Name; }
            set { Name = value; }
        }

        public override string ToString()
        {
            return NameLocal;
        }
    }
}