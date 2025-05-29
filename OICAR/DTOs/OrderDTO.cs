namespace OICAR.DTOs
{
    public class OrderDTO
    {
        public int IDOrder { get; set; }
        public int UserID { get; set; }
        public int StatusID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
