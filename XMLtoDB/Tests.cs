using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace XMLtoDB
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void CreateDbAndLoadXml()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MyDbContext>());
            MyDbContext context = new MyDbContext();
            context.Database.Initialize(true);

            XmlSerializer MERCHANT_DETAILS_xml = new XmlSerializer(typeof(MERCHANT_SO_ENTRY_DETAILS));
            MERCHANT_SO_ENTRY_DETAILS MERCHANT_DETAILS;
            using (FileStream fs = File.OpenRead("c:\\1\\PS_Batch_pmnt_stmt_NF_P_20170414_160014_1_2015-09-01.xml"))
            {
                MERCHANT_DETAILS = (MERCHANT_SO_ENTRY_DETAILS)MERCHANT_DETAILS_xml.Deserialize(fs);
            }
            using (context)
            {
                context.MERCHANT_DETAILS.Add(MERCHANT_DETAILS);
                context.SaveChanges();
                foreach (G_CONTRACTS gc in MERCHANT_DETAILS.LIST_G_CONTRACTS)
                {
                    context.G_CONTRACTS.Add(gc);
                    context.SaveChanges();
                    foreach (G_ORDER_TYPE got in gc.LIST_G_ORDER_TYPE)
                    {
                        context.G_ORDER_TYPES.Add(got);
                        context.SaveChanges();
                        foreach (G_ORDER go in got.LIST_G_ORDER)
                        {
                            go.G_ORDER_TYPE_ID = got.ID;
                            context.G_ORDERS.Add(go);
                            context.SaveChanges();
                            foreach (G_DEVICE gd in go.LIST_G_DEVICE)
                            {
                                gd.ORDER_ID = go.ID;
                                context.G_DEVICE.Add(gd);
                                context.SaveChanges();
                                foreach (G_TRANS_GROUP tg in gd.LIST_G_TRANS_GROUP)
                                {
                                    tg.DEVICE_ID = gd.ID;
                                    context.G_TRANS_GROUPS.Add(tg);
                                    context.SaveChanges();
                                    foreach (G_POSTING_DATE pd in tg.LIST_G_POSTING_DATE)
                                    {
                                        pd.G_TRANS_GROUP_ID = tg.ID;
                                        context.G_POSTING_DATES.Add(pd);
                                        context.SaveChanges();
                                        foreach (G_TRANS_DETAILS td in pd.LIST_G_TRANS_DETAILS)
                                        {
                                            td.G_POSTING_DATE_ID = pd.ID;
                                            context.G_TRANS_DETAILS.Add(td);
                                            context.SaveChanges();
                                            foreach (G_ENTRY ge in td.LIST_G_ENTRY)
                                            {
                                                ge.G_TRANS_DETAILS_ID = td.ID;
                                                context.G_ENTRYS.Add(ge);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        [Test]
        public void XmlToDbTest()
        {
            using (MyDbContext context = new MyDbContext())
            {
                foreach (G_ORDER go in context.G_ORDERS)
                {
                    PrintProperties(go, "G_ORDER");
                    context.G_DEVICE.Where(dev => dev.ORDER_ID.Equals(go.ID)).ToList().ForEach(dev =>
                    {
                        PrintProperties(dev, "G_DEVICE");
                        context.G_TRANS_GROUPS.Where(trg => trg.DEVICE_ID.Equals(go.ID)).ToList().ForEach(trg =>
                        {
                            PrintProperties(dev, "G_TRANS_GROUP");
                        });
                    });
                }

            }

        }

        private void PrintProperties(object myObject, string message)
        {
            Console.WriteLine(message);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myObject))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(myObject);
                Console.WriteLine("{0}={1}", name, value);
            }
        }
    }
}
