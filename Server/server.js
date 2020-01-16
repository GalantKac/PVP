const socket = require('socket.io');
const app = require('./app');
const db = require('./data');

db.initialiseDatabase(false, null);
const io = socket(app.listen(process.env.PORT || 3000, () => {
    console.log('App running on port 3000 or default');
}));

const User = require('./Classes/User');

let users = [];
let sockets = [];

io.on('connection', (socket) => {
        console.log('Connection mode');

        let user = new User();
        let thisUserId = 0;

        socket.on('loginCompleted', (loginUser) => {

            user = loginUser;
            thisUserId = loginUser.id;

            socket.on('join', () => {
                users[thisUserId] = user;
                sockets[thisUserId] = socket;
                console.log('Join to game Player: ' + thisUserId);

                //wysylanie inforamcji ze sie stowrzylem
                socket.emit('spawn', user);

                // wysylanie inforamcji do innych ze sie stworzylem
                socket.broadcast.emit('spawn', user);

                //powiedz mi kto jest w grze
                for (let playerID in users) {
                    if (playerID != thisUserId) {
                        {
                            socket.emit('spawn', users[thisUserId])
                        }
                    }
                }
            });
        });

        socket.on('disconnect', () => {
            console.log('User disconnected');
            delete users[thisUserId];
            delete sockets[thisUserId];
            socket.broadcast.emit('disconnected', user);
        })
    }
);