using System;
using System.Collections.Generic;
using System.Collections;

namespace LiteResquest
{
    [Serializable]
    public class HttpRecordCollection : ICollection<HttpRecord>, ICloneable
    {
        private readonly List<HttpRecord> _list;
        public HttpRecordCollection()
        {
            _list = new List<HttpRecord>();
        }
        #region ICollection<HttpRecord> 成员

        public void Add(HttpRecord item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(HttpRecord item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(HttpRecord[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count => _list.Count;

        public bool IsReadOnly => true;

        public bool Remove(HttpRecord item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<HttpRecord> 成员

        public IEnumerator<HttpRecord> GetEnumerator()
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

        public object Clone()
        {
            var returnValue = new HttpRecordCollection();
            foreach (var record in _list)
            {
                var newRecord = new HttpRecord
                {
                    Link = record.Link,
                    Method = record.Method
                };
                foreach (var paramater in record.Paramaters)
                {
                    newRecord.Paramaters.Add(new HttpParamater
                    {
                        Key = paramater.Key,
                        Value = paramater.Value
                    });
                }
                newRecord.RecordTime = record.RecordTime;
                newRecord.RequestHeaders.Add(record.RequestHeaders);
                newRecord.ResponseContent = record.ResponseContent;
                newRecord.ResponseHeaders.Add(record.ResponseHeaders);

                returnValue.Add(newRecord);
            }

            return returnValue;
        }
    }
}
