using writely.Models.Dto;

namespace writely.integration_tests
{
    public static class Helpers
    {
        public static UserRegistrationDto CreateRegistrationDto(
            string firstName = "John", string lastName = "Doe", string email = "john@dmail.com",
            string username = "john.doe")
        {
            return new UserRegistrationDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Username = username,
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };
        }
    }
}