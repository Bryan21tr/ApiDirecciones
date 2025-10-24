using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDirecciones.Model.Entities;
using APIDirecciones.Util;
using APIDirecciones.Postgres;
using APIDirecciones.Postgres.Utils;

namespace APIDirecciones.Model.IDAO.IRepository
{
    public interface IRepositoryDirecciones
    {

        #region Direcciones
        Task<List<DireccionEntidad>> GetAllAsync();
        Task<DireccionEntidad> CreateAsync(DireccionEntidad direccion);
        Task<DireccionEntidad> GetByIdAsync(int id);
        Task<DireccionEntidad> UpdateAsync(DireccionEntidad direccion, int id);
        Task<DireccionEntidad> DeleteAsync(int id);

        #endregion


    }
}
