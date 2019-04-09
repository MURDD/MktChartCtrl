using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace ForexPAF
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public PnF PointAndFigure { get; set; }
        [DataMember]
        public Showing Showing { get; set; }
        [DataMember]
        public WindowPos WindowPosition { get; set; }
        [DataMember]
        public List<MAConfig> Indicators { get; set; }
        [DataMember]
        public PFChartColors ChartColors { get; set; }

        [IgnoreDataMember]
        const string FileName = "AppSettings.json";

        public static Settings Instance = null;

        #region json indent

        private static readonly DataContractJsonSerializerSettings jsonsettings =
            new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };

        #endregion

        #region カルチャ

        private static CultureInfo _cultureStorage = null;

        private static void PushCulture(CultureInfo culture)
        {
            if (_cultureStorage != null)
                throw new ApplicationException("PushCulture : double pushed!");
            _cultureStorage = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        private static void PopCulture()
        {
            if (_cultureStorage == null)
                throw new ApplicationException("PopCulture : double popped!");
            Thread.CurrentThread.CurrentCulture = _cultureStorage;
            _cultureStorage = null;
        }
        
        #endregion

        static string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string FilePath = System.IO.Path.Combine(Path.GetDirectoryName(appPath), FileName);

        private Settings()
        {
        }

        public static bool Load()
        {
            bool result = true;
            if (System.IO.File.Exists(FilePath))
            {
                using (var sr = new StreamReader(FilePath, Encoding.UTF8))
                {
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sr.ReadToEnd())))
                    {
                        PushCulture(CultureInfo.InvariantCulture);

                        try
                        {
                            // DataContractJsonSerializer のインスタンスを作成
                            var serializer = new DataContractJsonSerializer(typeof(Settings), jsonsettings);

                            // クラスにデータを読み込む
                            Instance = serializer.ReadObject(ms) as Settings;

                        }
                        finally
                        {
                            PopCulture();
                        }

                        ms.Close();

                    };
                    sr.Close();
                };
            }
            else
            {
                #region Create New Settings

                Instance = new Settings();

                Instance.PointAndFigure = new PnF();
                Instance.Showing = new Showing();
                Instance.WindowPosition = new WindowPos();
                Instance.Indicators = new List<MAConfig>();
                Instance.ChartColors = new PFChartColors();

                Instance.PointAndFigure.BoxPips = 5;
                Instance.PointAndFigure.RevarsalBox = 2;

                Instance.Showing.ShowLeft = false;
                Instance.Showing.BarStyle = false;
                Instance.Showing.GridLine = false;
                Instance.Showing.SimpleLine = false;
                Instance.Showing.Scaling = 3;
                Instance.Showing.Margin = 1;

                Instance.WindowPosition.Top = MainForm.Instance.Top;
                Instance.WindowPosition.Left = MainForm.Instance.Left;
                Instance.WindowPosition.Width = MainForm.Instance.Width;
                Instance.WindowPosition.Height = MainForm.Instance.Height;

                #endregion

                result = false;
            }
            return result;
        }

        public static void Save()
        {
            //第２引数をfalseにすると上書きモード、trueにすると追記モード
            using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8))
            {
                Instance.Version = Application.ProductVersion;
                
                using (var stream = new MemoryStream())
                {
                    PushCulture(CultureInfo.InvariantCulture);

                    try
                    {
                        using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
                        {
                            var serializer = new DataContractJsonSerializer(Instance.GetType(), jsonsettings);
                            serializer.WriteObject(writer, Instance);
                            writer.Flush();
                            sw.Write(Encoding.UTF8.GetString(stream.ToArray()));

                            stream.Close();
                            sw.Close();
                        }
                    }
                    finally
                    {
                        PopCulture();
                    }
                }
            }
        }
    }

    [DataContract]
    public class Showing
    {
        [DataMember]
        public bool ShowLeft { get; set; }
        [DataMember]
        public bool BarStyle { get; set; }
        [DataMember]
        public bool GridLine { get; set; }
        [DataMember]
        public bool SimpleLine { get; set; }
        [DataMember]
        public int Scaling { get; set; }
        [DataMember]
        public int Margin { get; set; }
    }

    [DataContract]
    public class PnF
    {
        [DataMember]
        public int BoxPips { get; set; }
        [DataMember]
        public int RevarsalBox { get; set; }
    }

    [DataContract]
    public class MAConfig
    {
        [DataMember]
        public int PriceType { get; set; }
        [DataMember]
        public int Method { get; set; }
        [DataMember]
        public int Period { get; set; }
        [DataMember]
        public string Color { get; set; }
    }

    [DataContract]
    public class WindowPos
    {
        [DataMember]
        public int Top { get; set; }
        [DataMember]
        public int Left { get; set; }
        [DataMember]
        public int Width { get; set; }
        [DataMember]
        public int Height { get; set; }
    }

    [DataContract]
    public class PFChartColors
    {
        [DataMember]
        public string Background { get; set; }
        [DataMember]
        public string Foreground { get; set; }
        [DataMember]
        public string Grid { get; set; }
        [DataMember]
        public string PeriodSeparators { get; set; }
        [DataMember]
        public string BullOutline { get; set; }
        [DataMember]
        public string BearOutline { get; set; }
        [DataMember]
        public string BullFill { get; set; }
        [DataMember]
        public string BearFill { get; set; }
    }
}
