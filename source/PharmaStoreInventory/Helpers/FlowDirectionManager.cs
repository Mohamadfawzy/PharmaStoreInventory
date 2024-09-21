using DrasatHealthMobile.Helpers;
using System.Globalization;

namespace PharmaStoreInventory.Helpers;
public static class FlowDirectionManager
{
    //public static string Language
    //{
    //    get
    //    {
    //        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    //        if (lang == "ar" || lang == "en" || lang == "fr")
    //            return lang;
    //        else
    //            return "en";
    //    }
    //}

    public static FlowDirection CurentFlowDirection
    {
        get
        {
            if (AppValues.Language == "ar")
                return FlowDirection.RightToLeft;
            else
                return FlowDirection.LeftToRight;
        }
    }


    public static string ArrowIcon
    {
        get
        {
            if (AppValues.Language == "ar")
                return IconFont.ArrowRight;
            else
                return IconFont.ArrowLeft;
        }
    }
    //public static string DashboardNumberTemplateMargin
    //{
    //    get
    //    {
    //        if (AppValues.Language == "ar")
    //            return "RoundRectangle 50 6 6 6";
    //        else
    //            return "RoundRectangle 6 50 6 6";
    //    }
    //}

}