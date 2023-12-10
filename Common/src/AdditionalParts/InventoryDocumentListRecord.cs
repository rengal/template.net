using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class InventoryDocumentListRecord
    {
        public override string EmployeePassToAccountString
        {
            get
            {
                return FinedUsers.OrderBy(user => user.ToString())
                    .Select(user => user.ToString())
                    .JoinWithComma();
            }
        }

        public override Account AccountShortageData
        {
            get
            {
                return AccountShortage;
            }
        }

        public override Account AccountSurplusData
        {
            get
            {
                return AccountSurplus;
            }
        }

        public override decimal ShortageSummData
        {
            get
            {
                return shortageSum ?? 0;
            }
        }

        public override decimal SurplusSummData
        {
            get
            {
                return surplusSum ?? 0;
            }
        }

        public override bool IsAutomatic
        {
            get
            {
                return Automatic;
            }
        }
    }
}