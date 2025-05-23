﻿using DrasatHealthMobile.Helpers;

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
}