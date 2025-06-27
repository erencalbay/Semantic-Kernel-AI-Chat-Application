# Semantic Kernel AI Chat Application

This repository contains a basic full-stack AI-powered chat application built with .NET 8, [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel), SignalR, and a modern JavaScript UI. The backend leverages [OpenRouter](https://openrouter.ai/) as an API gateway to access advanced AI models, specifically Google's Gemini, for real-time conversational AI. The frontend is a simple, responsive chat interface that communicates with the backend via SignalR and REST.

---

## Table of Contents

- [Features](#features)
- [Architecture Overview](#architecture-overview)
- [OpenRouter & Gemini Integration](#openrouter--gemini-integration)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Backend Setup](#backend-setup)
  - [UI Setup](#ui-setup)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Customization](#customization)
- [Troubleshooting](#troubleshooting)
- [License](#license)

---

## Features

- **Real-time chat** with streaming AI responses using SignalR.
- **OpenAI-compatible API** via [OpenRouter](https://openrouter.ai/) for flexible model access.
- **Google Gemini model** integration for advanced conversational capabilities.
- **Plugin support** for extending AI with custom functions (e.g., calculator, product info).
- **Modern UI** with npm-based dependency management.
- **.NET 8** and **C#** backend for performance and scalability.

---

## Architecture Overview

- **Backend:** ASP.NET Core Web API (`SemanticKernel.SignalR.Streaming.Handler`)
  - Hosts SignalR hub for real-time communication.
  - Exposes REST endpoints for chat and plugin functions.
  - Integrates with OpenRouter and Gemini via Semantic Kernel.
- **Frontend:** Static JavaScript UI (`UI`)
  - Uses npm for dependency management.
  - Connects to backend SignalR hub and REST endpoints.
  - Provides a responsive chat interface.

---

## OpenRouter & Gemini Integration

### What is OpenRouter?

[OpenRouter](https://openrouter.ai/) is a universal API gateway that allows you to access a wide variety of AI models (including OpenAI, Google, Anthropic, and more) using a single API key and endpoint. This project uses OpenRouter to access the Gemini model.

### What is Gemini?

[Gemini](https://deepmind.google/technologies/gemini/) is Google's family of next-generation AI models, designed for advanced reasoning, conversation, and content generation. In this project, the Gemini model is accessed via OpenRouter's API.

### How is it used here?

- The backend is configured to use OpenRouter's endpoint and your API key.
- The Gemini model is specified as the target model for chat completion.
- All chat requests from the UI are routed through your backend, which streams responses from Gemini via OpenRouter.

---

## Project Structure

```
Semantic-Kernel-AI-Chat-Application/
│
├── SemanticKernel.SignalR.Streaming.Handler/   # .NET 8 backend (SignalR, API, plugins)
│   ├── Program.cs
│   ├── Services/
│   ├── Plugins/
│   ├── Hubs/
│   └── ...
│
├── Product.API/                                # Example plugin API (products)
│
├── UI/                                         # Frontend (npm, JS, HTML, CSS)
│   ├── index.html
│   ├── app.js
│   ├── style.css
│   ├── package.json
│   └── ...
│
└── ...
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/) (for UI)
- An [OpenRouter API key](https://openrouter.ai/keys)
- (Optional) Visual Studio 2022 for development

---

### Backend Setup

1. **Clone the repository:**
   ```sh
   git clone https://github.com/yourusername/Semantic-Kernel-AI-Chat-Application.git
   cd Semantic-Kernel-AI-Chat-Application
   ```

2. **Configure OpenRouter API Key:**
   - Open `SemanticKernel.SignalR.Streaming.Handler/Program.cs`.
   - Replace the placeholder API key with your actual OpenRouter API key:
     ```csharp
     new OpenAIClient(
         credential: new ApiKeyCredential("sk-..."), // <-- Your OpenRouter API key here
         options: new OpenAIClientOptions
         {
             Endpoint = new Uri("https://openrouter.ai/api/v1")
         })
     ```
   - Ensure the model is set to Gemini:
     ```csharp
     .AddOpenAIChatCompletion(
         modelId: "google/gemini-2.5-flash-lite-preview-06-17",
         ...
     )
     ```

3. **Restore and build the backend:**
   ```sh
   dotnet restore
   dotnet build
   ```

4. **Run the backend:**
   ```sh
   dotnet run --project SemanticKernel.SignalR.Streaming.Handler
   ```
   - The backend will start on `https://localhost:7091` (see `launchSettings.json`).

---

### UI Setup

> **Important:** The UI uses npm for dependency management. You must install dependencies before running or opening `index.html`.

1. **Navigate to the UI folder:**
   ```sh
   cd UI
   ```

2. **Install npm dependencies:**
   ```sh
   npm install
   ```
   - This will install `@microsoft/signalr` and any other dependencies listed in `package.json`.

3. **(Optional) Start a local static server for the UI:**
   - You can use [express](https://www.npmjs.com/package/express) or any static server.
   - Example using express (already in your dependencies):
     ```sh
     npm install express
     node server.js
     ```
     *(Create a simple `server.js` if not present: see below)*

   - Or use [live-server](https://www.npmjs.com/package/live-server):
     ```sh
     npx live-server
     ```

4. **Open the UI:**
   - Open `http://localhost:3000` (or the port your static server uses).
   - Alternatively, you can open `index.html` directly, but some browsers may block SignalR or API requests due to CORS or file protocol restrictions.

---

## Usage

- Enter your message in the chat input and click "Gönder".
- The UI sends your prompt to the backend via REST and SignalR.
- The backend streams the Gemini model's response in real time.
- Plugin functions (like calculator or product info) can be triggered by specific prompts.

---

## API Endpoints

- **SignalR Hub:**  
  `https://localhost:7091/ai-hub`  
  Used for real-time streaming of AI responses.

- **Chat Endpoint:**  
  `POST https://localhost:7091/chat`  
  Request body:
  ```json
  {
    "prompt": "Your message",
    "connectionId": "SignalR connection id"
  }
  ```

- **Plugins:**  
  - Example: `ProductsPlugin` fetches best-selling products from `Product.API`.

---

## Customization

- **Change AI Model:**  
  Edit the `modelId` in `Program.cs` to use a different model available on OpenRouter.
- **Add Plugins:**  
  Implement new plugins in the `Plugins/` folder and register them in `Program.cs`.
- **UI Styling:**  
  Modify `UI/style.css` for custom themes or layouts.

---

## Troubleshooting

- **UI does not connect / No response:**  
  - Ensure both backend and UI servers are running.
  - Check browser console for CORS or network errors.
  - Make sure npm dependencies are installed in the `UI` folder.
  - Verify your OpenRouter API key is valid and has access to the Gemini model.

- **SignalR connectionId is empty:**  
  - Make sure the backend is running and accessible at the correct URL.
  - Check for errors in the browser console and backend logs.

- **404 or CORS errors:**  
  - Always use a local server to serve the UI, not `file://` protocol.
  - Ensure CORS is enabled in the backend (`UseCors()` is present).


---

## References

- [OpenRouter Documentation](https://openrouter.ai/docs)
- [Google Gemini](https://deepmind.google/technologies/gemini/)
- [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel)
- [SignalR for ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)

---
