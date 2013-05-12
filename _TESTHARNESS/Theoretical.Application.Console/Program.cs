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
using DataMapper.Repositories;

namespace Theoretical.Application
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                TheoreticalEntities context = new TheoreticalEntities();
                context.OrderEntity.RemoveAll(context.OrderEntity.ToArray());
                context.SaveChanges();

                CachePerformanceTest(false);
                CachePerformanceTest(true);

                //this uses straight up mapping
                //DataMapBlock();

                //the thingie
                //SimpleThingy();

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

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
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

        static void CachePerformanceTest(Boolean cachingEnabled)
        {
            DateTime started = DateTime.Now;
            Int32 loopymax = 100000;
            EntityFrameworkRepositoryContext<TheoreticalEntities> context 
                = new EntityFrameworkRepositoryContext<TheoreticalEntities>();
            OrderDataMapBlock blocky;

            for (Int32 i = 0; i < loopymax; i++)
            {
                blocky = new OrderDataMapBlock();
                //blocky.Context = context;
                blocky.CachingEnabled = cachingEnabled;

                blocky.TouchDataMap();
            }
            var stringy = cachingEnabled ? "(Caching)" : "(No caching)";
            Console.WriteLine( "Time to complete" + stringy + ": " + DateTime.Now.Subtract(started).TotalSeconds.ToString());
        }
        static void DataMapBlockOdd()
        {
            try
            {
                OrderPocoDataMapBlock dataMapBlock = new OrderPocoDataMapBlock();
                EntityFrameworkRepositoryContext<TheoreticalEntities> context = new EntityFrameworkRepositoryContext<TheoreticalEntities>();
                dataMapBlock.Context = context;


                var findResult = dataMapBlock.TryFind(134);

                var addOrder = CreateNewOrderPoco();

                addOrder.Status = StatusEnum.Giggidy;

                //sl.Add(addOrder);
                dataMapBlock.Add(addOrder);
                context.SaveChanges();

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
                context.SaveChanges();

                addFindResult.OrderInformation.Last().TrackingNumber = "Giggidy";

                dataMapBlock.Update(addFindResult);
                context.SaveChanges();

                dataMapBlock.Delete(addFindResult);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }


        }

        static void SimpleThingy()
        {

            SimpleRepoThingy repo = new SimpleRepoThingy();

            //set the juicy context...
            var efContext = new TheoreticalEntities();
            efContext.Configuration.LazyLoadingEnabled = false;
            efContext.Configuration.ProxyCreationEnabled = false;
            //efContext.Configuration.ValidateOnSaveEnabled = false;
            var context = new DataMapper.Repositories.EntityFrameworkRepositoryContext<TheoreticalEntities>(efContext);
            repo.Context = context;

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

        static OrderPoco CreateNewOrderPoco()
        {
            OrderPoco newOrder = new OrderPoco()
            {
                Number = "4000" + Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                AccountId = 1,
                Status = StatusEnum.Giggidy,
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
            var orderEntity = new OrderEntity();
            var addOrder = CreateNewOrderPoco();
            TypeMapStore tms = new TypeMapStore();

            tms.Define<OrderEntity, OrderPoco>()
                .MapProperty(a => a.OrderId, b => b.MyId)
                .MapProperty(a => a.Status, b => b.Status);
            //.MapRemainingByConvention(PropertyMapUnresolvedBehavior.None);

            tms.Define<LocalDataStoreSlot, Object>();

            tms.Define<TimeZone, Object>();

            tms.Finish();

            tms.Map<OrderPoco, OrderEntity>(addOrder, orderEntity);

            var yo = 34;
            //dataMapBlock.Delete(addFindResult);
        }
    }



}

