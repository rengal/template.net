using System;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    /// <summary>
    /// Дополнительные данные о товаре в соответствии с законодательством Украины
    /// </summary>
    public partial class PrintTextTask
    {
        public PrintTextTask(bool success, Guid? id, Guid? deviceId, XElement document, string text)
            : this(success, id, deviceId, text)
        {
            Document = document;
        }

        [CanBeNull]
        public XElement Document { get; }
    }
}