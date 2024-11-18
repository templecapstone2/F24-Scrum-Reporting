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

        public List<Response> sortResponses(List<Response> responses)
        {

            if (responses == null || responses.Count == 0)
            {
                return new List<Response>();
            }

            List<Response> sortedResponses = new List<Response>();
            Dictionary<int, Scrum> scrumDict = new Dictionary<int, Scrum>();
            Dictionary<int, User> userDict = new Dictionary<int, User>();
            Dictionary<int, Team> teamDict = new Dictionary<int, Team>();
            Dictionary<int, TeamUser> teamUserDict = new Dictionary<int, TeamUser>();

            foreach (var scrum in scrums)
            {
                if (!scrumDict.ContainsKey(scrum.ID))
                {
                    scrumDict.Add(scrum.ID, scrum);
                }
            }
            foreach (var student in students)
            {
                if (!userDict.ContainsKey(student.ID))
                {
                    userDict.Add(student.ID, student);
                }
            }
            foreach (var teamUser in teamUsers)
            {
                if (!teamUserDict.ContainsKey(teamUser.UserID))
                {
                    teamUserDict.Add(teamUser.UserID, teamUser);
                }
            }
            foreach (var team in teams)
            {
                if (!teamDict.ContainsKey(team.ID))
                {
                    teamDict.Add(team.ID, team);
                }

            }
            foreach (var response in responses)
            {
                if (teamUserDict.ContainsKey(response.UserID))
                {
                    sortedResponses.Add(response);
                }
            }

            sortedResponses.Sort((x, y) =>
            {
                string scrumNameX = "";
                string scrumNameY = "";

                if (scrumDict.ContainsKey(x.ScrumID))
                {
                    scrumNameX = scrumDict[x.ScrumID].Name;
                }

                if (scrumDict.ContainsKey(y.ScrumID))
                {
                    scrumNameY = scrumDict[y.ScrumID].Name;
                }

                int scrumComparison = string.Compare(scrumNameX, scrumNameY, StringComparison.Ordinal);
                if (scrumComparison != 0) return scrumComparison;

                string teamNameX = "";
                string teamNameY = "";

                if (teamUserDict.ContainsKey(x.UserID) && teamDict.ContainsKey(teamUserDict[x.UserID].TeamID))
                {
                    teamNameX = teamDict[teamUserDict[x.UserID].TeamID].Name;
                }

                if (teamUserDict.ContainsKey(y.UserID) && teamDict.ContainsKey(teamUserDict[y.UserID].TeamID))
                {
                    teamNameY = teamDict[teamUserDict[y.UserID].TeamID].Name;
                }

                int teamComparison = string.Compare(teamNameX, teamNameY, StringComparison.Ordinal);
                if (teamComparison != 0) return teamComparison;

                string lastNameX = "";
                string lastNameY = "";

                if (userDict.ContainsKey(x.UserID))
                {
                    lastNameX = userDict[x.UserID].LastName;
                }

                if (userDict.ContainsKey(y.UserID))
                {
                    lastNameY = userDict[y.UserID].LastName;
                }

                return string.Compare(lastNameX, lastNameY, StringComparison.Ordinal);
            });

            return sortedResponses;
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
