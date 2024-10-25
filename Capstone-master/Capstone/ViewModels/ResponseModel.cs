using Capstone.Models;

namespace Capstone.ViewModels
{
    public class ResponseModel
    {
        private Scrum scrum;
        private User student;
        private Team team;
        private Response response;

        public ResponseModel() { }

        public ResponseModel(Scrum scrum, User student, Team team, Response response)
        {
            this.scrum = scrum;
            this.student = student;
            this.team = team;
            this.response = response;
        }

        public Scrum Scrum
        {
            get { return scrum; }
            set { scrum = value; }
        }

        public User Student
        {
            get { return student; }
            set { student = value; }
        }

        public Team Team
        {
            get { return team; }
            set { team = value; }
        }

        public Response Response
        {
            get { return response; }
            set { response = value; }
        }
    }
}
