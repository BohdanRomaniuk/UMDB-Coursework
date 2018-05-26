using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text;

namespace parser.WebLogginer
{
    public class Session
    {
        private bool _isPost;
        private bool _isDownload;
        private HtmlDocument _htmlDoc;
        private string _download;
        public CookieContainer cookiePot;

        public CookieCollection Cookies { get; set; }
        //Контейнер для пари ключ значення з форми запиту
        public FormElementCollection FormElements { get; set; }

        // Робить GET запит використовуючи COOKIES після логінування
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

        // Робить POST запит за заданою адресою
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

        /// Створює обєкт HtmlWeb та ініціалізує обробники подій 
        private HtmlWeb CreateWebRequestObject()
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
            web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
            web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
            return web;
        }

        // Обробник події HtmlWeb.PreRequestHandler. Виникає перед виконанням HTTP запиту.
        protected bool OnPreRequest(HttpWebRequest request)
        {
            AddCookiesTo(request);               // Додати кукі що були збережені з попередніх запитів
            if (_isPost) AddPostDataTo(request); // Якщо це пост запит то додати пари ключ значення з форми
            return true;
        }

        // Обробник події HtmlWeb.PostResponseHandler. Виникає після повернення HTTP відповіді.
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

        // Обробник події HtmlWeb.PreHandleDocumentHandler. Виникає перед тим як HTML документ обробляється
        protected void OnPreHandleDocument(HtmlAgilityPack.HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        // Збирає дані для Post запиту і прикріпляє до обєкта запиту
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = FormElements.AssemblePostPayload();
            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
            request.ContentLength = buff.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream reqStream = request.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
        }

        // Додає кукі до обєкту запиту
        private void AddCookiesTo(HttpWebRequest request)
        {
            if (Cookies != null && Cookies.Count > 0)
            {
                request.CookieContainer.Add(Cookies);
            }
        }

        // Зберігає кукі з запиту до локальної колекції CookieCollection
        private void SaveCookiesFrom(HttpWebRequest request, HttpWebResponse response)
        {
            //Зберігання кукі!!!
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

        // Зберігає пари ключ значення для форми запиту
        private void SaveHtmlDocument(HtmlAgilityPack.HtmlDocument document)
        {
            _htmlDoc = document;
            FormElements = new FormElementCollection(_htmlDoc);
        }

        // Робить GET запит без використання COOKIES після логінування
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
