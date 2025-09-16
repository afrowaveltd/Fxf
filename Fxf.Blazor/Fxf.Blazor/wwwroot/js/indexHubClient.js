// indexHubClient.js
// SignalR client for connecting to the IndexHub on the server.
// Handles connection lifecycle, status tracking, and automatic reconnection.

"use strict";

// Tracks the current connection status ("Disconnected", "Connecting", "Connected").
let connectionStatus = "Disconnected";

// Create a SignalR connection to the IndexHub endpoint.
const indexHubConnection = new signalR.HubConnectionBuilder()
      .withUrl("/index_hub")
      .configureLogging(signalR.LogLevel.Information)
   .build();

// Starts the SignalR connection and updates status.
const start = async () => {
   connectionStatus = "Connecting";
      try {
         await indexHubConnection.start();
         connectionStatus = "Connected";
         console.log("SignalR Connected.");
      } catch (err) {
         connectionStatus = "Disconnected";
         console.log(err);
         setTimeout(start, 5000); // Retry connection after 5 seconds
      }
}

// Handles connection closure and attempts to reconnect.
indexHubConnection.onclose(async () => {
   connectionStatus = "Disconnected";
   await start();
});

// Initiate the connection when the script loads.
start();