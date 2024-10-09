using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class User
    {
        private String id;
        private String email;
        private String name;
        private String userType;

        public User() { }

        public User(string id, string email, string name, string userType)
        {
            this.id = id;
            this.email = email;
            this.name = name;
            this.userType = userType;
        }

        [JsonPropertyName("id")]
        public string ID
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

        [JsonPropertyName("email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [JsonPropertyName("usertype_id")]
        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }
    }
}
