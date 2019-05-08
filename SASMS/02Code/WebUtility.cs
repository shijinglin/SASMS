using System;
using System.Data;

namespace SASMS
{
    /// <summary>
    /// Web工具类
    /// </summary>
    public class WebUtility
    {
        #region GetCallUrl

        /// <summary>
        /// 以Get方式调用Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCallUrl(string url)
        {
            string result = null;
            // 访问获取 
            if (!string.IsNullOrEmpty(url))
            {
                System.Net.HttpWebRequest req = null;
                System.Net.WebResponse resp = null;
                System.IO.StreamReader sr = null;
                try
                {
                    //创建连接
                    req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.Method = "GET";
                    req.UserAgent = "Stone.App.SMS. C# Get.";
                    //读取返回结果
                    resp = req.GetResponse();
                    sr = new System.IO.StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8);
                    result = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    throw new Exception("远程调用失败：" + ex.Message);
                }
                finally
                {
                    try
                    {
                        //关闭资源
                        if (sr != null)
                        {
                            sr.Dispose();
                            sr.Close();
                        }
                        if (resp != null)
                        {
                            resp.Close();
                        }
                        if (req != null) req = null;
                    }
                    catch
                    { }
                }
            }
            return result;
        }

        #endregion GetCallUrl

        #region PostCallUrl

        /// <summary>
        /// Post方式调用Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string PostCallUrl(string url, string contentType, string data)
        {
            string result = null;
            // 访问获取 
            if (!string.IsNullOrEmpty(url))
            {
                System.Net.HttpWebRequest req = null;
                System.Net.HttpWebResponse resp = null;
                System.IO.Stream os = null;
                System.IO.StreamReader sr = null;
                try
                {
                    //创建连接
                    req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    req.ContentType = contentType;
                    req.Method = "POST";
                    req.UserAgent = "CIIC CMC Component. C# POST.";
                    //读取返回结果
                    byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(data);
                    req.ContentLength = postBytes.Length;
                    os = req.GetRequestStream();
                    os.Write(postBytes, 0, postBytes.Length);
                    resp = (System.Net.HttpWebResponse)req.GetResponse();
                    sr = new System.IO.StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8);
                    result = sr.ReadToEnd();
                    //if (string.IsNullOrEmpty(result))
                    //{
                    //    throw new Exception("调用成功，无返回值");
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    try
                    {
                        //关闭资源
                        if (sr != null)
                        {
                            sr.Dispose();
                            sr.Close();
                        }
                        if (resp != null)
                        {
                            resp.Close();
                        }
                        if (req != null) req = null;
                    }
                    catch
                    { }
                }
            }
            return result;
        }

        #endregion PostCallUrl

        #region UrlEncode

        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="pInput"></param>
        /// <returns></returns>
        public static string UrlEncode(string pInput)
        {
            if (pInput == null || "".Equals(pInput)) return "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte[] byStr = System.Text.Encoding.Default.GetBytes(pInput);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + System.Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }

        #endregion UrlEncode

    }
}
