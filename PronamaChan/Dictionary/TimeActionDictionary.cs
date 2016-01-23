using PronamaChan.Bean;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExtensionMethods;
using PronamaChan.Utils;
using System.IO;
using System.Windows.Media.Animation;
using PronamaChan.Animation;
using System.Windows.Media.Imaging;

namespace PronamaChan.Dictionary
{

    internal class TimeActionDictionary
    {
        private VoiceDictionary mVoiceDict = new VoiceDictionary();

        /// <summary>
        /// 時報アクションを取得する.
        /// </summary>
        /// <param name="datetime">時間</param>
        /// <returns></returns>
        public TimeActionInfo Get(string dateKey)
        {

            ResourceManager resManager = Properties.Resources.ResourceManager;

            TimeActionInfo info = new TimeActionInfo();
            IniFile iniFile = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "TimeAction.ini"));

            // イメージ取得
            string imageName = iniFile.getValueString(dateKey, "Image");
            if (string.IsNullOrEmpty(imageName)) return null;
            SimpleStoryboard storyboard = GetAnimation(imageName);
            if (storyboard == null) return null;
            info.Animation = storyboard;

            // ボイス取得
            string voiceName = iniFile.getValueString(dateKey, "Voice");
            if (string.IsNullOrEmpty(voiceName)) return null;
            info.VoiceStream = resManager.GetStream(voiceName);

            // セリフ取得
            info.Serif = iniFile.getValueString(dateKey, "Serif");

            return info;
        }

        private SimpleStoryboard GetAnimation(string imageName)
        {
            SimpleStoryboard s = new SimpleStoryboard();
            s.SetFrameSpan(new TimeSpan(0, 0, 0, 500, 0));

            ResourceManager manager = Properties.Resources.ResourceManager;

            List<BitmapSource> animationImages = new List<BitmapSource>();
            animationImages.Add(manager.GetImageSource(imageName));

            s.SetAnimationImages(animationImages);
            s.Build();

            return s;
        }
    }

}
