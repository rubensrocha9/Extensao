﻿namespace GestorPay.Models.DTOs
{
    public class CompanyAddressDTO
    {
        public int CompanyId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public bool IsRemoved { get; set; }
    }
}
