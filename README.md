# Tail Buddys – Backend (.NET)

A real-time matchmaking and messaging backend for dogs and their humans. Built in ASP.NET Core, this backend powers the Tail Buddys platform with real-time chat, AI-enhanced conversations, dog and park management, and full admin controls.

## Table of Contents

- [Features](#features)
- [AI Integration](#ai-integration)
- [Admin and User Access](#admin-and-user-access)
- [Technologies Used](#technologies-used)
- [Installation](#installation)

## Features

- 🐶 Multi-dog account system with full CRUD  
- 📍 Realistic park data with coordinates and addresses across Israel  
- 💬 Real-time SignalR chat between matched dogs  
- 🔔 Notifications for unread messages and new matches  
- 🛠 Admin panel for managing users, dogs, and parks  
- 🔐 Secure login with JWT and optional Google Sign-In  
- 🌱 Data seeding for users, dogs, parks, and chat history  

## AI Integration

Tail Buddys integrates OpenAI GPT-4 Turbo for enhanced messaging:

- **Bot Dog Replies**  
  If one of the matched dogs is a bot, the system generates dynamic, conversational replies using GPT-4 Turbo.

- **(Planned) Dog Image Recognition**  
  In a future version, uploaded dog photos will be analyzed for breed detection.

## Admin and User Access

### Admin Credentials

Username: admin@tail.buddy
Password: Tail1234!

#### Admin Capabilities

- Full CRUD for parks  
- Delete users and dogs  
- Manage park locations, images, and details  

### Regular User Credentials

Username: regular@user.buddy
Password: Tail1234!


- Comes with seeded dogs  
- Has access to all features (except admin routes)

## Technologies Used

- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- SignalR  
- Serilog  
- JWT Authentication  
- AutoMapper  
- OpenAI GPT-4 Turbo  
- Google Maps API  

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/TailBuddys/TailBuddys.git

2. **update the database - for initial Data**
   ```bash
   dotnet ef database update


Run the server and enjoy :)
