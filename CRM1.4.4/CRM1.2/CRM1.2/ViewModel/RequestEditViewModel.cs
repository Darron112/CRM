using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM1._2.Models;
using System.ComponentModel.DataAnnotations;

namespace CRM1._2.ViewModel
{
    public class RequestEditViewModel
    {
        public int RequestID { get; set; }
        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagane!")]
        public string Title { get; set; }

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagane!")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagane!")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagane!")]
        public string Solution { get; set; }

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagane!")]
        public int? RequestTime { get; set; }

        public string CompanyName { get; set; }
        public string Type { get; set; }
        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagane!")]
        public int StatusId { get; set; }

        public string User { get; set; }
        public IEnumerable<StatusViewModel> Statuses { get; set; }
        public IEnumerable<DetailsViewModel> Details { get; set; }


        public RequestEditViewModel()
        {}

        public RequestEditViewModel(RequestTable requestResult)
        {
            Title = requestResult.Title;
            Description = requestResult.Description;
            RequestID = requestResult.RequestID;
            RequestDate = requestResult.RequestDate;
            Solution = requestResult.Solution;
            RequestTime = requestResult.RequestTime;
        }

        public void GetCompany(ClientTable clientResult)
        {
            CompanyName = clientResult.CompanyName;
        }

        public void GetType(TypeTable typedResult)
        {
            Type = typedResult.TypeName;
        }

        public void GetStatus(StatusTable statusResult)
        {
            StatusId = statusResult.StatusID;
        }

        public void GetUser(UserAccount userResult)
        {
            User = userResult.UserName;
        }

        public void GetStatuses(ICollection<StatusTable> statuses)
        {
            Statuses = statuses.Select(x => new StatusViewModel
            {
                StatusId = x.StatusID,
                StatusName = x.StatusName
            });
        }

        public void GetDetails(ICollection<RequestDetail> requestDetails)
        {
        }
    }
}