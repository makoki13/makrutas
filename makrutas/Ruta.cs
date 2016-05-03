using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makrutas
{
    class Ruta
    {
        String id;        
        String nombre;
        String usuario;
        DateTime fechaCreacion;
        DateTime? fechaPrimeraRealizacion;
        
        Ruta()
        {
            this.id = "1";
            this.nombre = "sin nombre";
            this.usuario = "root";
            this.fechaCreacion = DateTime.Now;
            this.fechaPrimeraRealizacion = null; 
        }

        Ruta(String id)
        {
            this.id = id;
        }
    }
}
