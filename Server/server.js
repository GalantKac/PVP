const io = require('socket.io')(process.env.PORT || 3000);

const Player = require('../Server/Classes/Player');

let players = [];
let sockets = [];

console.log('Server has started');

io.on('connection', (socket) => {
        console.log('Connection mode');

        const player = new Player();
        const thisPlayerId = player.id;

        players[thisPlayerId] = player;
        sockets[thisPlayerId] = socket;

        //wysylanie informacji tylko do tego gracza
        socket.emit('register', {id: thisPlayerId});
        // wysylanie inforamcji ze sie stowrzylem
        socket.emit('spawn', player);
        // wysylanie inforamcji do innych ze sie stworzylem
        socket.broadcast.emit('spawn', player);

        //powiedz mi kto jest w grze
        for (let playerID in players) {
            if (playerID != thisPlayerId) {
                {
                    socket.emit('spawn', players[thisPlayerId])
                }
            }
        }

        socket.on('disconnect', () => {
            console.log('User disconnected');
            delete players[thisPlayerId];
            delete sockets[thisPlayerId];
            socket.broadcast.emit('disconnected', player);
        })
    }
);