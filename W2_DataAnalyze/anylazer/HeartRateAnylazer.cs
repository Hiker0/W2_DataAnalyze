using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U32 = System.UInt32;
using U8 = System.Byte;
using U16 = System.UInt16;
using S32 = System.Int32;
using System.IO;
using System.Collections;

namespace W2_DataAnalyze
{
    class HeartRateAnylazer : Anylazer
    {
        struct hr_data
        {
            public U32 timeStamp;
            public U32 status;
        };

        public String anylzeFile(string filePath)
        {
            Debug.WriteLine("readCommonData start >> \n");
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("                  心率数据解析结果 \n");

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
                infoBuilder.AppendFormat("\n\n发现数据异常, 无法继续解析...\n");
                readStream.Close();
                return infoBuilder.ToString();
            }


            readStream.Seek(12, SeekOrigin.Begin);

            infoBuilder.AppendFormat("\n");
            infoBuilder.AppendFormat("{0}\t{1}\t{2}\t\t{3}\t{4}\n", "索引", "当天索引", "时间戳", "状态", "时间");
            try
            {
                if (total > 192 * 30)
                {
                    total = 192 * 30;
                }
                for (U32 i = 0; i < total; i++)
                {
                    Debug.WriteLine("read {0}  \n", i);

                    U8[] phi_data = new U8[8];
                    hr_data tmp = new hr_data();

                    ret_num = 8;
                    readStream.Read(phi_data, 0, ret_num);

                    tmp.timeStamp = (U32)(phi_data[0] + (phi_data[1] << 8) + (phi_data[2] << 16)
                            + (phi_data[3] << 24));
                    tmp.status = (U32)(phi_data[4] + (phi_data[5] << 8) + (phi_data[6] << 16)
                            + (phi_data[7] << 24));

                    if (tmp.timeStamp != 0) { 
                        DateTime time = Util.GetDateTimeFrom1970Ticks(tmp.timeStamp - 8 * 3600);
                        U32 index = (U32)((time.Hour * 3600 + time.Minute * 60 + time.Second) / (7.5f * 60));
                        infoBuilder.AppendFormat("[{0:D3}]\t[{1:D3}]\t{2},\t{3},\t{4}\n", i, index, tmp.timeStamp, tmp.status, time.ToString());
                        //printf("%02x%02x%02x%02x, %02x%02x%02x%02x\n",phi_data[0],phi_data[1],phi_data[2],phi_data[3],phi_data[4],phi_data[5], phi_data[6], phi_data[7]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                infoBuilder.AppendFormat("\n\n发现数据异常, 无法继续解析...\n");
                readStream.Close();
                return infoBuilder.ToString();
            }
            readStream.Close();
            return infoBuilder.ToString();
        }
    }
}
