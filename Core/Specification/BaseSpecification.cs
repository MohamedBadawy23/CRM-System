using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specification
{
    public class BaseSpecification<T> : Ispecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get     ; set ; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDesce { get ; set ; }

        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void OrderByAsc(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void OrderByDesc(Expression<Func<T, object>> orderBy)
        {
            OrderByDesce = orderBy;
        }

    }
}
