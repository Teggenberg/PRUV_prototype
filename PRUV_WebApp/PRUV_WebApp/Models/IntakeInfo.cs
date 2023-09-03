using Microsoft.Build.ObjectModelRemoting;

namespace PRUV_WebApp.Models
{
    public class IntakeInfo
    {
        public int Store { get; set; }
        public int User { get; set; }
        public string Year { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Ask { get; set; }
        public string Details { get; set; }
        public int Case { get; set; }   
        public IntakeInfo() { }
    }
}
