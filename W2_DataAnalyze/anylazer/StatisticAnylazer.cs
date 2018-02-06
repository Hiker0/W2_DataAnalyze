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
    class StatisticAnylazer : Anylazer
    {

        struct daily_statistic_store_t
        {
            public U32 timestamp;       //时间戳
            public U8 maxHr;            //最大心率
            public U8 minHr;            //最小心率
            public U8 averageHr;        //平均心率
            public U8 restHr;           //平均静息心率
            public U8 morningHr;        //晨脉
            public U8 recent;             //方便字节对齐, 未用

            public U32 sleepStartTime;       //时间戳
            public U32 sleepEndTime;       //时间戳
            public U16 totalSleep;       //全天睡眠 min
            public U16 weakSleep;        //浅睡时间 min
            public U16 deepSleep;        //深睡时间 min
            public U16 wakeTime;         //深睡时间 min

            public U32 stepCount;        //全天步数
            public U32 distance;         //全天距离    /m
            public U32 calorie;          //全天卡路里  /cal
        };

        public String anylzeFile(string filePath)
        {
            Debug.WriteLine("read statistic");
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("                                统计信息解析结果 \n");

            S32 ret_num = 0;
            byte[] phi_head = new byte[12];
            S32 total = 0;

            FileStream readStream = File.OpenRead(filePath);
            if (((readStream.Length - 12) % 40) != 0)
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

                total = phi_head[4] + (phi_head[5] << 8) + (phi_head[6] << 16)
                        + (phi_head[7] << 24);
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

            readStream.Seek(12, SeekOrigin.Begin);

            daily_statistic_store_t statisticData = new daily_statistic_store_t();

            for (U32 i = 0; i < total; i++)
            {
                infoBuilder.AppendFormat("-----------第 {0} 条----------------\n", i);

                byte[] phi_data = new byte[40];
                ret_num = 40;
                readStream.Read(phi_data, 0, ret_num);

                statisticData.timestamp = (U32)(phi_data[0] + (phi_data[1] << 8) + (phi_data[2] << 16) + (phi_data[3] << 24));
                statisticData.maxHr = (U8)phi_data[4];
                statisticData.minHr = (U8)phi_data[5];
                statisticData.averageHr = (U8)phi_data[6];
                statisticData.restHr = (U8)phi_data[7];
                statisticData.morningHr = (U8)phi_data[8];
                statisticData.recent = (U8)phi_data[9];

                statisticData.sleepStartTime = (U32)(phi_data[12] + (phi_data[13] << 8) + (phi_data[14] << 16) + (phi_data[15] << 24));
                statisticData.sleepEndTime = (U32)(phi_data[16] + (phi_data[17] << 8) + (phi_data[18] << 16) + (phi_data[19] << 24));
                statisticData.totalSleep = (U16)(phi_data[20] + (phi_data[21] << 8));
                statisticData.weakSleep = (U16)(phi_data[22] + (phi_data[23] << 8));
                statisticData.deepSleep = (U16)(phi_data[24] + (phi_data[25] << 8));
                statisticData.wakeTime = (U16)(phi_data[26] + (phi_data[27] << 8));

                statisticData.stepCount = (U32)(phi_data[28] + (phi_data[29] << 8) + (phi_data[30] << 16) + (phi_data[31] << 24));
                statisticData.distance = (U32)(phi_data[32] + (phi_data[33] << 8) + (phi_data[34] << 16) + (phi_data[35] << 24));
                statisticData.calorie = (U32)(phi_data[36] + (phi_data[37] << 8) + (phi_data[38] << 16) + (phi_data[39] << 24));

                DateTime time = Util.GetDateTimeFrom1970Ticks(statisticData.timestamp - 8 * 3600);

                infoBuilder.AppendFormat("日期     : {0}\n", time.ToShortDateString());
                infoBuilder.AppendFormat("最大心率 : {0}\n", statisticData.maxHr);
                infoBuilder.AppendFormat("最小心率 : {0}\n", statisticData.minHr);
                infoBuilder.AppendFormat("平均心率 : {0}\n", statisticData.restHr);
                infoBuilder.AppendFormat("静息心率 : {0}\n", statisticData.restHr);
                infoBuilder.AppendFormat("晨脉     : {0}\n", statisticData.morningHr);

                infoBuilder.AppendFormat("\n\n");
                if (0 != statisticData.sleepStartTime)
                {
                    time = Util.GetDateTimeFrom1970Ticks(statisticData.sleepStartTime - 8 * 3600);
                    infoBuilder.AppendFormat("睡眠开始时间: {0} \n", time.ToString());
                }

                if (0 != statisticData.sleepEndTime)
                {
                    time = Util.GetDateTimeFrom1970Ticks(statisticData.sleepEndTime - 8 * 3600);
                    infoBuilder.AppendFormat("睡眠结束时间: {0} \n", time.ToString());
                }

                infoBuilder.AppendFormat("总睡眠时间    {0:D2}: {1:D2}: {2:D2}\n", statisticData.totalSleep / 3600, (statisticData.totalSleep % 3600) / 60, statisticData.totalSleep % 60);
                infoBuilder.AppendFormat("浅睡时间      {0:D2}: {1:D2}: {2:D2}\n", statisticData.weakSleep / 3600, (statisticData.weakSleep % 3600) / 60, statisticData.weakSleep % 60);
                infoBuilder.AppendFormat("深睡时间      {0:D2}: {1:D2}: {2:D2}\n", statisticData.deepSleep / 3600, (statisticData.deepSleep % 3600) / 60, statisticData.deepSleep % 60);
                infoBuilder.AppendFormat("清醒时间      {0:D2}: {1:D2}: {2:D2}\n", statisticData.wakeTime / 3600, (statisticData.wakeTime % 3600) / 60, statisticData.wakeTime % 60);

                infoBuilder.AppendFormat("\n");
                infoBuilder.AppendFormat("总步数       : {0}\n", statisticData.stepCount);
                infoBuilder.AppendFormat("总距离       : {0}\n", statisticData.distance);
                infoBuilder.AppendFormat("消耗卡路里   : {0}\n", statisticData.calorie);

                //infoBuilder.AppendFormat("----- end {0}\n", i);
            }

            readStream.Close();
            //infoBuilder.AppendFormat(">> read statistic result end \n");
            return infoBuilder.ToString();
        }
    }
}
