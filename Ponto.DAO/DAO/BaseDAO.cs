using Ponto.DAL.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ponto.DAL.DAO
{
    public class BaseDAO<T> where T : class
    {
        public PontoContext pontoContext { get; set; }

        public BaseDAO()
        {
            pontoContext = new PontoContext();
        }

        public IEnumerable<T> GetAll() 
        {
            return pontoContext.Set<T>();
        }

        public virtual T? Create(T obj) 
        {
            pontoContext.Add(obj);
            pontoContext.SaveChanges();
            return obj;
        }
    }
}
