namespace ClaimManagementsystem.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; } = string.Empty;// type of claim 
        public string Description { get; set; } = string.Empty;// brief description of the claim
        public System.DateTime DateSubmitted { get; set; } = System.DateTime.Now; // date when the claim was submitted
        public string Status { get; set; } = string.Empty;// status of the claim (e.g., Pending, Approved, Rejected)
        public string Documents { get; set; } = string.Empty;// any supporting documents for the claim

        //link to user
        public int UserId { get; set; }
        public string SubmittedBy { get; set; } = string.Empty;// name of the user who submitted the claim

        //link to lecturer
        public int LecturerId { get; set; }
        public string LecturerName { get; set; } = string.Empty;// name of the lecturer associated with the claim

    }
}
