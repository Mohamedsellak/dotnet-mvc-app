public class Purchase 
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null;
    public int UserId { get; set; }

    public User User { get; set; } = null;
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}