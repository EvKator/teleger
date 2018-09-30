# teleger

Advanced telegram spammer   

Automatically enters to chats & channels, clicks on any buttons in bots, sends messages.      
This can be configured to work with multiple accounts, each with its own flow and progress bar in the control unit.     
Could be used to automatically earn by performing task in the bots for increasing the number of subscribers, etc (as SpeedMoneybot) or (if you have a lot of accounts) to increase the number of subscribers for money.    


Receives instructions from the file .json you can load by clicking 'load script' button     
Also you can connect file with the their account numbers      
File with numbers - just a bunch of phone numbers, separated by '\n' char.      

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


Demonstration: <https://youtu.be/VIlNSSt0zLU> 
