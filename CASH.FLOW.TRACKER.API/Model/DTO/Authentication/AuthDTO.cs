using CASH.FLOW.TRACKER.API.Model;

public record RegisterDto(string FirstName, string LastName, string Email, string Password);
public record ConfirmEmailDto(string userId, string token);
public record LoginDto(string Email, string Password);
public record ForgotPasswordDto(string Email);
public record ResetPasswordDto(string UserId, string Token, string NewPassword);
public record ChangePasswordDto(string UserId, string CurrentPassword, string NewPassword);
public record UpdateProfileDto(string UserId, string FirstName, string LastName);
public record MeDto(string userId, string email, string userName);