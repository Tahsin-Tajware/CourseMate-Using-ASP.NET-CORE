# CourseMate

## Overview
CourseMate is a web-based knowledge-sharing platform for university communities, built with ASP.NET Core MVC and C#. It enables students to post questions, receive answers via comments, and engage through features like upvoting/downvoting, academic tagging, anonymous posting, and admin moderation.

## Problem Statement
University students lack reliable, course-specific platforms for peer-reviewed answers outside classroom hours. CourseMate provides a university-focused solution with academic structure and community-driven quality control.

## Objectives
- Allow unrestricted question posting with quality responses.
- Implement upvote/downvote system for comment ranking.
- Integrate academic tagging (course/university info).
- Provide admin controls for content moderation.
- Support anonymous posting for sensitive topics.
- Include a points-based reputation system.
- Ensure scalability with ASP.NET Core MVC and C#.

## Features
- **User Management**: University-based registration, secure authentication, role-based access, reputation system.
- **Post Management**: Unrestricted posts, rich text editor, academic tagging, anonymous posting.
- **Response System**: Comment-based answers, upvoting/downvoting, ranking.
- **Admin Controls**: Content monitoring, abusive content removal, user restrictions, report handling.
- **Interactive Features**: Post saving, notifications, advanced search, trending content.
- **Quality Assurance**: Community flagging, reputation tracking, moderation.

## Societal & Environmental Impact
- **Societal**: Democratizes knowledge, fosters collaboration, ensures quality through voting/moderation.
- **Environmental**: Reduces paper, travel, and physical consultations via digital knowledge sharing.

# Installation

Follow these steps to set up and run the **CourseMate** project locally.

---

## Prerequisites
- [.NET 6.0+ SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) **or** [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

---

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/Tahsin-Tajware/CourseMate-Using-ASP.NET-CORE.git
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

---

### For **Visual Studio 2022** Users
1. Open the solution file (`CourseMate.sln`) in Visual Studio 2022.
2. Restore NuGet packages (automatically handled by the IDE or via `dotnet restore`).
3. i use database postgresql in render so no need for database setup, if locally setup then only change the connection string Update the connection string in `appsettings.json`.
4. Open the **Package Manager Console** and run:
   ```bash
   dotnet ef database update
   ```
5. Build and run the project using the **IIS Express** button or execute:
   ```bash
   dotnet run
   ```

---

### For **Visual Studio Code** Users
1. Open the project folder in VS Code.
2. Install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) if not already installed.
3. Open the integrated terminal and run:
   ```bash
   dotnet restore
   ```
4. i use database postgresql in render so no need for database setup, if locally setup then only change the connection string Update the connection string in `appsettings.json`.
5. Run the migrations:
   ```bash
   dotnet ef database update
   ```
6. Start the application:
   ```bash
   dotnet run
   ```
   The app will be available at:
   - `https://localhost:5001`
   - `http://localhost:5000`


## Usage
- Register as a student/admin.
- Post questions with academic tags.
- Comment, vote, or save posts.
- Admins can moderate content and manage reports.

## Contributors
- A.S.M. Tahsin Tajware (20220104006)
- Abdullah Al Tamim (20220104014)
- Sonod Sadman (20220104025)

## License
MIT License

## Report
For detailed insights, refer to [CourseMate.pdf](CourseMate.pdf).  
Prepared for CSE 3200: Software Development-V, Ahsanullah University of Science and Technology.  
Date: August 19, 2025.
