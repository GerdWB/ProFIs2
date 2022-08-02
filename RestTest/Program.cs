namespace RestTest
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Model;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var data = new ProFis2Data
            {
                DocKey = "1234ABCD",
                InstId = "76E4511230291EDCB1C796FAE1FA805B",
                TpyeId = "BUS20900",
                CatId = "BO",
                DocumentName = "test.docx",
                Date = DateTime.UtcNow.ToString("DD.MM.YYYY hh:mm:ss"),
                Document = File.ReadAllBytes("test_19.07.2022.docx")
            };

            var jsonString = JsonSerializer.Serialize(data);

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7250/ProFis2");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync("PF", data);
            response.EnsureSuccessStatusCode();
        }
    }
}