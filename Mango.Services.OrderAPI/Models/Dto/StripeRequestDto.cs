﻿using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI.Models
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl { get; set; }

        public string? StripeSessionId { get; set; }

        public string ApprovedUrl { get; set; }

        public string cancelUrl { get; set; }

        public OrderHeaderDto OrderHeader { get; set; }
    }
}
