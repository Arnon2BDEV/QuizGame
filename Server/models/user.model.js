var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var AccountSchema = new Schema({
    id:{type:String,indexes:true},
    name:{type: String,unique:true},
    score:{type:Number},
    status:{type:Number}
});

const Account = mongoose.model('Account',AccountSchema,'Account');
module.exports = Account;