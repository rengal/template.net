using System;
using System.Linq;

namespace Resto.Data
{
    public partial class Notice
    {
        /// <summary>
        /// Минимальное значение которое может принимать <see cref="Importance"/>
        /// </summary>
        public const int MinImportance = -2;

        public string PreviewText
        {
            get { return string.Join(Environment.NewLine, GetSplittedMesage().Skip(1)); }
        }

        public string Title
        {
            get { return GetSplittedMesage().FirstOrDefault(); }
        }

        private string[] GetSplittedMesage()
        {
            return message.Split(new[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
