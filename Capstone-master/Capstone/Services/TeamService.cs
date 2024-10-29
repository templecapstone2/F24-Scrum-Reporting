using Capstone.Models;

namespace Capstone.Services
{
    public class TeamService
    {
        //string baseURL = "https://np-stem.temple.edu/cis4396-f05/api/Team"; // published url
        string baseURL = "http://localhost:5050/api/Team"; // test url
        private readonly HttpClient httpClient;


        public TeamService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<bool> AddTeam(Team team)
        {
            try
            {
                return (await httpClient.PostAsJsonAsync(baseURL, team)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding team: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Team>> GetTeams()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Team>>(baseURL);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving teams: {ex.Message}");
                return new List<Team>();
            }
        }

        public async Task<bool> DeleteTeam(int id)
        {
            try
            {
                return (await httpClient.DeleteAsync($"{baseURL}/{id}")).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting team: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAllTeams()
        {
            try
            {
                // Retrieve all teams first
                var teams = await GetTeams();
                bool allDeleted = true; // Flag to track deletion success for all teams

                // Loop through each team and delete them
                foreach (var team in teams)
                {
                    bool success = await DeleteTeam(team.id);
                    if (!success)
                    {
                        Console.WriteLine($"Failed to delete team with ID: {team.id}");
                        allDeleted = false; // If any deletion fails, set the flag to false
                    }
                }

                return allDeleted; // Return true if all deletions succeeded,
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting all teams: {ex.Message}");
                return false; // error
            }
        }
    }
}
