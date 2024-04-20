//import {signalR} from "../lib/signalr/dist/browser/signalr";

let message = document.getElementById("message");
let userName = getUserName();
let modelId = getModelData();
console.log("Id: ",modelId);
let chatConnection = new signalR.HubConnectionBuilder().withUrl("/Home/Chat/chat").build();

document.getElementById("send").disabled = true;

chatConnection.on("RecieveMessage", function(userName, message, timestamp){
    console.log(userName+': ' + message);
    try {
        const date = new Date(timestamp);
        const formattedTime = date.toLocaleString('en-GB', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        }).replace(/,/, ' -');

        $("#chat-messages").append(`<span style='color:black;'>${formattedTime} ${userName}: ${message}</br></span>`);
        console.log(`${userName}: ${message}`);
    } catch (error) {
        console.error('Error in RecieveMessage:', error);
    }
});

chatConnection.start().then(function () {
    document.getElementById("send").disabled = false;
    fetchChatMessages(modelId);
    chatConnection.invoke("JoinChat", modelId).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

$("#send").click(function(){
console.log(message.value);
   chatConnection.invoke("SendMessage", parseInt(modelId), userName, message.value);
   message.value="";
});

chatConnection.onclose(function(error) {
    console.error('Connection disconnected:', error);
    console.error('Reconnecting...');

    // Спробуйте повторно підключитися після розриву з'єднання
    chatConnection.start().then(function() {
        console.log('Reconnected successfully');
        document.getElementById("send").disabled = false;
        fetchChatMessages(modelId);
        chatConnection.invoke("JoinChat", modelId).catch(function (err) {
            return console.error(err.toString());
        });
    }).catch(function(err) {
        console.error('Failed to reconnect:', err);
    });
});
async function fetchChatMessages(chatId) {
    try {
        // Виклик методу GetChatMessages для отримання повідомлень чату
        const messages = await chatConnection.invoke("GetChatMessages", chatId);

        for (const message of messages) {
            // Отримайте ім'я користувача за допомогою методу GetUserNameById
            const userName = await chatConnection.invoke("GetUserNameById", message.sender);

            // Обробіть повідомлення
            const date = new Date(message.message_time);
            const formattedTime = date.toLocaleString('en-GB', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
                hour12: false
            }).replace(/,/, ' -');

            // Відобразіть повідомлення з ім'ям користувача
            $("#chat-messages").append(
                `<span style='color:black;'>${formattedTime} ${userName}: ${message.content}</span><br>`
            );
        }
    } catch (err) {
        console.error('Error fetching chat messages:', err);
    }
}



    

