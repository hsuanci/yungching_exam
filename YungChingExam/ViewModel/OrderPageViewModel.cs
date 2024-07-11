namespace YungChingExam.ViewModel
{
    public class OrderPageViewModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public string ShipperCompanyName { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public List<OrderDetailPageViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailPageViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }

    public class OrderPaginationViewModel
    {
        public List<OrderPageViewModel> Orders { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
