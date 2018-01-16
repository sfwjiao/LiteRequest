using System.Net;

namespace LiteResquest
{
	public interface ICertModel
	{
		void SetCert(HttpWebRequest request);
	}
}
