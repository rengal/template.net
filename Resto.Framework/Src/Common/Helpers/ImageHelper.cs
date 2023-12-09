using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using log4net;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Helpers
{
    ///<summary>
    /// Попмощник для класса <seealso cref="System.Drawing.Image"/>.
    ///</summary>
    public static class ImageHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileHelper));

        /// <summary>
        /// Получает картинку из строки Base64
        /// </summary>
        /// <param name="image64">Строка, содержащая данные картинки</param>
        /// <returns>Картинка</returns>
        [CanBeNull]
        public static Image FromBase64String([CanBeNull] string image64)
        {
            return !string.IsNullOrEmpty(image64)
                ? Image.FromStream(new MemoryStream(Convert.FromBase64String(image64)))
                : null;
        }

        /// <summary>
        /// Преобразует картинку в формат Base64.
        /// </summary>
        /// <param name="image">Картинка</param>
        /// <returns>Строка Base64</returns>
        [NotNull]
        public static string ToBase64String([CanBeNull] this Image image)
        {
            return image != null ? Convert.ToBase64String(image.ToBytes()) : string.Empty;
        }

        /// <summary>
        /// Преобразует картинку в формат Base64.
        /// </summary>
        /// <param name="image">Картинка</param>
        /// <param name="format">Формат в котором сохранять изображение</param>
        /// <returns>Строка Base64</returns>
        /// <remarks>Картинка преобразуется к выбранному формату и далее конвертируется в формат Base64.</remarks>
        [NotNull]
        public static string ToBase64String([CanBeNull] this Image image, ImageFormat format)
        {
            return image != null ? Convert.ToBase64String(image.ToBytes(format)) : string.Empty;
        }

        /// <summary>
        /// Преобразует изображение к масиву байт.
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <returns>Массив байт.</returns>
        public static byte[] ToBytes([NotNull] this Image image)
        {
            return ToBytes(image, image.RawFormat);
        }

        /// <summary>
        /// Преобразует изображение к масиву байт.
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="format">Формат изображения.</param>
        /// <returns>Массив байт.</returns>
        public static byte[] ToBytes([NotNull] this Image image, [NotNull] ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// По необходимости уменьшает изображение чтобы поместилось в максимальные границы
        /// </summary>
        /// <param name="image">Исходное изображение</param>
        /// <param name="maxWidth">Максимальная ширина</param>
        /// <param name="maxHeight">Максимальная высота</param>
        /// <param name="highQuality">Если true resize с сохранением высокого качества изображения</param>
        /// <returns>исходное изображение если оно помещается в границы
        /// или новая уменьшенная версия в противном случае</returns>
        [CanBeNull]
        public static Image FitTo([CanBeNull] this Image image, int maxWidth, int maxHeight, bool highQuality = false)
        {
            if (image != null)
            {
                var newHeight = image.Height;
                var newWidth = image.Width;

                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (image.Width * maxHeight) / image.Height;
                }

                if (newWidth > maxWidth)
                {
                    newHeight = (image.Height * maxWidth) / image.Width;
                    newWidth = maxWidth;
                }

                if (image.Height != newHeight || image.Width != newWidth)
                {
                    if (!highQuality)
                    {
                        image = new Bitmap(image, newWidth, newHeight);
                    }
                    else
                    {
                        Bitmap memoryImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

                        using (Graphics graphics = Graphics.FromImage(memoryImage))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.Clear(Color.Transparent);
                            graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                        }

                        image = memoryImage;
                    }
                }
            }

            return image;
        }

        public static string ConvertBitmapToBase64(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap), "The provided Bitmap object cannot be null.");

            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            var imageBytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }

        public static (Bitmap bitmap, int xPos) CreateBitmapFromBase64(string base64String, int maxWidth,
            int? maxHeight, string align, string resizeMode, bool addLeftWhitespace)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(base64String);

                using var ms = new MemoryStream(imageBytes);
                var image = Image.FromStream(ms);
                var pixelFormat = image.PixelFormat;
                if (!pixelFormat.Equals(PixelFormat.Format1bppIndexed))
                    Log.Warn(
                        $"CreateBitmapFromBase64 - Expected monochrome pixel format, but actual value is: {pixelFormat}");

                if (resizeMode == "fit")
                    image = Resize(image, maxWidth, maxHeight);
                return Clip(image, maxWidth, maxHeight, align, addLeftWhitespace);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the Image object.", ex);
            }
        }


        private static Bitmap Resize(Image image, int maxWidth, int? maxHeight)
        {
            var imgWidth = image.Width;
            var imgHeight = image.Height;

            var widthRatio = (double)maxWidth / imgWidth;
            var heightRatio = maxHeight.HasValue
                ? (double)maxHeight.Value / imgHeight
                : 1.0;
            var ratio = maxHeight.HasValue
                ? Math.Min(widthRatio, heightRatio)
                : widthRatio;

            imgWidth = (int)(imgWidth * ratio);
            imgHeight = (int)(imgHeight * ratio);

            if (imgWidth != image.Width || imgHeight != image.Height)
            {
                Log.Warn(
                    $"Image resized. Quality loss possible. Original dimensions: {image.Width}x{image.Height}. " +
                    $"Resized dimensions: {imgWidth}x{imgHeight}.");
            }

            var resizedBitmap = new Bitmap(image, imgWidth, imgHeight);
            return resizedBitmap;
        }

        private static (Bitmap bitmap, int outputPosX) Clip(Image source, int maxWidth, int? maxHeight, string align,
            bool addLeftWhitespace)
        {
            var clippedImageWidth = addLeftWhitespace
                ? source.Width >= maxWidth
                    ? maxWidth
                    : align switch
                    {
                        "left" => source.Width,
                        "right" => maxWidth,
                        _ => source.Width + (maxWidth - source.Width) / 2
                    }
                : Math.Min(source.Width, maxWidth);

            var clippedImageHeight = maxHeight.HasValue
                ? Math.Min(maxHeight.Value, source.Height)
                : source.Height;
            var clippedBitmap = new Bitmap(clippedImageWidth, clippedImageHeight);
            using var g = Graphics.FromImage(clippedBitmap);
            using var brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, 0, 0, clippedImageWidth, clippedImageHeight);

            var sourceCopyStartX = source.Width <= maxWidth
                ? 0
                : align switch
                {
                    "left" => 0,
                    "right" => source.Width - maxWidth,
                    _ => (source.Width - maxWidth) / 2
                };

            var copyWidth = Math.Min(source.Width, maxWidth);

            var destCopyStartX = addLeftWhitespace
                ? source.Width >= maxWidth
                    ? 0
                    : align switch
                    {
                        "left" => 0,
                        "right" => maxWidth - source.Width,
                        _ => (maxWidth - source.Width) / 2
                    }
                : 0;

            var outputPosX = addLeftWhitespace
                ? 0
                : source.Width >= maxWidth
                    ? 0
                    : align switch
                    {
                        "left" => 0,
                        "right" => maxWidth - source.Width,
                        _ => (maxWidth - source.Width) / 2
                    };

            var sourceArea = new Rectangle(sourceCopyStartX, 0, copyWidth, clippedImageHeight);
            var destArea = new Rectangle(destCopyStartX, 0, copyWidth, clippedImageHeight);
            g.DrawImage(source, destArea, sourceArea, GraphicsUnit.Pixel);

            if (source.Width != copyWidth || source.Height != clippedImageHeight)
                Log.Warn($"Image clipped. Original dimensions: {source.Width}x{source.Height}. " +
                         $"Clipped dimensions: {copyWidth}x{clippedImageHeight}.");

            return (clippedBitmap, outputPosX);
        }

        public static (Bitmap bitmap, int outputXPos) Clip_old(Image source, int maxWidth, string align)
        {
            Bitmap clippedBitmap;
            int outputXPos;

            if (source.Width > maxWidth)
            {
                clippedBitmap = new Bitmap(maxWidth, source.Height);
                using var g = Graphics.FromImage(clippedBitmap);
                var copyStartX = align switch
                {
                    "left" => 0,
                    "right" => source.Width - maxWidth,
                    "center" => (source.Width - maxWidth) / 2,
                    _ => throw new ArgumentException("Invalid align value")
                };

                var sourceArea = new Rectangle(copyStartX, 0, maxWidth, source.Height);
                var destArea = new Rectangle(0, 0, maxWidth, source.Height);
                g.DrawImage(source, destArea, sourceArea, GraphicsUnit.Pixel);

                outputXPos = 0;
            }
            else
            {
                // If the original image's width is within the limit, return it unchanged
                clippedBitmap = new Bitmap(source);

                outputXPos = align switch
                {
                    "left" => 0,
                    "right" => maxWidth - source.Width,
                    "center" => (maxWidth - source.Width) / 2,
                    _ => throw new ArgumentException("Invalid align value")
                };
            }

            return (clippedBitmap, outputXPos);
        }

        public static (byte[], int widthInBytes) ImageToByteArray(Bitmap img, bool inverse = false)
        {
            var widthInBytes = (int)Math.Ceiling((double)img.Width / 8);
            var byteArray = new byte[widthInBytes * img.Height];

            for (var i = 0; i < img.Height; i++)
            {
                for (var j = 0; j < widthInBytes * 8; j++)
                {
                    bool set;
                    if (j < img.Width)
                    {
                        var pixel = img.GetPixel(j, i);
                        var luma = ColorToGrey(pixel.R, pixel.G, pixel.B);
                        set = inverse ? luma <= 127 : luma > 127;
                    }
                    else
                    {
                        set = inverse;
                    }

                    if (!set)
                        continue;
                    var bitnum = i * widthInBytes * 8 + j;
                    var bytenum = bitnum / 8;
                    var bitinbyte = 7 - bitnum % 8;
                    var mask = (byte)(1 << bitinbyte);
                    byteArray[bytenum] |= mask;
                }
            }

            return (byteArray, widthInBytes);
        }


        /// <summary>
        /// Подсчитать ширину штрихкода в точках
        /// </summary>
        /// <param name="code"></param>
        /// <param name="narrowWidth">Ширина узкого бара</param>
        /// <returns></returns>
        public static int CalcEan13BarCodeWidth(string code, int narrowWidth)
        {
            return 113 * narrowWidth;
        }

        private static byte[] ToBlackByteArray(Bitmap source, int maxwidth, out int adjustedWidth)
        {
            maxwidth -= maxwidth % 8;

            adjustedWidth = source.Width % 8 == 0
                ? source.Width
                : source.Width - source.Width % 8 + 8;

            adjustedWidth = Math.Min(adjustedWidth, maxwidth);

            var byteArray = new byte[(adjustedWidth * source.Height) / 8];

            for (var i = 0; i < source.Height; i++)
            {
                for (var j = 0; j < adjustedWidth; j++)
                {
                    Color pixel = j < source.Width
                        ? source.GetPixel(j, i)
                        : Color.White;

                    var luma = ColorToGrey(pixel.R, pixel.G, pixel.B);
                    var bitnum = (i * adjustedWidth) + (j + 1) - 1;
                    var bytenum = bitnum / 8;
                    var bitinbyte = 7 - bitnum % 8;
                    var mask = (byte)(1 << bitinbyte);
                    if (luma <= 127)
                        byteArray[bytenum] |= mask;
                    else
                        byteArray[bytenum] &= (byte)~mask;
                }
            }

            return byteArray;
        }

        public static byte[] ImageToEscPos([NotNull] Image srcImg, int maxWidth)
        {
            if (srcImg == null)
                throw new ArgumentNullException(nameof(srcImg));

            using var bmp = new Bitmap(srcImg);
            var byteArray = ToBlackByteArray(bmp, maxWidth, out var adjustedWidth);
            var m = (byte)(0);
            //xL + xH*256 = ширина изображения в байтах
            var xL = (byte)((adjustedWidth / 8) % 256);
            var xH = (byte)((adjustedWidth / 8) / 256);

            //yL + yH*256 = ширина изображения в пикселях
            var yL = (byte)(srcImg.Height % 256);
            var yH = (byte)(srcImg.Height / 256);

            Debug.Assert(byteArray.Length == (xL + xH * 256) * (yL + yH * 256));

            return Enumerable.Concat(new[] { m, xL, xH, yL, yH }, byteArray).ToArray();
        }

        /// <summary>
        /// Рассчитыват размер стороны QR-кода в модулях
        /// </summary>
        public static int CalculateQRCodeModulesCount(string encodedString, char errorCorrectionLevel)
        {
            //ISO/IEC 18004. Описывает максимальный размер данных в зависимости от версии и уровня коррекции
            var qrCodeCapacity = new Dictionary<int, Dictionary<char, int>>
        {
            {1, new Dictionary<char, int> {{'L', 152}, {'M', 128}, {'Q', 104}, {'H', 72}}},
            {2, new Dictionary<char, int> {{'L', 272}, {'M', 224}, {'Q', 176}, {'H', 128}}},
            {3, new Dictionary<char, int> {{'L', 440}, {'M', 352}, {'Q', 272}, {'H', 208}}},
            {4, new Dictionary<char, int> {{'L', 640}, {'M', 512}, {'Q', 384}, {'H', 388}}},
            {5, new Dictionary<char, int> {{'L', 864}, {'M', 688}, {'Q', 496}, {'H', 368}}},
            {6, new Dictionary<char, int> {{'L', 1088}, {'M', 864}, {'Q', 608}, {'H', 480}}},
            {7, new Dictionary<char, int> {{'L', 1248}, {'M', 992}, {'Q', 704}, {'H', 528}}},
            {8, new Dictionary<char, int> {{'L', 1552}, {'M', 1232}, {'Q', 880}, {'H', 688}}},
            {9, new Dictionary<char, int> {{'L', 1856}, {'M', 1456}, {'Q', 1056}, {'H', 800}}},
            {10, new Dictionary<char, int> {{'L', 2192}, {'M', 1728}, {'Q', 1232}, {'H', 976}}},
            {11, new Dictionary<char, int> {{'L', 2592}, {'M', 2032}, {'Q', 1440}, {'H', 1120}}},
            {12, new Dictionary<char, int> {{'L', 2960}, {'M', 2320}, {'Q', 1648}, {'H', 1264}}},
            {13, new Dictionary<char, int> {{'L', 3424}, {'M', 2672}, {'Q', 1952}, {'H', 1440}}},
            {14, new Dictionary<char, int> {{'L', 3688}, {'M', 2920}, {'Q', 2088}, {'H', 1576}}},
            {15, new Dictionary<char, int> {{'L', 4184}, {'M', 3320}, {'Q', 2360}, {'H', 1784}}},
            {16, new Dictionary<char, int> {{'L', 4712}, {'M', 3624}, {'Q', 2600}, {'H', 2024}}},
            {17, new Dictionary<char, int> {{'L', 5176}, {'M', 4056}, {'Q', 2936}, {'H', 2264}}},
            {18, new Dictionary<char, int> {{'L', 5768}, {'M', 4504}, {'Q', 3176}, {'H', 2504}}},
            {19, new Dictionary<char, int> {{'L', 6360}, {'M', 5016}, {'Q', 3560}, {'H', 2728}}},
            {20, new Dictionary<char, int> {{'L', 6888}, {'M', 5352}, {'Q', 3880}, {'H', 3080}}},
            {21, new Dictionary<char, int> {{'L', 7456}, {'M', 5712}, {'Q', 4096}, {'H', 3248}}},
            {22, new Dictionary<char, int> {{'L', 8048}, {'M', 6256}, {'Q', 4544}, {'H', 3536}}},
            {23, new Dictionary<char, int> {{'L', 8752}, {'M', 6880}, {'Q', 4912}, {'H', 3712}}},
            {24, new Dictionary<char, int> {{'L', 9392}, {'M', 7312}, {'Q', 5312}, {'H', 4112}}},
            {25, new Dictionary<char, int> {{'L', 10208}, {'M', 8000}, {'Q', 57444}, {'H', 4304}}},
            {26, new Dictionary<char, int> {{'L', 10960}, {'M', 8496}, {'Q', 6032}, {'H', 4768}}}
        };

            // Сколько нужно бит чтобы закодировать входную строку
            var requiredBits = encodedString.Length * 8;

            // Находим минимально необходимую версию, чтобы закодировать входную строку
            var version = 0;
            foreach (var entry in qrCodeCapacity)
            {
                if (entry.Value.ContainsKey(errorCorrectionLevel) && entry.Value[errorCorrectionLevel] >= requiredBits)
                {
                    version = entry.Key;
                    break;
                }
            }

            if (version == 0)
            {
                throw new ArgumentException("Input string is too large for given error correction level.");
            }
            return 21 + (version - 1) * 4;
        }

        public static int ColorToGrey(byte red, byte green, byte blue)
        {
            return (int)(red * 0.3 + green * 0.59 + blue * 0.11);
        }
    }
}