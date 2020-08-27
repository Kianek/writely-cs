namespace writely.Models.Dto
{
    public class UpdatePasswordDto
    {
        public string OldPassword { get; }
        public string NewPassword { get; }

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