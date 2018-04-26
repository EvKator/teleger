# teleger

Advanced telegram spammer   

Automatically enters chats, channels, clicks on any buttons in bots, sends messages.   
Сan work with multiple accounts, each with its own flow and progress bar in the control unit.   
Could be used to automatically earn by performing task in the bots for increasing the number of subscribers, etc (as SpeedMoneybot) or (if you have a lot of accs) to increase the number of subscribers for money.    

Receives instructions from the file .json you can load by clicking 'load script' button     
Also you need to connect file with the tm account numvers     

File with numbers - just a bunch of phone numvers, separated by '\n' char.    

Script file example:  

    { "arr": 
        [ 
            { 
                "username":"SpeedMoneybot",       
                "script": [      
                    {"sendmsg": "/start"},     
                    {"sendmsg": "➕ Подписаться на канал"},        
                    {"callbackbtn": { "Row":"0", "Btn":"0" } },      
                    {"callbackbtn": { "Row":"1", "Btn":"0" } }     
                ]     
            },     
            {
                "username":"SomeBot",    
                "script": [    
                    {"sendmsg": "/start"},     
                    {"sendmsg": "/stop"} 
                ]     
            }     
        ] 
    }    

You can create a script using <a href='https://github.com/EvKator/TeleScriptBuilder'>TeleScriptBuilder</a>    

Did not publish the whole project, only the sources, which helps to understand how the program works     
I am ready to share the whole project, in personal correspondece    

Contacts:     
Telegram: <a href="http://t.me/u221b">@u221b</a>      
Skype: ev.kator     
email: islyambagirov@gmail.com    

Demonstration: https://www.youtube.com/watch?v=Iu6AqI5Rqos    
