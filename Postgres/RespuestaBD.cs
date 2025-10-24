
using System;
using System.Data;
using System.Collections.Generic;


namespace APIDirecciones.Postgres.Utils
{

    public class RespuestaBD
    {
        private bool _Error;

        private string _Mensaje;

        private string _Detail;

        private string _CodeSqlError;

        public string _Respuesta;

        private Dictionary<string, object> _ParamSalida;

        private DataSet _DataSet;

        public bool ExisteError => _Error;

        public string CodeSqlError => _CodeSqlError;

        public string Mensaje => _Mensaje;

        public string Detail => _Detail;

        public DataSet Data
        {
            get
            {
                return _DataSet;
            }
            set
            {
                _DataSet = value;
            }
        }

        public object this[string nombre] => _ParamSalida[nombre];

        public RespuestaBD()
        {
            _Error = false;
            _Mensaje = "Ok";
            _Respuesta = "0";
            _CodeSqlError = string.Empty;
            _DataSet = new DataSet();
            _ParamSalida = new Dictionary<string, object>();
        }

        public void SetError(string mensajeError, string detail = "")
        {
            _Error = true;
            _Mensaje = mensajeError;
            _Detail = detail;
        }

        public void SetError(string mensajeError, string codeSqlError, string detail = "")
        {
            _Error = true;
            _Mensaje = mensajeError;
            _CodeSqlError = codeSqlError;
            _Detail = detail;
        }

        public void AgregarParametro(string nombre, object valor)
        {
            _ParamSalida.Add(nombre, valor);
        }
    }
}