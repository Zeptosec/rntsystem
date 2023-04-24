namespace RentSystem.Core.DTOs
{
    public class GetItemDTO
    {
        public int Id { get; set; }
        public int Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int State { get; set; }
    }
}
