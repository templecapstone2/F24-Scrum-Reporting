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

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string QuestionOne
        {
            get { return questionOne; }
            set { questionOne = value; }
        }

        public string QuestionTwo
        {
            get { return questionTwo; }
            set { questionTwo = value; }
        }

        public string QuestionThree
        {
            get { return questionThree; }
            set { questionThree = value; }
        }

        public DateTime DateSubmitted
        {
            get { return dateSubmitted; }
            set { dateSubmitted = value; }
        }

        public int ScrumID
        {
            get { return scrumID; }
            set { scrumID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
