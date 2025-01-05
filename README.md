# .NET Claude Wrapper API

A .NET Web API wrapper for Anthropic's Claude AI, featuring user authentication, chat history storage, and admin functionality.

## Features

- 🔐 User Authentication with JWT
- 💾 Chat History Storage with SQLite
- 👤 Default Admin Account
- 🔄 Conversation Management
- ⚙️ Configurable System Prompts

## Prerequisites

- .NET 8.0 or higher
- Anthropic API key
- SQLite

## Setup

1. Clone the repository:
```bash
git clone https://github.com/ivanjurina/dotnet-claude-wrapper-api
cd dotnet-claude-wrapper-api
```

2. Update appsettings.json:
```json
{
  "Claude": {
    "ApiKey": "YOUR_API_KEY_HERE"
  },
  "Jwt": {
    "Key": "your-secret-key-here-make-it-long-and-secure"
  }
}
```

3. Apply database migrations:
```bash
dotnet ef migrations add Initial
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

## Default Admin Account

- Username: `admin`
- Password: `admin`

⚠️ Change the admin password after first login!

## API Endpoints

### Authentication
```http
POST /auth/login
Content-Type: application/json

{
    "username": "admin",
    "password": "admin"
}
```

### Change Password
```http
POST /auth/change-password
Authorization: Bearer {token}
Content-Type: application/json

{
    "currentPassword": "admin",
    "newPassword": "newpassword"
}
```

### Chat
```http
POST /chat
Authorization: Bearer {token}
Content-Type: application/json

{
    "message": "Hello Claude!",
    "conversationId": "optional-id"
}
```

### Admin System Prompt
```http
GET /admin/system-prompt
Authorization: Bearer {token}

POST /admin/system-prompt
Authorization: Bearer {token}
Content-Type: application/json

{
    "systemPrompt": "You are a helpful assistant."
}
```

## Database Schema

- Users: Store user credentials and roles
- Chats: Track conversations
- Messages: Store chat history

## Security Features

- Password hashing with ASP.NET Core Identity
- JWT authentication
- Role-based authorization
- Secure password change mechanism

## Acknowledgments

- Uses [Anthropic's Claude API](https://docs.anthropic.com/claude/reference)
- Built with ASP.NET Core
- SQLite for data storage
