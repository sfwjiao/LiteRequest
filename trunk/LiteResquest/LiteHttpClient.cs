using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace LiteResquest
{
    public class LiteHttpClient
    {
        #region  --属性--

        /// <summary>
        /// 获取编码格式
        /// </summary>
        public Encoding Encoding { get; protected set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; protected set; }

        /// <summary>
        /// 证书模块
        /// </summary>
        public ICertModel CertModel { get; protected set; }

        /// <summary>
        /// 处理Cookie模块
        /// </summary>
        public ICookieModel CookieModel { get; protected set; }

        /// <summary>
        /// 记录请求过程的模块
        /// </summary>
        public IProcessRecordModel Record { get; set; }

        #endregion

        #region  --构造函数--

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encoding">请求过程中所使用的编码类型</param>
        /// <param name="cookieModel">请求过程中处理Cookie的模块</param>
        /// <param name="certModel">请求过程中使用的证书模块</param>
        /// <param name="record">记录请求过程的模块</param>
        public LiteHttpClient(Encoding encoding, ICookieModel cookieModel = null, ICertModel certModel = null, IProcessRecordModel record = null)
        {
            //初始化Cookie容器
            Encoding = encoding;
            Timeout = 300000;
            CookieModel = cookieModel ?? new CookieContainerModel();
            CertModel = certModel;//new ThumbprintCertModel(thumbprint)
            Record = record;
        }

        #endregion

        #region  --编码转换--

        /// <summary>
        /// 中文编码转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encode(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding);
        }

        /// <summary>
        /// 编码转中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Decode(string value)
        {
            return HttpUtility.UrlDecode(value, Encoding);
        }

        #endregion

        #region   --HTTP请求过程相关方法--

        /// <summary>
        /// GET方式获得响应
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>响应对象</returns>
        public HttpWebResponse Get(HttpWebRequest request)
        {
            //设置安全请求模式
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            //添加cookie
            CookieModel.SetCookies(request);

            //添加证书
            CertModel?.SetCert(request);

            //设置请求方式
            request.Method = "GET";
            request.Timeout = Timeout;

            //获得响应
            var response = (HttpWebResponse)request.GetResponse();
            CookieModel.GetCookies(response);
            Record?.Record(request, response);

            return response;
        }

        /// <summary>
        /// GET方式获得响应
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>响应对象</returns>
        public HttpWebResponse Get(string url)
        {
            return Get(DefaultRequest.FromString(url));
        }

        /// <summary>
        /// GET方式获得响应流对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>内存流</returns>
        public MemoryStream GetStream(HttpWebRequest request)
        {
            return StreamFromResponse(Get(request));
        }

        /// <summary>
        /// GET方式获得响应流对象
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>内存流</returns>
        public MemoryStream GetStream(string url)
        {
            return StreamFromResponse(Get(url));
        }

        /// <summary>
        /// GET方式获得响应流对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="fileName">文件路径</param>
        public void GetSaveAs(HttpWebRequest request, string fileName)
        {
            SaveAsFromResponse(Get(request), fileName);
        }

        /// <summary>
        /// GET方式获得响应流对象
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="fileName">文件路径</param>
        public void GetSaveAs(string url, string fileName)
        {
            SaveAsFromResponse(Get(url), fileName);
        }

        /// <summary>
        /// GET方式获得响应流对象
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>响应字符串</returns>
        public string GetString(HttpWebRequest request)
        {
            return StringFromResponse(Get(request), Encoding);
        }

        /// <summary>
        /// GET方式获得响应流对象
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>响应字符串</returns>
        public string GetString(string url)
        {
            return StringFromResponse(Get(url), Encoding);
        }

        /// <summary>
        /// POST方式获得响应
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="param">参数字符串</param>
        /// <returns>响应对象</returns>
        public HttpWebResponse Post(HttpWebRequest request, string param)
        {
            //设置安全请求模式
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            //添加cookie
            CookieModel.SetCookies(request);

            //添加证书
            CertModel?.SetCert(request);

            //设置请求方式
            request.Method = "POST";
            request.Timeout = Timeout;
            if (!string.IsNullOrEmpty(param)) SetPostData(request, param);

            //获得响应并设置Cookies
            var response = (HttpWebResponse)request.GetResponse();
            CookieModel.GetCookies(response);
            Record?.Record(request, response);

            return response;
        }

        /// <summary>
        /// POST方式获得响应
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数字符串</param>
        /// <returns>响应对象</returns>
        public HttpWebResponse Post(string url, string param)
        {
            return Post(DefaultRequest.FromString(url), param);
        }

        /// <summary>
        /// POST方式获得响应
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="collection">参数集合对象</param>
        /// <returns>响应对象</returns>
        public HttpWebResponse Post(HttpWebRequest request, NameValueCollection collection)
        {
            return Post(request, NameValueCollectionToString(collection));
        }

        /// <summary>
        /// POST方式获得响应
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="collection">参数集合对象</param>
        /// <returns>响应对象</returns>
        public HttpWebResponse Post(string url, NameValueCollection collection)
        {
            return Post(url, NameValueCollectionToString(collection));
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="param">参数字符串</param>
        /// <returns>内存流</returns>
        public MemoryStream PostStream(HttpWebRequest request, String param)
        {
            return StreamFromResponse(Post(request, param));
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数字符串</param>
        /// <returns>内存流</returns>
        public MemoryStream PostStream(string url, string param)
        {
            return StreamFromResponse(Post(url, param));
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="collection">参数集合对象</param>
        /// <returns>内存流</returns>
        public MemoryStream PostStream(HttpWebRequest request, NameValueCollection collection)
        {
            return StreamFromResponse(Post(request, collection));
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="collection">参数集合对象</param>
        /// <returns>内存流</returns>
        public MemoryStream PostStream(string url, NameValueCollection collection)
        {
            return StreamFromResponse(Post(url, collection));
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="param">参数字符串</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>内存流</returns>
        public void PostSaveAs(HttpWebRequest request, string param, string fileName)
        {
            SaveAsFromResponse(Post(request, param), fileName);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数字符串</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>内存流</returns>
        public void PostSaveAs(string url, string param, string fileName)
        {
            SaveAsFromResponse(Post(url, param), fileName);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="collection">参数集合对象</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>内存流</returns>
        public void PostSaveAs(HttpWebRequest request, NameValueCollection collection, string fileName)
        {
            SaveAsFromResponse(Post(request, collection), fileName);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="collection">参数集合对象</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>内存流</returns>
        public void PostSaveAs(string url, NameValueCollection collection, string fileName)
        {
            SaveAsFromResponse(Post(url, collection), fileName);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="param">参数字符串</param>
        /// <returns>响应字符串</returns>
        public string PostString(HttpWebRequest request, string param)
        {
            return StringFromResponse(Post(request, param), Encoding);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">参数字符串</param>
        /// <returns>响应字符串</returns>
        public string PostString(string url, string param)
        {
            return StringFromResponse(Post(url, param), Encoding);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="collection">参数集合对象</param>
        /// <returns>响应字符串</returns>
        public string PostString(HttpWebRequest request, NameValueCollection collection)
        {
            return StringFromResponse(Post(request, collection), Encoding);
        }

        /// <summary>
        /// POST方式获得响应流
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="collection">参数集合对象</param>
        /// <returns>响应字符串</returns>
        public string PostString(string url, NameValueCollection collection)
        {
            return StringFromResponse(Post(url, collection), Encoding);
        }

        #endregion

        #region  --静态方法--

        /// <summary>
        /// 从响应中获取内存流
        /// </summary>
        /// <param name="response">响应对象</param>
        /// <returns>内存流</returns>
        public static MemoryStream StreamFromResponse(HttpWebResponse response)
        {
            MemoryStream msr;
            try
            {
                //创建流对象，读取响应流，生成字符串
                var sr = response.GetResponseStream();
                msr = new MemoryStream();

                var buffer = new byte[1024];
                var count = sr.Read(buffer, 0, 1024);
                while (count > 0)
                {
                    msr.Write(buffer, 0, 1024);
                    count = sr.Read(buffer, 0, 1024);
                }
            }
            finally
            {
                //关闭响应流
                response.Close();
            }
            return msr;
        }

        /// <summary>
        /// 从响应中获取字符串
        /// </summary>
        /// <param name="response">响应对象</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>字符串</returns>
        public static string StringFromResponse(HttpWebResponse response, Encoding encoding)
        {
            var returnValue = new StringBuilder();
            try
            {
                //创建流对象，读取响应流，生成字符串
                var sr = new StreamReader(response.GetResponseStream(), encoding);
                returnValue.Append(sr.ReadToEnd());
            }
            finally
            {
                //关闭响应流
                response.Close();
            }

            //获得并返回响应
            return returnValue.ToString();
        }

        /// <summary>
        /// 将响应保存为文件
        /// </summary>
        /// <param name="response">响应对象</param>
        /// <param name="fileName">文件名（包含路径）</param>
        public static void SaveAsFromResponse(HttpWebResponse response, string fileName)
        {
            Stream sr = null;
            FileStream fs = null;
            try
            {
                //创建流对象
                sr = response.GetResponseStream();
                //创建文件流对象
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

                //循环读取流保存到文件
                var buffer = new byte[1024];
                var bytesRead = sr.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = sr.Read(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                fs?.Close();
                sr?.Close();
                response.Close();
            }
        }

        #endregion

        #region  --似有方法--

        /// <summary>
        /// 创建参数字符串
        /// </summary>
        /// <param name="collection">参数字典</param>
        /// <returns>字符串</returns>
        private static string NameValueCollectionToString(NameValueCollection collection)
        {
            var returnValue = new StringBuilder();

            //循环字典，将参数集合转换成字符串
            foreach (string key in collection.Keys)
            {
                returnValue.Append(key);
                returnValue.Append("=");
                returnValue.Append(collection[key]);
                returnValue.Append("&");
            }

            //删除最后一个&连接符号
            if (returnValue.ToString().EndsWith("&", StringComparison.OrdinalIgnoreCase)) returnValue.Remove(returnValue.Length - 1, 1);

            return returnValue.ToString();
        }

        /// <summary>
        /// 设置Post请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        private void SetPostData(HttpWebRequest request, string data)
        {
            //获取字符串字节值
            var param = Encoding.GetBytes(data);

            //写入请求流
            var requestStream = request.GetRequestStream();
            requestStream.Write(param, 0, param.Length);
            requestStream.Close();
        }

        #endregion
    }
}
