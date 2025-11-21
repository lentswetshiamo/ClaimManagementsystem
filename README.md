Claim Management System
A comprehensive ASP.NET Core MVC web application for managing lecturer claims with automated workflows, validation, and reporting capabilities.

Features
Core Functionality
Persistent Database Storage: Entity Framework Core with SQL Server
Role-Based Access Control: Lecturer, Programme Coordinator, Academic Manager, and HR roles
Auto-Calculation: Real-time claim total calculation based on hours worked and hourly rate
Frontend & Backend Validation: Comprehensive validation with user-friendly error messages
Audit Trail: Complete tracking of all claim actions and changes
Workflow Management: Automated approval workflow with status tracking
Notifications: In-app notifications for status changes
User Roles & Capabilities
Lecturer
Submit new claims with auto-calculated totals
Upload supporting documents (PDF, DOC, DOCX, TXT, JPG, PNG - max 5MB)
Track claim status in real-time
View submission history
Programme Coordinator / Academic Manager
View all submitted claims with advanced filtering
Verify claims before approval
Approve or reject claims with bulk actions
Add rejection reasons
Search and sort claims using DataTables
Export claims data
HR
View approved claims dashboard
Generate monthly reports with date filtering
Export claims to CSV format
View system-wide statistics with charts
Track total hours, amounts, and lecturer counts
Technical Features
Database: SQL Server with Entity Framework Core 8.0
Authentication: Session-based authentication with role management
Security: CSRF protection, secure file uploads, input validation
UI/UX: Responsive Bootstrap 5 design with custom CSS
JavaScript: Auto-calculation, real-time validation, DataTables integration
Export: CSV export functionality for reporting
Prerequisites
.NET 8.0 SDK
SQL Server or SQL Server Express/LocalDB
A modern web browser (Chrome, Firefox, Edge, Safari)
Installation & Setup
1. Clone the Repository
git clone https://github.com/lentswetshiamo/ClaimManagementsystem.git
cd ClaimManagementsystem
2. Configure Database Connection
Update the connection string in appsettings.json if needed:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ClaimManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
For SQL Server, use:

"DefaultConnection": "Server=YOUR_SERVER;Database=ClaimManagementDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=true"
3. Restore Dependencies
dotnet restore
4. Build the Project
dotnet build
5. Initialize Database
The application uses EnsureCreated() for development, which automatically creates the database and seeds demo data on first run.

For production, you should use migrations:

# Create initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
6. Run the Application
dotnet run
The application will start and be available at:

HTTPS: https://localhost:5001
HTTP: http://localhost:5000
Demo Accounts
The system comes pre-seeded with demo accounts:

