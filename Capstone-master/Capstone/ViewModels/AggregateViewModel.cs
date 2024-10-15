using Capstone.Models;

namespace Capstone.ViewModels
{
    public class AggregateViewModel
    {
        private List<Response> responses;
        private List<Scrum> scrums;
        private List<Team> teams;
        private List<User> students;
        private List<TeamUser> teamUsers;

        public AggregateViewModel() { }

        public AggregateViewModel(List<Response> responses, List<Scrum> scrums, List<Team> teams, List<User> students, List<TeamUser> teamUsers)
        {
            this.responses = responses;
            this.scrums = scrums;
            this.teams = teams;
            this.students = students;
            this.teamUsers = teamUsers;
        }

        public List<Response> Responses
        {
            get { return responses; }
            set { responses = value; }
        }

        public List<Scrum> Scrums
        {
            get { return scrums; }
            set {  scrums = value; }
        }

        public List<Team> Teams
        {
            get { return teams; }
            set { teams = value; }
        }

        public List<User> Students
        {
            get { return students; }
            set { students = value; }
        }

        public List<TeamUser> TeamUsers
        {
            get { return teamUsers; }
            set { teamUsers = value; }
        }
    }
}
