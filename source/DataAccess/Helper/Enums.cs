namespace DataAccess.Helper;
public enum OrderBy
{
    Non = 0,
    ProductId = 1,
    Name = 2,
    BiggestPrice = 3,
    LowestPrice = 4,
    MaxQuantity = 5,
    MinQuantity = 6
}
public enum ErrorCode
{
    EmailNotExist = 10,
    PasswordIsIncorrect = 11,
    VerificationCodeIsIncorrect = 12,
    InvalidIdentifier=13,
    UserLogOut = 14,

    // General Errors
    UnknownError = 1000,
    InvalidRequest = 1001,
    NotFound = 1002,
    OperationFailed = 1003,

    // Authentication Errors
    InvalidCredentials = 2000,
    UserLockedOut = 2001,
    EmailNotConfirmed = 2002,
    PasswordExpired = 2003,

    // Validation Errors
    InvalidEmailFormat = 3000,
    PasswordTooWeak = 3001,
    FieldRequired = 3002,
    ValueOutOfRange = 3003,

    // Database Errors
    DatabaseConnectionFailed = 4000,
    EntityNotFound = 4001,
    DuplicateEntity = 4002,
    ForeignKeyViolation = 4003,

    // Network Errors
    NetworkUnavailable = 5000,
    Timeout = 5001,

    // File Errors
    FileNotFound = 6000,
    FileUploadFailed = 6001,
    FileDownloadFailed = 6002,

    // Custom Errors
    CustomError1 = 9000,
    CustomError2 = 9001
}