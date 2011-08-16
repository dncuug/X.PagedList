using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;

namespace PagedList.Mvc.Example.Controllers
{
	public class AutomappedListController : Controller
	{
		public ActionResult Index(int? page)
		{
			const int pageSize = 2;

			var orders = new[]
			             	{
			             		new Order(new Customer {Name = "Jerry Seinfeld"}, new[]
			             		                                                  	{
			             		                                                  		new OrderLineItem(new Product("Superman Underoos", 17.99m), 7)
			             		                                                  	}),
			             		new Order(new Customer {Name = "George Costanza"}, new[]
			             		                                                   	{
			             		                                                   		new OrderLineItem(new Product("Bosco", 4.99m), 15)
			             		                                                   	}),
			             		new Order(new Customer {Name = "Cosmo Kramer"}, new[]
			             		                                                	{
			             		                                                		new OrderLineItem(new Product("Door Knobs", 7.95m), 30)
			             		                                                	}),
			             		new Order(new Customer {Name = "Elaine Benes"}, new[]
			             		                                                	{
			             		                                                		new OrderLineItem(new Product("Sponge", 6.95m), 500)
			             		                                                	}),
			             		new Order(new Customer {Name = "Newman"}, new[]
			             		                                          	{
			             		                                          		new OrderLineItem(new Product("Dinosaur Samples", 1500000m), 1)
			             		                                          	})
			             	};

			var pagedOrders = orders.ToPagedList(page ?? 1, pageSize);

			Mapper.CreateMap<Order, OrderDto>(); // create mapping between Order and OrderDto
			var dtoOrders = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(pagedOrders); // convert all the Orders in the paged list into OrderDtos
			var dto = new StaticPagedList<OrderDto>(dtoOrders, pagedOrders); // reconsitute a new paged list containing the OrderDtos

			return View(dto);
		}

		#region Example classes from: http://automapper.codeplex.com/wikipage?title=Flattening&referringTitle=Home

		public class OrderDto
		{
			public string CustomerName { get; set; }
			public decimal Total { get; set; }
		}

		public class Customer
		{
			public string Name { get; set; }
		}

		public class Product
		{
			public Product(string name, decimal price)
			{
				Name = name;
				Price = price;
			}

			public string Name{ get; private set; }
			public decimal Price{ get; private set; }
		}

		public class OrderLineItem
		{
			public OrderLineItem(Product product, int quantity)
			{
				OrderedProduct = product;
				Quantity = quantity;
			}

			public Product OrderedProduct { get; private set; }
			public int Quantity { get; private set; }

			public decimal GetTotal()
			{
				return OrderedProduct.Price*Quantity;
			}
		}

		public class Order
		{
			public Order(Customer customer, IEnumerable<OrderLineItem> lineItems)
			{
				Customer = customer;
				LineItems = lineItems;
			}

			public Customer Customer { get; private set; }
			public IEnumerable<OrderLineItem> LineItems { get; private set; }

			public decimal GetTotal()
			{
				return LineItems.Sum(x => x.GetTotal());
			}
		}

		#endregion
	}
}