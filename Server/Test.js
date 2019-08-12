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

    let _Question = Question.sort(() => Math.random() -0.5)
    console.log(_Question)