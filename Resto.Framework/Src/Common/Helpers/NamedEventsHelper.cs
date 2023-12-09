using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Resto.Framework.Common
{
    public static class NamedEventHelper
    {
        #region Methods

        private static void SetFullAccessRights(EventWaitHandle eventWaitHandle)
        {
            var eventSecurity = new EventWaitHandleSecurity();

            var sidOwner = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);
            var sidAuthUser = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            var rule = new EventWaitHandleAccessRule(
                sidOwner,
                EventWaitHandleRights.FullControl,
                AccessControlType.Allow);

            eventSecurity.AddAccessRule(rule);

            rule = new EventWaitHandleAccessRule(
                sidAuthUser,
                EventWaitHandleRights.FullControl,
                AccessControlType.Allow);

            eventSecurity.AddAccessRule(rule);

            eventWaitHandle.SetAccessControl(eventSecurity);
        }

        public static EventWaitHandle OpenExisting(string name, bool reset)
        {
            try
            {
                var result = EventWaitHandle.OpenExisting(
                    Win32KernelObjectsHelper.GetKernelObjectName(name, true),
                    EventWaitHandleRights.FullControl);
                if (reset)
                {
                    result.Reset();
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static EventWaitHandle CreateNew(string name)
        {
            try
            {
                var result = new EventWaitHandle(
                    false, 
                    EventResetMode.AutoReset, 
                    Win32KernelObjectsHelper.GetKernelObjectName(name, true));
                SetFullAccessRights(result);
                return result;
            }
            catch
            {
                return null;                
            }
        }

        public static EventWaitHandle OpenExistingOrCreateNew(string name, bool resetIfExist)
        {
            return OpenExisting(name, resetIfExist) ?? CreateNew(name);
        }

        #endregion
    }
}