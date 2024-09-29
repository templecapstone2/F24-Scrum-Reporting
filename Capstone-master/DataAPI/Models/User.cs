﻿namespace DataAPI.Models
{
    public class User
    {
        private int id;
        private String email;
        private String firstName;
        private String lastName;
        private int userTypeID;

        public User() { }

        public User(int id, string email, string firstName, string lastName, int userTypeID)
        {
            this.id = id;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
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

        public int UserTypeID
        {
            get { return userTypeID; }
            set { userTypeID = value; }
        }
    }
}
