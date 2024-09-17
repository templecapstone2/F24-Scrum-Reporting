namespace Capstone.Models
{
    public class Course
    {
        private int id;
        private int number;
        private int section;
        private String name;
        private int semesterID;

        public Course() { }

        public Course(int id, int number, int section, string name, int semesterID)
        {
            this.id = id;
            this.number = number;
            this.section = section;
            this.name = name;
            this.semesterID = semesterID;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public int Section
        {
            get { return section; }
            set { section = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int SemesterID
        {
            get { return semesterID; }
            set { semesterID = value; }
        }
    }
}
