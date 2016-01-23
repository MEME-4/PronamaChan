using PronamaChan.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace PronamaChan
{
    /// <summary>
    /// TextWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SerifWindow : Window
    {
        public SerifWindow()
        {
            InitializeComponent();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            // Alt+Tabに表示させないようにする
            var helper = new WindowInteropHelper(this);
            var exStyle = NativeUtil.GetWindowLontPtr(helper.Handle, (int)NativeUtil.GetWindowLongFields.GWL_EXSTYLE).ToInt32();
            exStyle = exStyle | (int)NativeUtil.ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            NativeUtil.SetWindowLongPtr(helper.Handle, (int)NativeUtil.GetWindowLongFields.GWL_EXSTYLE, new IntPtr(exStyle));
        }

        public void DisplayMessage(string message)
        {
            Dispatcher.Invoke(() => this.Serif.Text = message);
        }

        public void ClearMessage()
        {
            DisplayMessage(string.Empty);
        }
    }
}
