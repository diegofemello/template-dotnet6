namespace Application.DTO
{
    public class EmailConfirmDTO
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
    }
}
