using System.Text.Json.Serialization;

namespace DataAPI.Models
{
    public class Team
    {
        private int id;
        private string name;

        public Team() { }

        public Team(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        [JsonPropertyName("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonPropertyName("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
