using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U32 = System.UInt32;
using U8 = System.Byte;
using U16 = System.UInt16;
using S32 = System.Int32;
using System.IO;
using System.Collections;
using System.Diagnostics;
namespace W2_DataAnalyze
{
    class DailyStepAnylazer : Anylazer
    {

        public String anylzeFile(string filePath)
        {
            Debug.WriteLine("read statistic");
            ArrayList list = new ArrayList();
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("                  分时计步解析结果 \n");

            S32 ret_num = 0;
            U8[] phi_head = new U8[4];
            U32 total = 0;

            FileStream readStream = File.OpenRead(filePath);
            if (((readStream.Length - 4) % 52) != 0)
            {
                infoBuilder.AppendFormat("\n\n发现数据异常, 无法解析\n");
                readStream.Close();
                return infoBuilder.ToString();
            }

            try
            {
                ret_num = 4;
                readStream.Read(phi_head, 0, ret_num);

                total = (U32)((readStream.Length - 4) / 52);
                U32 sync = (U32)(phi_head[0] + (phi_head[1] << 8) + (phi_head[2] << 16)
                        + (phi_head[3] << 24));
                U32 syncNUm = (U32)((sync - 4) / 52);
                infoBuilder.AppendFormat("总条数: {0}\n", total);
                infoBuilder.AppendFormat("已同步: {0}\n", syncNUm);
            }
            catch (Exception e)
            {
                infoBuilder.AppendFormat("\n\n发现数据异常, 无法继续解析---\n");
                readStream.Close();
                return infoBuilder.ToString();
            }
            infoBuilder.AppendFormat("\n");
            infoBuilder.AppendFormat("\n");

            readStream.Seek(4, SeekOrigin.Begin);

            for (S32 i = 0; i < total; i++)
            {
                infoBuilder.AppendFormat("第{0}条", i);
                ret_num = 52;
                U8[] phi_data = new U8[52];
                readStream.Read(phi_data, 0, ret_num);
                U32 timeStamp = (U32)(phi_data[0] + (phi_data[1] << 8) + (phi_data[2] << 16)
                        + (phi_data[3] << 24));
                U16[] steps = new U16[24];
                for (U8 j =0; j < 24; j++)
                {
                    steps[j] = (U16)(phi_data[4 + 2 * j] + (phi_data[4 + 2 * j] << 8));
                }
                DateTime time = Util.GetDateTimeFrom1970Ticks(timeStamp - 8 * 3600);
                infoBuilder.AppendFormat("日期    :{0}\n", time.ToShortDateString());
                infoBuilder.AppendFormat("时间:\t");
                for (U8 j = 0; j < 24; j++)
                {
                    infoBuilder.AppendFormat("[{0}:00]\t", j);
                }
                infoBuilder.AppendFormat("\n");
                infoBuilder.AppendFormat("计步:\t");
                for (U8 j = 0; j < 24; j++)
                {
                    infoBuilder.AppendFormat("{0},\t", steps[j]);
                }
                infoBuilder.AppendFormat("\n");
                infoBuilder.AppendFormat("\n");
            }

            readStream.Close();


            return infoBuilder.ToString();
        }
    }
}
