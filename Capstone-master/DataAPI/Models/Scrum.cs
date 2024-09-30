namespace DataAPI.Models
{
    public class Scrum
    {
        private int id;
        private String name;
        private DateTime dateDue;
        private bool isActive;

        public Scrum() { }

        public Scrum(int id, string name, DateTime dateDue, bool isActive)
        {
            this.id = id;
            this.name = name;
            this.dateDue = dateDue;
            this.isActive = isActive;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime DateDue
        {
            get { return dateDue; }
            set { dateDue = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
    }
}
