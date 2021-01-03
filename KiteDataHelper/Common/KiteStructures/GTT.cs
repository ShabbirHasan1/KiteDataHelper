using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTT
    {
        public GTT(Dictionary<string, dynamic> data)
        {
            try
            {
                Id = Convert.ToInt32(data["id"]);
                Condition = new GTTCondition(data["condition"]);
                TriggerType = data["type"];

                Orders = new List<GTTOrder>();
                foreach (Dictionary<string, dynamic> item in data["orders"])
                    Orders.Add(new GTTOrder(item));

                Status = data["status"];
                CreatedAt = CommonFunctions.StringToDate(data["created_at"]);
                UpdatedAt = CommonFunctions.StringToDate(data["updated_at"]);
                ExpiresAt = CommonFunctions.StringToDate(data["expires_at"]);
                Meta = new GTTMeta(data["meta"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public int Id { get; set; }
        public GTTCondition? Condition { get; set; }
        public string TriggerType { get; set; }
        public List<GTTOrder> Orders { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public GTTMeta? Meta { get; set; }
    }
}
