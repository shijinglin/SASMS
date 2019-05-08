using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;

namespace SASMS
{
    [Activity(Label = "@string/AppName", MainLauncher = true, Icon = "@drawable/SMS")]
    public class MainActivity : Activity
    {
        SmsReceiver smsReceiver = null;
        int isLinsten = 0;
        private string filter = string.Empty;

        protected override void OnCreate(Bundle pSavedInstanceState)
        {
            base.OnCreate(pSavedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ToggleButton btnLinsten = FindViewById<ToggleButton>(Resource.Id.btnLinsten);
            btnLinsten.CheckedChange += (sender, e) =>
            {
                if (btnLinsten.Checked)
                {
                    linsten();
                }
                else
                {
                    cancelLinsten();
                }
            };

            Switch swhTest = FindViewById<Switch>(Resource.Id.swhTest);
            swhTest.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                if (smsReceiver != null)
                {
                    smsReceiver.IsDebug = ((Switch)sender).Checked;
                    TextView tbxState = FindViewById<TextView>(Resource.Id.tbxState);
                    tbxState.Text = isLinsten == 1 ? ("监听中" + (smsReceiver.IsDebug ? "-Debug" : "")) : "未监听";
                }
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            ToggleButton btnLinsten = FindViewById<ToggleButton>(Resource.Id.btnLinsten);
            if (isLinsten == 0)
            {
                btnLinsten.Checked = true;
            }
        }

        private void linsten()
        {
            TextView tbxState = FindViewById<TextView>(Resource.Id.tbxState);
            EditText tbxFilter = FindViewById<EditText>(Resource.Id.tbxFilter);
            ListView lvList = FindViewById<ListView>(Resource.Id.lvList);
            Switch swhTest = FindViewById<Switch>(Resource.Id.swhTest);

            smsReceiver = new SmsReceiver();
            smsReceiver.IsDebug = swhTest.Checked;
            smsReceiver.MsgList = lvList;
            smsReceiver.Filter = tbxFilter.Text;
            IntentFilter filter = new IntentFilter();
            filter.Priority = 1000;
            filter.AddAction("android.provider.Telephony.SMS_RECEIVED");
            RegisterReceiver(smsReceiver, filter);
            tbxState.Text = "监听中" + (smsReceiver.IsDebug ? "-Debug" : "");
            tbxFilter.Enabled = false;
            isLinsten = 1;
        }

        private void cancelLinsten()
        {
            if (smsReceiver != null)
            {
                UnregisterReceiver(smsReceiver);
                TextView tbxState = FindViewById<TextView>(Resource.Id.tbxState);
                EditText tbxFilter = FindViewById<EditText>(Resource.Id.tbxFilter);
                tbxState.Text = "未监听";
                tbxFilter.Enabled = true;
                isLinsten = 2;
            }
        }
    }
}

