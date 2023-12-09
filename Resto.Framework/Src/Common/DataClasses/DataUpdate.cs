using System;

namespace Resto.Framework.Data
{
    [DataClass("DataUpdate")]
    public abstract class DataUpdate
    {
        private Guid? serverInstanceId;
        private int revision;

        public DataUpdate() { }

        public DataUpdate(Guid? serverInstanceId, int revision)
        {
            this.serverInstanceId = serverInstanceId;
            this.revision = revision;
        }

        public Guid ServerInstanceId
        {
            get { return serverInstanceId ?? new Guid(); }
            set { serverInstanceId = value; }
        }

        public Guid? NullableServerInstanceId
        {
            get { return serverInstanceId; }
            set { serverInstanceId = value; }
        }

        public int Revision
        {
            get { return revision; }
            set { revision = value; }
        }

    }

}