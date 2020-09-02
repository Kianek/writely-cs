namespace writely.Models.Dto
{
    public class UpdatePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public UpdatePasswordDto()
        {
        }

        public UpdatePasswordDto(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}