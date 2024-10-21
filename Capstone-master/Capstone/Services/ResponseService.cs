using Capstone.Models;

namespace Capstone.Services
{
    public class ResponseService
    {
        //string baseURL = "https://np-stem.temple.edu/cis4396-f05/api/Response"; // published url
        string baseURL = "http://localhost:5051/api/Response"; // test URL
        private readonly HttpClient httpClient;


        public ResponseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<bool> AddResponse(Response response)
        {
            try
            {
                return (await httpClient.PostAsJsonAsync(baseURL, response)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding response: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Response>> GetResponses()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Response>>(baseURL);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving responses: {ex.Message}");
                return new List<Response>();
            }
        }

        public async Task<bool> ModifyResponse(int id, Response response)
        {
            try
            {
                return (await httpClient.PutAsJsonAsync($"{baseURL}/{id}", response)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while modifying response: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteResponses()
        {
            try
            {
                return (await httpClient.DeleteAsync(baseURL)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting responses: {ex.Message}");
                return false;
            }
        }
    }
}
