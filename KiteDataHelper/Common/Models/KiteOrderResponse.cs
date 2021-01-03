namespace KiteDataHelper.Common.Models
{
    public class KiteOrderResponse
    {
        private OrderId _orderId;
        public KiteOrderResponse()
        {
            this._orderId = new OrderId();
        }
        public string status { get; set; }
        public OrderId data
        {
            get
            {
                return _orderId;
            }

            set
            {
                _orderId = value;
            }
        }
    }

    public class OrderId
    {
        public string order_id { get; set; }
    }
}
