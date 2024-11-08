using Capstone.Models;

namespace Capstone.Services
{
    public class ScrumService
    {
        //string baseURL = "https://np-stem.temple.edu/cis4396-f05/api/Scrum"; // published url
        string baseURL = "http://localhost:5050/api/Scrum"; // test url
        private readonly HttpClient httpClient;


        public ScrumService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<bool> AddScrum(Scrum scrum)
        {
            try
            {
                return (await httpClient.PostAsJsonAsync(baseURL, scrum)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding scrum: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Scrum>> GetScrums()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Scrum>>(baseURL);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving scrums: {ex.Message}");
                return new List<Scrum>();
            }
        }

        public async Task<bool> ModifyScrum(int id, Scrum scrum)
        {
            try
            {
                return (await httpClient.PutAsJsonAsync($"{baseURL}/{id}", scrum)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while modifying scrum: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteScrum(int id)
        {
            try
            {
                return (await httpClient.DeleteAsync($"{baseURL}/{id}")).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting scrum: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAllScrums()
        {
            try
            {
                var scrums = await GetScrums(); // Retrieve the list of scrums
                bool allDeleted = true;

                foreach (var Scrum in scrums)
                {
                    // Delete each scrum individually
                    bool deleted = await DeleteScrum(Scrum.id);
                    if (!deleted)
                    {
                        allDeleted = false; // If any deletion fails, set the flag to false
                        Console.WriteLine($"Failed to delete scrum with ID: {Scrum.id}");
                    }
                }

                return allDeleted; // Return true only if all scrums were successfully deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting all scrums: {ex.Message}");
                return false;
            }
        }


    }
}
