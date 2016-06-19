using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LE2Sql
{
	/// <summary>
	/// 解決不直接 Delete/Insert 方式，改先 Delete 不存在的，在 Insert/Upate。 by Panther
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DIURepository<T>
		where T : class, new()
	{

		public int BulkInsert(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public int BulkUpdate(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public int DeleteNotExistsItems(IEnumerable<T> source, Expression<Func<T, bool>> predicate)
		{
			// predicate to fixedKeys

			var parms = new Dictionary<string, object>();
			var whereSql = "NOT (" + GetTrueWhereKey(source, parms) + ")"; // + fixedKeys
			whereSql = parms.Any() ? " WHERE " + whereSql : string.Empty;

			//return Execute("delete from TableName" + whereSql, parms);

			return 0;
		}

		public IEnumerable<T> GetInsertWithUpdateItems(IEnumerable<T> source, Expression<Func<T, bool>> predicate, out IList<T> insertItems, out IList<T> updateItems)
		{
			var ps = typeof(T).GetProperties();
			// 改取 [Key]
			var keyPs = ps.Take(2).ToList();

			var parms = new Dictionary<string, object>();
			var whereSql = GetTrueWhereKey(source, parms);
			whereSql = parms.Any() ? " WHERE " + whereSql : string.Empty;

			var existsItems = (!parms.Any()) ? Enumerable.Empty<T>() : null;    // null : Query<T>(sql, whereSql )

			updateItems = source.Where(item => existsItems.Any(x => keyPs.All(p => p.GetValue(x).Equals(p.GetValue(item))))).ToList();
			insertItems = source.Except(updateItems).ToList();
			return existsItems;
		}


		private static string GetTrueWhereKey(IEnumerable<T> source, Dictionary<string, object> parms)
		{
			var ps = typeof(T).GetProperties();
			// 改取 [Key]
			var keyPs = ps.Take(2).ToList();

			var pkSqls = new List<string>();
			int paramIndex = 0;

			foreach (var item in source)
			{
				var pairs = keyPs.Select(p => new { p.Name, Value = p.GetValue(item) }).ToList();
				if (pairs.Any(pair => pair.Value == null))
					continue;

				var pkSql = string.Join(" AND ", pairs.Select((pair, index) => $"{pair.Name} = :p{paramIndex + index}"));
				pkSqls.Add(pkSql);

				foreach (var pair in pairs)
				{
					parms.Add(":p" + paramIndex, pair.Value);
					paramIndex++;
				}
			}

			var whereSql = "(" + string.Join(") OR (", pkSqls) + ")";
			return whereSql;
		}
	}
}
