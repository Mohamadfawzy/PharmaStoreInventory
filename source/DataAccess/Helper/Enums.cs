﻿namespace DataAccess.Helper;
public enum ProductsOrderBy
{
    Non = 0,
    ProductId = 1,
    Name = 2,
    BiggestPrice = 3,
    LowestPrice = 4,
    MaxQuantity = 5,
    MinQuantity = 6
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
    EmailNotExist = 1, //
    PasswordIsIncorrect = 2,
    UserNotActive = 3,
    VerificationCodeIsIncorrect = 4, //
    UserLogOut = 5,
    InvalidIdentifier = 6,//
    OperationFailed = 7,
    ManyFailedAttempts = 8,


    // General Errors
    UnknownError = 1000,

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
}