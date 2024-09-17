namespace Capstone.Models
{
    public class Response
    {
        public int id;
        public String value;
        public int questionID;
        public int reportID;
        public int userID;

        public Response() { }

        public Response(int id, string value, int questionID, int reportID, int userID)
        {
            this.id = id;
            this.value = value;
            this.questionID = questionID;
            this.reportID = reportID;
            this.userID = userID;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public int QuestionID
        {
            get { return questionID; }
            set { questionID = value; }
        }

        public int ReportID
        {
            get { return reportID; }
            set { reportID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
