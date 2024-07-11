namespace YungChingExam.ViewModel
{
    public class OrderCreateViewModel
    {
        public string CustomerId { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public bool useCustomerCurrentAddressState { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }
        public short Quantity { get; set; }
    }
}
