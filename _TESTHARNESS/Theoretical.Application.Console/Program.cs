using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theoretical.Business;
using DataMapper;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;
using Moq;
using DataMapper.Building;
using Theoretical.Data;

namespace Theoretical.Application
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                //this uses straight up mapping
                //DataMapBlock();

                //the thingie
                SimpleThingy();

                //this shows how you can rename things
                //DataMapBlockOdd();

                //TypeMapper();

                //TestTwo();
            }
            catch (Exception ex)
            {
                //expect this...
                Console.WriteLine(ex.ToString());
            }
        }

        static void DataMapBlock()
        {
            try
            {
                OrderDataMapBlock dataMapBlock = new OrderDataMapBlock();

                //set the juicy context...
                var efContext = new TheoreticalEntities();
                efContext.Configuration.LazyLoadingEnabled = false;
                efContext.Configuration.ProxyCreationEnabled = false;
                //efContext.Configuration.ValidateOnSaveEnabled = false;
                var context = new DataMapper.Repositories.EntityFrameworkRepositoryContext<TheoreticalEntities>(efContext);
                dataMapBlock.Context = context;

                //just to simplify stuff, you can ignore these two lines
                var sl = new LastRevisionServiceLayer();
                sl.DeleteAll();

                var findResult = dataMapBlock.TryFind(134);

                var addOrder = CreateNewOrder();
                //sl.Add(addOrder);
                dataMapBlock.Add(addOrder);
                context.SaveChanges();

                var addFindResult = dataMapBlock.TryFind(addOrder.OrderId);

                //remove last and add a new one. 
                addFindResult.OrderItem.Remove(addFindResult.OrderItem.Last());
                addFindResult.OrderItem.Add(new OrderItem()
                {
                    HasSerialNumber = false,
                    SalePrice = 1000M,
                    SerialNumber = "ADDEDITEM",
                    Upc = "ADDUPC",
                    ConcurrencyId = 0
                });

                dataMapBlock.Update(addFindResult);
                context.SaveChanges();

                addFindResult.OrderInformation.Last().TrackingNumber = "Giggidy";

                dataMapBlock.Update(addFindResult);
                context.SaveChanges();

                dataMapBlock.Delete(addFindResult);
                context.SaveChanges();

                Order whatever;
                Order whatever2;
                using (var tranny = new System.Transactions.TransactionScope())
                {
                    whatever = CreateNewOrder();
                    whatever.Number = "FUck";
                    dataMapBlock.Add(whatever);
                    context.SaveChanges();

                    whatever2 = CreateNewOrder();
                    whatever2.Number = "YeahMan";
                    dataMapBlock.Add(whatever2);
                    context.SaveChanges();

                    tranny.Complete();
                }

                whatever = whatever;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        static Order CreateNewOrder()
        {
            Order newOrder = new Order()
            {
                Number = "4000" + Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                AccountId = 1,
                Status = 34,
                TaxRate = 34.23m,
                ConcurrencyId = 0,
                OptionalNote = null,
                OptionalPrice = 123M,
                OrderItem = new List<OrderItem>()
                        {
                            new OrderItem() {
                                ConcurrencyId = 0,
                                HasSerialNumber = false,
                                SalePrice = 34.12M,
                                SerialNumber = "",
                                Upc = "MYUPC"
                            },
                            new OrderItem() {
                                ConcurrencyId = 0,
                                HasSerialNumber = true,
                                SalePrice = 134.12M,
                                SerialNumber = "ASerialnumber",
                                Upc = "OHTERUPC"
                            }
                        },
                OrderInformation = new List<OrderInformation>()
                {
                    new OrderInformation()
                    {
                        TrackAmount = 100,
                        TrackDate = DateTime.Now,
                        TrackingNumber = "TRK100500"
                    },
                    new OrderInformation()
                    {
                        TrackAmount = null,
                        TrackDate = null,
                        TrackingNumber = null
                    }
                }
            };

            return newOrder;
        }

        static void DataMapBlockOdd()
        {
            try
            {
                OrderPocoDataMapBlock dataMapBlock = new OrderPocoDataMapBlock();

                //just to simplify stuff, you can ignore these two lines
                var sl = new LastRevisionServiceLayer();
                sl.DeleteAll();

                var findResult = dataMapBlock.TryFind(134);

                var addOrder = CreateNewOrderPoco();

                addOrder.RenamedStatus = StatusEnum.Giggidy;

                //sl.Add(addOrder);
                dataMapBlock.Add(addOrder);

                var addFindResult = dataMapBlock.TryFind(addOrder.MyId);

                //remove last and add a new one. 
                addFindResult.MyOrderPocoItems.Remove(addFindResult.MyOrderPocoItems.Last());
                addFindResult.MyOrderPocoItems.Add(new OrderItemPoco()
                {
                    HasSerialNumber = false,
                    SalePrice = 1000M,
                    SerialNumber = "ADDEDITEM",
                    RenamedUpc = "ADDUPC",
                    ConcurrencyId = 0
                });

                dataMapBlock.Update(addFindResult);

                addFindResult.OrderInformation.Last().TrackingNumber = "Giggidy";

                dataMapBlock.Update(addFindResult);

                //dataMapBlock.Delete(addFindResult);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void SimpleThingy()
        {
            try
            {
                SimpleRepoThingy repo = new SimpleRepoThingy();

                //set the juicy context...
                var efContext = new TheoreticalEntities();
                efContext.Configuration.LazyLoadingEnabled = false;
                efContext.Configuration.ProxyCreationEnabled = false;
                //efContext.Configuration.ValidateOnSaveEnabled = false;
                var context = new DataMapper.Repositories.EntityFrameworkRepositoryContext<TheoreticalEntities>(efContext);
                repo.Context = context;

                //just to simplify stuff, you can ignore these two lines
                var sl = new LastRevisionServiceLayer();
                sl.DeleteAll();

                var findResult = repo.TryFind(134);

                var addOrder = CreateNewOrderEntity();

                //addOrder.RenamedStatus = StatusEnum.Giggidy;

                //sl.Add(addOrder);
                repo.Add(addOrder);
                context.SaveChanges();

                var addFindResult = repo.TryFind(addOrder.OrderId);

                //remove last and add a new one. 
                addFindResult.OrderItem.Remove(addFindResult.OrderItem.Last());
                addFindResult.OrderItem.Add(new OrderItemEntity()
                {
                    HasSerialNumber = false,
                    SalePrice = 1000M,
                    SerialNumber = "ADDEDITEM",
                    Upc = "ADDUPC",
                    ConcurrencyId = 0
                });

                repo.Update(addFindResult);
                context.SaveChanges();

                addFindResult.OrderInformation.Last().TrackingNumber = "Giggidy";

                repo.Update(addFindResult);
                context.SaveChanges();

                repo.Delete(addFindResult);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static OrderPoco CreateNewOrderPoco()
        {
            OrderPoco newOrder = new OrderPoco()
            {
                Number = "4000" + Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                AccountId = 1,
                RenamedStatus =  StatusEnum.Giggidy,
                TaxRate = (34.23m),
                ConcurrencyId = 0,
                OptionalNote = null,
                OptionalPrice = 123M,
                MyOrderPocoItems = new List<OrderItemPoco>()
                        {
                            new OrderItemPoco() {
                                ConcurrencyId = 0,
                                HasSerialNumber = false,
                                SalePrice = 34.12M,
                                SerialNumber = "",
                                RenamedUpc = "MYUPC"
                            },
                            new OrderItemPoco() {
                                ConcurrencyId = 0,
                                HasSerialNumber = true,
                                SalePrice = 134.12M,
                                SerialNumber = "ASerialnumber",
                                RenamedUpc = "OHTERUPC"
                            }
                        },
                OrderInformation = new List<OrderInformationPoco>()
                {
                    new OrderInformationPoco()
                    {
                        TrackAmount = 100,
                        TrackDate = DateTime.Now,
                        TrackingNumber = "TRK100500"
                    },
                    new OrderInformationPoco()
                    {
                        TrackAmount = null,
                        TrackDate = null,
                        TrackingNumber = null
                    }
                }
            };

            return newOrder;
        }
        static OrderEntity CreateNewOrderEntity()
        {
            OrderEntity newOrder = new OrderEntity()
            {
                Number = "4000" + Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                AccountId = 1,
                Status = (Int32)StatusEnum.Giggidy,
                TaxRate = (34.23m),
                ConcurrencyId = 0,
                OptionalNote = null,
                OptionalPrice = 123M,
                OrderItem = new List<OrderItemEntity>()
                        {
                            new OrderItemEntity() {
                                ConcurrencyId = 0,
                                HasSerialNumber = false,
                                SalePrice = 34.12M,
                                SerialNumber = "",
                                Upc = "MYUPC"
                            },
                            new OrderItemEntity() {
                                ConcurrencyId = 0,
                                HasSerialNumber = true,
                                SalePrice = 134.12M,
                                SerialNumber = "ASerialnumber",
                                Upc = "OHTERUPC"
                            }
                        },
                OrderInformation = new List<OrderInformationEntity>()
                {
                    new OrderInformationEntity()
                    {
                        TrackAmount = 100,
                        TrackDate = DateTime.Now,
                        TrackingNumber = "TRK100500"
                    },
                    new OrderInformationEntity()
                    {
                        TrackAmount = null,
                        TrackDate = null,
                        TrackingNumber = null
                    }
                }
            };

            return newOrder;
        }

        static void TypeMapper()
        {
            try
            {
                var orderEntity = new OrderEntity();
                var addOrder = CreateNewOrderPoco();
                TypeMapStore tms = new TypeMapStore();

                tms.Define<OrderEntity, OrderPoco>()
                    .MapProperty(a=>a.OrderId,b=>b.MyId)
                    .MapProperty(a=>a.Status,b=>b.RenamedStatus);
                    //.MapRemainingByConvention(PropertyMapUnresolvedBehavior.None);

                tms.Define<LocalDataStoreSlot, Object>();

                tms.Define<TimeZone, Object>();

                tms.Finish();

                tms.Map<OrderPoco, OrderEntity>(addOrder, orderEntity);

                var yo = 34;
                //dataMapBlock.Delete(addFindResult);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        #region Ignore
        static void LastRevision()
        {
            try
            {
                LastRevisionServiceLayer sl = new LastRevisionServiceLayer();

                sl.DeleteAll();

                var findResult = sl.TryFind(134);

                var addOrder = CreateNewOrder();
                sl.Add(addOrder);
                var addFindResult = sl.TryFind(addOrder.OrderId);

                //remove last and add a new one. 
                addFindResult.OrderItem.Remove(addFindResult.OrderItem.Last());
                addFindResult.OrderItem.Add(new OrderItem()
                {
                    HasSerialNumber = false,
                    SalePrice = 1000M,
                    SerialNumber = "ADDEDITEM",
                    Upc = "ADDUPC",
                    ConcurrencyId = 0
                });

                sl.Update(addFindResult);

                sl.Delete(addFindResult);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void LessNaive()
        {
            try
            {
                LessNaiveServiceLayer sl = new LessNaiveServiceLayer();

                sl.DeleteAll();

                var findResult = sl.TryFind(134);

                var addOrder = CreateNewOrder();
                sl.Add(addOrder);
                var addFindResult = sl.TryFind(addOrder.OrderId);

                //remove last and add a new one. 
                //addFindResult.OrderItem.Remove(addFindResult.OrderItem.Last());
                addFindResult.OrderItem.Add(new OrderItem()
                {
                    HasSerialNumber = false,
                    SalePrice = 1000M,
                    SerialNumber = "ADDEDITEM",
                    Upc = "ADDUPC",
                    ConcurrencyId = 0
                });

                sl.Update(addFindResult);

                sl.Delete(addFindResult);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void Naive()
        {
            try
            {
                NaiveServiceLayer sl = new NaiveServiceLayer();

                //simple add test
                var newOrder = CreateNewOrder();
                sl.Add(newOrder);
                Console.WriteLine(
                    String.Format("Saved new order to database {0}",
                    newOrder.OrderId));

                //do a simple read test
                var newOrderReadFromDatabase = sl.TryFind(newOrder.OrderId);

                //test the delete case with the dumb delete method.
                var orderForDelete = CreateNewOrder();
                orderForDelete.Number = "orderForDelete" + Guid.NewGuid().ToString();
                sl.Add(orderForDelete);
                orderForDelete = sl.TryFind(orderForDelete.OrderId);
                sl.Delete(orderForDelete);
                Console.WriteLine(
                    String.Format("Deleted order from database {0}",
                    orderForDelete.OrderId));
                var orderForDeleteReadFromDatabase = sl.TryFind(orderForDelete.OrderId);

                var orderForDeleteBreak = CreateNewOrder();
                orderForDeleteBreak.Number = "orderForDeleteBreak" + Guid.NewGuid().ToString();
                sl.Add(orderForDeleteBreak);

                var bytestream = DataMapper.EntityFrameworkExtensions.SerializeBinaryToByteStream(orderForDeleteBreak);
                orderForDeleteBreak = DataMapper.EntityFrameworkExtensions.DeserializeBinaryByteStream<Order>(bytestream);
                //orderForDeleteBreak.OrderItem.Remove(orderForDeleteBreak.OrderItem.First());
                orderForDeleteBreak.OrderItem.Clear();
                try
                {
                    sl.Delete(orderForDeleteBreak);
                }
                catch (Exception ex)
                {
                    //expect this...
                    Console.WriteLine("Ran into error?");
                }



                //var loadResult = f.Ordering.TryFind(newOrder.OrderId);

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void NonNaive()
        {
            try
            {
                Console.WriteLine("Press any key to begin...");
                //Console.ReadKey();

                var newOrder = CreateNewOrder();
                //var newOrderCopy = newOrder.PrimitiveCopy();

                //var schemaBuilder = new DataMapSchemaBuilder();
                //var schemaMap = schemaBuilder.Build(typeof(Order), typeof(OrderEntity), "Entity");

                //var entityCopy = schemaMap.MapToNewCopy(schemaMap, newOrder, DataMapSchema.MapDirection.FirstToSecond);

                //var myCopy3 = (Order)schemaMap.DeepCopyFirst(schemaMap, newOrder);

                //myCopy3.OrderItem.First().SalePrice = 1222M;
                //myCopy3.Status = 4343;

                //var myCopy = schemaMap.DeepCopyFirst(schemaMap, newOrder);


                //Facade f = new Facade();

                //f.Ordering.AddNew(newOrder);

                //var loadResult = f.Ordering.TryFind(newOrder.OrderId);

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void FluentBuilder()
        {
            var builder = new DataMapBuilder<OrderEntity, Order>();


            //builder.MapProperty(a => a.OrderDate, b => b.OrderDate);
            //builder.MapSourcePropertiesByConvention();
            //builder.GetChildMapper<OrderItemEntity, OrderItem>()
            //    .MapSourcePropertiesByConvention();
            //builder.GetChildMapper<OrderInformationEntity, OrderInformation>()
            //    .MapSourcePropertiesByConvention();

            var yo = 45;
        }
        #endregion


        static void TestTwo()
        {
            try
            {
                using (var trans = new System.Transactions.TransactionScope())
{
                using (var context = new TheoreticalEntities())
                {

                    
                    var prod = new Product();
                    prod.Category = "Me";
                    prod.ConcurrencyId = 0;
                    prod.Description = "Me";
                    prod.IsDiscontinued = false;
                    prod.ProductId = 5;
                    prod.IsSerialNumberRequired = false;
                    prod.SalePrice = 5M;
                    prod.VendorName = "vendor";
                    context.Products.Add(prod);
                    context.SaveChanges();
                    
                  
                   
                }

                using (var context2 = new TheoreticalEntities1())
                {
                    var account = new Account();
                    account.AccountId = 5;
                    account.ConcurrencyId = 0;
                    account.Name = "bob";
                    account.Number = "1234";
                    context2.Accounts.Add(account);
                    context2.SaveChanges();
                }


                trans.Complete();
}
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }


    

    



}

