using System.ComponentModel.DataAnnotations;

namespace MAUIBasics
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress] // Validierung der E-Mail-Formatierung
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime Birthdate { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        public string? ApiKey { get; set; }
    }
}