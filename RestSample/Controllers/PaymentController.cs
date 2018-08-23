using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using RestSample.Models;
using RestSample.Services;
using System.Net.Http;
using System.Net;

namespace RestSample.Controllers
{
    public class PaymentController : ApiController
    {
        private PaymentRepository paymentRepository;

        public PaymentController()
        {
            this.paymentRepository = new PaymentRepository();
        }

        public Payment[] Get()
        {
            return this.paymentRepository.GetAllPayments();
        }

        public Payment GetById(string Id)
        {
            Payment item = paymentRepository.Get(Id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }


        public Payment PostProduct(Payment item)
        {
            item = paymentRepository.Add(item);
            return item;
        }


        public HttpResponseMessage Post(Payment pay)
        {
            this.paymentRepository.SavePayment(pay);

            var response = Request.CreateResponse<Payment>(System.Net.HttpStatusCode.Created, pay);

            return response;
        }
    }
}