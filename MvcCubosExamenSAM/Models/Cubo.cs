using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCubosExamenSAM.Models
{
    [Table("CUBOS")]
    public class Cubo
    {
        [Key]
        [Column("id_cubo")]
        public int IdCubo { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("marca")]
        public string Marca { get; set; }

        [Column("imagen")]
        public string Imagen { get; set; }

        [Column("precio")]
        public int Precio { get; set; }
    }
}
