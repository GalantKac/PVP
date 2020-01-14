using System;

[Serializable]
public class User
{
    public string email;
    public string password;

    public User(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}