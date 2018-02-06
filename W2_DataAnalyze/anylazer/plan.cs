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

namespace W2_DataAnalyze.anylazer
{
    class Plan
    {
        public const U32 PLAN_ID_LENGTH = 12;
        public const U32 MAX_PLAN_NAME_LENGTH = 16;
        public const U32 MAX_AIM_NUM = 20;
        public const U32 MAX_PLAN_NUM = 20;


        public enum PlanType
        {
            UNKOWN_PLAN,
            PHICOMM_PLAN,
            TARGET_PLAN,
            CUSTOM_PAN
        };

        public enum SportsType
        {
            SPORT_NONE,
            SPORT_ON_FOOT,
            SPORT_RUN,
            SPORT_SIT,
            SPORT_RIDING,
            SPORT_DRIVING,
            SPORT_HIKING,
            SPORT_RUN_OUT,
            SPORT_RUN_IN,
            SPORT_CROSS_COUNTRY,
            SPORT_RIDE_OUT,
            SPORT_SWIM_OUT,
            SPORT_SWIM_IN,
            SPORT_CLIMP,
            SPORT_SLOW_RUN,
            SPORT_BODY_ACTIVE,
            SPORT_AEROBIC_WALK,
            SPORT_AEROBIC,
            SPORT_STRETCH,
            SPORT_TYPE_NUM
        };


        public enum UnitsType
        {
            UNIT_KM,
            UNIT_MIN,
            UNIT_KCAL,
            UNIT_FLOOR,
            UNIT_TYPE_NUM
        };

        public enum TrainItemState
        {
            TIS_NOT_START,
            TIS_SKIPPED,
            TIS_COMPLETED,
            TIS_MAX
        };

        public  struct training_limit_t
        {
            public SportsType limitSportType;
            public U16 highVecSec;
            public U16 lowVecSec;
            public U8 highHR;
            public U8 lowHR;
        };

        public class plans_head_t
        {
            public char[] planName = new char[MAX_PLAN_NAME_LENGTH + 1];
            public char[] planId = new char[PLAN_ID_LENGTH + 1];
            public PlanType planType;
            public U8 aimNum;
        };

        public class plans_file_head_t
        {
            public  U16 check;
            public U16 version;
            public  training_date_t date = new training_date_t();
            public U16 totalNum;
            public U16[] offsetMap = new U16[MAX_PLAN_NUM];
        };

        public class training_aim_t
        {
            public SportsType sportType;
            public UnitsType unit;
            public TrainItemState state;
            public training_limit_t limit = new training_limit_t();
            public U32 value;                 //the real data is: value/10000
            public U32 timeStamp;
        };

        public struct training_date_t
        {
            public U16 year;
            public U8 month;
            public U8 day;
        };

        public struct aim_result_t
        {
            public U8 done;
            public  U32 timeStamp;
        };

        public class project_result_t
        {
            public char[] planId = new char[PLAN_ID_LENGTH + 1];
            public training_date_t planDate = new training_date_t();
            public U8 planType;
            public U8 aimNum;
            public aim_result_t[] aim = new aim_result_t[MAX_AIM_NUM];
        };
    }
}
