const shortId = require('shortid');

module.exports = class User {
    constructor(id,email,password,nick,wins,loses){
        this.id = id;
        this.email = email;
        this.password = password;
        this.nick = nick;
        this.wins = wins;
        this.loses = loses;
    }
}