using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace thunderbird_run
{
    public partial class thunderbird_run : Form
    {
        //dll読込

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll",CharSet=CharSet.Auto)]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //static extern int GetWindowTextLength(IntPtr hwnd);

        String window_name;

        public thunderbird_run()
        {
            InitializeComponent();

        }

        private void Form_Load(object sender, EventArgs e)
        {
            //フォーム非表示
            this.Hide();
            //プロセス生成
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = "C:/Program Files/Mozilla Thunderbird/thunderbird.exe";
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
            //プロセススタート
            System.Diagnostics.Process hProcess = System.Diagnostics.Process.Start(psi);

            //6秒待つ→タイミングがずれた時にうまくいかない→WaitForInputIdleで解決
            //System.Threading.Thread.Sleep(6000);

            //Thunderbirdが起動するまで待つ
            hProcess.WaitForInputIdle();

            //↓↓クラス名が分からない時の処理↓↓

            //アクティブウインドウのウィンドウハンドルを取得
            //IntPtr hwnd = GetForegroundWindow();  

            //(もしくはWinspector Spyを利用してクラス名を取得)

            //クラス名の長さを取得
            //int length = GetWindowTextLength(hwnd);

            //クラス名を取得
            //StringBuilder className = new StringBuilder(length);
            //int ret = GetClassName(hwnd, className, length);

            //取得したクラス名をMesssageBoxで表示
            //MessageBox.Show(className.ToString()); 

            //↑↑クラス名が分からない時の処理↑↑3

            //ウインドウタイトルをプロセス名から取得
            //プロセス一覧からthunderbirdを検知→ウインドウタイトルを変数に格納
            foreach (System.Diagnostics.Process p in
                 System.Diagnostics.Process.GetProcesses())
            {
                if (p.MainWindowTitle.Length != 0)
                {
                    if (p.ProcessName.Contains("thunderbird"))
                    {
                        window_name = p.MainWindowTitle;
                    }

                }
            }

            //事前に調べたクラス名(MozillaWindowClass)とウインドウタイトルからウインドウハンドルを取得
            IntPtr hwnd = FindWindow("MozillaWindowClass",window_name);  

            //thunderbirdのウインドウを最小化する
            ShowWindow(hwnd, 6);

            //ウインドウハンドルを閉じる
            hProcess.Close();
            //ウインドウハンドルを破棄する
            hProcess.Dispose();
            //本プログラムを終了させる
            this.Close();
        }
    }
}
