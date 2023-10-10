using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntidadesCompartidas
{
    public class Cliente
    {
        //atributos
        private int _CodCli;
        private string _NomCli;
        private string _DirCli;
        List<Telefono> _LosTelefonos; //atributo de asoc. 



        //Properties
        public int CodCLi
        {
            get { return _CodCli; }
            set { _CodCli = value; }
        }

        public string NomCli
        {
            get { return _NomCli; }
            set
            {
                if ((value.Trim().Length > 30) || (value.Trim().Length == 0))
                    throw new Exception("Error en largo del Nombre");
                else
                    _NomCli = value;
            }
        }

        public string DirCli
        {
            get { return _DirCli; }
            set
            {
                if (value.Trim().Length > 20) 
                    throw new Exception("Error en largo de Direccion");
                else
                    _DirCli = value;
            }
        }

        public List<Telefono> LosTelefonos
        {
            get { return _LosTelefonos; }

            set
            {
                if (value == null)
                    throw new Exception("Error - No existe Conjunto");
                if (value.Count == 0)
                    throw new Exception("Error - No hay telefonos");
                _LosTelefonos = value;
            }
        }

        //constructores
        public Cliente(int pCodCLi, string pNomCli, string pDirCli, List<Telefono> plista)
        {
            CodCLi = pCodCLi;
            NomCli = pNomCli;
            DirCli = pDirCli;
            LosTelefonos = plista;
        }
  
    
        //operaciones
 

    }
}
