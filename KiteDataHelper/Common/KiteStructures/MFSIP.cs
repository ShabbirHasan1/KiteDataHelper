using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct MFSIP
    {
        public MFSIP(Dictionary<string, dynamic> data)
        {
            try
            {
                DividendType = data["dividend_type"];
                PendingInstalments = Convert.ToInt32(data["pending_instalments"]);
                Created = CommonFunctions.StringToDate(data["created"]);
                LastInstalment = CommonFunctions.StringToDate(data["last_instalment"]);
                TransactionType = data["transaction_type"];
                Frequency = data["frequency"];
                InstalmentDate = Convert.ToInt32(data["instalment_date"]);
                Fund = data["fund"];
                SIPId = data["sip_id"];
                Tradingsymbol = data["tradingsymbol"];
                Tag = data["tag"];
                InstalmentAmount = Convert.ToInt32(data["instalment_amount"]);
                Instalments = Convert.ToInt32(data["instalments"]);
                Status = data["status"];
                OrderId = data.ContainsKey(("order_id")) ? data["order_id"] : "";
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public string DividendType { get; }
        public int PendingInstalments { get; }
        public DateTime? Created { get; }
        public DateTime? LastInstalment { get; }
        public string TransactionType { get; }
        public string Frequency { get; }
        public int InstalmentDate { get; }
        public string Fund { get; }
        public string SIPId { get; }
        public string Tradingsymbol { get; }
        public string Tag { get; }
        public int InstalmentAmount { get; }
        public int Instalments { get; }
        public string Status { get; }
        public string OrderId { get; }
    }
}
