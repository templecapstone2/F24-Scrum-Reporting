using System.Text.Json.Serialization;

namespace DataAPI.Models
{
    public class TeamUser
    {
        private int id;
        private int teamID;
        private int userID;

        public TeamUser() { }

        public TeamUser(int id, int teamID, int userID)
        {
            this.id = id;
            this.teamID = teamID;
            this.userID = userID;
        }

        [JsonPropertyName("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonPropertyName("team_id")]
        public int TeamID
        {
            get { return teamID; }
            set { teamID = value; }
        }

        [JsonPropertyName("user_id")]
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
