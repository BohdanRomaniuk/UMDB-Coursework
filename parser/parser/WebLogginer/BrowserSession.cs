using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text;

namespace parser.WebLogginer
{
    public class BrowserSession
    {
        private bool _isPost;
        private bool _isDownload;
        private HtmlAgilityPack.HtmlDocument _htmlDoc;
        private string _download;
        public CookieContainer cookiePot;   //<- This is the new CookieContainer
                                            // System.Net.CookieCollection. Provides a collection container for instances of Cookie class 
        public CookieCollection Cookies { get; set; }
        // Provide a key-value-pair collection of form elements 
        public FormElementCollection FormElements { get; set; }

        // Makes a HTTP GET request to the given URL using COOKIES AFTER LOGIN
        public string Get(string url)
        {
            _isPost = false;
            CreateWebRequestObject().Load(url);
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        public HtmlAgilityPack.HtmlDocument Load(string url)
        {
            _isPost = false;
            CreateWebRequestObject().Load(url);
            return _htmlDoc;
        }

        // Makes a HTTP POST request to the given URL
        public string Post(string url)
        {
            _isPost = true;
            CreateWebRequestObject().Load(url, "POST");
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        public string GetDownload(string url)
        {
            _isPost = false;
            _isDownload = true;
            CreateWebRequestObject().Load(url);
            return _download;
        }

        /// Creates the HtmlWeb object and initializes all event handlers. 
        private HtmlWeb CreateWebRequestObject()
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
            web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
            return web;
        }

        // Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
        protected bool OnPreRequest(HttpWebRequest request)
        {
            AddCookiesTo(request);               // Add cookies that were saved from previous requests
            if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
            return true;
        }

        // Event handler for HtmlWeb.PostResponseHandler. Occurs after a HTTP response is received
        protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            SaveCookiesFrom(request, response); // Save cookies for subsequent requests

            if (response != null && _isDownload)
            {
                Stream remoteStream = response.GetResponseStream();
                var sr = new StreamReader(remoteStream);
                _download = sr.ReadToEnd();
            }
        }

        // Event handler for HtmlWeb.PreHandleDocumentHandler. Occurs before a HTML document is handled
        protected void OnPreHandleDocument(HtmlAgilityPack.HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        // Assembles the Post data and attaches to the request object
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = FormElements.AssemblePostPayload();
            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
            request.ContentLength = buff.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream reqStream = request.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
        }

        // Add cookies to the request object
        private void AddCookiesTo(HttpWebRequest request)
        {
            if (Cookies != null && Cookies.Count > 0)
            {
                request.CookieContainer.Add(Cookies);
            }
        }

        // Saves cookies from the response object to the local CookieCollection object
        private void SaveCookiesFrom(HttpWebRequest request, HttpWebResponse response)
        {
            //save the cookies 😉
            if (request.CookieContainer.Count > 0 || response.Cookies.Count > 0)
            {
                if (Cookies == null)
                {
                    Cookies = new CookieCollection();
                }

                Cookies.Add(request.CookieContainer.GetCookies(request.RequestUri));
                Cookies.Add(response.Cookies);
            }
        }

        // Saves the form elements collection by parsing the HTML document
        private void SaveHtmlDocument(HtmlAgilityPack.HtmlDocument document)
        {
            _htmlDoc = document;
            FormElements = new FormElementCollection(_htmlDoc);
        }

        // Makes a HTTP GET request to the given URL  WITHOUT using COOKIES AFTER LOGIN
        public string Get2(string url)
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest2);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse2);
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            return doc.DocumentNode.InnerHtml;
        }

        public bool OnPreRequest2(HttpWebRequest request)
        {
            request.CookieContainer = cookiePot;
            return true;
        }
        protected void OnAfterResponse2(HttpWebRequest request, HttpWebResponse response)
        {
            //do nothing
        }
        private void SaveCookiesFrom(HttpWebResponse response)
        {
            if ((response.Cookies.Count > 0))
            {
                if (Cookies == null)
                {
                    Cookies = new CookieCollection();
                }
                Cookies.Add(response.Cookies);
                cookiePot.Add(Cookies);     //-> add the Cookies to the cookiePot
            }
        }
    }
}
