using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RandomQuoteApp
{
    public class Quote
    {
        [JsonProperty("q")]
        public string? Text { get; set; }

        [JsonProperty("a")]
        public string? Author { get; set; }
    }

    class Program
    {
        public static async Task Main()
        {
            Quote quote = await GetRandomQuoteAsync();

            if (quote != null)
            {
                Console.WriteLine($"\"{quote.Text}\"");
                Console.WriteLine($"- {quote.Author}");
            }
            else
            {
                Console.WriteLine("Не удалось получить цитату.");
            }
        }

        public static async Task<Quote> GetRandomQuoteAsync()
        {
            string url = "https://zenquotes.io/api/random";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    Quote[] quotes = JsonConvert.DeserializeObject<Quote[]>(responseBody);

                    return quotes[0];
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Ошибка при запросе: " + e.Message);
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Произошла ошибка: " + e.Message);
                    return null;
                }
            }
        }
    }
}
