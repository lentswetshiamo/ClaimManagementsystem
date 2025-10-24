using System.ComponentModel.DataAnnotations;

namespace ClaimManagementsystem.Models
{
        public class WorkflowState
        {
            [Key]
            public int WorkflowStateId { get; set; }
            public string State { get; set; }
            public string Description { get; set; }
            public DateTime Timestamp { get; set; }
            public string User { get; set; }
        }

        public class Approval
        {
            [Key]
            public int ApprovalId { get; set; }
            public int ClaimId { get; set; }
            public int ApproverId { get; set; }
            public string ApproverRole { get; set; }
            public string Comments { get; set; }
            public DateTime RequestDate { get; set; }
            public DateTime? ResponseDate { get; set; }
        }

        public class BusinessRule
        {
            [Key]
            public int BusinessRuleId { get; set; }
            public string RuleName { get; set; }
            public string Description { get; set; }
            public string Condition { get; set; }
            public string Action { get; set; }
            public bool IsActive { get; set; }
        }

        public class WorkflowConfiguration
        {
            [Key]
            public int WorkflowConfigurationId { get; set; }
            public string WorkflowType { get; set; }
            public int ApprovalLevels { get; set; }
            public string RequiredRoles { get; set; }
            public TimeSpan EscalationTime { get; set; }
            public bool AutoApprovalEnabled { get; set; }
        }
    }