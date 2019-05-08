//using Xamarin.Forms;

namespace SASMS
{
    public class RegisterSecondPageModel
    {
        public RegisterSecondPageModel()
        {
            SubscribeToOtpReceiving();
        }

        private void SubscribeToOtpReceiving()
        {
            //MessagingCenter.Subscribe<RegisterSecondPageModel, string>(this, "OtpReceived", (sender, code) =>
            //{
            //   // ActivationCode = code;
            //});
        }
    }
}