using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;


using System.Data;
using System.Data.SqlClient;



namespace Persistencia
{
   internal class PersitenciaCliente:IPersistenciaCliente
    {
        //singleton
        private static PersitenciaCliente _instancia;
        private PersitenciaCliente() { }
        public static PersitenciaCliente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaCliente();

            return _instancia;
        }

       public void Alta(EntidadesCompartidas.Cliente unCliente)
       {
            SqlConnection _cnn = new SqlConnection(Conexion.MiConexion);

            SqlCommand _comando = new SqlCommand("AltaCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCli", unCliente.CodCLi);
            _comando.Parameters.AddWithValue("@NomCli", unCliente.NomCli);
            SqlParameter _ParmRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _ParmRetorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_ParmRetorno);

            SqlTransaction _miTransaccion = null;

            try
            {
                //conecto a la bd
                _cnn.Open();

                //determino que voy a trabar con esa unica transaccion
                _miTransaccion = _cnn.BeginTransaction();

                //ejecuto comando de alta del Cliente en la transaccion
                _comando.Transaction = _miTransaccion;
                _comando.ExecuteNonQuery(); //se ejecuta dentro de la TRN Logica


                //verifico si hay errores
                int _CodCli = Convert.ToInt32(_ParmRetorno.Value);
                if (_CodCli == -1)
                    throw new Exception("Cliente ya existente");
                else if (_CodCli == 0)
                    throw new Exception("Error no especificado");

                //si llego aca es porque pude dar de alta el cliente
                foreach(EntidadesCompartidas.Telefono unTel in unCliente.LosTelefonos)
                {
                    PersitenciaTelefono.GetInstancia().Alta(unTel, _CodCli, _miTransaccion);

                }

                //si llegue aca es porque no hubo problemas con los telefonos
                _miTransaccion.Commit();

            }
            catch (Exception ex)
            {
                _miTransaccion.Rollback();
                throw ex;

            }
            finally
            {
                _cnn.Close();

            }


        }
       public List<EntidadesCompartidas.Cliente> Listo()
       {
           SqlConnection _cnn = new SqlConnection(Conexion.MiConexion);

           SqlCommand _comando = new SqlCommand("ListoClientes", _cnn);
           _comando.CommandType = CommandType.StoredProcedure;

           List<EntidadesCompartidas.Cliente> _Lista = new List<EntidadesCompartidas.Cliente>();
           EntidadesCompartidas.Cliente _unCliente = null;

           try
           {
               //me conecto
               _cnn.Open();

               //ejecuto consulta
               SqlDataReader _lector = _comando.ExecuteReader();

               //verifico si hay telefonos
               if (_lector.HasRows)
               {
                   while (_lector.Read())
                   {
                       int _codigo = (int)_lector["CodCli"];
                       string _nombre = (string)_lector["NomCli"];
                       string _direccion = (string)_lector["DirCli"];
                       _unCliente = new EntidadesCompartidas.Cliente(_codigo, _nombre, _direccion, PersitenciaTelefono.GetInstancia().CargoTelCliente(_codigo));
                       _Lista.Add(_unCliente);
                   }
               }

               _lector.Close();
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               _cnn.Close();
           }

           //retorno la lista de clientes
           return _Lista;
       }      
       public EntidadesCompartidas.Cliente Busco(int CodCli)
       {
           SqlConnection _cnn = new SqlConnection(Conexion.MiConexion);

           SqlCommand _comando = new SqlCommand("BuscoCliente", _cnn);
           _comando.CommandType = CommandType.StoredProcedure;
           _comando.Parameters.AddWithValue("@CodCli", CodCli);

           EntidadesCompartidas.Cliente _unCliente = null;

           try
           {
               //me conecto
               _cnn.Open();

               //ejecuto consulta
               SqlDataReader _lector = _comando.ExecuteReader();

               //verifico si hay telefonos
               if (_lector.HasRows)
               {
                   _lector.Read();
                    int _codigo = (int)_lector["CodCli"];
                    string _nombre = (string)_lector["NomCli"];
                    string _direccion = (string)_lector["DirCli"];
                    _unCliente = new EntidadesCompartidas.Cliente(_codigo, _nombre, _direccion, PersitenciaTelefono.GetInstancia().CargoTelCliente(_codigo));
                }

                _lector.Close();
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               _cnn.Close();
           }

           //retorno el cliente
           return _unCliente;
       }
       public void Modificar(EntidadesCompartidas.Cliente unCliente)
       {
            SqlConnection _cnn = new SqlConnection(Conexion.MiConexion);
            SqlTransaction _miTransaccion = null;

            SqlCommand _comando = new SqlCommand("ModificarCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@cod", unCliente.CodCLi);
            _comando.Parameters.AddWithValue("@Nom", unCliente.NomCli);
            _comando.Parameters.AddWithValue("@Dir", unCliente.DirCli);
            SqlParameter _ParmRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _ParmRetorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_ParmRetorno);

            try
            {
                //conecto a la bd
                _cnn.Open();

                //determino que voy a trabajar en una unica transaccion
                _miTransaccion = _cnn.BeginTransaction();

                _comando.Transaction = _miTransaccion;
                _comando.ExecuteNonQuery();

                //verifico si hay error en modificar el registro del cliente
                int _error = (int)_ParmRetorno.Value;
                if (_error == -1)
                    throw new Exception("El Cliente a Modificar no existe");

                PersitenciaTelefono.GetInstancia().EliminarTelsCliente(unCliente, _miTransaccion);

            }

            catch(Exception ex)
            {
                _miTransaccion.Rollback();
                throw ex;


            }
            finally
            {
                _cnn.Close();
            }
        }

    }
}
