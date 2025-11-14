using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDb.App_Code
{
    /// <summary>
    /// Summary description for Celeb
    /// </summary>
    public class Celeb
    {
        public Celeb()
        {
            // TODO: Add constructor logic here
        }

        private int celebId;
        public int CelebId
        {
            get { return celebId; }
            set { celebId = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string role;
        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        private string photo;
        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }

        private string bio;
        public string Bio
        {
            get { return bio; }
            set { bio = value; }
        }
    }
}
