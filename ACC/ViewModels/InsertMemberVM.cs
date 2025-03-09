namespace ACC.ViewModels
{
    public class InsertMemberVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public bool adminAccess { get; set; }
        public bool standardAccess { get; set; }
        public bool excutive { get; set; }
        public DateTime AddedOn { get; set; }

    }
}
