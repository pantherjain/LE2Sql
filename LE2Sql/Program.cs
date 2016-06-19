using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LE2Sql
{
	class Program
	{
		static void Main(string[] args)
		{

			var repo = new Repository<Product>();

			// 單筆或多筆固定更新
			var customer = new Customer { AddQty = 5 };
			repo.Update(
			SET: p => new Product
			{
				Qty = p.Qty + customer.AddQty
			},
			WHERE: p => p.ProductName == "123" && p.Qty > customer.AddQty);


			// 多筆更新
			var customers = new List<Customer>();
			repo.Update(
			SET: (p, c) => new Product
			{
				Qty = p.Qty + c.AddQty
			},
			WHERE: (p, c) => p.ProductName == "123" && p.Qty > c.AddQty,
			parms: customers);

			//-----------------------------------------------------------
			var sourceItems = Enumerable.Range(1, 10).Select(i => new Product {  ProductID = i,  ProductName = "qq" + i }).ToList();

			var diuRepo = new DIURepository<Product>();

			diuRepo.DeleteNotExistsItems(sourceItems, x => x.ProductName == "02");

			IList<Product> updateItems, insertItems;
			diuRepo.GetInsertWithUpdateItems(sourceItems, x => x.ProductName == "02", out insertItems, out updateItems);
			diuRepo.BulkInsert(insertItems);
			diuRepo.BulkUpdate(updateItems);

		}

	
		public class Product
		{
			public decimal ProductID { get; set; }
			public string ProductName { get; set; }
			public string AccountOpened { get; set; }
			public Customer Customer { get; set; }
			public int Qty { get; set; }
		}

		public class Customer
		{
			public decimal CustomerId { get; set; }
			public string CustomerName { get; set; }
			public int AddQty { get; set; }
		}


	}


}
