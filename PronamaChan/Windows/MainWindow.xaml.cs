using PronamaChan.Animation;
using PronamaChan.Bean;
using PronamaChan.Dictionary;
using PronamaChan.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WMPLib;

namespace PronamaChan
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        // セリフ表示時間
        private static readonly double SERIF_DISPLAY_SPAN = 10.0;

        // セリフ画面
        private SerifWindow mSerifWindow = new SerifWindow();

        // 時報用監視タイマー
        private DispatcherTimer mOvserbeTimer;

        // 現在時刻
        private string mCurrentTime;

        // プロ生ちゃんアニメーション
        private PronamaChanAnim mPronamaChanAnim;

        public MainWindow()
        {
            InitializeComponent();
            mPronamaChanAnim = new PronamaChanAnim(ImgPronama);

            ShowLicense();
        }

        /// <summary>
        /// ライセンスを表示する
        /// </summary>
        private void ShowLicense()
        {
            IniFile ini = new IniFile(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Config", "System.ini"));
            if (!string.Equals(ini.getValueString("Setup", "DisplayedAnnounce"), "1")) {
                MessageBox.Show("暮井 慧の音声は、許諾を得て利用しています。\n" +
                    "当アプリ以外での利用（音声の抽出、加工、公開などの行為）を禁止します。");
                ini.setValue("Setup", "DisplayedAnnounce", "1");
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // プロ生ちゃんを右下に配置
            this.Left = System.Windows.SystemParameters.WorkArea.Width - this.Width;
            this.Top = System.Windows.SystemParameters.WorkArea.Height - this.Height;

            // 最前面に表示
            mSerifWindow.Topmost = true;
            this.Topmost = true;

            // セリフウィンドウ表示
            mSerifWindow.Show();
            mSerifWindow.Visibility = Visibility.Hidden;

            // セリフウィンドウ位置調整
            Point pronamaImgPoint = ImgPronama.PointToScreen(new Point());
            mSerifWindow.Left = this.Left - mSerifWindow.Width;
            mSerifWindow.Top = this.Top + this.Height / 4;

            // 時報用監視タイマー構築
            BuildTimer();

            // プロ生ちゃん始動！！！
            StartPronamaChan();
        }

        /// <summary>
        /// プロ生ちゃん起動アニメーション開始
        /// </summary>
        private void StartPronamaChan()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3.0);
            timer.Tick += (sender, e) => {
                timer.Stop();
                // まばたきアニメーション開始
                mPronamaChanAnim.Begin();
            };
            timer.Start();

            // じゃじゃーん！！！！！！
            SoundPlayer player = new SoundPlayer(Properties.Resources.kei_voice_031_1);
            player.Play();

        }

        /// <summary>
        /// 時報用監視タイマー構築 (1秒ごとにハンドリング)
        /// </summary>
        private void BuildTimer()
        {
            mOvserbeTimer = new DispatcherTimer();
            mOvserbeTimer.Interval = TimeSpan.FromSeconds(1.0);
            mOvserbeTimer.Tick += OnTimerTick;
            mOvserbeTimer.Start();
        }

        /// <summary>
        /// 時報用監視タイマー構築 (1秒ごとにハンドリング)
        /// </summary>
        private void HideSerifWindow()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(SERIF_DISPLAY_SPAN);
            timer.Tick += (sender, e) => {
                    // セリフ非表示
                    mSerifWindow.Visibility = Visibility.Hidden;
                    // まばたき再開
                    mPronamaChanAnim.Begin();
                };
            timer.Start();
        }

        /// <summary>
        /// 時報アクションイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimerTick(object sender, EventArgs e)
        {
            // 時刻を取得(キーとしても使う)
            String timeKey = DateTime.Now.ToString("HH:mm");
            
            // 現在時刻と保持している時間が違えばイベント未処理として扱う
            if (string.Equals(mCurrentTime, timeKey)) return;
            mCurrentTime = timeKey;

            // 時報アクション取得
            TimeActionDictionary dict = new TimeActionDictionary();
            TimeActionInfo info = dict.Get(timeKey);

            if (info == null) return;

            // セリフ枠表示
            mSerifWindow.Visibility = Visibility.Visible;

            // ボイス再生
            if (info.VoiceStream != null)
            {
                SoundPlayer player = new SoundPlayer(info.VoiceStream);
                player.Play();
            }

            // 画像
            if (info.Animation != null)
            {
                // まばたき一時停止
                mPronamaChanAnim.Stop();
                // 画像表示
                info.Animation.Begin(ImgPronama);
            }

            // セリフ表示
            if (info.Serif != null)
            {
                mSerifWindow.DisplayMessage(info.Serif);
            }

            // セリフ非表示
            HideSerifWindow();

        }

        /// <summary>
        /// 終了ボタンイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickQuitMenuItem(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool mOnDrag = false;
        private Point mDragStart;

        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Character_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mOnDrag = true;
            mDragStart = e.GetPosition(this);
        }

        /// <summary>
        /// ドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Character_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mOnDrag = false;
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Character_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mOnDrag) return;
            Point p = Mouse.GetPosition(this);
            this.Left+= p.X - mDragStart.X;
            this.Top += p.Y - mDragStart.Y;
            mSerifWindow.Left += p.X - mDragStart.X;
            mSerifWindow.Top += p.Y - mDragStart.Y;

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (mPronamaChanAnim != null) mPronamaChanAnim.Stop();
            mPronamaChanAnim = null;

            if (mSerifWindow != null) mSerifWindow.Close();
            mSerifWindow = null;

            base.OnClosing(e);
        }

        /// <summary>
        /// 最前面メニューチェック時
        /// </summary>
        private void TopMostMenu_Checked(object sender, RoutedEventArgs e)
        {
            mSerifWindow.Topmost = true;
            this.Topmost = true;
        }

        /// <summary>
        /// 最前面メニューチェック解除時
        /// </summary>
        private void TopMostMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            mSerifWindow.Topmost = false;
            this.Topmost = false;
        }
    }
}
