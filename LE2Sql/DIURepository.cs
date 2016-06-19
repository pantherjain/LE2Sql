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
			if (!entities.Any())
				return 0;

			throw new NotImplementedException();
		}

		public int BulkUpdate(IEnumerable<T> entities)
		{
			if (!entities.Any())
				return 0;

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

			// 只取得在已存在的資料中，全部都沒有完全符合已存在資料裡面，表示有異動
			updateItems = updateItems.Where(item => existsItems.All(x => !ps.All(p => p.GetValue(x).Equals(p.GetValue(item))))).ToList();

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

		public UpdateResult DeleteThenInsertWithUpdate(IEnumerable<T> source, Expression<Func<T, bool>> predicate)
		{
			var deleteCount = DeleteNotExistsItems(source, predicate);

			IList<T> updateItems, insertItems;
			var existsItems = GetInsertWithUpdateItems(source, predicate, out insertItems, out updateItems);
			var insertCount = BulkInsert(insertItems);
			var updateCount = BulkUpdate(updateItems);

			return new UpdateResult
			{
				DeleteCount = deleteCount,
				UpdateCount = updateCount,
				InsertCount = insertCount,
				ExistsItems = existsItems,
				InsertItems = insertItems,
				UpdateItems = updateItems
			};
		}

		public class UpdateResult
		{
			public int DeleteCount { get; set; }
			public int InsertCount { get; set; }
			public int UpdateCount { get; set; }

			public IEnumerable<T> ExistsItems { get; set; }
			public IList<T> UpdateItems { get; set; }
			public IList<T> InsertItems { get; set; }
		}


	}
}
