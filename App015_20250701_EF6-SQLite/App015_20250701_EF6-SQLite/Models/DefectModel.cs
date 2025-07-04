public class DefectModel
{
    public double roll_x { get; set; } // mm
    public double roll_y { get; set; } // mm
    public double roll_width { get; set; } // mm
    public double roll_height { get; set; } // mm

    public long DefectIndex { get; set; }
    public string kind { get; set; }
    public double reliability { get; set; }



    public double CanvasWidth { get; set; } // px
    public double CanvasHeight { get; set; } // px

    public double CanvasX => roll_x * CanvasWidth / roll_width;
    public double CanvasY => roll_y * CanvasHeight / roll_height;
}