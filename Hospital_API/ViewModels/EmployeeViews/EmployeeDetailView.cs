using Hospital_API.Entities;
using Hospital_API.ViewModels;

namespace Hospital_API.ModelViews.EmployeeViews
{
    public class EmployeeDetailView : EmployeeView
    {
        public ICollection<DepartmentView>? Departments { get; set; }
        public ICollection<RoleView> Roles { get; set; }
    }
}
