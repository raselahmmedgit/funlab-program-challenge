using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FunlabProgramChallenge.ViewModels
{
    public class ScheduleViewModel
    {
        [Key]
        public int ScheduleId { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        public string CustomerTitle { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        public string Description { get; set; }

        [DisplayName("Start")]
        [Required(ErrorMessage = "Start Date Time is required")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start Date is required")]
        public string StartDate { get; set; }

        [Display(Name = "Start Time")]
        [Required(ErrorMessage = "Start Time is required")]
        public string StartTime { get; set; }

        [DisplayName("End")]
        [Required(ErrorMessage = "End Date Time is required")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End Date is required")]
        public string EndDate { get; set; }

        [Display(Name = "End Time")]
        [Required(ErrorMessage = "End Time is required")]
        public string EndTime { get; set; }

        [DisplayName("Url")]
        [MaxLength(200)]
        public string Url { get; set; }

        [DisplayName("Theme Color")]
        [MaxLength(200)]
        public string ThemeColor { get; set; }

        //All Day
        [DisplayName("All Day")]
        public bool AllDay { get; set; }

        //Allow
        [DisplayName("Allow")]
        public bool Allow { get; set; }

        [DisplayName("Background Color")]
        public string BackgroundColor { get; set; }

        [DisplayName("Border Color")]
        public string BorderColor { get; set; }

        [DisplayName("Text Color")]
        public string TextColor { get; set; }

        [DisplayName("Display")]
        public string Display { get; set; }

        [DisplayName("Group")]
        public int GroupId { get; set; }

        [DisplayName("Employee")]
        public int EmployeeId { get; set; }
    }
}
