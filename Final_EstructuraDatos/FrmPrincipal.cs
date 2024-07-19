using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using System.IO;

namespace Final_EstructuraDatos
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }
        private Color colorSeleccion = Color.FromArgb(100, 100, 100);
        private Color colorNormal = Color.FromArgb(180, 180, 180);
        ListaDoblementeEnlazada lista = new ListaDoblementeEnlazada();

        List<Producto> listaProductos = new List<Producto>(); // lista para pasar los productos de la grilla
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            
            this.ShowIcon = false;
            lblTituloApp.Text = "";
            PanelContenedor.Visible = false;
            PanelAgregar.Visible = false;
            PanelDesplegable.Height = 650;
            PanelContenedor.BringToFront();
            

            timerFecha.Start();
            LeerJson();
            lista.Recorrer(dgvGrilla);
            dgvGrilla.Columns[4].DefaultCellStyle.Format = "N0";
            //if (dgvGrilla.RowCount > 0)
            //{
            //    foreach (DataGridViewRow row in dgvGrilla.Rows)
            //    {
            //        Producto nuevo = new Producto();
            //        nuevo.cod = Convert.ToInt32(row.Cells[0].Value);
            //        nuevo.nom = row.Cells[1].Value.ToString();
            //        nuevo.desc = row.Cells[2].Value.ToString();
            //        nuevo.stock = Convert.ToInt32(row.Cells[3].Value);
            //        nuevo.monto = Convert.ToDecimal(row.Cells[4].Value);

            //        listaProductos.Add(nuevo);
            //    }

            //}
            
            CantidadStock(lblCantidadStock, listaProductos);
            GraficoTopStock(listaProductos);



        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ExportarJson(dgvGrilla);
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //Drag Form , para que el formulario se mueva desde los paneles
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PanelDesplegable_MouseDown(object sender, MouseEventArgs e)
        {
            // codigo movimiento panel desplegable
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void PanelLateral_MouseDown(object sender, MouseEventArgs e)
        {
            // codigo movimiento panel lateral
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // codigo movimiento panel imagen
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);

            // codigo paneles
            PanelTimer.Visible = true;
            PanelContenedor.Visible = false;
            PanelAgregar.Visible = false;
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            AnimacionBajarPanel();
            PanelContenedor.Visible = false;
            PanelAgregar.Visible = false;
            restrablecerBotones();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            AnimacionSubirPanel();
            PanelTimer.Visible = true;
        }

        public void AnimacionSubirPanel() 
        {
            lblTituloApp.Text = "TechStore";
            // Obtiene la altura actual del panel
            int alturaActual = PanelDesplegable.Height;

            // Define la altura final del panel según el estado actual
            int alturaFinal = (alturaActual == 32) ? 650 : 32;

            // Define el evento Tick del Timer
            tmrSubir.Tick += delegate (object sender2, EventArgs e2)
            {
                // Calcula la diferencia de altura
                int diferenciaAltura = alturaFinal - alturaActual;

                // Calcula el incremento de altura para cada tick
                int incrementoAltura = diferenciaAltura / 100; // Ajusta este valor para controlar la suavidad de la animación

                // Actualiza la altura del panel
                PanelDesplegable.Height += incrementoAltura;

                // Verifica si se ha alcanzado la altura final
                if ((diferenciaAltura > 0 && PanelDesplegable.Height >= alturaFinal) || (diferenciaAltura < 0 && PanelDesplegable.Height <= alturaFinal))
                {
                    // Ajusta la altura final al valor exacto si se ha excedido
                    PanelDesplegable.Height = alturaFinal;

                    // Detiene el Timer
                    tmrSubir.Stop();
                }
            };
            tmrSubir.Start();



        }

        public void AnimacionBajarPanel() 
        {
            lblTituloApp.Text = "";
            // Obtiene la altura actual del panel
            int alturaActual = PanelDesplegable.Height;

            // Define la altura final del panel
            int alturaFinal = 650; // Altura a la que se quiere subir

            // Define el evento Tick del Timer
            timerBajar.Tick += delegate (object sender, EventArgs e)
            {
                // Calcula la diferencia de altura
                int diferenciaAltura = alturaFinal - alturaActual;

                // Calcula el incremento de altura para cada tick
                int incrementoAltura = Math.Sign(diferenciaAltura) * Math.Max(1, Math.Abs(diferenciaAltura) / 100); // Ajusta este valor para controlar la suavidad de la animación

                // Actualiza la altura del panel
                PanelDesplegable.Height += incrementoAltura;

                // Verifica si se ha alcanzado la altura final
                if (PanelDesplegable.Height >= alturaFinal)
                {
                    // Ajusta la altura final al valor exacto si se ha excedido
                    PanelDesplegable.Height = alturaFinal;

                    // Detiene el Timer
                    timerBajar.Stop();
                }
            };
            timerBajar.Start();
        }

     

        private void btnVerProductos_MouseDown(object sender, MouseEventArgs e)
        {

            btnVerProductos.Font = new Font("Century Gothic", 12, style: FontStyle.Italic);
            btnVerProductos.BackColor = colorSeleccion;
            btnVerProductos.IconColor = colorNormal;
            btnVerProductos.ForeColor = colorNormal;
            btnVerProductos.IconSize = 38;

            btnAgregar.BackColor = colorNormal;
            btnAgregar.IconColor = Color.Black;
            btnAgregar.ForeColor = Color.Black;
            btnAgregar.IconSize = 20;
            btnAgregar.Font = new Font("Century Gothic", 8, style: FontStyle.Italic);
        }
        private void btnAgregar_MouseDown(object sender, MouseEventArgs e)
        {
            btnAgregar.IconSize = 38;
            btnAgregar.BackColor = colorSeleccion;
            btnAgregar.IconColor = colorNormal;
            btnAgregar.ForeColor = colorNormal;
            // cambio de color al boton no seleccionado y propiedades
            btnVerProductos.BackColor = colorNormal;
            btnVerProductos.IconColor = Color.Black;
            btnVerProductos.ForeColor = Color.Black;
            btnVerProductos.IconSize = 20;
            btnVerProductos.Font = new Font("Century Gothic", 8 , style:FontStyle.Italic);
        }

       
        

        private void btnVerProductos_Click(object sender, EventArgs e)
        {
            //AbrirFormulario<FrmProductos>();
            PanelContenedor.Visible = true;
            PanelAgregar.Visible = false;
            PanelTimer.Visible = false;


        }
        // codigo boton agregar productos
        private void iconButton3_Click(object sender, EventArgs e)
        {
            PanelAgregar.Visible = true;
            PanelContenedor.Visible = false;
            PanelTimer.Visible = false;





        }

        private void timerFecha_Tick(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToLongTimeString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PanelTimer.Visible = true;
            PanelContenedor.Visible = false;
            PanelAgregar.Visible = false;
           
            
        }
        public void restrablecerBotones() 
        {
            btnAgregar.IconSize = 38;
            btnAgregar.BackColor = colorNormal;
            btnAgregar.IconColor = Color.Black;
            btnAgregar.ForeColor = Color.Black;

            btnVerProductos.IconSize = 38;
            btnVerProductos.BackColor = colorNormal;
            btnVerProductos.IconColor = Color.Black;
            btnVerProductos.ForeColor = Color.Black;
        }
        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        // variable usada como bandera
        private bool CajasVacias = true;
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                ControlCajas();
                // Controlar que las cajas este completas
                if (CajasVacias == true)
                {
                    MessageBox.Show("Por favor complete todos los campos requeridos");
                }
                else
                {
                    Producto nuevo = new Producto();
                    nuevo.cod = Convert.ToInt32(txtCodigo.Text);
                    nuevo.nom = txtNombre.Text;
                    nuevo.desc = txtDescripcion.Text;
                    nuevo.stock = Convert.ToInt32(txtStock.Text);
                    nuevo.monto = Convert.ToDecimal(txtPrecio.Text);

                    lista.Agregar(nuevo,listaProductos);
                    
                   
                    lista.Recorrer(dgvGrilla);

                    LimpiarCajas();
                    CantidadStock(lblCantidadStock,listaProductos);
                    GraficoTopStock(listaProductos);



                }
            }
            catch (Exception EX)
            {

                //string mensaje = "Ha ocurrido un error inesperado,comuniquese con el desarrolador";

                //MessageBox.Show(mensaje, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(EX.ToString());
            }
           
            
        }
        public void ControlCajas() 
        {
            if (txtCodigo.Text!= "" && txtNombre.Text !="" && txtDescripcion.Text !="" && txtStock.Text !="" && txtPrecio.Text !="")
            {
                CajasVacias = false;
                
            }
            else
            {
                CajasVacias = true;

            }
        
        
        
        }
        public void LimpiarCajas()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtStock.Text = "";
            txtPrecio.Text = "";
        }

        private void chkListaDescendente_CheckedChanged(object sender, EventArgs e)
        {
            if (chkListaDescendente.Checked== true)
            {
                lista.RecorrerDes(dgvGrilla);
            }
            else
            {
                lista.Recorrer(dgvGrilla);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrilla.RowCount != 0)
                {
                    DialogResult pregunta = MessageBox.Show("¿Esta seguro que desea eliminar el producto seleccionado?", "ELIMINAR PRODUCTO",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                    if (pregunta == DialogResult.Yes)
                    {
                        Int32 cod_seleccionado = Convert.ToInt32(dgvGrilla.CurrentRow.Cells[0].Value);

                        // eliiminar de la lista doblemente enlazada

                        lista.Eliminar(cod_seleccionado);
                        // eliminar de la lista para los filtros
                        Producto seleccion = EncontrarProducto(cod_seleccionado);
                        listaProductos.Remove(seleccion);




                        lista.Recorrer(dgvGrilla);
                        txtBusqueda.Text = "";
                        CantidadStock(lblCantidadStock, listaProductos);
                        GraficoTopStock(listaProductos);
                    }
                  
                   


                }
                else
                {
                    MessageBox.Show("No hay productos para eliminar", "ELIMINAR PRODUCTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {

                string mensaje = "Ha ocurrido un error inesperado,comuniquese con el desarrolador";

                MessageBox.Show(mensaje, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            
        }

        List<Producto> listafiltro = new List<Producto>(); // lista para el filtro
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtBusqueda.Text;


                // creamos la lista filtro
                if (filtro.Length > 0)
                {
                    dgvGrilla.Rows.Clear();
                    int codBusqueda = Convert.ToInt32(filtro);

                    Producto encontrado = listaProductos.FirstOrDefault(x => x.cod == codBusqueda); // buscamos el producto 
                    if (encontrado != null)
                    {
                        listafiltro.Clear();// limpio la lista para q no se acumulen
                                            // si lo encontramos mostramos mediante la lista filtro a este mismo
                        listafiltro.Add(encontrado);
                        lista.Recorrer(listafiltro, dgvGrilla);
                    }
                    else
                    {

                    }

                }
                else
                {
                    // si no se encuentra mostramos con el metodo recorrer normal
                    lista.Recorrer(dgvGrilla);
                }
            }
            catch (Exception)
            {

                string mensaje = "Ha ocurrido un error inesperado,comuniquese con el desarrolador";

                MessageBox.Show(mensaje, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
           




        }

        public Producto EncontrarProducto(Int32 codigo)
        {

            Producto seleccionado = new Producto();
            foreach (var item in listaProductos)
            {
                if (item.cod == codigo)
                {
                    seleccionado = item;
                }
            }

            return seleccionado;
        }


        public void LeerJson()
        {
            try
            {
                string archivojson = File.ReadAllText(@"C:\Users\Pablo\Desktop\IES\SEGUNDO\Finales\Estructura de Datos\Productos.json");

                // Deserializa la cadena JSON en un DataTable
                DataTable productosJson = (DataTable)JsonConvert.DeserializeObject(archivojson, typeof(DataTable));

                // Convierte el DataTable en una lista de objetos Producto
                List<Producto> productos = new List<Producto>();
                foreach (DataRow row in productosJson.Rows)
                {
                    Producto nuevo = new Producto();
                    nuevo.cod = Convert.ToInt32(row[0]);
                    nuevo.nom = row[1].ToString();
                    nuevo.desc = row[2].ToString();
                    nuevo.stock = Convert.ToInt32(row[3]);
                    nuevo.monto = Convert.ToDecimal(row[4]);

                    listaProductos.Add(nuevo);
                    lista.AgregarParaJson(nuevo);
                }


                lista.Recorrer(dgvGrilla);

            }
            catch (Exception)
            {


                string mensaje = "Ha ocurrido un error inesperado,comuniquese con el desarrolador";

                MessageBox.Show(mensaje, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }


        public void GraficoTopStock(List<Producto> lista) 
        {
            try
            {
                chart1.Series["Stock Productos"].Points.Clear();

                chart1.Series["Stock Productos"].IsValueShownAsLabel = true;

                var MayorStock = (from producto in lista
                                  orderby producto.stock
                                  select new { producto.nom, producto.stock }).Take(5);

                foreach (var item in MayorStock)
                {
                    chart1.Series["Stock Productos"].Points.AddXY(item.nom, item.stock);
                }
            }
            catch (Exception)
            {

                string mensaje = "Ha ocurrido un error inesperado,comuniquese con el desarrolador";

                MessageBox.Show(mensaje, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            

        }

        public void CantidadStock (Label label , List<Producto> lista) 
        {
            try
            {
                int CantidadStock = lista.Count();
                label.Text = CantidadStock.ToString();
            }
            catch (Exception)
            {
                string mensaje = "Ha ocurrido un error inesperado,comuniquese con el desarrolador";
               
                MessageBox.Show(mensaje,"error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
           
        
        
        
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Title = "Guardar Reporte";
                saveFileDialog1.Filter = "Valores separados por comas(.csv)|*.csv";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    //Codigo para exportar al excel

                    ExportarExcel(saveFileDialog1.FileName);

                    MessageBox.Show("Reporte guardado correctamente");


                }


            }
            catch (Exception)
            {

                MessageBox.Show("Ha ocurrido un error en el guardado del reporte", "Guardar Reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ExportarExcel(String RutaArchivo) 
        {
            try
            {

                Int32 Cantidad = 0;

                StreamWriter AD = new StreamWriter(RutaArchivo, false, Encoding.UTF8);

                AD.WriteLine(";Lista Productos en stock\n");


                AD.WriteLine("Codigo; Nombre;Descripcion;Stock;Precio\n");

                foreach (DataGridViewRow row in dgvGrilla.Rows)
                {
                    Int32 codigo= Convert.ToInt32(row.Cells[0]?.Value); ;
                    string nombre = row.Cells[1]?.Value?.ToString() ?? ""; // los signos de pregunta son para ahorrar valores nulos en las celdas
                    string Desc = row.Cells[2]?.Value?.ToString() ?? "";
                    Int32 stock = Convert.ToInt32(row.Cells[3]?.Value);
                    Decimal Precio = Convert.ToDecimal(row.Cells[4]?.Value ?? 0);



                    AD.Write(codigo);
                    AD.Write(";");
                    AD.Write(nombre);
                    AD.Write(";");
                    AD.Write(Desc);
                    AD.Write(";");
                    AD.Write(stock);
                    AD.Write(";");
                    AD.WriteLine(Precio.ToString("N2")); ;


                   

                    Cantidad++;
                }


                AD.WriteLine(";");
                AD.Write($"Cantidad de Productos:  {Cantidad}");
               
               
               

                AD.Close();
                AD.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        
        
        
        
        }

        private void ExportarJson(DataGridView dataGridView)
        {
            // Ruta del archivo JSON
            string rutaArchivoJSON = "./Productos.json";

            // Crear un DataTable
            DataTable ds = new DataTable();

            // Agregar columnas al DataTable
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                ds.Columns.Add(column.HeaderText);
            }

            // Agregar filas al DataTable
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = ds.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.ColumnIndex] = cell.Value;
                }
            }

            // Convertir el DataTable a JSON
            string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

            // Escribir el JSON en un archivo
            File.WriteAllText(rutaArchivoJSON, json);

            
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

      

    }
}
