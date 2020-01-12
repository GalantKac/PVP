const shortId = require('shortid');

module.exports = class Player {
    constructor(){
        this.username = '';
        this.id = shortId.generate();
    }
}