# EduAccess

**Helping South African matriculants make informed decisions about life after matric.**

EduAccess is a web-based platform that helps students explore university degrees, diplomas, TVET programmes, and funding opportunities based on their matric results.

## Features

- **Study Passport:** Enter matric subjects and marks and calculate your APS automatically.
- **Course Matching:** Explore qualifications and institutions that match your academic results.
- **Plan B:** Discover alternative pathways, including extended programmes and TVET options.
- **Funding:** Explore NSFAS information and bursary opportunities.
- **AI Chatbot:** Ask questions about universities, courses, and bursaries.
- **Face Login:** Facial recognition login using Face-API.js. This feature is still under development.
- **Admin Dashboard:** Access administrative features, platform information, and dashboard charts.

## Technology Stack

- C# and ASP.NET MVC 5
- Entity Framework 6 (Code First)
- SQL Server LocalDB
- HTML, CSS, JavaScript and Bootstrap
- Hugging Face API
- Face-API.js
- Chart.js

## Installation and Setup

### Requirements

- Windows and Visual Studio
- The .NET Framework version required by the project
- SQL Server LocalDB
- Git
- Hugging Face API token for chatbot functionality

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/Kwazi34/EduAcess.git
   ```

2. Open `EduAcess.sln` in Visual Studio.
3. Configure the database connection string and required API settings in `Web.config`.
4. Restore the required NuGet packages.
5. Open **Tools → NuGet Package Manager → Package Manager Console** and run:

   ```powershell
   Update-Database
   ```

6. Set the web project as the startup project.
7. Press **F5** to build and run the application.

The application seeds the default identity roles, administrator account, institutions, and bursary data during startup.

## How to Use EduAccess

### Student

1. Register an account and log in.
2. Complete your Study Passport with your matric subjects and marks.
3. Review your calculated APS.
4. Explore matching courses and institutions.
5. Check alternative study pathways if you do not meet the requirements for your preferred course.
6. Browse funding information and ask the chatbot questions.

Always verify admission requirements and funding deadlines with the relevant institutions or official providers.

### Administrator

1. Start the application and open the login page.
2. Sign in using the administrator credentials below.
3. Access the Admin Dashboard.
4. Use the available administrative features to review platform information and supported records.
5. Log out when finished.

## Default Administrator Account

The administrator account is created automatically on application startup if it does not already exist.

- **Email:** `admin@eduaccess.co.za`
- **Default password:** `Admin@12345`
- **Role:** Admin

The code sets `MustChangePassword = true`, but the application must explicitly enforce this flag for a password change to be mandatory. If the administrator account already exists, the seeding code does not reset its password.

**Security:** Change the default password before public deployment. Never publish production credentials or API keys in a public repository.

## Current Limitations

- The database contains approximately 92 institutions and 100 programmes, so coverage is incomplete.
- Face login may fail on some devices.
- Chatbot usage depends on API availability and rate limits.
- Some bursary information requires updating.
- The onboarding page needs further mobile responsiveness improvements.

EduAccess is a guidance platform and does not guarantee admission or funding. Students should confirm information with official providers.

## Repository

GitHub: https://github.com/Kwazi34/EduAcess

**Project goal:** Make post-matric opportunities easier to discover so students can make informed decisions about their future.
