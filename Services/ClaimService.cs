using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class ClaimService
        {
            private readonly List<Claim> _claims = new List<Claim>();
            private readonly List<User> _users = new List<User>();
            private int _claimIdCounter = 1;

            public ClaimService()
            {
                InitializeSampleData();
            }

            private void InitializeSampleData()
            {
                _users.AddRange(new[]
                {
                new User { UserId = 1, Name = "Tshiamo Lentswe", Email = "tshiamo@university.ac.za", Password = "password", Role = "Lecturer", HourlyRate = 350.00m },
                new User { UserId = 2, Name = "Prof. Sarah Johnson", Email = "coordinator@university.ac.za", Password = "password", Role = "Coordinator", HourlyRate = 0 },
                new User { UserId = 3, Name = "Dr. Michael Brown", Email = "manager@university.ac.za", Password = "password", Role = "Manager", HourlyRate = 0 },
                new User { UserId = 5, Name = "Ntokozo Nhleko", Email = "ntokozo@university.ac.za", Password = "password", Role = "Lecturer", HourlyRate = 320.00m },
                new User { UserId = 6, Name = "Deshi Mfolo", Email = "deshi@university.ac.za", Password = "password", Role = "Lecturer", HourlyRate = 340.00m },
                new User { UserId = 7, Name = "Kamogelo Lentswe", Email = "kamogelo@university.ac.za", Password = "password", Role = "Lecturer", HourlyRate = 330.00m }
            });

                // Tshiamo Lentswe's claims
                _claims.Add(new Claim
                {
                    ClaimId = _claimIdCounter++,
                    UserId = 1,
                    UserName = "Tshiamo Lentswe",
                    Month = "September",
                    Year = 2024,
                    TotalHours = 45.5m,
                    HourlyRate = 350.00m,
                    TotalAmount = 15925.00m,
                    Status = "Submitted",
                    SubmittedDate = DateTime.Now.AddDays(-2)
                });

                _claims.Add(new Claim
                {
                    ClaimId = _claimIdCounter++,
                    UserId = 1,
                    UserName = "Tshiamo Lentswe",
                    Month = "August",
                    Year = 2024,
                    TotalHours = 40.0m,
                    HourlyRate = 350.00m,
                    TotalAmount = 14000.00m,
                    Status = "Approved Level 2",
                    SubmittedDate = DateTime.Now.AddDays(-15),
                    ApprovedDate = DateTime.Now.AddDays(-10)
                });

                // Ntokozo Nhleko's claims
                _claims.Add(new Claim
                {
                    ClaimId = _claimIdCounter++,
                    UserId = 5,
                    UserName = "Ntokozo Nhleko",
                    Month = "September",
                    Year = 2024,
                    TotalHours = 38.0m,
                    HourlyRate = 320.00m,
                    TotalAmount = 12160.00m,
                    Status = "Submitted",
                    SubmittedDate = DateTime.Now.AddDays(-1)
                });

                // Deshi Mfolo's claims
                _claims.Add(new Claim
                {
                    ClaimId = _claimIdCounter++,
                    UserId = 6,
                    UserName = "Deshi Mfolo",
                    Month = "September",
                    Year = 2024,
                    TotalHours = 42.5m,
                    HourlyRate = 340.00m,
                    TotalAmount = 14450.00m,
                    Status = "Approved Level 1",
                    SubmittedDate = DateTime.Now.AddDays(-3),
                    ApprovedDate = DateTime.Now.AddDays(-1)
                });

                // Kamogelo Lentswe's claims
                _claims.Add(new Claim
                {
                    ClaimId = _claimIdCounter++,
                    UserId = 7,
                    UserName = "Kamogelo Lentswe",
                    Month = "August",
                    Year = 2024,
                    TotalHours = 36.0m,
                    HourlyRate = 330.00m,
                    TotalAmount = 11880.00m,
                    Status = "Approved Level 2",
                    SubmittedDate = DateTime.Now.AddDays(-20),
                    ApprovedDate = DateTime.Now.AddDays(-12)
                });
            }

            public void SubmitClaim(Claim claim)
            {
                claim.ClaimId = _claimIdCounter++;
                claim.Status = "Submitted";
                claim.SubmittedDate = DateTime.Now;
                _claims.Add(claim);
            }

            public List<Claim> GetUserClaims(int userId)
            {
                return _claims.Where(c => c.UserId == userId).OrderByDescending(c => c.SubmittedDate).ToList();
            }

            public List<Claim> GetClaimsByStatus(int userId, string status)
            {
                return _claims.Where(c => c.UserId == userId && c.Status == status).ToList();
            }

            public List<Claim> GetClaimsForReview()
            {
                return _claims.Where(c => c.Status == "Submitted").ToList();
            }

            public List<Claim> GetClaimsForFinalApproval()
            {
                return _claims.Where(c => c.Status == "Approved Level 1").ToList();
            }

            public void ApproveClaim(int claimId, int userId, string comments, string role)
            {
                var claim = _claims.FirstOrDefault(c => c.ClaimId == claimId);
                if (claim != null)
                {
                    claim.Status = role == "Coordinator" ? "Approved Level 1" : "Approved Level 2";
                    claim.ApprovedDate = DateTime.Now;
                    claim.Comments = comments;
                }
            }

            public void RejectClaim(int claimId, int userId, string comments)
            {
                var claim = _claims.FirstOrDefault(c => c.ClaimId == claimId);
                if (claim != null)
                {
                    claim.Status = "Rejected";
                    claim.Comments = comments;
                }
            }

            public SystemStatistics GetSystemStatistics()
            {
                return new SystemStatistics
                {
                    TotalClaims = _claims.Count,
                    PendingClaims = _claims.Count(c => c.Status == "Submitted"),
                    ApprovedClaims = _claims.Count(c => c.Status.Contains("Approved")),
                    TotalAmount = _claims.Where(c => c.Status.Contains("Approved")).Sum(c => c.TotalAmount)
                };
            }

            public List<Claim> GetRecentApprovals()
            {
                return _claims.Where(c => c.Status.Contains("Approved"))
                             .OrderByDescending(c => c.ApprovedDate)
                             .Take(5)
                             .ToList();
            }

            public List<Activity> GetRecentActivities()
            {
                return new List<Activity>
            {
                new Activity { Description = "New claim submitted by Tshiamo Lentswe", Timestamp = DateTime.Now.AddHours(-1), User = "Tshiamo Lentswe" },
                new Activity { Description = "Claim approved for Ntokozo Nhleko", Timestamp = DateTime.Now.AddHours(-2), User = "Prof.Ntokozo Nhleko " },
                new Activity { Description = "New claim submitted by Deshi Mfolo", Timestamp = DateTime.Now.AddHours(-3), User = "Deshi Mfolo" },
                new Activity { Description = "System backup completed", Timestamp = DateTime.Now.AddHours(-4), User = "System" }
            };
            }

            public object GenerateReports()
            {
                return new
                {
                    MonthlySummary = _claims.GroupBy(c => new { c.Month, c.Year })
                                          .Select(g => new { Period = $"{g.Key.Month} {g.Key.Year}", Count = g.Count(), Total = g.Sum(x => x.TotalAmount) }),
                    UserSummary = _claims.GroupBy(c => c.UserName)
                                       .Select(g => new { Lecturer = g.Key, Claims = g.Count(), TotalAmount = g.Sum(x => x.TotalAmount) })
                };
            }

            public Claim GetClaimById(int claimId)
            {
                return _claims.FirstOrDefault(c => c.ClaimId == claimId);
            }
        }
    }
