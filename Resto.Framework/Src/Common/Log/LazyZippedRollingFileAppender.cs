using log4net.Appender;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Log
{
    /// <summary>
    /// Создаёт файл лога при первой записи в лог, а не при конфигурации логгера.
    /// Предназначен для ведения логов по операциям, которые выполняются не у каждого клиента или не каждый день, чтобы не плодить пустые файлы логов.
    /// </summary>
    /// <remarks>
    /// Обратная сторона медали ленивого создания файла — потенциальные проблемы с созданием или блокированием файла вылезут при первом логировании.
    /// Подробнее см. https://stackoverflow.com/questions/2533403/how-to-disable-creation-of-empty-log-file-on-app-start
    /// </remarks>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
    public sealed class LazyZippedRollingFileAppender : ZippedRollingFileAppender
    {
        private bool configured;

        protected override void OpenFile(string fileName, bool append)
        {
            if (configured)
                base.OpenFile(fileName, append);
            else
                configured = true;
        }
    }
}
