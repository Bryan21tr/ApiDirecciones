using Microsoft.Extensions.Logging;
using Npgsql;

namespace APIDirecciones.Postgres.Utils
{
    public class LoadDatabaseCommand
    {
        private readonly ILogger<LoadDatabaseCommand> _logger;
        private readonly string extensionFile = "*.sql";
        private readonly string _connectionString;
        private readonly string _initialScriptsPath;
        private readonly string _functionsScriptsPath;

        public LoadDatabaseCommand(ILogger<LoadDatabaseCommand> logger, string connectionString, string initialScriptsPath, string functionsScriptsPath)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _initialScriptsPath = initialScriptsPath ?? throw new ArgumentNullException(nameof(initialScriptsPath));
            _functionsScriptsPath = functionsScriptsPath ?? throw new ArgumentNullException(nameof(functionsScriptsPath));
        }

        public async Task ExecuteCreateAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await ExecuteScriptsAsync(connection, _initialScriptsPath, "** creación de esquema **");
            await ExecuteScriptsAsync(connection, _functionsScriptsPath, "** crear / actualizar funciones **");
        }

        public async Task ExecuteFunctionsAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await ExecuteScriptsAsync(connection, _functionsScriptsPath, "creación de funciones");
        }

        private async Task ExecuteScriptsAsync(NpgsqlConnection connection, string scriptsPath, string operationDescription)
        {
            try
            {
                if (!Directory.Exists(scriptsPath))
                {
                    _logger.LogWarning($"APP: La carpeta de scripts para {operationDescription} no existe en la ruta {scriptsPath}.");
                    return;
                }

                var sqlFiles = Directory.GetFiles(scriptsPath, extensionFile, SearchOption.AllDirectories)
                                        .OrderBy(c => c)
                                        .ToList();

                if (!sqlFiles.Any())
                {
                    _logger.LogWarning($"APP: No se encontraron archivos SQL para {operationDescription} en la ruta {scriptsPath}.");
                    return;
                }

                _logger.LogInformation($"APP: Iniciando {operationDescription}...");
                await connection.OpenAsync();

                foreach (var scriptFile in sqlFiles)
                {
                    try
                    {
                        _logger.LogInformation($"APP: Ejecutando script: {scriptFile}");
                        var script = File.ReadAllText(scriptFile);
                        using var command = new NpgsqlCommand(script, connection);
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        _logger.LogError($"APP: Error al ejecutar el script {scriptFile}.");
                        throw;
                    }
                }

                await connection.CloseAsync();
                _logger.LogInformation($"APP: {operationDescription} completada con éxito.");
            }
            catch (Exception)
            {
                _logger.LogError($"APP: Ocurrió un error al realizar la operación {operationDescription}.");
                throw;
            }
        }
    }
}