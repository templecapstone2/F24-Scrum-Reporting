using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class User
    {
        private int id;
        private string tuid;
        private string firstName;
        private string lastName;
        private string userType;
        public SelectList SelectList { get; set; }

        public User() { }

        public User(int id, string tuid, string firstName, string lastName, string userType)
        {
            this.id = id;
            this.tuid = tuid;
            this.firstName = firstName;
            this.lastName = lastName;
            this.userType = userType;
        }

        [JsonPropertyName("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonPropertyName("tuid")]
        public string TUID
        {
            get { return tuid; }
            set { tuid = value; }
        }

        [JsonPropertyName("first_name")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [JsonPropertyName("last_name")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [JsonPropertyName("user_type")]
        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }
    }
}
