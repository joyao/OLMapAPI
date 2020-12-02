using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLMapAPI.Models.apiUser
{
    public class apiUserClass
    {
    }
    /// <summary>
    /// apiUser
    /// </summary> 
    public class apiUserObj
    {
        public string id;
        public string userId;
        public string unit;
        public string note;
        public string password;
        public string lockYN;

        public apiUserObj(string _id, string _userId, string _unit, string _note, string _password, string _lockYN)
        {
            id = _id;
            userId = _userId;
            unit = _unit;
            note = _note;
            password = _password;
            lockYN = _lockYN;
        }
        public apiUserObj() { }
    }

    public class insertApiUserObj
    {
        public string userId;
        public string unit;
        public string note;
        public string password;
        public string lockYN;

        public insertApiUserObj(string _userId, string _unit, string _note, string _password)
        {
            userId = _userId;
            unit = _unit;
            note = _note;
            password = _password;
            lockYN = "0";
        }
        public insertApiUserObj() { }
    }

    /// <summary>
    /// removeApiUserObj
    /// </summary> 
    public class removeApiUserObj
    {
        public string id;

        public removeApiUserObj(string _id)
        {
            id = _id;
        }
        public removeApiUserObj() { }
    }

    /// <summary>
    /// lockApiUserObj
    /// </summary> 
    public class lockApiUserObj
    {
        public string id;

        public lockApiUserObj(string _id)
        {
            id = _id;
        }
        public lockApiUserObj() { }
    }

}