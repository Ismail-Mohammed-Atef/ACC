namespace DataLayer.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
    }
}