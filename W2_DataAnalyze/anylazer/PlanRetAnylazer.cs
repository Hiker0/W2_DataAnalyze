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
using W2_DataAnalyze.anylazer;

namespace W2_DataAnalyze
{
    class PlanRetAnylazer : Anylazer
    {



        public String anylzeFile(string filePath)
        {
            Debug.WriteLine("read plan ret");
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("                  训练结果解析 \n");

            S32 ret_num = 0;
            U8[] phi_head = new U8[12];
            U32 total = 0;

            FileStream readStream = File.OpenRead(filePath);
            if (((readStream.Length - 12) % 180) != 0)
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
            
            readStream.Seek(12, SeekOrigin.Begin);

            Plan.project_result_t planResult = new Plan.project_result_t(); ;

            for (U32 i = 0; i < total; i++)
            {
                infoBuilder.AppendFormat("-----------------第[{0}] 条训练计划----------------\n", i);

                U8[] phi_data = new U8[180];
                ret_num = 180;
                readStream.Read(phi_data, 0, ret_num);

                for (U8 j =0; j< Plan.PLAN_ID_LENGTH; j++ )
                {
                    planResult.planId[j] = (char)phi_data[j];
                }
                planResult.planId[Plan.PLAN_ID_LENGTH] = '\0';
                infoBuilder.AppendFormat("planid hex:");
                for (U8 j = 0; j < Plan.PLAN_ID_LENGTH; j++)
                {
                    infoBuilder.AppendFormat("{0:X2}", phi_data[j]);
                }
                infoBuilder.AppendFormat("\n\n");

                planResult.planDate.year = (U16)(phi_data[12] + (phi_data[13] << 8));
                planResult.planDate.month = phi_data[14];
                planResult.planDate.day = phi_data[15];
                planResult.planType = phi_data[16];
                planResult.aimNum = phi_data[17];
                infoBuilder.AppendFormat("日期        :{0}-{1}-{2}\n", planResult.planDate.year, planResult.planDate.month, planResult.planDate.day);
                infoBuilder.AppendFormat("类型        :{0}\n", planResult.planType);
                infoBuilder.AppendFormat("小目标个数  :{0}\n", planResult.aimNum);
                infoBuilder.AppendFormat("\n");
                infoBuilder.AppendFormat("*\t 小目标索引\t 时间戳\t\t 是否完成\t 完成时间 \n");

                for (U8 j = 0; j < Plan.MAX_AIM_NUM && j < planResult.aimNum; j++)
                {
                    planResult.aim[j].timeStamp = (U32)(phi_data[20 + j * 8] + (phi_data[21 + j * 8] << 8) + (phi_data[22 + j * 8] << 16) + (phi_data[23 + j * 8] << 24));
                    planResult.aim[j].done = (U8)(phi_data[24 + j * 8] + (phi_data[25 + j * 8] << 8) + (phi_data[26 + j * 8] << 16) + (phi_data[27 + j * 8] << 24));

                    if (planResult.aim[j].timeStamp > 0) {
                        DateTime time = Util.GetDateTimeFrom1970Ticks(planResult.aim[j].timeStamp - 8 * 3600);
                        infoBuilder.AppendFormat("*\t {0}\t\t {1:D12} \t{2}\t\t {3} \n", j, planResult.aim[j].timeStamp, planResult.aim[j].done,time.ToString());
                    }
                    else
                    {
                        infoBuilder.AppendFormat("*\t {0}\t\t {1:D12} \t{2}\t\t {3} \n", j, planResult.aim[j].timeStamp, planResult.aim[j].done, "--");
                    }

                }

            }
            infoBuilder.AppendFormat("\n");
            readStream.Close();
            return infoBuilder.ToString();
        }
    }
}
