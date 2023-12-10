using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class Permission
    {
        public bool IsAllowedForCurrentUser
        {
            get
            {
                return ServerSession.IsConnected
                       && ServerSession.CurrentSession.GetCurrentUser() != null
                       && ServerSession.CurrentSession.GetCurrentUser().HasPermission(this);
            }
        }

        /// <summary>
        /// Локализованное (в зависимости от языка и режима ТП) название права.
        /// </summary>
        public string LocalName
        {
            get { return this.GetLocalName(); }
        }

        /// <summary>
        /// Возвращат true, если лицензия разрешает отображение данного права
        /// </summary>
        public bool IsAllowedByLicense
        {
            get { return true; }
        }
    }
}