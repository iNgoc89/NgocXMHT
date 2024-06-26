namespace NGOCXMHT_Identity.Models.ViewModels
{
    public class AssignRoleVM
    {
        public string UserId { get; set;} = string.Empty;
        public IList<string> SelectedRoles { get; set;}

        public IList<RoleVM>? AllRoles { get; set;}
    } 

    public class RoleVM{
        public string Id { get; set;} = string.Empty;
        public string Name { get; set;} = string.Empty;
    }
}