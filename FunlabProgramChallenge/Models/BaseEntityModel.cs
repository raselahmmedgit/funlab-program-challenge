using System;

namespace FunlabProgramChallenge.Models
{
    public class BaseEntityModel
    {
        public bool IsActive { get; set; }
    }

    public interface IChangeTrackerEntity
    {
        DateTime CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string? UpdatedBy { get; set; }
    }

    public class ChangeTrackerEntity : IChangeTrackerEntity
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }

    public interface IDeleteTrackerEntity
    {
        bool? IsDeleted { get; set; }
        DateTime? DeletedDate { get; set; }
        string? DeletedBy { get; set; }
        string? DeleteReason { get; set; }
    }

}
