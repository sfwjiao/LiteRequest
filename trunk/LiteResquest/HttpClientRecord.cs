using System;
using System.Collections.Generic;
using System.Net;

namespace LiteResquest
{
	[Serializable]
	public class HttpClientRecord : IProcessRecordModel
	{
		private readonly Dictionary<HttpWebRequest, HttpWebResponse> _list;
		public HttpClientRecord()
		{
			_list = new Dictionary<HttpWebRequest, HttpWebResponse>();
		}

		public void Record(HttpWebRequest request, HttpWebResponse response)
		{
			_list.Add(request, response);
		}
	}
}
