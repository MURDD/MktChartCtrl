using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ForexPAF
{
    public partial class Version : Form
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);  

        // SHGetFileInfo用のフラグ。   
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon  
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon  
        public const uint SHGFI_PIDL = 0x8; // pidl  
  

        public Version()
        {
            InitializeComponent();
        }

        private void Version_Load(object sender, EventArgs e)
        {
            // ●＜会社名＞／＜製品名＞／＜バージョン名＞

            // バージョン名（AssemblyInformationalVersion属性）を取得
            string appVersion = Application.ProductVersion;

            // 製品名（AssemblyProduct属性）を取得
            string appProductName = Application.ProductName;

            // 会社名（AssemblyCompany属性）を取得
            string appCompanyName = Application.CompanyName;


            // ●＜コピーライト＞／＜説明記述＞
            Assembly mainAssembly = Assembly.GetEntryAssembly();

            // コピーライト情報を取得
            string appCopyright = "-";
            object[] CopyrightArray = mainAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if ((CopyrightArray != null) && (CopyrightArray.Length > 0))
            {
                appCopyright =
                  ((AssemblyCopyrightAttribute)CopyrightArray[0]).Copyright;
            }

            // 詳細情報を取得
            string appDescription = "-";
            object[] DescriptionArray = mainAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if ((DescriptionArray != null) && (DescriptionArray.Length > 0))
            {
                appDescription =
                  ((AssemblyDescriptionAttribute)DescriptionArray[0]).Description;
            }

            // ●アプリケーション・アイコン
            Icon appIcon;
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hSuccess = SHGetFileInfo(mainAssembly.Location, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            if (hSuccess != IntPtr.Zero)
            {
                appIcon = Icon.FromHandle(shinfo.hIcon);
            }
            else
            {
                appIcon = SystemIcons.Application;
            }


            label1.Text = appCompanyName + " " + appProductName;
            label2.Text = "Version  " + appVersion;
            label3.Text = appCopyright;
            pictureBox1.Image = appIcon.ToBitmap();
        }
    }
}
