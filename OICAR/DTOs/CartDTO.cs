namespace OICAR.DTOs
{
    public class CartDTO
    {
        public int IDCart { get; set; }
        public int UserID { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
