using ExamPlayground.Models;
using Newtonsoft.Json;
using System.Net;

class MainClass {

    public const string REQUEST_URL = "https://jsonmock.hackerrank.com/api/articles?page=";

    public static async Task<string[]?> TopArticles(int limit) {
        var articles = new List<Article>();
        var page = 1;
        var totalPages = 0;

        // make an intial request to the api
        // and get the total pages
        using (var hc = new HttpClient()) {
            try {
                var stream = await hc.GetStreamAsync(REQUEST_URL + page);
                var reader = new StreamReader(stream);
                var rawString = reader.ReadToEnd();
                
                if (!string.IsNullOrEmpty(rawString)) {
                    var data = JsonConvert.DeserializeObject<ResponseObject>(rawString);
                    articles.AddRange(data.Data);
                    totalPages = data.Total_Pages;
                }

                // if totalPages is not updated after the
                // initial request, exit the method, no
                // data was pulled from the request
                if (totalPages == 0)
                    return null;
            }
            // just ignore the exceptions
            catch (Exception ex) { }
        }


        // make next requests to the api
        // until all pages are completed
        while (page < totalPages) {
            page++;
            using (var hc = new HttpClient()) {
                try {
                    var stream = await hc.GetStreamAsync(REQUEST_URL + page);
                    var reader = new StreamReader(stream);
                    var rawString = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(rawString)) {
                        var data = JsonConvert.DeserializeObject<ResponseObject>(rawString);
                        articles.AddRange(data.Data);
                    }
                }
                // just ignpore the exception
                catch (Exception ex) { }
            }
        }

        // filter all the fetched articles
        var titles = articles
                         .Where(x =>
                            !string.IsNullOrEmpty(x.Title) 
                            || !string.IsNullOrEmpty(x.Story_Title)
                         )
                         .OrderByDescending(x => x.Num_Comments)
                         .ThenByDescending(x => string.IsNullOrEmpty(x.Title) ?  x.Story_ID : x.Title)
                         .Take(limit)
                         .Select(x => x.Title ?? x.Story_Title)
                         .ToArray();

        return titles;
    }

    private 

    static void Main() {
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input) || !input.All(char.IsNumber))
               return;

        var limit = int.Parse(input);

        if (limit > 0) {
            var titles = TopArticles(limit)
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
            if (titles != null && titles.Count() > 0) {
                foreach (var title in titles) {
                    Console.WriteLine(title);
                }
            }
        }
    }

}