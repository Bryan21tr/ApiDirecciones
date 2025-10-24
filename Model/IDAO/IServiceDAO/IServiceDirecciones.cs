using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDirecciones.Model.Entities;
using System.Reflection.Metadata;
using APIDirecciones.Model.ViewModels.Enums;
using APIDirecciones.Util;
using APIDirecciones.Postgres;
using APIDirecciones.Postgres.Utils;
using Microsoft.AspNetCore.Mvc;



namespace APIDirecciones.Model.IDAO.IServiceDAO
{
  public interface IServiceDirecciones
  {
    #region Direcciones     
    Task<ResultOperation<List<DireccionEntidad>>> ObtenerAll();
    Task<ResultOperation<DireccionEntidad>> ObtenerporID(int id);
    Task<ResultOperation<DireccionEntidad>> Add(DireccionEntidad direccion);
    Task<ResultOperation<DireccionEntidad>> UpdateAsync(DireccionEntidad direccion, int id);
    Task<ResultOperation<DireccionEntidad>> DeleteAsync(int id);

    #endregion

  }
}
