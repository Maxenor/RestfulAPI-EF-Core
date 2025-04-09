using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestfulAPI.src.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Events related to software, hardware, and the internet.", "Technology" },
                    { 2, "Concerts, festivals, and music workshops.", "Music" },
                    { 3, "Sporting events, competitions, and fitness activities.", "Sports" },
                    { 4, "Culinary events, food festivals, and wine tastings.", "Food &amp; Drink" },
                    { 5, "Exhibitions, theatre performances, and cultural festivals.", "Arts &amp; Culture" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Capacity", "City", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "123 Main St", 500, "New York", "USA", "Conference Center A" },
                    { 2, "456 Innovation Dr", 300, "San Francisco", "USA", "Tech Hub B" },
                    { 3, "789 Community Ave", 1000, "London", "UK", "Event Hall C" }
                });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "Id", "Company", "Email", "FirstName", "JobTitle", "LastName" },
                values: new object[,]
                {
                    { 1, "TechCorp", "alice.smith@example.com", "Alice", "Software Engineer", "Smith" },
                    { 2, "Innovate Ltd.", "bob.johnson@example.com", "Bob", "Project Manager", "Johnson" },
                    { 3, "Data Solutions", "charlie.brown@sample.net", "Charlie", "Data Analyst", "Brown" },
                    { 4, "Global Enterprises", "diana.prince@mail.org", "Diana", "Marketing Specialist", "Prince" },
                    { 5, "Security Inc.", "ethan.hunt@secure.com", "Ethan", "Security Consultant", "Hunt" }
                });

            migrationBuilder.InsertData(
                table: "Speakers",
                columns: new[] { "Id", "Bio", "Company", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Expert in cloud technologies and DevOps practices.", "Cloud Solutions Inc.", "alice.j@example.com", "Alice", "Johnson" },
                    { 2, "Specialist in modern web development frameworks.", "WebDev Experts", "bob.s@example.com", "Bob", "Smith" },
                    { 3, "Data scientist with a focus on machine learning applications.", "Data Insights Co.", "charlie.b@example.com", "Charlie", "Brown" },
                    { 4, "Agile coach and project management professional.", "Agile Transformations Ltd.", "diana.p@example.com", "Diana", "Prince" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CategoryId", "Description", "EndDate", "LocationId", "StartDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Annual conference focusing on the latest trends in technology, AI, and software development.", new DateTime(2025, 10, 17, 17, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Published", "Tech Conference 2025" },
                    { 2, 2, "A curated collection of modern art from renowned artists.", new DateTime(2025, 12, 20, 18, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), "Published", "Art Exhibition: Modern Masters" },
                    { 3, 3, "5k charity run to support local community projects. All ages welcome.", new DateTime(2025, 9, 20, 11, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 9, 20, 8, 30, 0, 0, DateTimeKind.Utc), "Completed", "Community Charity Run" },
                    { 4, 4, "Weekend music festival featuring diverse genres and artists.", new DateTime(2025, 8, 3, 23, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 8, 1, 14, 0, 0, 0, DateTimeKind.Utc), "Completed", "Music Festival: Summer Sounds" },
                    { 5, 1, "Deep dive into advanced C# features and .NET internals.", new DateTime(2025, 11, 26, 16, 30, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 11, 25, 9, 0, 0, 0, DateTimeKind.Utc), "Published", "Advanced C# Workshop" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "LocationId", "Name" },
                values: new object[,]
                {
                    { 1, 500, 1, "Grand Ballroom" },
                    { 2, 50, 1, "Meeting Room 101" },
                    { 3, 50, 1, "Meeting Room 102" },
                    { 4, 300, 2, "Innovation Hall" },
                    { 5, 75, 2, "Workshop Alpha" },
                    { 6, 200, 3, "Lecture Hall C1" },
                    { 7, 40, 3, "Seminar Room C2" }
                });

            migrationBuilder.InsertData(
                table: "EventParticipants",
                columns: new[] { "EventId", "ParticipantId", "AttendanceStatus", "RegistrationDate" },
                values: new object[,]
                {
                    { 1, 1, "Registered", new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 2, "Registered", new DateTime(2025, 1, 16, 11, 30, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, "Attended", new DateTime(2025, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 3, "Registered", new DateTime(2025, 2, 5, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 3, "Cancelled", new DateTime(2025, 3, 10, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 4, "Registered", new DateTime(2025, 3, 11, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 4, "Attended", new DateTime(2025, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 5, "Registered", new DateTime(2025, 4, 2, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 1, "Registered", new DateTime(2025, 5, 5, 9, 30, 0, 0, DateTimeKind.Utc) },
                    { 5, 5, "Attended", new DateTime(2025, 5, 6, 11, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Description", "EndTime", "EventId", "RoomId", "StartTime", "Title" },
                values: new object[,]
                {
                    { 1, "Overview of new features in C# 12.", new DateTime(2025, 10, 20, 10, 30, 0, 0, DateTimeKind.Utc), 1, 1, new DateTime(2025, 10, 20, 9, 0, 0, 0, DateTimeKind.Utc), "Introduction to C# 12" },
                    { 2, "Deep dive into asynchronous programming in .NET.", new DateTime(2025, 10, 20, 12, 30, 0, 0, DateTimeKind.Utc), 1, 2, new DateTime(2025, 10, 20, 11, 0, 0, 0, DateTimeKind.Utc), "Advanced Async Patterns" },
                    { 3, "Learn how to design and build microservices.", new DateTime(2025, 11, 15, 11, 30, 0, 0, DateTimeKind.Utc), 2, 3, new DateTime(2025, 11, 15, 10, 0, 0, 0, DateTimeKind.Utc), "Building Microservices with ASP.NET Core" },
                    { 4, "Tips and tricks for efficient data access with EF Core.", new DateTime(2025, 11, 15, 14, 30, 0, 0, DateTimeKind.Utc), 3, 4, new DateTime(2025, 11, 15, 13, 0, 0, 0, DateTimeKind.Utc), "Entity Framework Core Best Practices" },
                    { 5, "Authentication and authorization strategies for APIs.", new DateTime(2025, 12, 5, 11, 0, 0, 0, DateTimeKind.Utc), 4, 5, new DateTime(2025, 12, 5, 9, 30, 0, 0, DateTimeKind.Utc), "Securing Web APIs" },
                    { 6, "Building interactive web UIs with C# and Blazor.", new DateTime(2025, 12, 5, 13, 0, 0, 0, DateTimeKind.Utc), 5, 1, new DateTime(2025, 12, 5, 11, 30, 0, 0, DateTimeKind.Utc), "Introduction to Blazor" },
                    { 7, "Creating lightweight APIs with minimal code.", new DateTime(2025, 10, 20, 15, 30, 0, 0, DateTimeKind.Utc), 1, 3, new DateTime(2025, 10, 20, 14, 0, 0, 0, DateTimeKind.Utc), "Minimal APIs in .NET 8" }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "Comment", "ParticipantId", "Score", "SessionId" },
                values: new object[,]
                {
                    { 1, "Excellent session, very informative!", 1, 5, 1 },
                    { 2, "Good content, but the speaker was a bit fast.", 2, 4, 2 },
                    { 3, "Average session, met expectations.", 3, 3, 3 },
                    { 4, "Fantastic speaker, very engaging.", 4, 5, 4 },
                    { 5, "Content was not relevant to my interests.", 5, 2, 5 }
                });

            migrationBuilder.InsertData(
                table: "SessionSpeakers",
                columns: new[] { "SessionId", "SpeakerId", "Role" },
                values: new object[,]
                {
                    { 1, 1, "Lead Speaker" },
                    { 2, 2, "Panelist" },
                    { 3, 3, "Keynote" },
                    { 4, 4, "Workshop Facilitator" },
                    { 5, 1, "Co-Speaker" },
                    { 5, 2, "Co-Speaker" },
                    { 6, 3, "Moderator" },
                    { 6, 4, "Panelist" },
                    { 7, 1, "Lead Speaker" },
                    { 7, 3, "Guest Speaker" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "ParticipantId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "SessionSpeakers",
                keyColumns: new[] { "SessionId", "SpeakerId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Speakers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Speakers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Speakers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Speakers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
