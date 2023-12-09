using System.Drawing;

namespace Resto.Framework.GuiCommon.Helpers
{
    /// <summary>
    /// утилитка для снятия скрина
    /// </summary>
    public static class ScreenshotMaker
    {
        public static Bitmap GetCurrentBitmap(Rectangle bounds)
        {
            var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return bitmap;
        }
    }
}