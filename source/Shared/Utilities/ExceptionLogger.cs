namespace Shared.Utilities;

public static class ExceptionLogger
{
    public static void LogException(Exception ex, string additionalInfo = "")
    {
        try
        {
            // إعداد نص الرسالة المراد تسجيلها
            string logMessage = $"Timestamp: {DateTime.Now}\n" +
                                $"Message: {ex.Message}\n" +
                                $"Stack Trace: {ex.StackTrace}\n" +
                                $"Source: {ex.Source}\n" +
                                $"Additional Info: {additionalInfo}\n" +
                                $"---------------------------------------\n";

            // كتابة الرسالة إلى ملف نصي
            //File.AppendAllText(logFilePath, logMessage);
        }
        catch (Exception loggingEx)
        {
            // في حالة فشل تسجيل الاستثناء، يمكن التعامل مع الأمر هنا (إرسال بريد إلكتروني أو إشعار)
            Console.WriteLine($"Failed to log exception: {loggingEx.Message}");
        }
    }
}
