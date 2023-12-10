using System.Drawing;
using Resto.Framework.Common.Helpers;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class OfficialEmployee
    {
        private bool signatureChanged;

        [Transient]
        private Image signature;

        /// <summary>
        /// Получает и устанавливает подпись.
        /// </summary>
        /// <value>Подпись.</value>
        /// <remarks>Используется как локальный кэш для изображения. Загружается с сервера при 
        /// необходимости.</remarks>
        public Image Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        /// <summary>
        /// Получает и устанавливает признак изменения подписи.
        /// </summary>
        /// <value>Признак изменения подписи.</value>
        /// <remarks>При сохранении на сервер признак используется как флаг необходимости обновления
        /// значения сервера.</remarks>
        public bool SignatureChanged
        {
            get { return signatureChanged; }
            set { signatureChanged = value; }
        }

        /// <summary>
        /// Загружает подпись с сервера и заносит ее в поле <c>Signature</c>.
        /// </summary>
        /// <returns>Объект с изображением подписи.</returns>
        public OfficialEmployee WithSignature()
        {
            if (signatureImage != null && !signatureChanged)
            {
                Signature = ImageHelper.FromBase64String(
                    ServiceClientFactory.ImageService.LoadImageAsBase64(signatureImage.ImageId).CallSync());
            }
            return this;
        }
    }
}