using CASH.FLOW.TRACKER.API.Model;

public record RegisterDto(string FirstName, string LastName, string Email, string Password);
public record LoginDto(string Email, string Password);
public record ForgotPasswordDto(string Email);
public record ResetPasswordDto(string UserId, string Token, string NewPassword);
public record ChangePasswordDto(ApplicationUser User, string CurrentPassword, string NewPassword);