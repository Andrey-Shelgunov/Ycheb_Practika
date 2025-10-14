using System;

namespace TestPriem
{
    public class GoodsReceipt
    {
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public int QuantityReceived { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int ReceivedByStaffId { get; set; }
        public int SupplierId { get; set; }
        public string BatchNumber { get; set; }
    }
}