using System;
using Npgsql; //Esta librería la he tenido que agregar como un complemento. La hizo alguien y sirve para acceder a base de datos PostgreSQL
using System.Data; //ESta librería es necesaria para los tipos y variables relacionados con las bases de datos.

namespace Satrapia
{

    public class SGBD
    {
        //EStos parámetros son los que necesitamos para conectar con una BBDD PostgreSQL
        private string _servidor; //Puede ser una IP
        private string _puerto; //Normalmente es el 5432 pero por seguridad podríamos cambiar el puerto.
        private string _basededatos; //Se llama makrutas
        private string _usuario; //postgres
        private string _password; //mak0k1

        private NpgsqlConnection conexion; //Esta variable es nuestro enlace con Postgres

        //La clase tiene un constructor, que es una función con el mismo nombre que la clase y que se ejecuta cada vez que creamosun objeto. Sirve, principalmente, para inicializar los objetos.
        public SGBD(string servidor, string puerto, string basededatos, string usuario, string password)
        {
            string cadenaConexion = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};CommandTimeout=60;", servidor, puerto, usuario, password, basededatos);

            this._servidor = servidor;
            this._puerto = puerto;
            this._basededatos = basededatos;
            this._usuario = usuario;
            this._password = password;

            this.conexion = new NpgsqlConnection(cadenaConexion);            
        }

        //Esta función es la que realmente nos conecta con la base de datos.
        public void conecta()
        {
            this.conexion.Open();
        }

        //Esta función es para los SELECT. En realidad es más interesante ver como se trabaja con ella. Pero aquí tienes un ejemplo de gestión de excepciones (try...catch). Ya hablaremos de ello.
        public DataTable consulta(string sql)
        {
            //Siempre entramos en el try. Las instrucciones que hay dentro se ejecutan a no ser que ocurra un error, porque entonces saltamos al catch
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //Usamos la conexión enviandole una orden SQL
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, this.conexion);
            ds.Reset();                
        INTENTALO:
            try
            {                
                da.Fill(ds);
                dt = ds.Tables[0];
                //Basicamente lo que hemos hecho aquí es crear una variable de tipo DataTable y rellenarla con los datos que hay en la base de datos.
                return dt;
            }
            //El catch solo se ejecuta si ha habido un error dentro del try (no fuera). Podemos saber que error ha ocurrido con funciones de C#. Incluso generar nuestras propias excepciones de usuario. 
            catch
            {
                Console.WriteLine("Error en consulta de base de datos " + sql);
                goto INTENTALO;
                throw;
            }
        }

        //Esta función es para las instrucciones de edicion (INSERT, UPDATE y DELETE) y para otras instrucciones que no creo que gastemos. (P.ej borrar toda la base de datos jjjjj)
        public int ejecuta(string sql, bool debug = false)
        {
            NpgsqlCommand ordenSQL = new NpgsqlCommand(sql, this.conexion);
            Int32 filas;

            try
            {
                filas = ordenSQL.ExecuteNonQuery();
                //Cada vez que se ejecuta esta función vuelco a la consola lo que ha hecho.
                if (debug == true) Console.WriteLine(DateTime.Now + " ::: " + sql, filas);
            }
            catch
            {
                Console.WriteLine("Error en consulta de base de datos " + sql);
                throw;
            }

            return filas;
        }

        // Nunca está de más cerrar una conexión cuando ya no la vamos a utilizar.    
        public void cierra()
        {
            conexion.Close();
        }
    }
}