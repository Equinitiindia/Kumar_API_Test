using RestSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestSample.Services
{
    public class sampleRepository
    {
        private const string CacheKey = "PaymentStore";
        private List<Payment> payment = new List<Payment>();
        private int _nextId = 1;
        public sampleRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var pay = new Payment[]
                    {
                        new Payment
                        {
                            Id= "f37dd903-f833-4bd2-9350-81b0e947d9ad",
                            ApplicationId = 197104,
                            Type = "Debit",
                            Summary = "Payment",
                            Amount = 51.23M,
                            PostingDate = "2016-12-01T00:00:00",
                            IsCleared = true,
                            ClearedDate = "2016-12-02T00:00:00"
                        },

                        new Payment
                        {
                            Id= "2b5192b3-61f6-4635-adba-69b1b3ff3718",
                            ApplicationId = 456299,
                            Type = "Credit",
                            Summary = "Refund",
                            Amount = 8.62M,
                            PostingDate = "2016-12-01T00:00:00",
                            IsCleared = true,
                            ClearedDate = null
                        },
                    };

                    ctx.Cache[CacheKey] = pay;
                }
            }
        }


        public Payment[] GetAllPayments()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Payment[])ctx.Cache[CacheKey];
            }

            return new Payment[]
                {
                    new Payment
                    {
                            Id= "7a66f608-2979-4b79-ba2e-d9b4d573b294",
                            ApplicationId = 456299,
                            Type = "Debit",
                            Summary = "Payment",
                            Amount = 95.11M,
                            PostingDate = "2017-01-23T00:00:00",
                            IsCleared = false,
                            ClearedDate = null
                    }
                };
        }


        public Payment Get(string id)
        {
            return payment.Find(p => p.Id == id);
        }

        public Payment Add(Payment item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Id = Convert.ToString(_nextId++);
            payment.Add(item);
            return item;
        }

        public bool Update(Payment item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = payment.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            payment.RemoveAt(index);
            payment.Add(item);
            return true;
        }

        public bool SavePayment(Payment pay)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Payment[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(pay);
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}