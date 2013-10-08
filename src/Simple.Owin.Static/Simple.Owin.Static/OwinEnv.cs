namespace Simple.Owin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using StaticMiddleware;

    public struct OwinEnv
    {
        private readonly IDictionary<string, object> _env;

        public OwinEnv(IDictionary<string, object> env) : this()
        {
            _env = env;
        }

        public Stream RequestBody
        {
            get { return (Stream) _env[OwinKeys.RequestBody]; }
        }

        public IDictionary<string, string[]> RequestHeaders
        {
            get {  return (IDictionary<string,string[]>)_env[OwinKeys.RequestHeaders]; }
        }

        public string RequestMethod
        {
            get { return (string) _env[OwinKeys.Method]; }
        }

        public string RequestPath
        {
            get { return (string) _env[OwinKeys.Path]; }
        }

        public string RequestPathBase
        {
            get { return (string) _env[OwinKeys.PathBase]; }
        }

        public string RequestProtocol
        {
            get { return (string) _env[OwinKeys.Protocol]; }
        }

        public string RequestQueryString
        {
            get { return (string) _env[OwinKeys.QueryString]; }
        }

        public string RequestScheme
        {
            get { return (string) _env[OwinKeys.Scheme]; }
        }

        public Stream ResponseBody
        {
            get { return (Stream) _env[OwinKeys.ResponseBody]; }
        }

        public IDictionary<string, string[]> ResponseHeaders
        {
            get {  return (IDictionary<string,string[]>)_env[OwinKeys.ResponseHeaders]; }
        }

        public int? ResponseStatusCode
        {
            get { return _env.ContainsKey(OwinKeys.StatusCode) ? (int?) _env[OwinKeys.StatusCode] : null; }
            set { _env[OwinKeys.StatusCode] = value; }
        }
        
        public string ResponseReasonPhrase
        {
            get { return _env.ContainsKey(OwinKeys.ReasonPhrase) ? (string) _env[OwinKeys.ReasonPhrase] : null; }
            set { _env[OwinKeys.ReasonPhrase] = value; }
        }
        
        public string ResponseProtocol
        {
            get { return _env.ContainsKey(OwinKeys.ResponseProtocol) ? (string) _env[OwinKeys.ResponseProtocol] : null; }
            set { _env[OwinKeys.ResponseProtocol] = value; }
        }

        public CancellationToken CallCancelled
        {
            get { return (CancellationToken) _env[OwinKeys.CallCancelled]; }
        }

        public string Version
        {
            get { return (string)_env[OwinKeys.Version]; }
        }

        public Func<string, long, long?, CancellationToken, Task> SendFileAsync
        {
            get
            {
                object obj;
                return _env.TryGetValue(OwinKeys.SendFileAsync, out obj)
                    ? obj as Func<string, long, long?, CancellationToken, Task>
                    : null;
            }
        }
    }
}