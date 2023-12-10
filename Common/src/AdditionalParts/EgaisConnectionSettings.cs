using System.Collections.Generic;
using Resto.Common.Extensions;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class EgaisConnectionSettings
    {
        #region Fields

        [Transient]
        private bool? correctConnection;

        [Transient]
        private string testCorrectionErrorMessage;

        [Transient]
        private bool duplicatedFsRarId;

        [Transient]
        private bool duplicatedUrl;

        [Transient]
        private bool notLicensedFsRarId;

        [Transient]
        private bool isTerminalChanged;

        [Transient]
        private List<object> allowedTerminalsInList;

        #endregion

        #region CTOR

        public EgaisConnectionSettings(EgaisConnectionSettings settings)
            : this(settings.name, settings.fsRarId, settings.versionTtn, settings.versionTtnNew, settings.versionTtnStatus, settings.url, settings.terminalId, settings.terminalName)
        {
            allowedTerminals = settings.allowedTerminals;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Признак корректности настроек. Поддерживает 3 состояния:
        /// 1. null: проверка корректности не проводилась
        /// 2. true: настройки корректны
        /// 3. false: настройки НЕ корректны
        /// </summary>
        public bool? CorrectConnection
        {
            get { return correctConnection; }
            set { correctConnection = value; }
        }

        /// <summary>
        /// Содержит сообщение об ошибке после проверки корректности настроек
        /// </summary>
        public string TestCorrectionErrorMessage
        {
            get { return testCorrectionErrorMessage; }
            set { testCorrectionErrorMessage = value; }
        }

        /// <summary>
        /// Признак того что УТМ с таким РАР-идентификатором уже зарегистрирован в системе
        /// </summary>
        public bool DuplicatedFsRarId
        {
            get { return duplicatedFsRarId; }
            set { duplicatedFsRarId = value; }
        }

        /// <summary>
        /// Признак того что УТМ с таким Url уже зарегистрирован в системе
        /// </summary>
        public bool DuplicatedUrl
        {
            get { return duplicatedUrl; }
            set { duplicatedUrl = value; }
        }

        /// <summary>
        /// Признак того что УТМ с таким РАР-идентификатором не лицензирован
        /// </summary>
        public bool NotLicensedFsRarId
        {
            get { return notLicensedFsRarId; }
            set { notLicensedFsRarId = value; }
        }

        /// <summary>
        /// Признак того что настройки прокси изменились
        /// </summary>
        public bool IsTerminalChanged
        {
            get { return isTerminalChanged; }
        }

        /// <summary>
        /// Терминалы, для которых утм является видимым, находящиеся в выпадающем списке
        /// </summary>
        /// <remarks>
        /// Советуют использовать именно object
        /// https://www.devexpress.com/Support/Center/Question/Details/T422646/binding-checkedcomboboxedit-to-a-bindinglist-string
        /// </remarks>
        public List<object> AllowedTerminalsInList
        {
            get { return allowedTerminalsInList; }
            set { allowedTerminalsInList = value; }
        }

        public Terminal Terminal
        {
            get
            {
                if (terminalId == null)
                {
                    return null;
                }

                if (EntityManager.INSTANCE.Contains<Terminal>(TerminalId.GetValueOrFakeDefault()))
                {
                    return EntityManager.INSTANCE.Get<Terminal>(TerminalId.GetValueOrFakeDefault());
                }

                return new Terminal()
                {
                    Id = TerminalId.GetValueOrFakeDefault(),
                    ComputerName = TerminalName
                };
            }
            set
            {
                if (value == null)
                {
                    terminalId = null;
                    terminalName = null;
                }
                else
                {
                    terminalId = value.Id;
                    terminalName = value.ComputerName;
                }
                isTerminalChanged = true;
            }
        }

        #endregion
    }
}
