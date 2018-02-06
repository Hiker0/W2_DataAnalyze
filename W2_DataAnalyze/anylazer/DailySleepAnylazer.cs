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
    class DailySleepAnylazer : Anylazer
    {
        struct sleep_data
        {
            public U32 timeStamp;
            public U32 status;
        };

        public struct DailySleepData
        {
            public DailySleepData(ArrayList list)
            {
                num = 0;
                dataArray = list;
            }

            public U32 num;
            public ArrayList dataArray;
            //public     sleep_data* dataArray;
        };

        struct simple_sleep_data
        {
            public U32 statTime;
            public U32 endTime;
            public U16 totalTime;
            public U16 wakeTime;
            public U16 deepTime;
            public U16 weakTime;
        };

        public String anylzeFile(string filePath)
        {
            Debug.WriteLine("read statistic");
            ArrayList list = new ArrayList();
            DailySleepData sleepData = new DailySleepData(list);
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("                  睡眠数据解析结果 \n");

            S32 ret_num = 0;
            U8[] phi_head = new U8[12];
            U32 total = 0;

            FileStream readStream = File.OpenRead(filePath);
            if (((readStream.Length - 12) % 8) != 0)
            {
                infoBuilder.AppendFormat("\n\n发现数据异常, 无法解析\n");
                readStream.Close();
                return infoBuilder.ToString();
            }

            try
            {
                ret_num = 12;
                readStream.Read(phi_head, 0, ret_num);

                S32 version = phi_head[2] + (phi_head[3] << 8);

                total = (U32)(phi_head[4] + (phi_head[5] << 8) + (phi_head[6] << 16)
                        + (phi_head[7] << 24));
                S32 sync = phi_head[8] + (phi_head[9] << 8) + (phi_head[10] << 16)
                        + (phi_head[11] << 24);
                infoBuilder.AppendFormat("校验值     {0:X2}{1:X2} \n", phi_head[0], phi_head[1]);
                infoBuilder.AppendFormat("数据版本   {0} \n", version);
                infoBuilder.AppendFormat("总条数     {0} \n", total);
                infoBuilder.AppendFormat("已同步条数 {0} \n", sync);
            }
            catch (Exception e)
            {

            }

            sleepData.num = total;

            readStream.Seek(12, SeekOrigin.Begin);

            infoBuilder.AppendFormat("\n");
            infoBuilder.AppendFormat("{0}\t{1}\t\t{2}\t{3}\n", "索引", "时间戳", "状态", "时间");
            for (U32 i = 0; i < total; i++)
            {
                U8[] phi_data = new U8[8];
                sleep_data tmp = new sleep_data();

                ret_num = 8;
                readStream.Read(phi_data, 0, ret_num);

                tmp.timeStamp = (U32)(phi_data[0] + (phi_data[1] << 8) + (phi_data[2] << 16)
                        + (phi_data[3] << 24));
                tmp.status = (U32)(phi_data[4] + (phi_data[5] << 8) + (phi_data[6] << 16)
                        + (phi_data[7] << 24));

                sleepData.dataArray.Add(tmp);

                DateTime time = Util.GetDateTimeFrom1970Ticks(tmp.timeStamp - 8 * 3600);

                infoBuilder.AppendFormat("[{0:D3}]\t{1},\t{2},\t{3}\n", i, tmp.timeStamp, tmp.status, time.ToString());
                //printf("%02x%02x%02x%02x, %02x%02x%02x%02x\n",phi_data[0],phi_data[1],phi_data[2],phi_data[3],phi_data[4],phi_data[5], phi_data[6], phi_data[7]);

            }
            readStream.Close();


            return infoBuilder.ToString();
        }
    }
}
