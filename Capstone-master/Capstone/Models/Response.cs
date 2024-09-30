using System.Text.Json.Serialization;

namespace Capstone.Models
{
    public class Response
    {
        private int id;
        private String questionOne;
        private String questionTwo;
        private String questionThree;
        private DateTime dateSubmitted;
        private int scrumID;
        private int userID;

        public Response() { }

        public Response(int id, string questionOne, string questionTwo, string questionThree, DateTime dateSubmitted, int scrumID, int userID)
        {
            this.id = id;
            this.questionOne = questionOne;
            this.questionTwo = questionTwo;
            this.questionThree = questionThree;
            this.dateSubmitted = dateSubmitted;
            this.scrumID = scrumID;
            this.userID = userID;
        }

        [JsonPropertyName("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [JsonPropertyName("question_one")]
        public string QuestionOne
        {
            get { return questionOne; }
            set { questionOne = value; }
        }

        [JsonPropertyName("question_two")]
        public string QuestionTwo
        {
            get { return questionTwo; }
            set { questionTwo = value; }
        }

        [JsonPropertyName("question_three")]
        public string QuestionThree
        {
            get { return questionThree; }
            set { questionThree = value; }
        }

        [JsonPropertyName("date_submitted")]
        public DateTime DateSubmitted
        {
            get { return dateSubmitted; }
            set { dateSubmitted = value; }
        }

        [JsonPropertyName("scrum_id")]
        public int ScrumID
        {
            get { return scrumID; }
            set { scrumID = value; }
        }

        [JsonPropertyName("user_id")]
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
