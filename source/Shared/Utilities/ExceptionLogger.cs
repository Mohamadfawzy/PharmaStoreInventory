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

    //[HttpGet("exeption")]
    //public IActionResult MyExeption()
    //{
    //    var list = new List<string>();
    //    try
    //    {
    //        // كود قد يسبب استثناء
    //        throw new InvalidOperationException("Sample exception");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Exception Message: " + ex.Message);

    //        // إنشاء StackTrace من الاستثناء
    //        StackTrace trace = new StackTrace(ex, true);

    //        // الحصول على الإطارات (frames) داخل stack trace
    //        for (int i = 0; i < trace.FrameCount; i++)
    //        {
    //            StackFrame frame = trace.GetFrame(i);
    //            list.Add($"Method: {frame.GetMethod().Name}, Line: {frame.GetFileLineNumber()}");
    //        }
    //    }
    //    return Ok(list);
    //}
}
