using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Data;
using System.Data.SqlClient;



namespace Persistencia
{
   internal class PersitenciaTelefono
    {
        //singleton
        private static PersitenciaTelefono _instancia;
        private PersitenciaTelefono() { }
        public static PersitenciaTelefono GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersitenciaTelefono();

            return _instancia;
        }

       internal void Alta(EntidadesCompartidas.Telefono unTelefono, int pCodCli, SqlTransaction _pTransaccion)
        {
            SqlCommand _comando = new SqlCommand("AltaTelefono", _pTransaccion.Connection);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCli", pCodCli);
            _comando.Parameters.AddWithValue("@NumTel", unTelefono.UnTelefono);
            SqlParameter _ParmRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _ParmRetorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_ParmRetorno);

            try
            {
                //determino que debo trabajar con la misma transaccion
                _comando.Transaction = _pTransaccion;

                //ejecuto comando
                _comando.ExecuteNonQuery(); //se ejecuta dentro de la TRN Logica

                //verifico si hay errores

                int _CodCli = Convert.ToInt32(_ParmRetorno.Value);
                if (_CodCli == -1)
                    throw new Exception("Cliente Invalido");
                else if (_CodCli == -2)
                    throw new Exception("Error ese telefono ya existe para dicho cliente");



            }
            catch (Exception ex)
            {
                throw ex;

            }


        }//fin alta

        internal List<EntidadesCompartidas.Telefono> CargoTelCliente(int pCodCli)
       {
          List<EntidadesCompartidas.Telefono> _lista = new List<EntidadesCompartidas.Telefono>();

          SqlConnection _cnn = new SqlConnection(Conexion.MiConexion);

           SqlCommand _comando = new SqlCommand("TelefonoDeUnCliente", _cnn);
           _comando.CommandType = CommandType.StoredProcedure;
           _comando.Parameters.AddWithValue("@CodCLi", pCodCli);

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
                        _lista.Add(new EntidadesCompartidas.Telefono((string)_lector["NumTel"]));
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

            return _lista; 
       }//final op cargo


       internal void EliminarTelsCliente(EntidadesCompartidas.Cliente unCliente, SqlTransaction _pTransaccion)
       {
            SqlCommand _comando = new SqlCommand("EliminoTelsDeCliente", _pTransaccion.Connection);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCli", unCliente.CodCLi);
            SqlParameter _ParmRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _ParmRetorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_ParmRetorno);

            try
            {
                //determino que debo trabajar con la misma transaccion
                _comando.Transaction = _pTransaccion;

                //ejecuto comando
                _comando.ExecuteNonQuery();


            }
            catch(Exception ex)
            {
                throw ex;

            }


       }//fin op eliminar tels

    }
}
