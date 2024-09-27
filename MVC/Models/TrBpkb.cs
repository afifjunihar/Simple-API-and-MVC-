using System;
using System.Collections.Generic;

namespace MVC.Models
{
    public partial class TrBpkb
    {
        public string AgreementNumber { get; set; } = null!;
        public string? BpkbNo { get; set; }
        public string? BranchId { get; set; }
        public DateTime? BpkbDate { get; set; }
        public string? FakturNo { get; set; }
        public DateTime? FakturDate { get; set; }
        public string? LocationId { get; set; }
        public string? PoliceNo { get; set; }
        public DateTime? BpkbDateIn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public virtual MsStorageLocation? Location { get; set; }
    }
}
