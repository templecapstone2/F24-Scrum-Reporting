using Capstone.Models;

namespace Capstone.ViewModels
{
    public class StudentManagementModel
    {
        private List<User> students;
        private List<Team> teams;
        private List<TeamUser> teamUsers;

        public StudentManagementModel() { }

        public StudentManagementModel(List<User> students, List<Team> teams, List<TeamUser> teamUsers)
        {
            this.students = students;
            this.teams = teams;
            this.teamUsers = teamUsers;
        }

        public List<User> Students
        {
            get { return students; }
            set { this.students = value; }
        }

        public List<Team> Teams
        {
            get { return teams; }
            set { this.teams = value; }
        }

        public List<TeamUser> TeamUsers
        {
            get { return teamUsers; }
            set { this.teamUsers = value; }
        }
    }
}
