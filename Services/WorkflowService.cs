using ClaimManagementsystem.Data;
using ClaimManagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Services
{
    public class WorkflowService
    {
        private readonly DatabaseContext _context;

        public WorkflowService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task InitializeWorkflowAsync(int claimId)
        {
            var workflow = new Workflow
            {
                ClaimId = claimId,
                CurrentStage = "Submitted",
                StageStartDate = DateTime.Now,
                RequiresAction = true,
                NextApprover = "Programme Coordinator"
            };

            _context.Workflows.Add(workflow);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWorkflowStageAsync(int claimId, string newStage)
        {
            var workflow = await _context.Workflows
                .FirstOrDefaultAsync(w => w.ClaimId == claimId);

            if (workflow != null)
            {
                workflow.StageEndDate = DateTime.Now;
                workflow.CurrentStage = newStage;
                workflow.StageStartDate = DateTime.Now;
                workflow.RequiresAction = newStage != "Approved" && newStage != "Rejected";

                if (newStage == "Verified")
                {
                    workflow.NextApprover = "Academic Manager";
                }
                else if (newStage == "Approved" || newStage == "Rejected")
                {
                    workflow.NextApprover = null;
                    workflow.RequiresAction = false;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Workflow?> GetWorkflowByClaimIdAsync(int claimId)
        {
            return await _context.Workflows
                .FirstOrDefaultAsync(w => w.ClaimId == claimId);
        }

        public async Task<IEnumerable<Workflow>> GetPendingWorkflowsAsync()
        {
            return await _context.Workflows
                .Where(w => w.RequiresAction)
                .OrderBy(w => w.StageStartDate)
                .ToListAsync();
        }
    }
}
