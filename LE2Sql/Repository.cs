using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LE2Sql
{
	public class Repository<T>
		where T : class
	{

		public int Update(
			Expression<Func<T, T>> SET,
			Expression<Func<T, bool>> WHERE = null)
		{
			throw new NotImplementedException();
		}

		public int Update<TParam>(
			Expression<Func<T, TParam, T>> SET,
			Expression<Func<T, TParam, bool>> WHERE,
			IEnumerable<TParam> parms)
		{
			throw new NotImplementedException();
		}

		public int Update<TParam1, TParam2>(
			Expression<Func<T, TParam1, TParam2, T>> SET,
			Expression<Func<T, TParam1, TParam2, bool>> WHERE,
			IEnumerable<TParam1> parms, TParam2 fixedParam)
		{
			throw new NotImplementedException();
		}

	}
}
