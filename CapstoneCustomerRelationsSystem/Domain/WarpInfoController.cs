using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CapstoneCustomerRelationsSystem.Domain.Models;
using CapstoneCustomerRelationsSystem.TechnicalServices;

namespace CapstoneCustomerRelationsSystem.Domain
{
    public class CapstoneInfoController
    {
        CapstoneInfoManager CapstoneManager = new CapstoneInfoManager();

        public CapstoneInfo RetrieveCapstoneInfo(string _CapstoneStoreName)
        {
            CapstoneInfo CapstoneInformation = new CapstoneInfo();

            CapstoneInformation = CapstoneManager.GetCapstoneInfo(_CapstoneStoreName);

            return CapstoneInformation;
        }

        public bool ModifyCapstoneInfo(CapstoneInfo _CapstoneInfo)
        {
            bool Confirmation;

            Confirmation = CapstoneManager.UpdateCapstoneInfo(_CapstoneInfo);

            return Confirmation;
        }
    }
}
