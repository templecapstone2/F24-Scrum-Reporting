namespace Capstone.Models
{
    public class Team
    {
        private int id;
        private string name;

        public Team() { }

        public Team(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
    }
}
