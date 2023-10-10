using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logica
{
   internal class LogicaCliente: ILogicaCliente
    {
        private static LogicaCliente _instancia = null;
        private LogicaCliente() { }
        public static LogicaCliente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaCliente();

            return _instancia;
        }
        public void Alta(EntidadesCompartidas.Cliente unCliente)
        {
            Persistencia.FabricaPersistencia.GetPersistenciaCliente().Alta(unCliente);
        }


        public List<EntidadesCompartidas.Cliente> Listo()
        {
            return (Persistencia.FabricaPersistencia.GetPersistenciaCliente().Listo());
        }


        public EntidadesCompartidas.Cliente Busco(int pCodCli)
        {
            return (Persistencia.FabricaPersistencia.GetPersistenciaCliente().Busco(pCodCli));        
        }


        public void Modificar(EntidadesCompartidas.Cliente unCliente)
        {
            Persistencia.FabricaPersistencia.GetPersistenciaCliente().Modificar(unCliente);               
        }

    }
}
