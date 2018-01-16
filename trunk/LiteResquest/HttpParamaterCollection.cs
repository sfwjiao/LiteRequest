using System;
using System.Collections.Generic;
using System.Collections;

namespace LiteResquest
{
    [Serializable]
    public class HttpParamaterCollection : ICollection<HttpParamater>
    {
        private readonly List<HttpParamater> _list;

        public HttpParamaterCollection()
        {
            _list = new List<HttpParamater>();
        }

        #region ICollection<HttpRecord> 成员

        public void Add(HttpParamater item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(HttpParamater item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(HttpParamater[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(HttpParamater item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<HttpParamater> 成员

        public IEnumerator<HttpParamater> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
