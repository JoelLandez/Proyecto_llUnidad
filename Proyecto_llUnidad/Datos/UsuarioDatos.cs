using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos
{
    public class UsuarioDatos
    {
        public async Task<bool> LoginAysnc(string codigo, string clave)
        {
            bool valido = false;
            try
            {
                string sql = "SELECT 1 FROM usuario WHERE Codigo=@Codigo AND Clave=@Clave;";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = codigo;
                        comand.Parameters.Add("@Clave", MySqlDbType.VarChar, 120).Value = clave;

                        valido = Convert.ToBoolean(await comand.ExecuteScalarAsync());
                        

                        
                    }
                }
            }
            catch (Exception jl)
            {

            }
            return valido;
        }

        public async Task<DataTable> DevolverListAsync()
        {
            DataTable ddt = new DataTable();
            try
            {
                string sql = "SELECT + FROM usuario";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        MySqlDataReader ddr = (MySqlDataReader)await comand.ExecuteReaderAsync();
                        ddt.Load(ddr);


                    }
                }
            }
            catch (Exception jl)
            {

               
            }

            return ddt;
        } 

        public async Task<bool> InsertarAsync(Usuario usuario)
        {
            bool inserto = false;
            try
            {
                string sql = "INSERT INTO usuario VALUES (@Codigo, @Nombre, @Clave, @Correo, @Rol, @EstaActivo);";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = usuario.Codigo;
                        comand.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = usuario.Nombre;
                        comand.Parameters.Add("@Clave", MySqlDbType.VarChar, 120).Value = usuario.Clave;
                        comand.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = usuario.Correo;
                        comand.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = usuario.Rol;
                        comand.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = usuario.EstaActivo;

                        await comand.ExecuteNonQueryAsync();
                        inserto = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return inserto;
        }

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            bool actualizo = false;
            try
            {
                string sql = "UPDATE usuario SET Nombre=@Nombre, Clave=@Clave, Correo=@Correo, Rol=@Rol, EstaActivo=@EstaActivo;";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = usuario.Codigo;
                        comand.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = usuario.Nombre;
                        comand.Parameters.Add("@Clave", MySqlDbType.VarChar, 120).Value = usuario.Clave;
                        comand.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = usuario.Correo;
                        comand.Parameters.Add("@Rol", MySqlDbType.VarChar, 20).Value = usuario.Rol;
                        comand.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = usuario.EstaActivo;

                        await comand.ExecuteNonQueryAsync();
                        actualizo = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return actualizo;
        }

        public async Task<bool> EliminarAsync(string codigo)
        {
            bool elimino = false;
            try
            {
                string sql = "DELETE FROM usuario WHERE Codigo =@Codigo;";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.VarChar, 20).Value = codigo;
                        await comand.ExecuteNonQueryAsync();
                        elimino = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return elimino;
        }


    }
}
