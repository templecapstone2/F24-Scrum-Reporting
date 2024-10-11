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

        public async Task<bool> AddUser(User user)
        {
            try
            {
                return (await httpClient.PostAsJsonAsync(baseURL, user)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding user: {ex.Message}");
                return false;
            }
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

        public async Task<bool> DeleteStudents()
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
