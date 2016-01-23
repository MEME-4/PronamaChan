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
    /// アニメーション
    /// </summary>
    class SimpleStoryboard
    {
        private Storyboard mStoryboard  = new Storyboard();

        private ObjectAnimationUsingKeyFrames mAnimation;

        // 画像切り替え間隔
        private TimeSpan mFrameSpan = TimeSpan.Zero;

        private List<BitmapSource> mImages;

        public void Build()
        {
            mStoryboard = new Storyboard();
            mAnimation = new ObjectAnimationUsingKeyFrames();

            TimeSpan nowFrame = TimeSpan.Zero;
            foreach (var i in mImages)
            {
                DiscreteObjectKeyFrame key = new DiscreteObjectKeyFrame();
                key.KeyTime = nowFrame;
                key.Value = i;

                mAnimation.KeyFrames.Add(key);

                nowFrame += mFrameSpan;
            }

            mStoryboard.Children.Add(mAnimation);
        }

        public SimpleStoryboard SetAnimationImages(List<BitmapSource> images)
        {
            mImages = images;
            return this;
        }

        public SimpleStoryboard SetNextAnimationInterval(TimeSpan timespan)
        {
            mAnimation.Duration = timespan;
            return this;
        }

        public SimpleStoryboard SetFrameSpan(TimeSpan timespan)
        {
            mFrameSpan = timespan;
            return this;
        }

        public SimpleStoryboard SetRepeatBehavior(RepeatBehavior rb)
        {
            mAnimation.RepeatBehavior = rb;
            return this;
        }

        public void Begin(FrameworkElement image)
        {
            Storyboard.SetTargetName(mAnimation, image.Name);
            Storyboard.SetTargetProperty(mAnimation, new PropertyPath("Source"));
            mStoryboard.Begin(image);
        }

        public void Resume()
        {
            mStoryboard.Resume();
        }

        public void Pause()
        {
            mStoryboard.Pause();
        }

        public void Stop()
        {
            mStoryboard.Stop();
        }

    }
}
