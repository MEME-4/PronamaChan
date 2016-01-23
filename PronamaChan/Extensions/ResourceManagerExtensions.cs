using System;
using System.Drawing;
using System.Resources;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ExtensionMethods
{
    /// <summary>
    /// ResourceManager拡張クラス
    /// </summary>
    public static class ResourceManagerExtensions
    {
        /// <summary>
        /// 画像リソースをImageSourceで取得する
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static BitmapSource GetImageSource(this ResourceManager manager, String name)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ((Bitmap)manager.GetObject(name)).GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
