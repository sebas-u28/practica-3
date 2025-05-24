using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practica.Models
{
    [Table("t_feedback")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe llenar el campo PostId")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un sentimiento")]
        public SentimientoTipo Sentimiento { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }

    public enum SentimientoTipo
    {
        Like,
        Dislike
    }
}
