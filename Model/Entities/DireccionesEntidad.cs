using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDirecciones.Model.Entities
{
    public class DireccionEntidad
    {
        public int? Id { get; set; }

        public string? Calle { get; set; }
        public string? Colonia { get; set; }
        
        public string? Municipio { get; set; }

        public int? Numero { get; set; }
        
        public string? CP { get; set; }

        public Boolean? Activo { get; set; }

        public DateTime? Fecha_modificacion { get; set; }
        
        public DateTime? Fecha_creacion { get; set; }
    }
}