//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM1._2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ClientTable
    {
        [Key]
        public int ClientID { get; set; }

        [Required(ErrorMessage = "Nip jest wymagany!")]
        [MinLength(10, ErrorMessage = "Nip powinien zawierac minimum 10 liczb")]
        [MaxLength(10, ErrorMessage = "Nip powinien zawierac maximum 10 liczb")]
        public string Nip { get; set; }

        [Required(ErrorMessage = "Nazwa firmy jest wymagana!")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Adres firmy jest wymagany!")]
        public string Address { get; set; }

        public string Phone { get; set; }

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Prosze wprowadzic poprawny Email!")]
        public string Email { get; set; }
    }
}
