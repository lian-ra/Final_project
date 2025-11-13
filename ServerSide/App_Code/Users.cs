using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Users
/// </summary>
public class Users
{
    public Users()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string usern;
    public string UserN
    {
        get { return usern; }
        set { usern = value; }
    }

    private string pass;
    public string Pass
    {
        get { return pass; }
        set { pass = value; }
    }
    private string Firstn;
    public string NameF
    {
        get { return Firstn; }
        set { Firstn = value; }
    }

    private string lastn;
    public string LastN
    {
        get { return lastn; }
        set { lastn = value; }
    }

    private string fulladdres;
    public string Fulladdres
    {
        get { return fulladdres; }
        set { fulladdres = value; }
    }

    private string email;
    public string Email
    {
        get { return email; }
        set { email = value; }
    }
    private string pic;
    public string Pic
    {
        get { return pic; }
        set { pic = value; }
    }
    private string gender;
    public string Gender
    {
        get { return gender; }
        set { gender = value; }
    }
    private string birthday;
    public string Birthday
    {
        get { return birthday; }
        set { birthday = value; }
    }
    private string phoneN;
    public string PhoneN
    {
        get { return phoneN; }
        set { phoneN = value; }
    }
    
}