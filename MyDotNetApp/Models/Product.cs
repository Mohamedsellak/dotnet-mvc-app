public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Image { get; set; } = null;
    
        public decimal Price { get; set; }
    }