# EduAccess

Helps SA matriculants figure out what to do after school.

## What it does
- **Study Passport:** Enter your marks and subjects. It works out your APS automatically.
- **Matches:** Shows you degrees, diplomas and TVET courses you actually qualify for.
- **Plan B:** If you're a few points short, it suggests extended programmes or TVET options.
- **Funding:** Checks if you qualify for NSFAS and lists bursaries closing soon.
- **Chatbot:** You can ask it questions about universities and bursaries.
- **Face ID:** You can log in with your face (still working on this one, it's buggy on some phones).

## Why I built this
I finished matric in 2025 and had no idea what to do. My cousin got 28 APS and thought her life was over because she didn't make it into BCom. She didn't know about extended programmes or TVET. I figured there must be hundreds of thousands of kids like her. So I built this to solve that problem.

## How it works
- ASP.NET MVC 5, C#, Entity Framework (code-first)
- SQL Server LocalDB
- Bootstrap for styling
- HuggingFace for the chatbot (using the free tier)
- Face-API.js for the face login
- Chart.js for the admin graphs

## Running it
1. Clone the repo
2. Add your keys to Web.config
3. Update-Database
4. F5

## TODO (Stuff I still need to fix)
- Face ID fails sometimes on older phones
- The chatbot rate limits if too many people use it
- Need to add more private colleges to the DB
- Mobile view on the Onboarding page is a bit broken

## Notes
- I only seeded about 92 institutions and ~100 programmes because I ran out of time. The full list would be much bigger.
- I hardcoded some of the bursary data but the rest is in the seed file.
- The admin dashboard uses fake "activity feed" timestamps for the demo.
