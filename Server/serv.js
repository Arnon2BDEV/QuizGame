const io = require('socket.io')(3000)
const shortid = require('shortid')
const randomize = require('randomatic')
const mongoose = require('mongoose')

const players = []
const rooms = []
let isAnswer = false;
const Question = [
    {   id: 1,
        question: 'What is the capital city of Thailand ?',
        choices: [{id:1,text:'Bangkok',value: true},
                  {id:2,text:'Nontaburi',value: false},
                  {id:3,text:'Ayuthaya',value: false},
                  {id:4,text:'Saraburi',value: false}],
        answer: 'Bangkok'
    },
    {
        id: 2,
        question: 'What is Sir Isaac Newton Nationality ?',
        choices: [{id:1,text:'English',value: true},
                  {id:2,text:'Spanish',value: false},
                  {id:3,text:'Portuguese',value: false},
                  {id:4,text:'Italian',value: false}],
        answer: 'English'
    },
    {
        id: 3,
        question: 'What name is Jon Snow direwolf in Series Game of thorne ?',
        choices: [{id:1,text:'Nymeria',value: false},
                  {id:2,text:'Grey Wind',value: false},
                  {id:3,text:'Lady',value: false},
                  {id:4,text:'Ghost',value: true}],
        answer: 'Ghost'
    },
    {
        id: 4,
        question: 'Who is ironman ?',
        choices: [{id:1,text:'Robert Stark',value: false},
                  {id:2,text:'Anthony Stark',value: true},
                  {id:3,text:'Bran Stark',value: false},
                  {id:4,text:'Ned Stark',value: false}],
        answer: 'Anthony Stark'
    },
    {
        id: 5,
        question: 'Who is the richest man in the world ?',
        choices: [{id:1,text:'Bill Gates',value: false},
                  {id:2,text:'Warren Buffett',value: false},
                  {id:3,text:'Jeff Bezos',value: true},
                  {id:4,text:'Bernard Arnault',value: false}],
        answer: 'Jeff Bezos'
    },
    {
        id: 6,
        question: 'Which one is fish ? ',
        choices: [{id:1,text:'Dolphin',value: false},
                  {id:2,text:'Whale',value: false},
                  {id:3,text:'Eel',value: true},
                  {id:4,text:'Dugong',value: false}],
        answer: 'Eel'
    },
    {
        id: 7,
        question: 'What is the most populated country in the world ?',
        choices: [{id:1,text:'China',value: true},
                  {id:2,text:'USA',value: false},
                  {id:3,text:'India',value: false},
                  {id:4,text:'Japan',value: false}],
        answer: 'China'
    },
    {
        id: 8,
        question: 'What team is a champion of  UEFA 2018/2019 ?',
        choices: [{id:1,text:'Manchester United',value: false},
                  {id:2,text:'Chelsea',value: false},
                  {id:3,text:'Liverpool',value: true},
                  {id:4,text:'Barcelona',value: false}],
        answer: 'Liverpool'
    },
    {
        id: 9,
        question: 'Which one is Bob marley song ?',
        choices: [{id:1,text:'A lalala long',value: true},
                  {id:2,text:'Sunday morning',value: false},
                  {id:3,text:'Sunflower',value: false},
                  {id:4,text:'Rap God',value: false}],
        answer: 'A lalala long'
    },
    {
        id: 10,
        question: 'Which one is currency of Cambodia ?',
        choices: [{id:1,text:'Kyat',value: false},
                  {id:2,text:'Dong',value: false},
                  {id:3,text:'Rupiah',value: false},
                  {id:4,text:'Riel',value: true}],
        answer: 'Riel'
    }]

