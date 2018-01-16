using System;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.ComponentModel;

namespace LiteResquest
{
	public class ThumbprintCertModel : ICertModel
	{
		/// <summary>
		/// 证书
		/// </summary>
		public X509Certificate2 Cert { get; protected set; }

		/// <summary>
		/// 验证证书事件
		/// </summary>
		public event EventHandler<CancelEventArgs> OnCheckValidationResult;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="thumbprint"></param>
		public ThumbprintCertModel(string thumbprint)
		{
		    Cert = CertCreater.GetCertByThumbprint(thumbprint);
		}

		public void SetCert(HttpWebRequest request)
		{
			//添加证书
		    if (Cert == null) return;

		    ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
		    request.ClientCertificates.Add(Cert);
		}

		/// <summary>
		/// 证书相关，安全策略模式
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="certificate"></param>
		/// <param name="chain"></param>
		/// <param name="errors"></param>
		/// <returns></returns>
		private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			//通过委托事件来处理验证证书过程
			var args = new CancelEventArgs();
            OnCheckValidationResult?.Invoke(this, new CancelEventArgs());
            return !args.Cancel;
		}
	}
}
