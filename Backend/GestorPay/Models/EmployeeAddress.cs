﻿using GestorPay.Helper;

namespace GestorPay.Models
{
    public class EmployeeAddress : BaseEntity
    {
        public int EmployeeId { get; set; }
        public string Street { get; set; }
        public string HomeNumber { get; set; }
        public string? Complement { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public bool IsRemoved { get; set; }
    }
}
