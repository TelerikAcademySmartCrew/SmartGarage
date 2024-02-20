"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {

    if (message == "")
        return;
    
    let currentUser = document.getElementById("current-user");
    let ul = document.getElementById("messagesList");
    let li = document.createElement("li");
    li.style.listStyle = "none";
    
    let itsMe = user === currentUser
    let template = !itsMe ? "/MyMessage.html" : "/OthersMessage.html";
    
    fetch(template)
        .then(response => response.text())
        .then(html => {

            li.innerHTML = html;

            if (user)
                li.querySelector(".user").textContent = user;

            li.querySelector(".message").textContent = message;

            ul.appendChild(li);

            ul.scrollTop = ul.scrollHeight;
        })
        .catch(error => console.error("Error fetching template:", error));
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});