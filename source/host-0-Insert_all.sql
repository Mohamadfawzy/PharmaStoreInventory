
 --USE master;
 --IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = 'msoftStock')
 --BEGIN
 --    CREATE DATABASE msoftStock COLLATE Arabic_100_CI_AS_KS_WS_SC_UTF8;
 --END
 --GO

 USE msoftStock
 drop table Branches;
 drop table Users;
 drop table EmailVerificationCodes;

 CREATE TABLE Users(
 	[Id] [INT] IDENTITY (1,1),
 	[FullName] [NVARCHAR](100) NOT NULL,
 	[PharmacyName] [NVARCHAR](100) NOT NULL,
 	[PhoneNumber] [NVARCHAR](20) NULL,
 	[Email] [NVARCHAR](256) Not Null,
 	[DeviceID] [NVARCHAR](70) NULL,
 	[DeviceModel] [NVARCHAR](100) NULL,
    [IsActive] [BIT] NOT NULL DEFAULT 0,
	[UserRole] [CHAR](1) DEFAULT 'C',
    [IsLoggedIn] [BIT] NOT NULL DEFAULT 0,
 	[AccessFailedCount] [INT] NOT NULL DEFAULT 0,

 	EmailConfirmed BIT NOT NULL DEFAULT 0,
 	[PhoneNumberConfirmed] [BIT] NOT NULL DEFAULT 0,
    [VerificationCode] VARCHAR(8),
    [VCodeExpirationTime] DATETIMEOFFSET(7), 

    [TwoFactorEnabled] [BIT] NOT NULL DEFAULT 0,
    [CreateOn] DATETIMEOFFSET(7) DEFAULT GETUTCDATE(),
 	[LockoutEnd] [DATETIMEOFFSET](7) NULL,
	[LoggedOutAt] [DATETIMEOFFSET](7),
 	[PasswordHash] [NVARCHAR](MAX) NOT NULL,

 	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT UQ_Users_Email UNIQUE (Email),
	--CONSTRAINT UQ_Users_PhoneNumber UNIQUE (PhoneNumber),

	INDEX IX_Users_Email NONCLUSTERED (Email),
	INDEX IX_Users_PhoneNumber NONCLUSTERED (PhoneNumber),
	INDEX IX_Users_UserRole NONCLUSTERED (UserRole)

 );
 -- ADD branches
 GO
----------------
CREATE TABLE Branches
(
    [Id] UNIQUEIDENTIFIER,
	[BrachName] NVARCHAR(50),
	[Username] NVARCHAR(50) NOT NULL,
	[Password] NVARCHAR(50) NOT NULL,
	[Telephone] NVARCHAR(50),
    [IpAddress] NVARCHAR(50) NOT NULL,
	[Port] NVARCHAR(10) NOT NULL,
    [CreateOn] DATETIME DEFAULT GETUTCDATE(),
	[UserId] INT NOT NULL,

	CONSTRAINT PK_Branches PRIMARY KEY (Id),
	CONSTRAINT FK_Branches_UserId
    FOREIGN KEY (UserId)
      REFERENCES Users(Id)
		ON DELETE NO ACtion ON UPDATE NO ACTION,

	INDEX IX_Users_UserId NONCLUSTERED (UserId)
);

CREATE TABLE EmailVerificationCodes 
(
    Id INT IDENTITY(1,1),
    [Email] [NVARCHAR](256) Not Null,
    VerificationCode NVARCHAR(10) NOT NULL,
    ExpirationDate DATETIME NOT NULL,
    IsUsed BIT DEFAULT 0, -- حالة استخدام الرمز، 0 تعني أنه غير مستخدم و1 يعني أنه مستخدم
    CreatedOn DATETIME DEFAULT GETDATE()

	CONSTRAINT PK_EmailVerifCodes PRIMARY KEY (Id),
	INDEX IX_EmailVerifCodes_Email NONCLUSTERED (Email)
);

GO
INSERT INTO Users (
    FullName, PharmacyName, PhoneNumber, Email, IsActive, IsLoggedIn, TwoFactorEnabled,UserRole, PasswordHash)
	VALUES (
		'Admin User',                  -- FullName
		'Admin Pharmacy App',          -- PharmcyName
		'admin',                       -- PhoneNumber
		N'admin',           -- Email
		1,                             -- IsActive
		1,                             -- IsLoggedIn
		1,							   --TwoFactorEnabled
		'A',
		'admin'            -- PasswordHash (this should be hashed in practice)
);
go
-- Add Employees
INSERT INTO Users (FullName, PharmacyName, Email, IsActive,UserRole, PasswordHash) 
	VALUES (N'الموظف الأول','ModernSoft', 'Emp1@mosoft.com',1,'A','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive,UserRole, PasswordHash) 
	VALUES (N'الموظف الثاني','ModernSoft', 'Emp2@mosoft.com',1,'E','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive,UserRole, PasswordHash) 
	VALUES (N'الموظف الثالث','ModernSoft', 'Emp3@mosoft.com',1,'E','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive,UserRole, PasswordHash) 
	VALUES (N'الموظف الرابع','ModernSoft', 'Emp4@mosoft.com',1,'E','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive,UserRole, PasswordHash) 
	VALUES (N'الموظف الخامس','ModernSoft', 'Emp5@mosoft.com',1,'E','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go

 
use msoftStock
 -- Add Users

INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'أبو بكر','PharmacyName', N'user2@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go

INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'عمر بن الخطاب','PharmacyName', N'user3@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');

INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'عثمان بن عفان','PharmacyName', N'user4@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'علي ابن أبي طالب','PharmacyName', N'user5@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'طلحة بن عبيد الله','PharmacyName', N'user6@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'زيد بن ثابت','PharmacyName', N'user7@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'عبيده ابن الجراح','PharmacyName', N'user8@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'خالد بن الوليد','PharmacyName', N'user9@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'عمرو بن العاص','PharmacyName', N'user10@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'أبو هريرة','PharmacyName', N'user11@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'أنس ابن مالك','PharmacyName', N'user12@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'عبد الله بن عمر','PharmacyName', N'user13@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'اسامة بن زيد','PharmacyName', N'user14@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'حمزة بن عبد المطلب','PharmacyName', N'user15@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'الأرقم بن أبي الأرقم','PharmacyName', N'user16@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'حسان بن ثابت','PharmacyName', N'user17@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'خديجة بنت خويلد','PharmacyName', N'user18@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'عائشة بنت أبي بكر','PharmacyName', N'user19@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'حفصة','PharmacyName', N'user20@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'زينب','PharmacyName', N'user21@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'أم سلمة رضي الله عنها','PharmacyName', N'user22@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'الإمام الشافعي','PharmacyName', N'user23@mosoft.com',1,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');
go
INSERT INTO Users (FullName, PharmacyName, Email, IsActive, PasswordHash) 
	VALUES (N'الإمام أبو حنيفة','PharmacyName', N'user24@mosoft.com',0,'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92');

SELECT * FROM Users



