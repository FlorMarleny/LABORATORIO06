using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace el3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }
        DataView dv;
        void cargaPedidos()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                   
                    SqlDataAdapter da = new SqlDataAdapter("pa_listaClientes", cn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "cliente");
                    dv = new DataView(ds.Tables["cliente"]);
                    dataGridView1.DataSource = dv;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
       
        void buscarClientes(string dato)
        {
            dv.RowFilter = "IdCliente like '%" + dato + "%' or NombreCia like '%" + dato + "%'";
            dataGridView1.DataSource = dv;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            buscarClientes(textBox1.Text);
        }
        void cargarDetallesPedido(string idCliente)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    string query = "pa_listaOrdenesPorCliente";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcliente", idCliente);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "detallePedido");
                    dataGridView2.DataSource = ds.Tables["detallePedido"]; // Cargar los detalles del pedido en dataGridView2
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        // mas adelante


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargaPedidos();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                string idCliente = dataGridView1.Rows[e.RowIndex].Cells["IdCliente"].Value.ToString();

                cargarDetallesPedido(idCliente);
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string idCliente = dataGridView1.SelectedRows[0].Cells["IdCliente"].Value.ToString();

                cargarDetallesPedido(idCliente);
            }
        }




    }
}
