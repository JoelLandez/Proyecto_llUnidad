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
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }

        UsuarioDatos userDatos = new UsuarioDatos();
        string tipoOperacion = string.Empty;
        Usuario user;
        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            CompletarDataGrid();
        }

        private async void CompletarDataGrid()
        {
            UsuariosDataGridView.DataSource = await userDatos.DevolverListAsync();
        }

        private void NuevoButton_Click(object sender, EventArgs e)
        {
            ActivarControles();
            tipoOperacion = "Nuevo";
            
        }

        private void ActivarControles()
        {
            CodigoTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            ClaveTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            RolComboBox.Enabled = true;
            ActivoCheckBox.Enabled = true;
        }

        private void DesactivarControles()
        {
            CodigoTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            ClaveTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            RolComboBox.Enabled = false;
            ActivoCheckBox.Enabled = false;
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DesactivarControles();
            VaciarControles();
        }

        private void VaciarControles()
        {
            CodigoTextBox.Clear();
            NombreTextBox.Clear();
            ClaveTextBox.Clear();
            CorreoTextBox.Clear();
            RolComboBox.Text = String.Empty;
            ActivoCheckBox.Checked = false;
        }

        private void ModificarButton_Click(object sender, EventArgs e)
        {
            tipoOperacion = "Modificar";
            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                CodigoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString();
                NombreTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                ClaveTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Clave"].Value.ToString();
                CorreoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                RolComboBox.Text = UsuariosDataGridView.CurrentRow.Cells["Rol"].Value.ToString();
                ActivoCheckBox.Checked = Convert.ToBoolean(UsuariosDataGridView.CurrentRow.Cells["EstaActivo"].Value);
                ActivarControles();
                CodigoTextBox.ReadOnly = true;

            }
            else
            {
                MessageBox.Show("Debe Seleccionar un registro", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void GuardarButton_Click(object sender, EventArgs e)
        {
            user = new Usuario();

            if (tipoOperacion == "Nuevo")
            {
                if (CodigoTextBox.Text == "")
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un Codigo");
                    CodigoTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese un Nombre");
                    NombreTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ClaveTextBox.Text))
                {
                    errorProvider1.SetError(ClaveTextBox, "Ingrese una Clave");
                    ClaveTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(RolComboBox.Text))
                {
                    errorProvider1.SetError(RolComboBox, "Seleccione un Rol");
                    RolComboBox.Focus();
                    return;
                }

                user.Codigo = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Clave = ClaveTextBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.EstaActivo = ActivoCheckBox.Checked;

                bool inserto = await userDatos.InsertarAsync(user);

                if (inserto)
                {
                    CompletarDataGrid();
                    VaciarControles();
                    DesactivarControles();
                    MessageBox.Show("Usuario Guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo Guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (tipoOperacion == "Modificar")
            {
                if (CodigoTextBox.Text == "")
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un Codigo");
                    CodigoTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese un Nombre");
                    NombreTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ClaveTextBox.Text))
                {
                    errorProvider1.SetError(ClaveTextBox, "Ingrese una Clave");
                    ClaveTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(RolComboBox.Text))
                {
                    errorProvider1.SetError(RolComboBox, "Seleccione un Rol");
                    RolComboBox.Focus();
                    return;
                }

                user.Codigo = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Clave = ClaveTextBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.EstaActivo = ActivoCheckBox.Checked;

                bool modifico = await userDatos.ActualizarAsync(user);
                if (modifico)
                {
                    CompletarDataGrid();
                    VaciarControles();
                    DesactivarControles();
                    MessageBox.Show("Usuario Guardado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo Guardar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void EliminarButton_Click(object sender, EventArgs e)
        {
            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                bool elimino = await userDatos.EliminarAsync(UsuariosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                if (elimino)
                {
                    CompletarDataGrid();
                    MessageBox.Show("Usuario Eliminado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Usuario no se pudo Eliminar", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
