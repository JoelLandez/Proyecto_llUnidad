using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ProductoDatos
    {
        public async Task<DataTable> DevolverListAsync()
        {
            DataTable ddt = new DataTable();
            try
            {
                string sql = "SELECT + FROM producto";

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
        public async Task<bool> InsertarAsync(Producto producto)
        {
            bool inserto = false;
            try
            {
                string sql = "INSERT INTO producto VALUES (@Codigo, @Descripcion, @Existencia, @Precio, @FechaCreacion, @Imagen);";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = producto.Codigo;
                        comand.Parameters.Add("@Descripcion", MySqlDbType.VarChar, 50).Value = producto.Descripcion;
                        comand.Parameters.Add("@Existencia", MySqlDbType.Int32).Value = producto.Existencia;
                        comand.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = producto.Precio;
                        comand.Parameters.Add("@FechaCreacion", MySqlDbType.DateTime).Value = producto.FechaCreacion;
                        comand.Parameters.Add("@Imagen", MySqlDbType.LongBlob).Value = producto.Imagen;

                        await comand.ExecuteNonQueryAsync();
                        inserto = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return inserto;
        }

        public async Task<bool> ActualizarAsync(Producto producto)
        {
            bool actualizo = false;
            try
            {
                string sql = "UPDATE producto SET Descripcion=@Descripcion, Existencia=@Existencia, Precio=@Precio, FechaCreacion=@FechaCreacion, Imagen=@Imagen WHERE Codigo=@Codigo;";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = producto.Codigo;
                        comand.Parameters.Add("@Descripcion", MySqlDbType.VarChar, 50).Value = producto.Descripcion;
                        comand.Parameters.Add("@Existencia", MySqlDbType.Int32).Value = producto.Existencia;
                        comand.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = producto.Precio;
                        comand.Parameters.Add("@FechaCreacion", MySqlDbType.DateTime).Value = producto.FechaCreacion;
                        comand.Parameters.Add("@Imagen", MySqlDbType.LongBlob).Value = producto.Imagen;

                        await comand.ExecuteNonQueryAsync();
                        actualizo = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return actualizo;
        }

        public async Task<bool> EliminarAsync(string codigo)
        {
            bool elimino = false;
            try
            {
                string sql = "DELETE FROM producto WHERE Codigo =@Codigo;";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = codigo;
                        await comand.ExecuteNonQueryAsync();
                        elimino = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return elimino;
        }

        public async Task<byte[]> SeleccionarImagen(string codigo)
        {
            byte[] imagen = new byte[0];

            try
            {
                string sql = "SELECT Imagen FROM producto WHERE Codigo =@Codigo;";

                using (MySqlConnection _connection = new MySqlConnection(CadenaConexion.Cadena))
                {
                    await _connection.OpenAsync();
                    using (MySqlCommand comand = new MySqlCommand(sql, _connection))
                    {
                        comand.CommandType = System.Data.CommandType.Text;
                        comand.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = codigo;

                        MySqlDataReader dr = (MySqlDataReader)await comand.ExecuteReaderAsync();
                        if (dr.Read())
                        {
                            imagen = (byte[])dr["Imagen"];
                        }
                       
                    }
                }
            }
            catch (Exception)
            {
            }
            return imagen;
        }
    }
}
