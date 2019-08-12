var io = require('socket.io')(3000);
var mongoose = require('mongoose');
var shortid = require('shortid');
mongoose.connect('mongodb://localhost:27017/MultiplayerQuizGame',{useNewUrlParser: true});
var db = mongoose.connection;
const Account = require('./models/user.model');
const Room = require('./models/room.model');

db.on('error',console.error.bind(console,'connection error:'));
db.once('open',function(){console.log("Connected to Db ");});

var clients = [];

io.on('connection',function(socket)
{
    var currentUser;
    console.log("Clents is connect");
    socket.on('SETNAME',function(data)
    {
        console.log("Finding")
        var createAccount = new Account({
                                        id:shortid.generate(),
                                        name:data.name,
                                        score:0,
                                        status:0});
            currentUser = createAccount;
        Account.find({name:data.name},function(err,result)
        {
            if(result[0] == null)
                {
                    console.log("Can use this name");
                    createAccount.save(function(err,Account)
                    {
                        console.log("Insert done");
                        clients.push(currentUser);
                        if(err) return console.error(err);
                        else 
                        {
                            socket.emit('N1',createAccount);
                            socket.broadcast.emit('USER_CONNECTED',currentUser);
                            return;
                        }
                    });
                }
            else 
            {
                console.log("Cannot use this name");
                socket.emit('CUNAME',"Error");
                return;
            }
        });
            
    });
        

    socket.on('disconnect',function()
    {
        console.log(clients.length);
        for(var i=0;i<clients.length;i++)
        {
            console.log("MMMMMMMM")
            if(clients[i].name == currentUser.name && clients[i].id == currentUser.id)
            {
                console.log("User : "+clients[i].name+"  id :"+clients[i].id+"has disconnected");
                clients.splice(i,1);
                Account.deleteOne({name:currentUser.name},async function(err)
                {if(err) console.error(err); 
                    else{
                        console.log("Call back return");
                        await socket.broadcast.emit('USER_DISCONNECTED',currentUser);
                        await console.log("Call back has no return");
                    }
                });
            }
        }
        console.log(clients.length);
        return;
    });

});

console.log("-------------Server Started-----------");