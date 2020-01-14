const jsonServer = require('json-server');
const auth = require('json-server-auth');
const socket = require('socket.io');

const app = jsonServer.create();
const router = jsonServer.router('data/db.json');

app.db = router.db;

//middleware
app.use(auth);
app.use(router);

// app.listen(3000);
const io = socket(app.listen(3000, () => {
    console.log('App running on port 3000');
}));

const Player = require('./Classes/Player');

let players = [];
let sockets = [];

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