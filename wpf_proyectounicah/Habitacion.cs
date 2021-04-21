﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Agregar los namespaces requeridos
using System.Data.SqlClient;
using System.Configuration;

namespace wpf_proyectounicah
{
    //Crear una variable que mantenga los valores  para los estados de la habitacion

    public enum EstadosHabitacion 
    {
        Ocupada = 'O',
        Disponible = 'D',
        Mantenimiento  = 'M', 
        FueraDeServicio= 'F'
    }
    class Habitacion
    {
        //Variables miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["wpf_proyectounicah.Properties.Settings.ReservacionesConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);
        
        //Propiedades
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int Numero { get; set; }

        public EstadosHabitacion Estado  { get; set;  }

        //Constructores
        public Habitacion() { }

        public Habitacion(string descripcion, int numero, EstadosHabitacion estado)
        {
            Descripcion = descripcion;
            Numero = numero;
            Estado = estado;
        }
        //Metodos
        /// <summary>
        /// Retorna el estado de la habitacion desde el enum de estados
        /// </summary>
        /// <param name="estado">El valor dentro del enum </param>
        /// <returns>Estado valido dentro de la base de datos</returns>
        private string ObtenerEstados(EstadosHabitacion estado) 
        {
            switch (estado) 
            {
                case EstadosHabitacion.Ocupada: 
                return "OCUPADA";

                case EstadosHabitacion.Disponible:
                return "DISPONIBLE";

                case EstadosHabitacion.Mantenimiento:
                    return "MANTENIMIENTO";

                case EstadosHabitacion.FueraDeServicio: 
                    return "FUERA DE SERVICIO";
                default:
                    return "DISPONIBLE";
            }
            }
        /// <summary>
        /// Inserta una habitacion.
        /// </summary>
        /// <param name="habitacion">La informacion de la habitacion</param>
        public void CrearHabitacion(Habitacion habitacion ) 
        {
            try 
            {
                //Query de inserción
                string query = @"INSERT INTO Habitaciones.Habitacion(descripcion, numero, estado)
                                VALUES(@descripcion,@numero, @estado)";

                //Establecer conexion
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer los valores de los parametros
                sqlCommand.Parameters.AddWithValue(@"descripcion", habitacion.Descripcion);
                sqlCommand.Parameters.AddWithValue(@"numero", habitacion.Numero);
                sqlCommand.Parameters.AddWithValue(@"estado", ObtenerEstados(habitacion.Estado));

                //Ejecutar el comando de inserción
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }
            
        }
        /// <summary>
        /// Muestra todas las habitaciones 
        /// </summary>
        /// <returns>Un listado de habitaciones </returns>
        public List<Habitacion> MostrarHabitaciones() 
        {
            //Inicializa una lista vacia de habitaciones
            List<Habitacion> habitaciones = new List<Habitacion>();
            try
            {
                //Query de seleccion
                string query = @"SELECT id, descripcion 
                                FROM  habitaciones.habitacion";
                //Establecer la conexión
                sqlConnection.Open();

                //Crear el comando SQL 
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Obtener los datos de las habitaciones
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        habitaciones.Add(new Habitacion { Id = Convert.ToInt32(rdr["id"]), Descripcion = rdr["descripcion"].ToString() });
                    }
                }
                return habitaciones;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }

        }

        /// <summary>
        /// Obtiene una habitacion por su id
        /// </summary>
        /// <param name="id">El id de la habitacion </param>
        /// <returns>Los datos de la habitacion </returns>
        public Habitacion BuscarHabitacion(int id)
        {
            Habitacion lahabitacion = new Habitacion();

            try
            {
                //Query de busqueda 
                string query = @"Select * FROM Habitaciones.Habitacion 
                                WHERE  id = @id";

                //Establecer la conexion
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer el valor del parametro 
                sqlCommand.Parameters.AddWithValue("@id", id);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                       

                        lahabitacion.Id = Convert.ToInt32(rdr["id"]);
                        lahabitacion.Descripcion = rdr["descripcion"].ToString();
                        lahabitacion.Numero = Convert.ToInt32(rdr["numero"]);
                        lahabitacion.Estado = (EstadosHabitacion)Convert.ToChar(rdr["estado"].ToString().Substring(0,1));
                    }

                }

                return lahabitacion;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// Modifica los datos de una habitacion 
        /// </summary>
        /// <param name="habitacion">El id de la habitacion</param>
        public void ModificarHabitacion(Habitacion habitacion) 
        {
            try
            {
                //Query de actualizacion 
                string query = @"UPDATE Habitaciones.Habitacion
                                SET descripcion = @descripcion, numero = @numero, estado = @estado
                                WHERE id = @id";
                //Establecer la conexion
                sqlConnection.Open();

                //Crear el comando sql
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer los valores de los parametros
                sqlCommand.Parameters.AddWithValue("@id", habitacion.Id);
                sqlCommand.Parameters.AddWithValue("@descripcion", habitacion.Descripcion);
                sqlCommand.Parameters.AddWithValue("@numero", habitacion.Numero);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstados(habitacion.Estado));


                //Ejecutar el comando de actualizacion
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw e;
            }
            finally 
            {
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// Elimina una habitacion 
        /// </summary>
        /// <param name="id">El id de la habitacion </param>
        public void EliminarHabitacion( int id) 
        {
            try
            {
                //Query de eliminacion
                string query = @"DELETE FROM Habitaciones.Habitacion
                                Where id = @id ";

                //Establecer  la conexion
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer el valor del parametro 
                sqlCommand.Parameters.AddWithValue("@id", id);

                //Ejecutar el comando de eliminacion
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;

            }
            finally 
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }
        }


    }   
}
