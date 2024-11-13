namespace DataAccess.Helper;

public class Common
{
    public static string GenerateVerificationCode()
    {
        return new Random().Next(1000, 9999).ToString();
    }

    public static string GenerateEmailBody(string verificationCode, string userFullName)
    {
        return $@"
<html>
<head>
    <style>
        p {{
            font-size: 16px; 
        }}
        .verification-code {{
            font-size: 25px;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <p>Dear {userFullName},</p>
                                 
    <p>Thank you for registering with our service! To complete your account registration and ensure the security of your account, we require you to verify your email Address.</p>
                                 
    <p><strong>Verification Code: <span class='verification-code'>{verificationCode}</span></strong></p>
                                 
    <p>To verify your email Address, please follow these steps:</p>
    <ol>
        <li>Open our website/app and log in to your account.</li>
        <li>Go to the verification page or click on the verification link provided.</li>
        <li>Enter the above verification code when prompted.</li>
    </ol>
                                 
    <p>If you did not initiate this registration, please disregard this email.</p>
                                 
    <p>Thank you for being a part of our community!</p>
                                 
    <p>Best regards,</p>
    <p>Modern soft Team</p>
</body>
</html>";
    }

}
