namespace vk_app.Entities
{
    public class UserState
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
