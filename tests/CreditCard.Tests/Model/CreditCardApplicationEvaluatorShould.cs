using CreditCards.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CreditCard.Tests.Model
{
    public class CreditCardApplicationEvaluatorShould
    {
        private const int ExpectedLowIncomeThreshold = 20_000;
        private const int ExpectedHighIncomeThreshold = 100_000;


        [Theory]
        [InlineData(ExpectedHighIncomeThreshold)]
        [InlineData(ExpectedHighIncomeThreshold + 1)]
        [InlineData(int.MaxValue)]
        public void AcceptAllHighIncomeApplicants(int income)
        {
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = income,
                Age = 21
            };

            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, sut.Evaluate(application));
        }


        [Theory]
        [InlineData(20)]
        [InlineData(19)]
        [InlineData(0)]
        [InlineData(int.MinValue)]

        public void ReferYoungApplicantsWhoAreNotHighIncome(int age)
        {
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = ExpectedHighIncomeThreshold - 1,
                Age = age
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, sut.Evaluate(application));
        }


        [Theory]
        [InlineData(ExpectedLowIncomeThreshold)]
        [InlineData(ExpectedLowIncomeThreshold + 1)]
        [InlineData(ExpectedHighIncomeThreshold - 1)]

        public void ReferNonYoungApplicantsWhoAreMiddleIncome(int income)
        {
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = income,
                Age = 21
            };

            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, sut.Evaluate(application));
        }

        [Theory]
        [InlineData(ExpectedLowIncomeThreshold - 1)]
        [InlineData(0)]
        [InlineData(int.MinValue)]

        public void DeclineAllApplicantsWhoAreLowIncome(int income)
        {
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = income,
                Age = 21
            };

            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, sut.Evaluate(application));
        }


    }
}
