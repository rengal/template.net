using Resto.Data;
namespace Resto.Common.Services
{
    public class ChainPropertiesService
    {
        private bool? isAccountingOnlyInChain = null;

        public bool IsAccountingOnlyInChain()
        {
            if (isAccountingOnlyInChain == null)
            {
                isAccountingOnlyInChain =
                    ServiceClientFactory.DocumentService.IsAccountingOnlyInChain().CallSync();
            }
            return isAccountingOnlyInChain.Value;
        }
    }
}