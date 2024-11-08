using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class Scrum
    {
        public int id;
        private String name;
        private DateTime dateDue;
        private bool isActive;

        public Scrum() { }

        public Scrum(int id, string name, DateTime dateDue, bool isActive)
        {
            this.id = id;
            this.name = name;
            this.dateDue = dateDue;
            this.isActive = isActive;
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

        [JsonPropertyName("date_due")]
        public DateTime DateDue
        {
            get { return dateDue; }
            set { dateDue = value; }
        }

        [JsonPropertyName("is_active")]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
    }
}
