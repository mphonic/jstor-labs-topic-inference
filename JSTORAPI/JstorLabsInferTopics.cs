using JstorLabs.DataContracts;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JstorLabs
{
    // Compile and run:
    // JstorLabsInferTopics PATH_OR_URL
    // or dotnet JstorLabsInferTopics.dll PATH_OR_URL
    class JstorLabsInferTopics
    {
        private static readonly string JstorToken = ENTER_YOUR_TOKEN_HERE;

        private static readonly string LabsTopicsUrl = "https://labs.jstor.org/api/labs-topic-service/topics";

        private static readonly string LabsTextExtractorUrl = "https://labs.jstor.org/api/labs-text-extractor-service/v2/extract";

        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter the path or URL for the file you want to analyze.");
                return;
            }
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", JstorToken);

            var location = args[0];
            string text;
            bool locationIsUrl = Uri.TryCreate(location, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            Console.WriteLine("Extracting text.");
            if (locationIsUrl)
                text = ExtractTextFromUrl(location).GetAwaiter().GetResult();
            else
                text = ExtractTextFromFile(location).GetAwaiter().GetResult();

            if (text == null)
            {
                Console.WriteLine("Attempt to retrieve text from document failed.");
                return;
            }
            Console.WriteLine("Text extracted.");

            Console.WriteLine("Retrieving topics.");
            InferredTopics returnedTopics = GetTopics(text).GetAwaiter().GetResult();
            if (returnedTopics == null)
            {
                Console.WriteLine("Attempt to retrieve topics failed.");
                return;
            }

            Console.WriteLine("");
            Console.WriteLine("The following topics were returned:");
            foreach (var topic in returnedTopics.Topics)
            {
                Console.WriteLine(String.Format("{0} (weight {1})", topic.Topic, topic.Weight.ToString()));
            }
        }

        // Extracting text from documents
        public async static Task<string> ExtractTextFromUrl(string url)
        {
            var encodedUrl = HttpUtility.JavaScriptStringEncode(url, true);
            var content = new StringContent(String.Format("{{\"url\": {0}}}", encodedUrl), Encoding.UTF8, "application/json");

            return await ExtractText(content);
        }

        public async static Task<string> ExtractTextFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found.");
                return null;
            }

            byte[] bytes = File.ReadAllBytes(path);
            string encoded = Convert.ToBase64String(bytes);
            var content = new StringContent(String.Format("{{\"coords\": false, \"fileAsBase64\": \"{0}\"}}", encoded), Encoding.UTF8, "application/json");
            return await ExtractText(content);
        }

        private async static Task<string> ExtractText(StringContent content)
        {
            var result = await client.PostAsync(LabsTextExtractorUrl, content);
            
            Console.WriteLine(String.Format("Extract Text Result: {0}", result.ReasonPhrase));

            if (result.IsSuccessStatusCode)
            {
                var resultStream = await result.Content.ReadAsStreamAsync();
                var serializer = new DataContractJsonSerializer(typeof(ExtractedText));
                var response = serializer.ReadObject(resultStream) as ExtractedText;
                return response.Text;
            }
            return null;
        }
        // End text extraction

        // Topic inference
        private async static Task<InferredTopics> GetTopics(string text)
        {
            var encodedText = HttpUtility.JavaScriptStringEncode(text, true);
            var content = new StringContent(String.Format("{{\"text\": {0}}}",
            encodedText), Encoding.UTF8, "application/json");

            var result = await client.PostAsync(LabsTopicsUrl, content);

            Console.WriteLine(String.Format("Get Topics Result: {0}", result.ReasonPhrase));

            if (result.IsSuccessStatusCode)
            {
                var resultStream = await result.Content.ReadAsStreamAsync();
                var serializer = new DataContractJsonSerializer(typeof(InferredTopics));
                var response = serializer.ReadObject(resultStream) as InferredTopics;
                return response;
            }
            return null;
        } 
    }
}