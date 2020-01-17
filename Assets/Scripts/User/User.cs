using System;
using System.Numerics;

[Serializable]
public class User
{
    public int id;
    public string email;
    public string password;
    public string nick;
    public int wins;
    public int loses;
    public string x;
    public string y;

    public User(string email, string password, string nick)
    {
        this.email = email;
        this.password = password;
        this.nick = nick;
    }

    public User(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    public User(string email, string password, string nick, int wins, int loses) : this(email, password, nick)
    {
        this.wins = wins;
        this.loses = loses;
    }

    public User()
    {
    }
}