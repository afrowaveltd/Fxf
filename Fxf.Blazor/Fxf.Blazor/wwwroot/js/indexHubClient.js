// indexHubClient.js
// SignalR client for connecting to the IndexHub on the server.
// Handles connection lifecycle, status tracking, and automatic reconnection.

"use strict";

// Tracks the current connection status ("Disconnected", "Connecting", "Connected").
let connectionStatus = "Connecting";

// Declare local constants and variables
const userElementTopbar = document.getElementById("topbar_user");
const connectedUser = '<span class="bi bi-person-fill" style="color: green" id="connected_users_count"></span>';
const disconnectedUser = '<span class="bi bi-person-fill" style="color: red"  id="connected_users_count"></span>';
const connectingUser = '<span class="bi bi-person-fill" style="color: orange"  id="connected_users_count"></span>';

// Create a SignalR connection to the IndexHub endpoint.
const indexHubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/index_hub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Starts the SignalR connection and updates status.
const start = async () => {
    userElementTopbar.innerHTML = connectingUser;
    connectionStatus = "Connecting";
    try {
        connectionStatus = "Connecting";
        userElementTopbar.innerHTML = connectingUser;
        await indexHubConnection.start();
        connectionStatus = "Connected";
        userElementTopbar.innerHTML = connectedUser;
        console.log("SignalR Connected.");
    } catch (err) {
        connectionStatus = "Connectiong";
        userElementTopbar.innerHTML = disconnectedUser
        console.log(err);
        setTimeout(start, 5000); // Retry connection after 5 seconds
    }
}

// Handles connection closure and attempts to reconnect.
indexHubConnection.onclose(async () => {
    connectionStatus = "Disconnected";
    await start();
});

const reloadStatus = () => {
    setInterval(() => {
        if (connectionStatus == "Disconnected")
            userElementTopbar.innerHTML = disconnectedUser
        else if (connectionStatus == "Connected")
            userElementTopbar.innerHTML = connectedUser
        else
            userElementTopbar.innerHTML = connectingUser
    }, 50);
}

// Initiate the connection when the script loads.
start();
reloadStatus();