namespace Fanex.CustomerSetting.Web
{
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class JsonNetResult : JsonResult
    {
        public JsonNetResult(object data)
        {
            Data = data;
        }

        public JsonNetResult(object data, JsonRequestBehavior behavior)
        {
            Data = data;
            JsonRequestBehavior = behavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context != null)
            {
                HttpResponseBase response = context.HttpContext.Response;

                response.ContentType = "application/json";

                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }

                if (Data != null)
                {
                    var serializer = JsonSerializer.Create(new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    var writer = new JsonTextWriter(response.Output)
                    {
                        Formatting = Formatting.Indented
                    };

                    serializer.Serialize(writer, Data);
                    writer.Flush();
                }
            }
        }
    }
}