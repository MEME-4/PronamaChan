using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using ExtensionMethods;

namespace PronamaChan.Animation
{
    /// <summary>
    /// プロ生ちゃんアニメーション
    /// </summary>
    class PronamaChanAnim
    {
        // まばたきスパン
        private static readonly TimeSpan BLINK_SPAN = new TimeSpan(0, 0, 0, 7, 0);

        private Storyboard storyboard  = new Storyboard();

        private FrameworkElement mImage;

        public PronamaChanAnim(FrameworkElement image)
        {
            mImage = image;
            ResourceManager manager = Properties.Resources.ResourceManager;

            List<BitmapSource> animationImages = new List<BitmapSource>();
            animationImages.Add(manager.GetImageSource("sd_eye0"));
            animationImages.Add(manager.GetImageSource("sd_eye1"));
            animationImages.Add(manager.GetImageSource("sd_eye2"));
            animationImages.Add(manager.GetImageSource("sd_eye0"));

            storyboard = new Storyboard();
            ObjectAnimationUsingKeyFrames animation = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTargetName(animation, image.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Source"));
            //ずっとリピート
            animation.RepeatBehavior = RepeatBehavior.Forever;

            //画像切り替え間隔
            TimeSpan frameSpan = new TimeSpan(0, 0, 0, 0, 90);

            TimeSpan nowFrame = TimeSpan.Zero;
            foreach (var i in animationImages)
            {
                DiscreteObjectKeyFrame key = new DiscreteObjectKeyFrame();
                key.KeyTime = nowFrame;
                key.Value = i;

                animation.KeyFrames.Add(key);

                nowFrame += frameSpan;
            }

            animation.Duration = nowFrame + BLINK_SPAN;
            storyboard.Children.Add(animation);
        }

        public void Begin()
        {
            storyboard.Begin(mImage);
        }

        public void Resume()
        {
            storyboard.Resume();
        }

        public void Pause()
        {
            storyboard.Pause();
        }

        public void Stop()
        {
            storyboard.Stop();
        }

    }
}
