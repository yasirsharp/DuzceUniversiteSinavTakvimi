using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entity.DTOs
{
    public class SinavGuncelleDTO : IDto
    {
        public int Id { get; set; }
        public int DbapId { get; set; }
        public DateTime SinavTarihi { get; set; }
        public string SinavSaati { get; set; }  // HH:mm formatında string olarak
        public List<DerslikGozetmenDTO> Derslikler { get; set; }
    }
} 