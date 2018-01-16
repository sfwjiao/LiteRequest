using System.Net;
using System.Collections;
using System.Reflection;

namespace LiteResquest
{
	public class CookieContainerModel : ICookieModel
	{
		private readonly CookieContainer _cookieContainer;
		public CookieContainerModel()
		{
		    _cookieContainer = new CookieContainer { MaxCookieSize = 100 };
		}

		public void SetCookies(HttpWebRequest request)
		{
			request.CookieContainer = _cookieContainer;
		}

		public void GetCookies(HttpWebResponse response)
		{
		}

		public CookieCollection Cookies => GetAllCookies(_cookieContainer);

	    private static CookieCollection GetAllCookies(CookieContainer cc)
		{
			var lstCookies = new CookieCollection();

			var table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
			    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, cc, new object[] { });

			foreach (var pathList in table.Values)
			{
				var lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
				    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, pathList, new object[] { });
				foreach (CookieCollection colCookies in lstCookieCol.Values)
				{
					foreach (Cookie c in colCookies)
					{
						lstCookies.Add(c);
					}
				}
			}

			return lstCookies;
		}
	}
}
