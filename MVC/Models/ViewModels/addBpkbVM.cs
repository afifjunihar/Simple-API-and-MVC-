using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.ViewModels
{
    public class AddBpkbVM
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
    }
}
