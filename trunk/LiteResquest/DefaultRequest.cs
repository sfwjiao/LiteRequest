using System;
using System.Net;

namespace LiteResquest
{
    /// <summary>
    /// 默认值类
    /// </summary>
    public sealed class DefaultRequest
    {
        public static string UserAgent => "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";

        public static string Accept => "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";

        public static string ContentType => "application/x-www-form-urlencoded";

        /// <summary>
        /// 将url包装为默认配置的请求
        /// </summary>
        /// <param name="urlstring">url地址</param>
        /// <returns></returns>
        public static HttpWebRequest FromString(string urlstring)
        {
            return FromUrl(new Uri(urlstring, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// 将url包装为默认配置的请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public static HttpWebRequest FromUrl(Uri url)
        {
            var returnRequest = (HttpWebRequest)WebRequest.Create(url);
            returnRequest.UserAgent = UserAgent;
            returnRequest.Accept = Accept;
            returnRequest.ContentType = ContentType;
            returnRequest.ServicePoint.Expect100Continue = false;
            returnRequest.KeepAlive = true;
            return returnRequest;
        }
    }
}
