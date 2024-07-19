using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Final_EstructuraDatos
{
    public class ListaDoblementeEnlazada
    {
        //Declaro los dos campos
        private Producto pri;
        private Producto ult;

        //Declaro las dos propiedades
        public Producto Primero
        {
            get { return pri; }
            set { pri = value; }
        }
        public Producto Ultimo
        {
            get { return ult; }
            set { ult = value; }
        }

        //declaro los metodos 
        public void Agregar(Producto nuevo, List<Producto> listaaux)
        {
            if (Primero == null && Ultimo == null)
            {
                Primero = nuevo;
                Ultimo = nuevo;
                listaaux.Add(nuevo);
                MessageBox.Show("Producto agregado con exito al stock", "NUEVO PRODUCTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Verifico si el codigo existe
                Producto actual = Primero;
                while (actual != null)
                {
                    if (actual.cod == nuevo.cod)
                    {

                        MessageBox.Show("El codigo de producto ya se encuentra registrado", "NUEVO PRODUCTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    actual = actual.Siguiente;
                }

                // Si el codigo no esta repetido lo agrego

                if (nuevo.cod < Primero.cod)
                {
                    nuevo.Siguiente = Primero;
                    Primero.Anterior = nuevo;
                    Primero = nuevo;
                   
                }
                else if (nuevo.cod > Ultimo.cod)
                {
                    Ultimo.Siguiente = nuevo;
                    nuevo.Anterior = Ultimo;
                    Ultimo = nuevo;
                   

                }
                else
                {
                    Producto aux = Primero;
                    Producto ant = Primero;
                    while (aux.cod < nuevo.cod)
                    {
                        ant = aux;
                        aux = aux.Siguiente;
                    }

                    ant.Siguiente = nuevo;
                    nuevo.Siguiente = aux;
                    aux.Anterior = nuevo;
                    nuevo.Anterior = ant;
                   
                }
                listaaux.Add(nuevo);
                MessageBox.Show("Producto agregado con exito al stock", "NUEVO PRODUCTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void AgregarParaJson(Producto nuevo)
        {
            if (Primero == null && Ultimo == null)
            {
                Primero = nuevo;
                Ultimo = nuevo;
               
            }
            else
            {
                // Verifico si el codigo existe
                Producto actual = Primero;
                while (actual != null)
                {
                    if (actual.cod == nuevo.cod)
                    {

                        MessageBox.Show("El codigo de producto ya se encuentra registrado", "NUEVO PRODUCTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    actual = actual.Siguiente;
                }

                // Si el codigo no esta repetido lo agrego

                if (nuevo.cod < Primero.cod)
                {
                    nuevo.Siguiente = Primero;
                    Primero.Anterior = nuevo;
                    Primero = nuevo;

                }
                else if (nuevo.cod > Ultimo.cod)
                {
                    Ultimo.Siguiente = nuevo;
                    nuevo.Anterior = Ultimo;
                    Ultimo = nuevo;

                }
                else
                {
                    Producto aux = Primero;
                    Producto ant = Primero;
                    while (aux.cod < nuevo.cod)
                    {
                        ant = aux;
                        aux = aux.Siguiente;
                    }

                    ant.Siguiente = nuevo;
                    nuevo.Siguiente = aux;
                    aux.Anterior = nuevo;
                    nuevo.Anterior = ant;

                }
                
            }
        }
       
        public void Eliminar(Int32 codigo)
        {
            if (Primero == null)
            {
                MessageBox.Show("No hay elementos para eliminar");
            }
            else if (Primero.cod == codigo && Ultimo == Primero) // si hay un solo elemento y este es igual al que necesitamos 
            {
                // se elimina 
                Primero = null;
                Ultimo = null;


            }
            else
            {
                if (Primero.cod == codigo)// si el primer nodo es el que coincide
                {
                    Primero = Primero.Siguiente; // Pasamos el puntero PRIMERO la siguiente nodo
                    Primero.Anterior = null; // Eliminamos el nodo que es igual a codigo
                }
                else
                {
                    if (Ultimo.cod == codigo)//Si el ultimo nodo es el que coincide
                    {
                        Ultimo = Ultimo.Anterior; // el puntero ultimo pasa al anterior nodo que sea ahora el ultimo
                        Ultimo.Siguiente = null;  // eliminamos el siguiente (de ahora  nuevo ultimo) que es el que coincidio
                    }
                    else// si el que buscamos no esta en los extremos 
                    {
                        // tenemos que encontrarlo con una repetitiva
                        // para eso utilizamos 2 auxiliares mas para siguiente y anterior (punteros internos)
                        Producto aux = Primero;
                        Producto ant = Primero;
                        while (aux.cod!= codigo)
                        {
                            //Mientras sea diferente
                            ant = aux; // anterior toma el lugar del aux que comparo
                            aux = aux.Siguiente; // aux pasa a la siguiente posicion para seguir comparando
                        }

                        // si lo encontro
                        ant.Siguiente = aux.Siguiente; // esto va a dar null entonces lo elimina
                        aux = aux.Siguiente; // esto tmb es null por ende lo elimina
                        aux.Anterior = ant;


                    }
                }
            }







        }




        public void Recorrer(DataGridView Grilla)
        {
            Producto aux = Primero;
            Grilla.Rows.Clear();
            while (aux != null)
            {
                Grilla.Rows.Add(aux.cod, aux.nom, aux.desc, aux.stock, aux.monto);
                aux = aux.Siguiente;
            }

        }

        public void Recorrer(List<Producto> lista , DataGridView Grilla)
        {
           
            Grilla.Rows.Clear();
            foreach (var item in lista)
            {
                
                
                    Grilla.Rows.Add(item.cod,item.nom,item.desc,item.stock,item.monto);
                  
                
            }
            

        }



        public void RecorrerDes(DataGridView Grilla)
        {
            Producto aux = Ultimo;
            Grilla.Rows.Clear();
            while (aux != null)
            {
                Grilla.Rows.Add(aux.cod, aux.nom, aux.desc, aux.stock, aux.monto);
                aux = aux.Anterior;
            }
        }



    }
}
