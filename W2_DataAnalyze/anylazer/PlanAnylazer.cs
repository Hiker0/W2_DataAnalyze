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
    class PlanAnylazer : Anylazer
    {
        struct sleep_data
        {
            public U32 timeStamp;
            public U32 status;
        };

        public String anylzeFile(string filePath)
        {
            Debug.WriteLine("readSleepData");
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("                  训练计划解析结果 \n");

            S32 ret_num = 0;
            U8[] phi_head = new U8[50];

            FileStream readStream = File.OpenRead(filePath);

            ret_num = 50;
            readStream.Read(phi_head,0, ret_num);
            Plan.plans_file_head_t header = new Plan.plans_file_head_t();
            try
            {
                header.version = (U16)(phi_head[2] + (phi_head[3] << 8));
        
                header.date.year =(U16)( phi_head[4] + (phi_head[5] << 8));
                header.date.month = phi_head[6];
                header.date.day = phi_head[7];
                header.totalNum =(U16)( phi_head[8] + (phi_head[9] << 8));

                infoBuilder.AppendFormat("校验    : {0:X2}{0:X2} \n", phi_head[0], phi_head[1]);
                infoBuilder.AppendFormat("版本    : {0} \n", header.version);
                infoBuilder.AppendFormat("日期    : {0}/{1}/{2}\n", header.date.year, header.date.month, header.date.day);
                infoBuilder.AppendFormat("总数    : {0} \n", header.totalNum);

                if (header.totalNum > Plan.MAX_PLAN_NUM)
                {
                    header.totalNum = (U16)Plan.MAX_PLAN_NUM;
                }

                for (U8 i = 0; i < header.totalNum; i++)
                {
                    header.offsetMap[i] =(U16)( phi_head[10 + i * 2] + (phi_head[11 + i * 2] << 8));
                    //printf("    offset[i]: %d \n", header.offsetMap[i]);
                }
            }
            catch (Exception e)
            {

            }

            
            U8[] planHeadBuff = new U8[46];
            Plan.plans_head_t planHead = new Plan.plans_head_t();
            for (U8 i = 0; i < header.totalNum; i++)
            {
                readStream.Seek(header.offsetMap[i], SeekOrigin.Begin);
                ret_num = 46;
                readStream.Read(planHeadBuff, 0, ret_num);
                for (U8 j = 0; j < Plan.MAX_PLAN_NAME_LENGTH; j++)
                {
                    planHead.planName[j] = (char)(planHeadBuff[j * 2] + (planHeadBuff[1 + j * 2] << 8));
                }

                for (U8 j = 0; j < Plan.PLAN_ID_LENGTH; j++)
                {
                    planHead.planId[j] = (char)(planHeadBuff[j + 32]);
                }
                planHead.planType = (Plan.PlanType)planHeadBuff[44];
                planHead.aimNum = planHeadBuff[45];
                infoBuilder.AppendFormat("\n---------------- 第[{0}]条计划 ----------------\n", i);
                infoBuilder.AppendFormat("  偏移量\t: {0} \n", header.offsetMap[i]);
                infoBuilder.AppendFormat("  名称\t: {0} \n", new string(planHead.planName).Trim());
                infoBuilder.AppendFormat("  训练ID\t:");
                for (U8 j = 0; j < Plan.PLAN_ID_LENGTH; j++)
                {
                    infoBuilder.AppendFormat("{0:X2}", planHeadBuff[j + 32]);
                }
                infoBuilder.AppendFormat("  \n:");
                infoBuilder.AppendFormat("  训练类型\t: {0} \n", planHead.planType);
                infoBuilder.AppendFormat("  小目标数\t: {0} \n", planHead.aimNum);

                U8[] aimBuff = new U8[20];
                Plan.training_aim_t myAim = new Plan.training_aim_t();
                for (U8 j = 0; j < planHead.aimNum; j++)
                {
                    ret_num = 20;
                    //memset(aimBuff, 0, ret_num);
                    // memset(&myAim, 0, sizeof(training_aim_t));
                    readStream.Read(aimBuff, 0, ret_num);

                    myAim.sportType = (Plan.SportsType)aimBuff[0];
                    myAim.unit = (Plan.UnitsType)aimBuff[1];
                    myAim.state = (Plan.TrainItemState)aimBuff[2];
                    myAim.limit.limitSportType = (Plan.SportsType)aimBuff[4];
                    myAim.limit.highVecSec = (U16)(aimBuff[4] + (aimBuff[5] << 8));
                    myAim.limit.lowVecSec = (U16)(aimBuff[6] + (aimBuff[7] << 8));
                    myAim.limit.highHR = (U8)(aimBuff[8] + (aimBuff[9] << 8));
                    myAim.limit.lowHR = (U8)(aimBuff[10] + (aimBuff[11] << 8));

                    myAim.value = (U32)(aimBuff[12] + (aimBuff[13] << 8) + (aimBuff[14] << 16) + (aimBuff[15] << 24));
                    myAim.timeStamp = (U32)(aimBuff[16] + (aimBuff[17] << 8) + (aimBuff[18] << 16) + (aimBuff[19] << 24));

                    infoBuilder.AppendFormat("\n\t-------第[{0}]个小目标----- \n", j);
                    infoBuilder.AppendFormat("\t 运动类型 : {0} \n", myAim.sportType);
                    infoBuilder.AppendFormat("\t 单位     : {0} \n", myAim.unit);
                    infoBuilder.AppendFormat("\t 状态     : {0}\n", myAim.state);
                    infoBuilder.AppendFormat("\t 数值     : {0}\n", myAim.value / 10000);

                    if (myAim.timeStamp > 0) { 
                        DateTime time = Util.GetDateTimeFrom1970Ticks(myAim.timeStamp - 8 * 3600);

                        infoBuilder.AppendFormat("\t 完成时间: {0},  {1} \n", myAim.timeStamp, time.ToString());
                    }
                };

                infoBuilder.AppendFormat("    \n\n");
            }
            
            readStream.Close();
            return infoBuilder.ToString();
        }
    }
}
