using System.Net;

namespace LiteResquest
{
	public interface ICookieModel
	{
		void SetCookies(HttpWebRequest request);
		void GetCookies(HttpWebResponse response);
		CookieCollection Cookies { get; }
	}
}
