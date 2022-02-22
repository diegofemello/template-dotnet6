namespace Domain.VO
{
    public class EmailConfirmVO
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
    }
}
