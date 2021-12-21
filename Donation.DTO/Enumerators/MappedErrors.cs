using System.ComponentModel;

namespace Donation.Domain.Enumerators
{
    public enum MappedErrors
    {
        [Description("The user name or password is incorrect.")]
        UsernameOrPasswordIncorrect = 001,

        [Description("The user have been blocked.")]
        UserBlocked = 002,

        [Description("The refresh token was expired.")]
        RefreshTokenExpired = 003,

        [Description("The user was inactivated by account administrator.")]
        UserInactive = 004,

        [Description("The old password doesn't match.")]
        ChangePasswordUnmatch = 005,

        [Description("The verification code doesn't match.")]
        VerificationCodeUnmatch = 006,

        [Description("The verification code was expired.")]
        VerificationCodeExpired = 007,

        [Description("Password does not meet criteria.")]
        ChangePasswordCriteria = 008,

        [Description("The terms of gpdr have already been accepted.")]
        GpdrAlreadyAccepted = 009,

        [Description("Too many failed login attempts. Please try again in a few minutes.")]
        UserBlockedForManyFailedAttempts = 010,

        [Description("The username already exists.")]
        UserAlreadyExists = 011,

        [Description("User not found.")]
        UserNotFound = 012,

        [Description("Campaign not found or is inactive.")]
        CampaigNotFound = 013,

        [Description("This campaign does not belong to this user.")]
        WrongCampaignUser = 014,

        [Description("This campaign is already inactivated.")]
        CampaginInactive = 015
    }
}