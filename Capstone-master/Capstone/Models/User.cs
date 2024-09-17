namespace Capstone.Models
{
    public class User
    {
        private int id;
        private String firstName;
        private String lastName;
        private String email;
        private String password;
        private int userTypeID;

        public User() { }

        public User(int id, string firstName, string lastName, string email, string password, int userTypeID)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
            this.userTypeID = userTypeID;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public int UserTypeID
        {
            get { return userTypeID; }
            set { userTypeID = value; }
        }
    }
}
