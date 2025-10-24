using System.Data;

namespace APIDirecciones.Model.ViewModels
{
    public class DataTableView<T>
    {
        public Pager Pager { get; protected set; }

        public List<T> Results { get; set; }

        public DataTableView(Pager pager, List<T> list)
        {
            this.Pager = pager;
            this.Results = list;
        }
    }

    public class Pager
    {
        public int Total { get; protected set; }
        public int Pages { get; protected set; }
        public int Page { get; protected set; }

        public Pager(int page, int pageSize, int total)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page), "La página no puede ser menor o igual 0.");

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "El tamaño de la página no puede ser menor o igual 0.");

            if (total < 0)
                throw new ArgumentOutOfRangeException(nameof(total), "El total no puede ser menor a 0.");

            if (total == 0)
            {
                this.Total = 0;
                this.Pages = 1;
                this.Page = 1;
                return;
            }

            this.Total = total;
            this.Page = page;

            decimal totalPages =
                    Convert.ToDecimal(total)
                    / Convert.ToDecimal(pageSize);
            this.Pages = (int)Math.Ceiling(totalPages);
        }
    }
}