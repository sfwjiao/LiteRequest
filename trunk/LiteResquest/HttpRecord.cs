using System;
using System.Collections.Generic;
using System.Net;

namespace LiteResquest
{
    [Serializable]
    public class HttpRecord
    {
        public HttpRecord()
        {
            RequestHeaders = new WebHeaderCollection();
            ResponseHeaders = new WebHeaderCollection();
            Paramaters = new HttpParamaterCollection();
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求头信息
        /// </summary>
        public WebHeaderCollection RequestHeaders { get; }

        /// <summary>
        /// 响应头信息
        /// </summary>
        public WebHeaderCollection ResponseHeaders { get; }

        /// <summary>
        /// 请求参数列表
        /// </summary>
        public HttpParamaterCollection Paramaters { get; }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="paramaters"></param>
        public void SetParamaters(Dictionary<string, string> paramaters)
        {
            foreach (var key in paramaters.Keys)
            {
                Paramaters.Add(new HttpParamater(key, paramaters[key]));
            }
        }

        /// <summary>
        /// 响应正文
        /// </summary>
        public string ResponseContent { get; set; }

        /// <summary>
        /// 生成记录时间
        /// </summary>
        public DateTime RecordTime { get; set; }
    }
}
