using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ServerInstance
    {
        [NotNull]
        public static ServerInstance INSTANCE
        {
            get
            {
                IList<ServerInstance> setups = EntityManager.INSTANCE.GetAll<ServerInstance>();
                if (setups.Count == 0)
                {
                    throw new RestoException("No ServerInstance object found");
                }
                if (setups.Count > 1)
                {
                    throw new RestoException("Too many ServerInstance objects found: " + setups);
                }
                return setups[0];
            }
        }

        public DepartmentEntity Department
        {
            get
            {
                if (currentNode.Chain) throw new InvalidOperationException("Chain cannot have single department");
                return currentNode.RmsDepartment;
            }
        }
    }
}