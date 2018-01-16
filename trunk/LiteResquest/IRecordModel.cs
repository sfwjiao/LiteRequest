using System.Net;

namespace LiteResquest
{
	/// <summary>
	/// 请求过程记录接口
	/// </summary>
	public interface IProcessRecordModel
	{
		void Record(HttpWebRequest request, HttpWebResponse response);
	}
}
