using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
        public class WorkflowService
        {
            private readonly List<WorkflowState> _workflowStates;
            private readonly List<BusinessRule> _businessRules;

            public WorkflowService()
            {
                _workflowStates = new List<WorkflowState>();
                _businessRules = InitializeBusinessRules();
            }

            private List<BusinessRule> InitializeBusinessRules()
            {
                return new List<BusinessRule>
            {
                new BusinessRule {
                    RuleId = 1,
                    RuleName = "MonthlyHoursLimit",
                    Description = "Maximum hours per month validation",
                    Condition = "TotalHours > 160",
                    Action = "Reject",
                    IsActive = true
                },
                new BusinessRule {
                    RuleId = 2,
                    RuleName = "SubmissionDeadline",
                    Description = "Claim submission deadline",
                    Condition = "SubmissionDate > 5thOfFollowingMonth",
                    Action = "RequireManagerOverride",
                    IsActive = true
                },
                new BusinessRule {
                    RuleId = 3,
                    RuleName = "RateValidation",
                    Description = "Hourly rate against programme standards",
                    Condition = "HourlyRate > MaxProgrammeRate",
                    Action = "FlagForReview",
                    IsActive = true
                }
            };
            }

            public string GetNextState(string currentState, string action, string userRole)
            {
                var stateTransitions = new Dictionary<string, Dictionary<string, string>>
                {
                    ["Draft"] = new Dictionary<string, string> { ["Submit"] = "Submitted" },
                    ["Submitted"] = new Dictionary<string, string>
                    {
                        ["Approve"] = userRole == "Coordinator" ? "Approved Level 1" : "Approved Level 2",
                        ["Reject"] = "Rejected",
                        ["RequestInfo"] = "Information Requested"
                    },
                    ["Approved Level 1"] = new Dictionary<string, string>
                    {
                        ["Approve"] = "Approved Level 2",
                        ["Reject"] = "Rejected"
                    },
                    ["Information Requested"] = new Dictionary<string, string>
                    {
                        ["Resubmit"] = "Submitted"
                    }
                };

                if (stateTransitions.ContainsKey(currentState) &&
                    stateTransitions[currentState].ContainsKey(action))
                {
                    return stateTransitions[currentState][action];
                }

                return currentState;
            }

            public List<BusinessRule> GetActiveBusinessRules()
            {
                return _businessRules.Where(r => r.IsActive).ToList();
            }

            public bool ValidateClaimAgainstRules(Claim claim)
            {
                var activeRules = GetActiveBusinessRules();
                foreach (var rule in activeRules)
                {
                    if (!EvaluateRule(rule, claim))
                    {
                        return false;
                    }
                }
                return true;
            }

            private bool EvaluateRule(BusinessRule rule, Claim claim)
            {
                // Simplified rule evaluation - in real implementation, use a rules engine
                switch (rule.RuleName)
                {
                    case "MonthlyHoursLimit":
                        return claim.TotalHours <= 160;
                    case "RateValidation":
                        return claim.HourlyRate <= 500; // Maximum rate
                    default:
                        return true;
                }
            }

            public void LogWorkflowState(int claimId, string state, string user)
            {
                _workflowStates.Add(new WorkflowState
                {
                    State = state,
                    Description = $"Claim moved to {state}",
                    Timestamp = DateTime.Now,
                    User = user
                });
            }

        public List<WorkflowState> GetClaimWorkflowHistory(int claimId) => _workflowStates.Where(w => w.State.Contains(claimId.ToString()))
                                 .OrderByDescending(w => w.Timestamp)
                                 .ToList();
    }

    internal class WorkflowState
    {
        public string State { get; internal set; }
        public string Description { get; internal set; }
        public string User { get; internal set; }
        public DateTime Timestamp { get; internal set; }
    }
}
