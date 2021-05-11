
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Google.Protobuf;
using ProtoBuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonFlatten;

namespace Com.Cirruslink.Sparkplug.Protobuf
{
    public static class SpBDecoder
    {

        [FunctionName("SpBDecoder")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get HTTP request body, put into byte array
            MemoryStream ms = new MemoryStream();
            await req.Body.CopyToAsync(ms);
            byte[] data = ms.ToArray();

            // Parse byte array to protobuf IMessage
            MessageParser<Payload> parser = Com.Cirruslink.Sparkplug.Protobuf.Payload.Parser;
            IMessage<Payload> parsed = parser.ParseFrom(data);

            // Convert IMessage to JSON string
            string jsonString = "";
            JsonFormatter formatter = new JsonFormatter(new JsonFormatter.Settings(false));
            jsonString = formatter.Format(parsed);

            log.LogInformation("got the body: " + jsonString);

            await PublishToSolace(jsonString, log);

            return new OkObjectResult("Success");
        }

        private async static Task PublishToSolace(string body, ILogger log)
        {

            // Set the REST hostname and port for the Solace broker (eg. http://solace:9000/)
            string solaceURL = "";

            JObject rss = JObject.Parse(body);
            string timestamp = (string)rss["timestamp"];
            JArray items = (JArray)rss["metrics"];

            for (int i = 0; i< items.Count; i++)
            {
                // Create a JSON Object to populate
                JObject jsonBody = new JObject();
                jsonBody.Add("timestamp", timestamp);

                string aliasNum = (string)rss["metrics"][i]["alias"];

                // Create a nest JSON Object
                JObject jsonMetrics = new JObject();
                jsonMetrics.Add("alias", aliasNum);
                jsonMetrics.Add("datatype", (string)rss["metrics"][i]["datatype"]);
                jsonMetrics.Add("floatValue", (string)rss["metrics"][i]["floatValue"]);

                jsonBody.Add("metrics", jsonMetrics);
                jsonBody.Add("seq", (string)rss["seq"]);

                string solaceTopic;

                if (aliasNum.Equals("4"))
                {
                    solaceTopic = "mes/pem/level/data";
                } else if (aliasNum.Equals("7"))
                {
                    solaceTopic = "erp/logistics/flow/data";
                } else
                {
                    solaceTopic = "watson/packaging/AI/data";
                }

                // Populate Solace URL with device alias
                solaceURL = solaceURL + solaceTopic;

                log.LogInformation("published body: " + jsonBody.ToString());

                // POST the message to Solace
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(jsonBody.ToString());
                HttpResponseMessage response = await client.PostAsync(solaceURL, content);
                response.EnsureSuccessStatusCode();
                log.LogInformation(response.ToString());
            }

        }

    }
}
