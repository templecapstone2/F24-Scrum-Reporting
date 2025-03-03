﻿using Capstone.Models;

namespace Capstone.Services
{
    public class TeamUserService
    {
        //string baseURL = "https://np-stem.temple.edu/cis4396-f05/api/TeamUser"; // published url
        string baseURL = "http://localhost:5050/api/TeamUser"; // test url
        private readonly HttpClient httpClient;


        public TeamUserService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<bool> AddTeamUser(int teamID, int userID)
        {
            try
            {
                return (await httpClient.PostAsync($"{baseURL}/{teamID}/{userID}", null)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding team user: {ex.Message}");
                return false;
            }
        }

        public async Task<List<TeamUser>> GetTeamUsers()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<TeamUser>>(baseURL);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving team users: {ex.Message}");
                return new List<TeamUser>();
            }
        }

        public async Task<bool> ModifyTeamUser(int teamID, int userID)
        {
            try
            {
                return (await httpClient.PutAsync($"{baseURL}/{teamID}/{userID}", null)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while modifying team user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTeamUser(int userID)
        {
            try
            {
                return (await httpClient.DeleteAsync($"{baseURL}/{userID}")).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting team user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTeamUsers()
        {
            try
            {
                return (await httpClient.DeleteAsync(baseURL)).IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting all team users: {ex.Message}");
                return false;
            }
        }
    }
}
