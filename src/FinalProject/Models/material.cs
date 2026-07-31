namespace FinalProject.Models
{
    public class material
    {
        public string Name { get; set; }
        public string Category { get; set; } //Concrete, Steel, Paint, Tiles, General
        public string Unit { get; set; } //Ton, kg, m³, m², Liter, Piece
        public decimal UnitPrice { get; set; } //Price in EGP
    }
}
