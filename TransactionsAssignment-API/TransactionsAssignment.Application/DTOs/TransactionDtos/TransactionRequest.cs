using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionsAssignment.Application.DTOs.TransactionDtos
{
    public class TransactionRequest
    {
        public string ProcessingCode { get; set; }
        public int SystemTraceNr { get; set; }
        public string FunctionCode { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public decimal AmountTrxn { get; set; }
        public int CurrencyCode { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(ProcessingCode)) return false;
            if (SystemTraceNr <= 0) return false;
            if (string.IsNullOrWhiteSpace(FunctionCode)) return false;
            if (string.IsNullOrWhiteSpace(CardNo)) return false;
            if (AmountTrxn <= 0) return false;
            if (CurrencyCode <= 0) return false;

            return true;
        }
    }


}
