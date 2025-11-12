namespace PharmaStoreInventory.Helpers;

public static class PriceConverter
{
    public static decimal CalculateBuyPrice(decimal price, decimal percentage)
    {
        decimal result = price - (price * (percentage / 100));
        return result;
    }


    // تحويل من (علب + شرائط) إلى كمية عشرية
    public static decimal ToDecimalQuantity(int boxesCount, int stripsCount, int unitsPerBox)
    {
        if (unitsPerBox <= 0)
            throw new ArgumentException("Units per box must be greater than zero.", nameof(unitsPerBox));

        decimal total = boxesCount + ((decimal)stripsCount / unitsPerBox);

        return Math.Round(total, 3, MidpointRounding.AwayFromZero);
    }

    // تحويل من كمية عشرية إلى (علب + شرائط)
    public static (int Boxes, int Strips) FromDecimalQuantity(decimal totalQuantity, int unitsPerBox)
    {
        if (unitsPerBox <= 0)
            throw new ArgumentException("Units per box must be greater than zero.", nameof(unitsPerBox));

        totalQuantity = Math.Round(totalQuantity, 3, MidpointRounding.AwayFromZero);

        int boxes = (int)Math.Floor(totalQuantity);
        decimal fractionalPart = totalQuantity - boxes;

        // نحول الجزء العشري إلى عدد شرائط
        decimal stripsDecimal = fractionalPart * unitsPerBox;

        // تقريب الشرائط إلى أقرب عدد صحيح
        int strips = (int)Math.Round(stripsDecimal, MidpointRounding.AwayFromZero);

        // ✅ إذا تجاوز الشرائط عدد الوحدات في العلبة، نحولها إلى علبة إضافية
        if (strips >= unitsPerBox)
        {
            boxes += 1;
            strips = 0;
        }

        return (boxes, strips);
    }

    // Convert boxes + strips → decimal quantity
    //public static decimal ToDecimalQuantity(int boxesCount, int stripsCount, int unitsPerBox)
    //{
    //    if (unitsPerBox <= 0)
    //        throw new ArgumentException("Units per box must be greater than zero.");

    //    return Math.Round(boxesCount + (decimal)stripsCount / unitsPerBox);
    //}

    //// Convert decimal quantity → boxes + strips
    //public static (decimal Boxes, decimal Strips) FromDecimalQuantity(decimal totalQuantity, int unitsPerBox)
    //{
    //    if (unitsPerBox <= 0)
    //        throw new ArgumentException("Units per box must be greater than zero.");

    //    decimal boxes = (decimal)Math.Floor(totalQuantity);
    //    decimal strips = (decimal)Math.Round((totalQuantity - boxes) * unitsPerBox, 3);
    //    return (boxes, strips);
    //}
}
