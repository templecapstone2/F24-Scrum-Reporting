using Capstone.Models;

namespace Capstone.Services
{
    public class UserService
    {
        //string baseURL; // published url
        string baseURL = "http://localhost:5182/api/User"; // test url
        private readonly HttpClient httpClient;


        public UserService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<User>>($"{baseURL}/users");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while getting users: " + ex.Message);
                return new List<User>();
            }
        }

        public async Task<List<User>> GetStudents()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<User>>($"{baseURL}/students");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while getting students: " + ex.Message);
                return new List<User>();
            }
        }
    }
}
