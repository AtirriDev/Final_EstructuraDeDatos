using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_EstructuraDatos
{
    public class Producto
    {
        //Propiedades privadas
         private Int32 Codigo { get; set; }
         private string Nombre { get; set; }
         private string Descripcion { get; set; }

        private Int32 Cantidad_Stock { get; set; }
        private Decimal Precio { get; set; }

        private Producto sig { get; set; }

        private Producto ant { get; set; }

        // PROPIEDADES ACCESIBLES 
        public Int32 cod
        {
            get { return Codigo; }
            set { Codigo = value; }
        }

        public string nom
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public string desc
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public Int32 stock
        {
            get { return Cantidad_Stock; }
            set { Cantidad_Stock = value; }
        }


        public Decimal monto
        {
            get { return Precio; }
            set { Precio = value; }
        }

        public Producto Siguiente
        {
            get { return sig; }
            set { sig = value; }

        }

        public Producto Anterior
        {
            get { return ant; }
            set { ant = value; }

        }
    }
}
