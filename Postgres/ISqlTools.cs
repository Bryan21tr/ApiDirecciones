using APIDirecciones.Postgres;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using APIDirecciones.Postgres.Utils;
using System;
using System.Threading.Tasks; 

namespace APIDirecciones.Postgres
{
    public interface ISqlTools
    {
        Task<RespuestaBD> ExecuteFunctionAsync(string storedProcedure, ParameterPGsql[] parametros = null!, ParameterPGsql[] outParametros = null!);
        
        //Task<RespuestaBD> ExecuteFunctionFileAsync(string storedProcedure, DataFile dataFile, ParameterPGsql[] parametros = null!, ParameterPGsql[] outParametros = null!);
        //Task<RespuestaBD> ExecuteFunctionFileAsync(string storedProcedure, List<DataFile> dataFile, ParameterPGsql[] parametros = null!, ParameterPGsql[] outParametros = null!);
    }

    public class SqlTools : ISqlTools
    {
        //private readonly IFileSystemService _fileSystemService;
        private int _commandTimeout = 600;

        private string _connectionString;


        public SqlTools(string connectionString
        //,IFileSystemService fileSystemService
        )
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));            
            //_fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        }

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        public async Task<RespuestaBD> ExecuteFunctionAsync(
    string functionName,
    ParameterPGsql[]? parametros = null,
    ParameterPGsql[]? outParametros = null)
{
    var respuestaBD = new RespuestaBD();

    
    
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // Armar llamada SQL tipo: SELECT * FROM function_name(@param1, @param2, ...)
        string sql = $"SELECT * FROM {functionName}";

        if (parametros is not null && parametros.Any())
        {
            string paramPlaceholders = string.Join(", ", parametros.Select(p => $"@{p.ParameterName}"));
            sql += $"({paramPlaceholders})";
        }
        else
        {
            sql += "()";
        }

        using var command = new NpgsqlCommand(sql, connection)
        {
            CommandType = CommandType.Text,
            CommandTimeout = _commandTimeout
        };

        // Agregar parámetros de entrada
        if (parametros is not null)
        {
            foreach (var param in parametros)
            {
                command.Parameters.Add(new NpgsqlParameter(param.ParameterName, param.ParameterType)
                {
                    IsNullable = true,
                    Value = param.Value ?? DBNull.Value
                });
            }
        }

        // Agregar parámetros de salida (si aplica)
        if (outParametros is not null)
        {
            foreach (var param in outParametros)
            {
                command.Parameters.Add(new NpgsqlParameter(param.ParameterName, param.ParameterType)
                {
                    Direction = ParameterDirection.Output
                });
            }
        }

        // Ejecutar comando
        using var reader = await command.ExecuteReaderAsync();

        // Llenar DataSet con el resultado
        var dataTable = new DataTable();
        dataTable.Load(reader);
        respuestaBD.Data.Tables.Add(dataTable);

        // Obtener valores de salida si los hay
       
   
    return respuestaBD;
}


        public async Task<RespuestaBD> ExecuteFunctionFileAsync(string storedProcedure
        //, DataFile dataFile
        , ParameterPGsql[] parametros = null!, ParameterPGsql[] outParametros = null!)
        {
            RespuestaBD respuestaBD = new();
            
                using (NpgsqlConnection npgsqlConnection = new(_connectionString))
                {
                    await npgsqlConnection.OpenAsync();
                    using (NpgsqlTransaction transaction = npgsqlConnection.BeginTransaction())
                    {
                        using (NpgsqlCommand selectCommand = new(storedProcedure, npgsqlConnection))
                        {
                            selectCommand.CommandType = CommandType.StoredProcedure;
                            if (parametros is not null && parametros.Any())
                            {
                                foreach (ParameterPGsql parameterPGsql in parametros)
                                {
                                    selectCommand.Parameters.Add(new(parameterPGsql.ParameterName, parameterPGsql.ParameterType)
                                    {
                                        IsNullable = true,
                                        Value = parameterPGsql.Value ?? DBNull.Value,
                                    });
                                }
                            }

                            if (outParametros is not null && outParametros.Any())
                            {
                                foreach (ParameterPGsql parameterPGsql2 in outParametros)
                                {
                                    selectCommand.Parameters.Add(new(parameterPGsql2.ParameterName, parameterPGsql2.ParameterType)
                                    {
                                        Direction = ParameterDirection.Output,
                                    });
                                }
                            }

                            selectCommand.CommandTimeout = _commandTimeout;
                            await selectCommand.PrepareAsync();
                            using (NpgsqlDataAdapter npgsqlDataAdapter = new(selectCommand))
                            {
                                npgsqlDataAdapter.Fill(respuestaBD.Data);
                                if (outParametros is not null && outParametros.Any())
                                {
                                    foreach (ParameterPGsql parameterPGsql3 in outParametros)
                                    {
                                        respuestaBD.AgregarParametro(parameterPGsql3.ParameterName, parameterPGsql3.Value!);
                                    }
                                }
                            }
                            
                            if (respuestaBD.ExisteError || respuestaBD.Data.Tables.Count <= 0 || respuestaBD.Data.Tables[0].Rows.Count <= 0)
                            {
                                return respuestaBD;
                            }

                            /*if (dataFile is not null)
                            {
                                var success = respuestaBD.Data.Tables[0].Rows[0].Field<bool>(1);
                                if (!success)
                                {
                                    await transaction.RollbackAsync();
                                    return respuestaBD;
                                }

                                if (!await _fileSystemService.CreateAsync(dataFile))
                                {
                                    await transaction.RollbackAsync();
                                    respuestaBD.SetError("Ah ocurrido un error al almacenar el documento.");
                                    return respuestaBD;
                                }
                            }*/

                            
                            await transaction.CommitAsync();
                        }
                    }
                }
            
            return respuestaBD;
        }

        /*public async Task<RespuestaBD> ExecuteFunctionFileAsync(string storedProcedure, List<DataFile> dataFile, ParameterPGsql[] parametros = null!, ParameterPGsql[] outParametros = null!)
        {
            RespuestaBD respuestaBD = new();
            try
            {
                using (NpgsqlConnection npgsqlConnection = new(_connectionString))
                {
                    await npgsqlConnection.OpenAsync();
                    using (NpgsqlTransaction transaction = npgsqlConnection.BeginTransaction())
                    {
                        using (NpgsqlCommand selectCommand = new(storedProcedure, npgsqlConnection))
                        {
                            selectCommand.CommandType = CommandType.StoredProcedure;
                            if (parametros is not null && parametros.Any())
                            {
                                foreach (ParameterPGsql parameterPGsql in parametros)
                                {
                                    selectCommand.Parameters.Add(new(parameterPGsql.ParameterName, parameterPGsql.ParameterType)
                                    {
                                        IsNullable = true,
                                        Value = parameterPGsql.Value ?? DBNull.Value,
                                    });
                                }
                            }

                            if (outParametros is not null && outParametros.Any())
                            {
                                foreach (ParameterPGsql parameterPGsql2 in outParametros)
                                {
                                    selectCommand.Parameters.Add(new(parameterPGsql2.ParameterName, parameterPGsql2.ParameterType)
                                    {
                                        Direction = ParameterDirection.Output,
                                    });
                                }
                            }

                            selectCommand.CommandTimeout = _commandTimeout;
                            await selectCommand.PrepareAsync();
                            using (NpgsqlDataAdapter npgsqlDataAdapter = new(selectCommand))
                            {
                                npgsqlDataAdapter.Fill(respuestaBD.Data);
                                if (outParametros is not null && outParametros.Any())
                                {
                                    foreach (ParameterPGsql parameterPGsql3 in outParametros)
                                    {
                                        respuestaBD.AgregarParametro(parameterPGsql3.ParameterName, parameterPGsql3.Value!);
                                    }
                                }
                            }

                            if (respuestaBD.ExisteError || respuestaBD.Data.Tables.Count <= 0 || respuestaBD.Data.Tables[0].Rows.Count <= 0)
                            {
                                return respuestaBD;
                            }

                            if (dataFile is not null && dataFile.Any())
                            {
                                foreach (var item in dataFile)
                                {
                                    var success = respuestaBD.Data.Tables[0].Rows[0].Field<bool>(1);
                                    if (!success)
                                    {
                                        await transaction.RollbackAsync();
                                        return respuestaBD;
                                    }

                                    if (!await _fileSystemService.CreateAsync(item))
                                    {
                                        await transaction.RollbackAsync();
                                        respuestaBD.SetError("Ocurrió un error al almacenar el documento.");
                                        return respuestaBD;
                                    }
                                }
                            }


                            await transaction.CommitAsync();
                        }
                    }
                }
            }
            catch (PostgresException _ex)
            {
                _logger.LogError(_ex, "Error en DB");
                respuestaBD.SetError(_ex.Message, _ex.ErrorCode.ToString(), _ex.Detail!);
            }
            catch (NpgsqlOperationInProgressException _ex)
            {
                _logger.LogError(_ex, "Error en tiempo de ejecución SQL");
                respuestaBD.SetError(_ex.Message, _ex.ErrorCode.ToString());
            }
            catch (NpgsqlException _ex)
            {
                _logger.LogError(_ex, "Error en DB");
                respuestaBD.SetError(_ex.Message, _ex.ErrorCode.ToString());
            }
            catch (Exception _ex)
            {
                _logger.LogError(_ex, "Error operacion");
                throw;
            }

            return respuestaBD;
        }*/
    }
}