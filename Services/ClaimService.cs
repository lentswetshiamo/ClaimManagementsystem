using ClaimManagementsystem.Data;
using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Services
{
    public class ClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly AuditService _auditService;
        private readonly NotificationService _notificationService;
        private readonly WorkflowService _workflowService;

        public ClaimService(
            IClaimRepository claimRepository,
            AuditService auditService,
            NotificationService notificationService,
            WorkflowService workflowService)
        {
            _claimRepository = claimRepository;
            _auditService = auditService;
            _notificationService = notificationService;
            _workflowService = workflowService;
        }

        public async Task<Claim> CreateClaimAsync(Claim claim, string userName)
        {
            // Auto-calculate total
            claim.CalculateTotal();
            claim.Status = "Pending";
            claim.DateSubmitted = DateTime.Now;

            var createdClaim = await _claimRepository.AddClaimAsync(claim);

            // Log audit
            await _auditService.LogActionAsync(createdClaim.Id, "Created", userName, "Claim submitted");

            // Create workflow
            await _workflowService.InitializeWorkflowAsync(createdClaim.Id);

            // Send notification
            await _notificationService.NotifyClaimStatusChangeAsync(createdClaim.Id, createdClaim.UserId, "submitted");

            return createdClaim;
        }

        public bool ValidateClaim(Claim claim, out List<string> errors)
        {
            return claim.IsValid(out errors);
        }

        public async Task<Claim?> GetClaimByIdAsync(int id)
        {
            return await _claimRepository.GetClaimByIdAsync(id);
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            return await _claimRepository.GetAllClaimsAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaimsByStatusAsync(string status)
        {
            return await _claimRepository.GetClaimsByStatusAsync(status);
        }

        public async Task<IEnumerable<Claim>> GetClaimsByLecturerAsync(int lecturerId)
        {
            return await _claimRepository.GetClaimsByLecturerAsync(lecturerId);
        }

        public async Task<IEnumerable<Claim>> GetClaimsByUserAsync(int userId)
        {
            return await _claimRepository.GetClaimsByUserAsync(userId);
        }

        public async Task ApproveClaimAsync(int claimId, string approverName)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                claim.ApprovedBy = approverName;
                claim.ApprovedDate = DateTime.Now;
                await _claimRepository.UpdateClaimAsync(claim);

                await _auditService.LogActionAsync(claimId, "Approved", approverName, $"Claim approved by {approverName}");
                await _workflowService.UpdateWorkflowStageAsync(claimId, "Approved");
                await _notificationService.NotifyClaimStatusChangeAsync(claimId, claim.UserId, "approved");
            }
        }

        public async Task VerifyClaimAsync(int claimId, string verifierName)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Verified";
                await _claimRepository.UpdateClaimAsync(claim);

                await _auditService.LogActionAsync(claimId, "Verified", verifierName, $"Claim verified by {verifierName}");
                await _workflowService.UpdateWorkflowStageAsync(claimId, "Verified");
                await _notificationService.NotifyClaimStatusChangeAsync(claimId, claim.UserId, "verified");
            }
        }

        public async Task RejectClaimAsync(int claimId, string rejectorName, string reason)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                claim.RejectionReason = reason;
                await _claimRepository.UpdateClaimAsync(claim);

                await _auditService.LogActionAsync(claimId, "Rejected", rejectorName, $"Claim rejected: {reason}");
                await _workflowService.UpdateWorkflowStageAsync(claimId, "Rejected");
                await _notificationService.NotifyClaimStatusChangeAsync(claimId, claim.UserId, "rejected");
            }
        }

        public async Task BulkApproveClaimsAsync(List<int> claimIds, string approverName)
        {
            foreach (var claimId in claimIds)
            {
                await ApproveClaimAsync(claimId, approverName);
            }
        }

        public async Task BulkRejectClaimsAsync(List<int> claimIds, string rejectorName, string reason)
        {
            foreach (var claimId in claimIds)
            {
                await RejectClaimAsync(claimId, rejectorName, reason);
            }
        }
    }
}
