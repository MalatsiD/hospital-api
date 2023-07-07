namespace Hospital_API.ViewModels.PatientViews
{
    public class AdmitAilmentView
    {
        public int PatientAdmitId { get; set; }
        public int AilmentId { get; set; }
        public string? AilmentName { get; set; }
        public string? AilmentDescription { get; set; }
    }
}
