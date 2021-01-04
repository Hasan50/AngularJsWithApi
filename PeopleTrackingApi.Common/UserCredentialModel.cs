using System;
using System.ComponentModel.DataAnnotations;

namespace PeopleTrackingApi.Common.Models
{
   
   public class UserCredentialModel
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Login ID field is required")]
        public string LoginID { get; set; }
        [Required(ErrorMessage = "The Full Name field is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "The Contact No field is required")]
        public string ContactNo { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }

        public string CreatedAt { get; set; }

        public string AssistantId { get; set; }
        public string AssistantName { get; set; }

        public string DoctorId { get; set; }
        public int? Theme { get; set; }
        public int CompanyId { get; set; }
        public string OrganizationName { get; set; }
        public string CompanyName { get; set; }
        public string Gender { get; set; }
        public string PushToken { get; set; }

    }

    [Serializable]
    public class UserSessionModel
    {
        public string Id { get; set; }
        public int UserTypeId { get; set; }
        public int CompanyId { get; set; }
        public string ContactNo { get; set; }
        public int? Theme { get; set; }
        public string FullName { get; set; }
        public string UserInitial { get; set; }
        public string Email { get; set; }
        
    }
    [Serializable]
    public class UserLanguageSessionModel
    {
        public string Language { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
    [Serializable]
    public class UserLanguageModel
    {
        public string Dashboard { get; set; }
        public string Tasks { get; set; }
        public string Leaves { get; set; }
        public string Reports { get; set; }
        public string Employees { get; set; }
        public string Company { get; set; }
        public string Setting { get; set; }
        public string SickType { get; set; }

    }
}
