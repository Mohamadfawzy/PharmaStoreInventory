namespace DataAccess.DomainModel;
public enum ProductsOrderBy
{
    Non = 0,
    Name = 2,
    ProductId = 1,
    LowestPrice = 4,
    BiggestPrice = 3,
    MinQuantity = 6,
    MaxQuantity = 5,
}

public enum UsersOrderBy
{
    Non = 0,
    Id = 1,
    Name = 2,
    NameDescending = 3,
    CreateOn = 4,
    CreateOnDescending = 5,
    EmailNotConfirmed = 6,
}


public enum ErrorCode
{
    NoError = 0,

    EmailNotExist = 1, //
    EmailAlreadyExists = 2,
    PhoneNumberNotExist = 3,
    PhoneNumberAlreadyExists =4,
    ItemIsExist = 5,

    PasswordIsIncorrect = 10,
    VerificationCodeIsIncorrect = 7, //
    InvalidIdentifier = 11,//
    OperationFailed = 12,
    ManyFailedAttempts = 13,
    NotFoundById = 14,

    NullValue = 21,
    ExceptionError =22,

    AccessLimitation = 31,
    UserNotActive = 32,
    UserLogOut = 33,
    InvalidRole = 34,
    // General Errors
    UnknownError = 1000,
    MultipleErrors = 1001,
    // Authentication Errors
    InvalidCredentials = 2000,
    UserLockedOut = 2001,
    EmailNotConfirmed = 2002,
    PasswordExpired = 2003,

    // Validation Errors
    InvalidEmailFormat = 3000,
    PasswordTooWeak = 3001,

    ValueOutOfRange = 3003,

    // Database Errors
    DatabaseConnectionFailed = 4000,
}

public enum InputType
{
    Text = 0,
    Phone = 1,
    Email = 2,
    Empty = 3,
    Admin = 4
}


public enum ConnectionErrorCode
{
    Success,
    Fail,
    Exception,
    Username,
    Password,
    UsernameOrPass,
    ConnectionString,
    UnauthorizedRole
}
