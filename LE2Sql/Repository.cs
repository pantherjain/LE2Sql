using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LE2Sql
{
	/// <summary>
	/// Lambda Expressions to Sql Repository prototype by Panther
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Repository<T>
		where T : class
	{
		public T Get(T entity)
		{
			throw new NotImplementedException();
		}

		public T Get(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> GetAll()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> SearchFor(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public bool Insert(T entity)
		{
			throw new NotImplementedException();
		}

		public int BulkInsert(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public bool Update(T entity)
		{
			throw new NotImplementedException();
		}

		public int BulkUpdate(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public int Update(
			Expression<Func<T, T>> SET,
			Expression<Func<T, bool>> WHERE = null)
		{
			throw new NotImplementedException();
		}

		public int Update<TEntity>(
			Expression<Func<T, TEntity, T>> SET,
			Expression<Func<T, TEntity, bool>> WHERE,
			IEnumerable<TEntity> parms)
		{
			throw new NotImplementedException();
		}

		public int Update<TEntity, TFixedParam>(
			Expression<Func<T, TEntity, TFixedParam, T>> SET,
			Expression<Func<T, TEntity, TFixedParam, bool>> WHERE,
			IEnumerable<TEntity> parms, TFixedParam fixedParam)
		{
			throw new NotImplementedException();
		}

		public int Delete(T entity)
		{
			throw new NotImplementedException();
		}

		public int Delete(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public int BulkDelete(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public bool Exists(T entity)
		{
			throw new NotImplementedException();
		}

		public bool Exists(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public bool NotExists(Expression<Func<T, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> Query(string sql, object parm = null)
		{
			throw new NotImplementedException();
		}

		public int Execute(string sql, object parm = null)
		{
			throw new NotImplementedException();
		}
	}
}
