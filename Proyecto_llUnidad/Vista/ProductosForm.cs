using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductosForm : Form
    {
        public ProductosForm()
        {
            InitializeComponent();
        }
        ProductoDatos proDatos = new ProductoDatos();
        Producto producto;
        string tipoOperacion = string.Empty;
        private void ProductosForm_Load(object sender, EventArgs e)
        {
            CompletarProductos();
        }

        private async void CompletarProductos()
        {
            ProductosDataGridView.DataSource = await proDatos.DevolverListAsync();
        }

        private void ActivarControles()
        {
            CodigoTextBox.Enabled = true;
            DescripcionTextBox.Enabled = true;
            PrecioTextBox.Enabled = true;
            ExistenciaTextBox.Enabled = true;
            FechaDateTimePicker1.Enabled = true;
            ImagenPictureBox.Enabled = true;
            AdjuntarImagenButton.Enabled = true;
        }

        private void VaciarControles()
        {
            CodigoTextBox.Clear();
            DescripcionTextBox.Clear();
            PrecioTextBox.Clear();
            ExistenciaTextBox.Clear();
            FechaDateTimePicker1.Value = DateTime.Now;   
            ImagenPictureBox.Enabled = true;
            AdjuntarImagenButton.Enabled = true;
        }

        private void DesactivarControles()
        {
            CodigoTextBox.Enabled = false;
            DescripcionTextBox.Enabled = false;
            PrecioTextBox.Enabled = false;
            ExistenciaTextBox.Enabled = false;
            FechaDateTimePicker1.Enabled = false;
            ImagenPictureBox.Enabled = false;
            AdjuntarImagenButton.Enabled = false;
        }

    
        private void NuevoButton_Click(object sender, EventArgs e)
        {
            tipoOperacion = "Nuevo";
            ActivarControles();
        }

        private async Task GuardarButton_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CodigoTextBox.Text))
            {
                errorProvider1.SetError(CodigoTextBox, "Ingrese un Codigo");
                CodigoTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DescripcionTextBox.Text))
            {
                errorProvider1.SetError(DescripcionTextBox, "Ingrese una Descripcion");
                DescripcionTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(PrecioTextBox.Text))
            {
                errorProvider1.SetError(PrecioTextBox, "Ingrese un Precio");
                PrecioTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(ExistenciaTextBox.Text))
            {
                errorProvider1.SetError(ExistenciaTextBox, "Ingrese una Existencia");
                ExistenciaTextBox.Focus();
                return;
            }
            producto = new Producto();

            if (ImagenPictureBox.Image != null)
            {
                MemoryStream ms = new MemoryStream();
                ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                producto.Imagen = ms.GetBuffer();

            }
            else
            {
                producto.Imagen = null;
            }

            producto.Codigo = Convert.ToInt32(CodigoTextBox.Text);
            producto.Descripcion = DescripcionTextBox.Text;
            producto.Existencia = Convert.ToInt32(ExistenciaTextBox.Text);
            producto.Precio = Convert.ToDecimal(PrecioTextBox.Text);
            producto.FechaCreacion = FechaDateTimePicker1.Value;

            if (tipoOperacion == "Nuevo")
            {
                bool inserto = await proDatos.InsertarAsync(producto);

                if (inserto)
                {
                    CompletarProductos();
                    VaciarControles();
                    DesactivarControles();
                    MessageBox.Show("Producto Guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Producto no se pudo Guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (tipoOperacion == "Modificar")
            {
                bool modifico = await proDatos.ActualizarAsync(producto);
                if (modifico)
                {
                    CompletarProductos();
                    VaciarControles();
                    DesactivarControles();
                    MessageBox.Show("Producto Guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Producto no se pudo Guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AdjuntarImagenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ImagenPictureBox.Image = Image.FromFile(dialog.FileName);
            }
        }

        private async void ModificarButton_Click(object sender, EventArgs e)
        {
            if (ProductosDataGridView.SelectedRows.Count > 0)
            {
                tipoOperacion = "Modificar";
                ActivarControles();
                CodigoTextBox.ReadOnly = true;

                CodigoTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString();
                DescripcionTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Descripcion"].Value.ToString();
                ExistenciaTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Existencia"].Value.ToString();
                PrecioTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Precio"].Value.ToString();
                FechaDateTimePicker1.Value = Convert.ToDateTime(ProductosDataGridView.CurrentRow.Cells["FechaCreacion"].Value);

                byte[] imagenDeBaseDatos = await proDatos.SeleccionarImagen(ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());

                if (imagenDeBaseDatos.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(imagenDeBaseDatos);
                    ImagenPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);

                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void EliminarButton_Click(object sender, EventArgs e)
        {
            if (ProductosDataGridView.SelectedRows.Count > 0)
            {
                bool elimino = await proDatos.EliminarAsync(ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                if (elimino)
                {
                    CompletarProductos();
                    MessageBox.Show("Producto Eliminado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Producto no se pudo Eliminar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExistenciaTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PrecioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