io.on('connection',socket =>{

    console.log("Client is connect")

    socket.on('SETNAME',data =>{

        let _players = players
        let checkDuplicate = _players.filter(v => {
            return v.name === data.name
        })
            console.log(checkDuplicate)
            if(checkDuplicate.length){
                console.log("Can not use this name")
                socket.emit('CUNAME')
            }
            else{
                let createAccount = ({id:shortid.generate(),
                    name:data.name,
                    status:0,
                    inroomname:0})
                    players.push(createAccount)
                    socket.emit('NEXT',createAccount)
                    console.log(players)
            }

    })

    socket.on('CREATE_ROOM', data =>{
        console.log('roomname', data.roomname)
        
        let _rooms = rooms
        let checkDuplicate = _rooms.filter(v =>{
            return v.roomname === data.roomname
        })
        console.log(checkDuplicate)
        if(checkDuplicate.length){
            console.log("Can not use this name")
            socket.emit('CUROOMNAME',"error")
        }
        else{
            let createRoom = ({
                id:shortid.generate(),
                roomname:data.roomname,
                roompin:randomize('A0A000',6),
                players:[],
                playerinroom:1,
                status:false,
                result:[]
            })
            let owner = ({
                    id : data.id,
                    name : data.name,
                    status : 1,
                    inroomname: data.roomname})
            createRoom.players.push(owner)
            rooms.push(createRoom)
            socket.emit('INROOM',owner)
            socket.broadcast.emit('ADD_ROOM',createRoom)
            console.log("createroom" + createRoom.players)
        }  
    })

    socket.on('UPDATE_START', data =>{
        socket.emit('UPDATE_ROOM',{rooms})
    })


    socket.on('LOAD', data => {
        console.log("Load : " + data)
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        console.log(rooms[i])
        socket.emit('SETROOM',rooms[i])
        socket.join(rooms[i].roomname)
        })

    socket.on('JOINROOM', data => {
        console.log("Join Event" + data)
        let i = rooms.findIndex(v => v.roomname === data.roomname)
        console.log(rooms[i])
        if(rooms[i].playerinroom<5){
            let player = ({
                id : data.id,
                name : data.name,
                status : 0,
                inroomname: data.roomname
            })
            rooms[i].players.push(player)
            rooms[i].playerinroom += 1
            socket.emit('INROOM',player)
            socket.broadcast.emit('UPDATE',rooms[i])
            socket.to(rooms[i].roomname).emit('ADD_PLAYER',player)
            console.log(rooms[i])
        }
        else if (rooms[i].playerinroom>5)
        {
            console.log("room is full")
            rooms[i].status = true
            socket.emit('FULL')
        }
    })

    socket.on('BACK', data =>{
        console.log("BACK EVENT" + data)
        let i = players.findIndex(v => v.id === data)
        console.log(i)
        players.splice(i,1)
        console.log(players)
        return
    })

    socket.on('EXITROOM', data =>{
        console.log("Exit room " + data)
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        console.log(i)
        let p = rooms[i].players.findIndex(v => v.id === data.id)
        socket.to(rooms[i].roomname).emit('DPLAYER',rooms[i].players[p])
        rooms[i].players.splice(p,1)
        rooms[i].playerinroom -=1
        console.log(rooms[i])
        socket.emit('OUT')
        socket.broadcast.emit('UPDATE',rooms[i])
    })

    socket.on('DESTROY_ROOM',data =>{
        console.log("Destroy room " + data)
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        console.log(i)
        console.log(rooms[i])
        socket.to(rooms[i].roomname).emit('OUT')
        socket.broadcast.emit('DROOM',rooms[i])
        socket.emit('OUT')
        rooms.splice(i,1)
    })

    socket.on('STARTGAME', data=>{
        console.log(data)
        socket.emit('START')
        socket.to(data).emit('START')
    })

    socket.on('LoadResult', data =>{
        console.log(data)
        socket.join(data.inroomname)

        console.log("Result Call")
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        console.log('room[i]: ', rooms[i].result.length)

        if (!rooms[i].result.length) {
            let resultPlayer = rooms[i].players.map(v => ({
                id: v.id,
                name: v.name,
                score: 0
            }))
            rooms[i].result = resultPlayer
        }

        socket.emit('ShowResult',rooms[i])
        console.log("Result : " + rooms[i].result)
    })

    socket.on('LoadQ', data =>{
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        socket.join(data.inroomname)
        let _Question = Question.sort(() => Math.random() -0.5)
        let roundTimer = 5000
        let numberQ = 5
        numberQ = numberQ - 1
        console.log('_Quesion :', _Question[numberQ]);
        io.to(data.inroomname).emit('Q1',_Question[numberQ])
        let setQuestionTimer = setInterval(() => {
            numberQ = numberQ - 1
            console.log('_Quesion :', _Question[numberQ]);
            isAnswer = false
            io.to(data.inroomname).emit('Q1', _Question[numberQ])
            if (numberQ === 0) {
                clearInterval(setQuestionTimer)
                setTimeout(() => {
                    isAnswer = false
                    let newResult = rooms[i].result
                    newResult.sort((a, b) => b.score - a.score)
                    io.to(data.inroomname).emit('gameResult')
               }, roundTimer);
                console.log('end');
            }
        }, roundTimer);
    })

    socket.on('Answer', data => {
        console.log("Answer Event")
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        if (!isAnswer) {
            if (rooms[i].result.length) {
                for (let j = 0; j < rooms[i].result.length; j++) {
                    if (data.name === rooms[i].result[j].name) {
                      rooms[i].result[j].score = rooms[i].result[j].score + 1
                      console.log(data)
                    }
                  }
            } else {
                let resultPlayer = rooms[i].players.map(v => ({
                    id: v.id,
                    name: v.name,
                    score: 0
                }))
                rooms[i].result = resultPlayer
                for (let j = 0; j < rooms[i].result.length; j++) {
                    if (data.name === rooms[i].result[j].name) {
                      rooms[i].result[j].score = rooms[i].result[j].score + 1
                      console.log(data)
                    }
                  }
            }
            isAnswer = true
        }
        
      })

    socket.on('LeaveRoom',data => {
        console.log("Leave" + data)
        let i = rooms.findIndex(v => v.roomname === data.inroomname)
        if(data.status === 1){
            socket.broadcast.emit('DROOM',rooms[i])
            rooms.splice(i,1)
            socket.emit("Leave")
            console.log(rooms)
        }
        else{
            socket.emit("Leave")
        }
      })
})

    

console.log("-------------Server Started-----------")