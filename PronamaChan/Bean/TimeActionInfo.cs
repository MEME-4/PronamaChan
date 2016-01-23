using PronamaChan.Animation;
using System.IO;
using System.Windows.Media.Animation;

namespace PronamaChan.Bean
{
    /// <summary>
    /// 時報アクション情報
    /// </summary>
    class TimeActionInfo
    {
        /// <summary>ボイスファイルURL</summary>
        internal Stream VoiceStream { get; set; }

        /// <summary>アニメーション</summary>
        internal SimpleStoryboard Animation { get; set; }

        /// <summary>セリフ</summary>
        internal string Serif { get; set; }
    }
}
