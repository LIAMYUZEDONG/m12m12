﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace m12.Models
{
    public class AppointmentResponse
    {
        //Adding model validation for the sign up form:
        [Key]
        [Required]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = " Please enter a valid group name.")]
        public string Name { get; set; }

        [Required]
        [Range(1, 15, ErrorMessage = "Do not enter more than 15")]
        public int Size { get; set; }

        [Required(ErrorMessage = " Please enter a valid email address.")]
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

        //Users can double check their selected time and date. 
        public string Date { get; set; }
        public string Time { get; set; }

    }
}
