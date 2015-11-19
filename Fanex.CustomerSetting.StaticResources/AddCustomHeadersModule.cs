namespace Fanex.CustomerSetting.StaticResources
{
    using System;
    using System.Web;

    public class AddCustomHeadersModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;

            response.AddHeader("Access-Control-Allow-Origin", "*");
        }

        #endregion IHttpModule Members
    }
}