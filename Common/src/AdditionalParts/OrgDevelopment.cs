using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class OrgDevelopment
    {
        public OrgDevelopment(OrgDevelopment orgDevelopment)
            : this(GuidGenerator.Next(), orgDevelopment.Description, orgDevelopment.Name, orgDevelopment.Parent)
        {
        }

        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.ORGDEVELOPMENT;
        }
    }
}