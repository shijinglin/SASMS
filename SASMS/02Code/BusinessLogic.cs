using System.Text;
using System;

namespace SASMS
{
    public static class BusinessLogic
    {
        public static string GetMsg(string pFilter)
        {
            //获取系统短信
            //SMS
            return pFilter;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="pNumber"></param>
        /// <param name="pSmsContent"></param>
        /// <returns></returns>
        public static void SendSMS(string pNumber, string pSmsContent)
        {
            //SMS
            //SmsMessage sm = new SmsMessage(pSmsContent, pNumber);
            //Sms.ComposeAsync(sm);
            //return pFilter;
        }
    }
}