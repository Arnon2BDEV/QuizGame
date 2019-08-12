var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var RoomSchema = new Schema({
    id:{type:String,unique:true},
    gamepin:{type: String,unique:true},
    player:[],
    owner:[],
    status:{type:Number}
});

const Account = mongoose.model('Room',RoomSchema,'Room');
module.exports = Account;