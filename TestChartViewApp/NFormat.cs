using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestChartViewApp
{
    class NFormat
    {
        public static Tuple<int, int> Get(string filename, int iMaxLength = 7)
        {
            try
            {
                int imx = 200;
                int i = 0;

                int leftStr = 0;
                int rightStr = 0;

                using (var sr = new System.IO.StreamReader(filename))
                {
                    // skip
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        if (i > imx)
                            break;

                        var line = sr.ReadLine();

                        var values = line.Split(',');
                        if (values.Length < 6)
                            continue;

                        Action<string> f = (v) => { int x = v.IndexOf("."); if (leftStr < x) leftStr = x; };
                        Action<string> g = (v) => 
                        {
                            int x = v.Length - leftStr;
                            if (rightStr < x)
                            {
                                int y = iMaxLength - (leftStr + 1);
                                if (y > x)
                                    rightStr = x;
                                else
                                    rightStr = y;
                            }
                        };

                        f(values[1]);
                        f(values[2]);
                        f(values[3]);
                        f(values[4]);

                        g(values[1]);
                        g(values[2]);
                        g(values[3]);
                        g(values[4]);

                        ++i;
                    }
                    sr.Close();
                }

                // 上記でサポートしていない 99999.99 のパターンの対応コード
                if (leftStr > 3)
                    if (rightStr < 2)
                        rightStr = 2;

                return new Tuple<int, int>(leftStr, rightStr);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
