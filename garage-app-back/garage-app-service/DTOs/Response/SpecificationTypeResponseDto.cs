﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace garage_app_service.DTOs.Response
{
    public class SpecificationTypeResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsRequiredForCar { get; set; }
    }
}