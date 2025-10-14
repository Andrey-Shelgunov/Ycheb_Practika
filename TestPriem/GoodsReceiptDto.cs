namespace TestPriem
{
    internal class GoodsReceiptDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int ReceivedByStaffId { get; set; }
        public int SupplierId { get; set; }
        public string BatchNumber { get; set; }
    }
}