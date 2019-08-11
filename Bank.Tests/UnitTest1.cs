using System;
using System.Collections.Generic;
using Bank.Controllers;
using Bank.Repository.Interface;
using Bank.Repository.Model;
using Bank.Repository.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bank.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SaveCard()
        {
            //Arrange
            TransactionRepository test = new TransactionRepository();

            List<GatewayPaymentViewModel> collection = new List<GatewayPaymentViewModel>();

            GatewayPaymentViewModel validCard = new GatewayPaymentViewModel()
            {
                Amount = 10000,
                CardNumber = 378282246310005,
                ExpiryMonth = 12,
                ExpiryYear = 2030
            };
            collection.Add(validCard);

            GatewayPaymentViewModel merchantCard = new GatewayPaymentViewModel()
            {
                Amount = 10000M,
                CardNumber = 5312794635897512,
                ExpiryMonth = 12,
                ExpiryYear = 2030
            };
            collection.Add(merchantCard);

            GatewayPaymentViewModel invalidMonth = new GatewayPaymentViewModel()
            {
                Amount = 10000M,
                CardNumber = 5312794635897512,
                ExpiryMonth = 01,
                ExpiryYear = 2030
            };
            collection.Add(invalidMonth);

            GatewayPaymentViewModel invalidYear = new GatewayPaymentViewModel()
            {
                Amount = 10000M,
                CardNumber = 5312794635897512,
                ExpiryMonth = 12,
                ExpiryYear = 2015
            };
            collection.Add(invalidYear);



            //Act
            foreach (var item in collection)
            {
                var result = test.AddCard(item);
            }

            //Assert
        }
    }
}
