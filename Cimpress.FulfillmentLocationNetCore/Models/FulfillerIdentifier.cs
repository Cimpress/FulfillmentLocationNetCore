using System;

namespace Cimpress.FulfillmentLocationNetCore.Models
{
    public class FulfillerIdentifier
    {
        private int? _internalFulfillerId;
        private string _fulfillerId;
        private static readonly string _conflictExceptionText = "A fulfiller ID has already been specified. Clear the other identifier and redo the operation.";

        public int? InternalFulfillerId
        {
            get
            {
                return _internalFulfillerId;
            }
            set
            {
                if (!String.IsNullOrEmpty(_fulfillerId))
                {
                    throw new Exception(_conflictExceptionText);
                }
                _internalFulfillerId = value;
            }
        }
        public string FulfillerId
        {
            get
            {
                return _fulfillerId;
            }
            set
            {
                if (_internalFulfillerId.HasValue)
                {
                    throw new Exception(_conflictExceptionText);
                }
                _fulfillerId = value;
            }
        }

        public override string ToString()
        {
            if (_internalFulfillerId.HasValue)
            {
                return _internalFulfillerId.ToString();
            }
            else
            {
                return _fulfillerId.ToString();
            }
        }
    }
}
