using System;
using Resto.Common.Extensions;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class BudgetPlanAccountHierarchyNode : IEquatable<BudgetPlanAccountHierarchyNode> 
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((BudgetPlanAccountHierarchyNode)obj);
        }

        public bool Equals(BudgetPlanAccountHierarchyNode other)
        {
            if (other == null)
            {
                return false;
            }

            return BudgetPlanAccountId.GetValueOrFakeDefault() == other.BudgetPlanAccountId.GetValueOrFakeDefault() && Type == other.Type;
        }

        public override int GetHashCode()
        {
            return new { BudgetPlanAccountId = BudgetPlanAccountId.GetValueOrFakeDefault(), Type }.GetHashCode();
        }

        public BudgetPlanItemAccount BudgetAccount
        {
            get { return EntityManager.INSTANCE.Get<BudgetPlanItemAccount>(BudgetPlanAccountId.GetValueOrFakeDefault()); }
        }

        public Guid GroupId
        {
            get { return BudgetAccount.SyntheticId.GetValueOrFakeDefault(); }
        }

        public Guid AccountId
        {
            get { return Type == BudgetPlanNodeType.ACCOUNT ? BudgetAccount.Account.Id : GroupId; }
        }
    }
}