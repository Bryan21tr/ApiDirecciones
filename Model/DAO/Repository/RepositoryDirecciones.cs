using System.Data;
using APIDirecciones.Model.IDAO.IRepository;
using APIDirecciones.Model.ViewModels.Enums;
using APIDirecciones.Model.Entities;
using NpgsqlTypes;
using APIDirecciones.Model;
using APIDirecciones.Util;
using APIDirecciones.Postgres;
using APIDirecciones.Postgres.Utils;
using Microsoft.AspNetCore.Mvc;

namespace APIDirecciones.Model.DAO.Repository
{
    public class RepositoryDirecciones : IRepositoryDirecciones
    {
        #region Variables

        private readonly ISqlTools _sqlTools;



        public RepositoryDirecciones(ISqlTools sqlTools)
        {
            _sqlTools = sqlTools ?? throw new ArgumentNullException(nameof(sqlTools));
        }

        #endregion
        #region Direcciones

        public async Task<List<DireccionEntidad>> GetAllAsync()
        {

            List<DireccionEntidad> Lista = new List<DireccionEntidad>();


            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(EnumFunctions.DireccionesCrud, new ParameterPGsql[]
             { new ParameterPGsql("op_opciones", NpgsqlTypes.NpgsqlDbType.Integer, EnumOpciones.ReadAll),});
            RespuestaBD respuestaBD = await respuestaBDTask;
            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                 && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow fila in respuestaBD.Data.Tables[0].Rows)
                    {
                        DireccionEntidad aux = new DireccionEntidad
                        {

                            Id = (int)fila["id"],
                            Calle = fila["calle"].ToString(),
                            Colonia = fila["colonia"].ToString(),
                            Municipio = fila["municipio"].ToString(),
                            Numero = (int)fila["numero"],
                            CP = fila["cp"].ToString(),
                            Activo = fila["activo"] as bool?,
                            Fecha_modificacion = fila["fecha_modificacion"] as DateTime?,
                            Fecha_creacion = fila["fecha_creacion"] as DateTime?

                        };
                        Lista.Add(aux);
                    }
                }

            }
            else
            {
                //TODO Agregar error en el log             
                if (respuestaBD.ExisteError)
                    Console.WriteLine("Error {0} - {1} - {2} - {3}", respuestaBD.ExisteError, respuestaBD.Mensaje, respuestaBD.CodeSqlError, respuestaBD.Detail);
                throw new Exception(respuestaBD.Mensaje);
            }
            return Lista;

        }
        public async Task<DireccionEntidad> GetByIdAsync(int id)
        {
            DireccionEntidad resultOperation = new DireccionEntidad();

            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(EnumFunctions.DireccionesCrud, new ParameterPGsql[]{
                    new ParameterPGsql("op_opciones", NpgsqlTypes.NpgsqlDbType.Integer, EnumOpciones.Read),
                    new ParameterPGsql("id", NpgsqlTypes.NpgsqlDbType.Integer,id),
                });
            RespuestaBD respuestaBD = await respuestaBDTask;

            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {
                    DireccionEntidad aux = new DireccionEntidad
                    {
                        Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id"],
                        Calle = respuestaBD.Data.Tables[0].Rows[0]["calle"].ToString(),
                        Colonia = respuestaBD.Data.Tables[0].Rows[0]["colonia"].ToString(),
                        Municipio = respuestaBD.Data.Tables[0].Rows[0]["municipio"].ToString(),
                        Numero = (int)respuestaBD.Data.Tables[0].Rows[0]["numero"],
                        CP = respuestaBD.Data.Tables[0].Rows[0]["cp"].ToString(),
                        Activo = respuestaBD.Data.Tables[0].Rows[0]["activo"] as bool?,
                        Fecha_modificacion = respuestaBD.Data.Tables[0].Rows[0]["fecha_modificacion"] as DateTime?,
                        Fecha_creacion = respuestaBD.Data.Tables[0].Rows[0]["fecha_creacion"] as DateTime?
                    };
                    resultOperation = aux;

                }
                else

                {
                               
                    if (respuestaBD.ExisteError)
                        Console.WriteLine("Error {0} - {1} - {2} - {3}", respuestaBD.ExisteError, respuestaBD.Mensaje, respuestaBD.CodeSqlError, respuestaBD.Detail);
                    throw new Exception(respuestaBD.Mensaje);
                }

            }
            return resultOperation;
        }

        public async Task<DireccionEntidad> CreateAsync(DireccionEntidad direccion)
        {
            DireccionEntidad resultOperation = new DireccionEntidad();

            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(
                EnumFunctions.DireccionesCrud,
                new ParameterPGsql[]
                {
                new ParameterPGsql("op_opciones", NpgsqlTypes.NpgsqlDbType.Integer, EnumOpciones.Create),
                new ParameterPGsql("p_id_direccion", NpgsqlTypes.NpgsqlDbType.Integer, direccion.Id),
                new ParameterPGsql("p_calle", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.Calle),
                new ParameterPGsql("p_colonia", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.Colonia),
                new ParameterPGsql("p_municipio", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.Municipio),
                new ParameterPGsql("p_numero", NpgsqlTypes.NpgsqlDbType.Integer, direccion.Numero),
                new ParameterPGsql("p_cp", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.CP),
                new ParameterPGsql("p_activo", NpgsqlTypes.NpgsqlDbType.Boolean, direccion.Activo),
                new ParameterPGsql("p_fecha_modificacion", NpgsqlTypes.NpgsqlDbType.Date, direccion.Fecha_modificacion),
                new ParameterPGsql("p_fecha_creacion", NpgsqlTypes.NpgsqlDbType.Date, direccion.Fecha_creacion)
                }
            );

            RespuestaBD respuestaBD = await respuestaBDTask;


            if (!respuestaBD.ExisteError)
            {
                DireccionEntidad aux = new DireccionEntidad
                {
                    Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id"],
                    Calle = respuestaBD.Data.Tables[0].Rows[0]["calle"].ToString(),
                    Colonia = respuestaBD.Data.Tables[0].Rows[0]["colonia"].ToString(),
                    Municipio = respuestaBD.Data.Tables[0].Rows[0]["municipio"].ToString(),
                    Numero = (int)respuestaBD.Data.Tables[0].Rows[0]["numero"],
                    CP = respuestaBD.Data.Tables[0].Rows[0]["cp"].ToString(),
                    Activo = respuestaBD.Data.Tables[0].Rows[0]["activo"] as bool?,
                    Fecha_modificacion = respuestaBD.Data.Tables[0].Rows[0]["fecha_modificacion"] as DateTime?,
                    Fecha_creacion = respuestaBD.Data.Tables[0].Rows[0]["fecha_creacion"] as DateTime?

                };
                resultOperation = aux;

            }
            return resultOperation;
        }


        public async Task<DireccionEntidad> DeleteAsync(int id)
        {
            DireccionEntidad resultOperation = new DireccionEntidad();


            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(
                EnumFunctions.DireccionesCrud,
                new ParameterPGsql[]
                {
                new ParameterPGsql("op_opciones", NpgsqlTypes.NpgsqlDbType.Integer, EnumOpciones.Delete),
                new ParameterPGsql("p_id_direccion", NpgsqlTypes.NpgsqlDbType.Integer, id)

                }
            );

            RespuestaBD respuestaBD = await respuestaBDTask;

            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {
                    DireccionEntidad aux = new DireccionEntidad
                    {
                        Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id"],
                        Calle = respuestaBD.Data.Tables[0].Rows[0]["calle"].ToString(),
                    };
                    resultOperation = aux;
                }

            }

            return resultOperation;
        }

        public async Task<DireccionEntidad> UpdateAsync(DireccionEntidad direccion, int Id)
        {
            DireccionEntidad resultOperation = new DireccionEntidad();
            direccion.Id = Id;

            Task<RespuestaBD> respuestaBDTask = _sqlTools.ExecuteFunctionAsync(
                EnumFunctions.DireccionesCrud,
                new ParameterPGsql[]
                {
                new ParameterPGsql("op_opciones", NpgsqlTypes.NpgsqlDbType.Integer, EnumOpciones.Update),
                new ParameterPGsql("p_id_direccion", NpgsqlTypes.NpgsqlDbType.Integer, direccion.Id),
                new ParameterPGsql("p_calle", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.Calle),
                new ParameterPGsql("p_colonia", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.Colonia),
                new ParameterPGsql("p_municipio", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.Municipio),
                new ParameterPGsql("p_numero", NpgsqlTypes.NpgsqlDbType.Integer, direccion.Numero),
                new ParameterPGsql("p_cp", NpgsqlTypes.NpgsqlDbType.Varchar, direccion.CP),
                new ParameterPGsql("p_activo", NpgsqlTypes.NpgsqlDbType.Boolean, direccion.Activo),
                new ParameterPGsql("p_fecha_modificacion", NpgsqlTypes.NpgsqlDbType.Date, direccion.Fecha_modificacion),
                new ParameterPGsql("p_fecha_creacion", NpgsqlTypes.NpgsqlDbType.Date, direccion.Fecha_creacion)

                }
            );
            RespuestaBD respuestaBD = await respuestaBDTask;

            if (!respuestaBD.ExisteError)
            {
                if (respuestaBD.Data.Tables.Count > 0
                && respuestaBD.Data.Tables[0].Rows.Count > 0)
                {
                    DireccionEntidad aux = new DireccionEntidad
                    {
                        Id = (int)respuestaBD.Data.Tables[0].Rows[0]["id"],
                        Calle = respuestaBD.Data.Tables[0].Rows[0]["calle"].ToString(),
                        Colonia = respuestaBD.Data.Tables[0].Rows[0]["colonia"].ToString(),
                        Municipio = respuestaBD.Data.Tables[0].Rows[0]["municipio"].ToString(),
                        Numero = (int)respuestaBD.Data.Tables[0].Rows[0]["numero"],
                        CP = respuestaBD.Data.Tables[0].Rows[0]["cp"].ToString(),
                        Activo = respuestaBD.Data.Tables[0].Rows[0]["activo"] as bool?,
                        Fecha_modificacion = respuestaBD.Data.Tables[0].Rows[0]["fecha_modificacion"] as DateTime?,
                        Fecha_creacion = respuestaBD.Data.Tables[0].Rows[0]["fecha_creacion"] as DateTime?
                    };
                    resultOperation = aux;
                }
            }
            return resultOperation;
        }



        #endregion


    }
}