Email	Password	Role
lecturer@example.com	password123	Lecturer
coordinator@example.com	password123	Programme Coordinator
manager@example.com	password123	Academic Manager
hr@example.com	password123	HR
Usage Guide
Submitting a Claim (Lecturer)
Log in with a lecturer account
Navigate to Dashboard → Submit Claim
Fill in claim details:
Claim Type (e.g., Lecture, Tutorial, Marking)
Description (minimum 10 characters)
Select Lecturer from dropdown
Enter Hours Worked (0-1000)
Enter Hourly Rate (0-10000)
The total amount will be auto-calculated
Click "Submit Claim" (button enables only when validation passes)
Approving Claims (Coordinator/Manager)
Log in with coordinator or manager account
Navigate to Dashboard → View Claims
Use DataTables features to filter/search/sort claims
For individual actions:
Click verify icon to verify a pending claim
Click approve icon to approve a verified claim
Click reject icon to reject a claim (provide reason)
For bulk actions:
Select multiple claims using checkboxes
Click "Approve Selected" or "Reject Selected"
Generating Reports (HR)
Log in with HR account
Navigate to HR Dashboard
Options available:
Monthly Report: Select year/month, view detailed report
Export CSV: Download claims data in CSV format
Statistics: View system-wide statistics with charts
Project Structure
ClaimManagementsystem/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs    # Authentication
│   ├── ClaimController.cs      # Claim management
│   ├── DashboardController.cs  # Main dashboard
│   └── HRController.cs         # HR reporting
├── Data/                 # Database & Repositories
│   ├── DatabaseContext.cs      # EF Core DbContext
│   ├── ClaimRepository.cs      # Claim data access
│   └── UserRepository.cs       # User data access
├── Models/              # Data models
│   ├── Claim.cs         # Claim entity with validation
│   ├── User.cs          # User entity
│   ├── Audit.cs         # Audit trail
│   ├── Notification.cs  # User notifications
│   └── Workflow.cs      # Approval workflow
├── Services/            # Business logic
│   ├── ClaimService.cs        # Claim operations
│   ├── AuthService.cs         # Authentication
│   ├── AuditService.cs        # Audit logging
│   ├── NotificationService.cs # Notifications
│   ├── WorkflowService.cs     # Workflow management
│   └── DocumentService.cs     # File handling
├── Views/               # Razor views
│   ├── Account/         # Login/Register
│   ├── Claim/           # Claim views
│   ├── Dashboard/       # Dashboard
│   ├── HR/              # HR reporting views
│   └── Shared/          # Shared layouts
├── wwwroot/             # Static files
│   ├── css/
│   │   └── claims.css   # Custom styles
│   ├── js/
│   │   └── claim.js     # Auto-calculation & validation
│   └── lib/             # Third-party libraries
└── Program.cs           # Application entry point
Key Technologies
Backend: ASP.NET Core 8.0 MVC
ORM: Entity Framework Core 8.0
Database: SQL Server
Frontend: Bootstrap 5, jQuery, Font Awesome
Data Tables: DataTables.js for advanced table features
Charts: Chart.js for statistics visualization
Validation: Data Annotations + Client-side validation
Security Features
✅ Session-based authentication
✅ Role-based authorization
✅ CSRF protection on all forms (AntiForgeryToken)
✅ Input validation (client and server-side)
✅ Secure file uploads with type and size restrictions
✅ SQL injection protection (parameterized queries via EF Core)
✅ XSS protection (automatic encoding in Razor)
Database Schema
Main Tables
Users: User accounts with roles
Claims: Claim records with calculations
Lecturers: Lecturer information
Audits: Action audit trail
Notifications: User notifications
Workflows: Approval workflow tracking
Relationships
Claims → Users (many-to-one)
Claims → Lecturers (many-to-one)
Audits → Claims (many-to-one)
Notifications → Claims & Users
Workflows → Claims (one-to-one)
API Endpoints
Authentication
GET /Account/Login - Login page
POST /Account/Login - Authenticate user
GET /Account/Register - Registration page
POST /Account/Register - Create new user
GET /Account/Logout - Logout user
Claims
GET /Claim/Submit - Submit claim form
POST /Claim/Submit - Create new claim
GET /Claim/TrackStatus - View user's claims
GET /Claim/ViewClaims - View all claims (authorized roles)
GET /Claim/VerifyClaims/{id} - Verify claim
GET /Claim/ApproveClaim/{id} - Approve claim
GET /Claim/RejectClaim/{id} - Reject claim form
POST /Claim/RejectClaim/{id} - Reject claim with reason
POST /Claim/BulkApprove - Approve multiple claims
POST /Claim/BulkReject - Reject multiple claims
HR
GET /HR/Index - HR dashboard
GET /HR/MonthlyReport - Monthly report with filtering
GET /HR/Statistics - System statistics
GET /HR/ExportCsv - Export claims to CSV
Testing
Manual Testing Workflow
Registration & Login

Register new users with different roles
Test login with valid/invalid credentials
Verify session persistence
Lecturer Claims

Submit claims with various inputs
Test auto-calculation accuracy
Verify validation (client and server)
Upload documents with valid/invalid types
Approval Workflow

Verify claims as coordinator
Approve claims as manager
Test bulk actions
Reject claims with reasons
HR Functions

Generate monthly reports
Export CSV files
View statistics
Automated Testing (Future)
To add unit tests:

# Create test project
dotnet new xunit -n ClaimManagementsystem.Tests
cd ClaimManagementsystem.Tests
dotnet add reference ../ClaimManagementsystem/ClaimManagementsystem.csproj
Troubleshooting
Database Connection Issues
Verify SQL Server is running
Check connection string in appsettings.json
Ensure Windows Authentication or SQL Authentication is properly configured
Build Errors
dotnet clean
dotnet restore
dotnet build
Database Reset
# Delete database and recreate
dotnet ef database drop
dotnet run  # Will recreate with EnsureCreated()
Missing Dependencies
dotnet restore --force
Future Enhancements
 Email notifications for status changes
 PDF export for reports
 Advanced analytics dashboard
 File preview for uploaded documents
 Multi-factor authentication
 API endpoints for mobile app
 Automated unit and integration tests
 Docker containerization
Contributing
Fork the repository
Create a feature branch (git checkout -b feature/AmazingFeature)
Commit your changes (git commit -m 'Add some AmazingFeature')
Push to the branch (git push origin feature/AmazingFeature)
Open a Pull Request
License
This project is developed for educational purposes as part of the PROG2B course.

Support
For issues, questions, or contributions, please create an issue in the GitHub repository.

Developer: Lentswetswe T.K.L
Student ID: ST10448558
Course: PROG2B - Part 3
