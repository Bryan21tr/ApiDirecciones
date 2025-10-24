using APIDirecciones.Model.IDAO.IRepository;
using APIDirecciones.Model.IDAO.IServiceDAO;
using APIDirecciones.Model.ViewModels.Enums;
using APIDirecciones.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using APIDirecciones.Util;
using APIDirecciones.Postgres;
using APIDirecciones.Postgres.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Text;



namespace APIDirecciones.Model.DAO.ServicesDAO
{
    public class ServiceDirecciones : IServiceDirecciones
    {
        #region variables
        private readonly IRepositoryDirecciones _repositoryDirecciones;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;


        public ServiceDirecciones(
            IConfiguration configuration,
            IRepositoryDirecciones repositoryDirecciones
            )
        {
            this.configuration = configuration;
            _repositoryDirecciones = repositoryDirecciones ?? throw new ArgumentNullException(nameof(repositoryDirecciones));

        }

        #endregion

        #region Direcciones


        public async Task<ResultOperation<List<DireccionEntidad>>> ObtenerAll()
        {
            var resultOperation = new ResultOperation<List<DireccionEntidad>>();

            try
            {
                var result = await _repositoryDirecciones.GetAllAsync();

                if (result == null)
                {
                    resultOperation.Result = new List<DireccionEntidad>();
                    resultOperation.Success = false;
                    resultOperation.AddWarningMessage("No se encontraron direcciones.");
                    return resultOperation;
                }
                if (result == null)
                {
                    resultOperation.Result = new List<DireccionEntidad>();
                    resultOperation.Success = false;
                    resultOperation.AddWarningMessage("No se encontraron direcciones.");
                    return resultOperation;
                }

                resultOperation.Result = result;
                resultOperation.Success = true;
                resultOperation.AddSuccessMessage("Direcciones obtenidas exitosamente.");
                resultOperation.Result = result;
                resultOperation.Success = true;
                resultOperation.AddSuccessMessage("Direcciones obtenidas exitosamente.");

                return resultOperation;
            }
            catch (Exception ex)
            {
                resultOperation.Result = null;
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Ocurrió un error al obtener las direcciones: {ex.Message}");
                return resultOperation;
            }
        }

        public async Task<ResultOperation<DireccionEntidad>> ObtenerporID(int id)
        {
            var resultOperation = new ResultOperation<DireccionEntidad>();

            try
            {
                var result = await _repositoryDirecciones.GetByIdAsync(id);



                if (result == null)
                {
                    resultOperation.Result = new DireccionEntidad();
                    resultOperation.Success = false;
                    resultOperation.AddWarningMessage("No se encontraron direcciones.");
                    return resultOperation;
                }

                resultOperation.Result = result;
                resultOperation.Success = true;
                resultOperation.AddSuccessMessage("Direcciones obtenidas exitosamente.");

                return resultOperation;

            }
            catch (Exception ex)
            {
                resultOperation.Result = null;
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Ocurrió un error al obtener las direcciones: {ex.Message}");
                return resultOperation;
            }


        }
        public async Task<ResultOperation<DireccionEntidad>> Add(DireccionEntidad direccion)
        {
            var resultOperation = new ResultOperation<DireccionEntidad>();

            try
            {
                var result = await _repositoryDirecciones.CreateAsync(direccion);

                if (result == null)
                {
                    resultOperation.Result = new DireccionEntidad();
                    resultOperation.Success = false;
                    resultOperation.AddWarningMessage("No se encontraron direcciones.");
                    return resultOperation;
                }

                resultOperation.Result = result;
                resultOperation.Success = true;
                resultOperation.AddSuccessMessage("Direcciones obtenidas exitosamente.");

                return resultOperation;
            }
            catch (Exception ex)
            {
                resultOperation.Result = null;
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Ocurrió un error al obtener las direcciones: {ex.Message}");
                return resultOperation;
            }
        }


        public async Task<ResultOperation<DireccionEntidad>> DeleteAsync(int id)
        {
            var resultOperation = new ResultOperation<DireccionEntidad>();

            try
            {
                var result = await _repositoryDirecciones.DeleteAsync(id);



                if (result == null)
                {
                    resultOperation.Result = new DireccionEntidad();
                    resultOperation.Success = false;
                    resultOperation.AddWarningMessage("No se encontraron direcciones.");
                    return resultOperation;
                }

                resultOperation.Result = result;
                resultOperation.Success = true;
                resultOperation.AddSuccessMessage("Direcciones obtenidas exitosamente.");

                return resultOperation;

            }
            catch (Exception ex)
            {
                resultOperation.Result = null;
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Ocurrió un error al obtener la direccion: {ex.Message}");
                return resultOperation;
            }
        }

        public async Task<ResultOperation<DireccionEntidad>> UpdateAsync(DireccionEntidad direccion, int id)
        {
            var resultOperation = new ResultOperation<DireccionEntidad>();

            try
            {
                var result = await _repositoryDirecciones.UpdateAsync(direccion, id);


                if (result == null)
                {
                    resultOperation.Result = new DireccionEntidad();
                    resultOperation.Success = false;
                    resultOperation.AddWarningMessage("No se encontraron direcciones.");
                    return resultOperation;
                }

                resultOperation.Result = result;
                resultOperation.Success = true;
                resultOperation.AddSuccessMessage("Direcciones obtenidas exitosamente.");

                return resultOperation;

            }
            catch (Exception ex)
            {
                resultOperation.Result = null;
                resultOperation.Success = false;
                resultOperation.AddErrorMessage($"Ocurrió un error al obtener las direcciones: {ex.Message}");
                return resultOperation;
            }
        }




        #endregion

    }
}