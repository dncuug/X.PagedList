using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;

namespace X.PagedList.Mvc.Example.Controllers
{
    public class AutomapperController : Controller
    {
        public class OrderOrderViewModelProfile : Profile
        {
            public override string ProfileName => "Mapping between Order and OrderViewModel";

            public OrderOrderViewModelProfile()
            {
                CreateMap<Order, OrderViewModel>();
            }
        }


        public ActionResult Index(int? page)
        {

            Mapper.Initialize(x => x.AddProfile<OrderOrderViewModelProfile>()); // create mapping between Order and OrderViewModel
            const int pageSize = 2;

            var orders = GetAllOrdersFromDatabase();
            var pagedOrders = orders.ToPagedList(page ?? 1, pageSize);

            var viewmodel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(pagedOrders.ToArray()); // convert all the Orders in the paged list into viewmodels
            var pagedViewmodel = new StaticPagedList<OrderViewModel>(viewmodel, pagedOrders.GetMetaData()); // reconsitute a new IPagedList containing the viewmodels

            return View(pagedViewmodel);
        }

        public IEnumerable<Order> GetAllOrdersFromDatabase()
        {
            var jerrysOrder = new Order(
                new Customer { Name = "Jerry Seinfeld" },
                new[] { new OrderLineItem(new Product("Superman Underoos", 17.99m), 7) }
            );
            var georgesOrder = new Order(
                new Customer { Name = "George Costanza" },
                new[] { new OrderLineItem(new Product("Bosco", 4.99m), 15) }
            );
            var kramersOrder = new Order(
                new Customer { Name = "Cosmo Kramer" },
                new[] { new OrderLineItem(new Product("Door Knobs", 7.95m), 30) }
            );
            var elainesOrder = new Order(
                new Customer { Name = "Elaine Benes" },
                new[] { new OrderLineItem(new Product("Sponge", 6.95m), 500) }
            );
            var newmansOrder = new Order(
                new Customer { Name = "Newman" },
                new[] { new OrderLineItem(new Product("Dinosaur DNA", 1500000m), 1) }
            );

            var orders = new List<Order>();
            orders.Add(jerrysOrder);
            orders.Add(georgesOrder);
            orders.Add(kramersOrder);
            orders.Add(elainesOrder);
            orders.Add(newmansOrder);
            return orders;
        }

        public class OrderViewModel
        {
            public string CustomerName { get; set; }
            public decimal Total { get; set; }
        }

        // Example classes from: http://automapper.codeplex.com/wikipage?title=Flattening&referringTitle=Home

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

            public string Name { get; private set; }
            public decimal Price { get; private set; }
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
                return OrderedProduct.Price * Quantity;
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
    }
}
