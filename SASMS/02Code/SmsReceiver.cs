using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Telephony;
using Android.Widget;
using Java.Lang;
//using Xamarin.Forms;

namespace SASMS
{
    [BroadcastReceiver(Enabled = true, Label = "SMS Receiver")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED", Intent.CategoryDefault })]
    public class SmsReceiver : BroadcastReceiver
    {
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";
        private List<string> items = new List<string>();

        /// <summary>
        /// 界面表格对象
        /// </summary>
        public Android.Widget.ListView MsgList
        { get; set; }

        /// <summary>
        /// 是否调试模式
        /// </summary>
        public bool IsDebug
        { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter
        { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action != IntentAction) return;
                var bundle = intent.Extras;
                if (bundle == null) return;
                var pdus = bundle.Get("pdus");
                //var castedPdus = JNIEnv.GetArray(pdus.Handle);
                var castedPdus = JNIEnv.GetArray<Object>(pdus.Handle);
                var msgs = new SmsMessage[castedPdus.Length];
                string sender = null;
                for (var i = 0; i < msgs.Length; i++)
                {
                    var bytes = new byte[JNIEnv.GetArrayLength(castedPdus[i].Handle)];
                    JNIEnv.CopyArray(castedPdus[i].Handle, bytes);
                    string format = bundle.GetString("format");
                    msgs[i] = SmsMessage.CreateFromPdu(bytes, format);
                    if (sender == null)
                        sender = msgs[i].OriginatingAddress;
                    //Toast.MakeText(context, sb.ToString(), ToastLength.Long).Show();
                    //Log.Error("Vahid", sb.ToString());
                    var msgBody = msgs[i].MessageBody;
                    //界面显示
                    items.Insert(0, System.DateTime.Now.ToString("MM-dd HH:mm:ss") + ":ok");
                    ArrayAdapter<string> adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItem1, items);//多行一列的显示方法
                    MsgList.Adapter = adapter;
                    //通知
                    if (validateFilter(msgBody))
                    {
                        if (IsDebug)
                        {
                            WebUtility.GetCallUrl("http://************?Token=test&content=" + WebUtility.UrlEncode(msgBody));
                        }
                        else
                        {
                            WebUtility.GetCallUrl("http://************&content=" + WebUtility.UrlEncode(msgBody));
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("NameResolutionFailure"))
                {
                    items.Insert(0, "请检查网络或授权");
                }
                else
                {
                    items.Insert(0, ex.Message);
                }
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItem1, items);//多行一列的显示方法
                MsgList.Adapter = adapter;
            }
        }

        /// <summary>
        /// 验证过滤条件
        /// </summary>
        /// <param name="pMsgBody"></param>
        /// <returns></returns>
        private bool validateFilter(string pMsgBody)
        {
            bool result = string.IsNullOrEmpty(Filter);
            if (!result)
            {
                string[] fs = Filter.Split(new string[] { ",", "，" }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in fs)
                {
                    result = pMsgBody.Contains(item);
                    if (!result)
                    {
                        break;
                    }
                }
            }
            return result;
        }
    }
}