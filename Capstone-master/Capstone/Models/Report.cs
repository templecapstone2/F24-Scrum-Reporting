namespace Capstone.Models
{
    public class Report
    {
        private int id;
        private String name;
        private DateTime dateOpen;
        private DateTime dateDue;
        private bool isActive;
        private int courseID;

        public Report() { }

        public Report(int id, string name, DateTime dateOpen, DateTime dateDue, bool isActive, int courseID)
        {
            this.id = id;
            this.name = name;
            this.dateOpen = dateOpen;
            this.dateDue = dateDue;
            this.isActive = isActive;
            this.courseID = courseID;
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

        public DateTime DateOpen
        {
            get { return dateOpen; }
            set { dateOpen = value; }
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

        public int CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }
    }
}
