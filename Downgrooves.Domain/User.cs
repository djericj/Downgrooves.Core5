namespace Downgrooves.Domain
{
    /// <summary>
    /// A generic User
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}