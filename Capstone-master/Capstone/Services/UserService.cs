using Capstone.Models;

namespace Capstone.Services
{
    public class UserService
    {
        //string baseURL = "https://np-stem.temple.edu/cis4396-f05/api/User"; // published url
        string baseURL = "http://localhost:5051/api/User"; // test url
        private readonly HttpClient httpClient;


        public UserService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(baseURL, user);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>(); // Deserialize to User
                }
                return null; // Or handle errors as needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding user: {ex.Message}");
                return null;
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
