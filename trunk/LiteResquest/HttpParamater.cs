using System;

namespace LiteResquest
{
    [Serializable]
    public class HttpParamater
    {
        public HttpParamater()
        {
        }

        public HttpParamater(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
