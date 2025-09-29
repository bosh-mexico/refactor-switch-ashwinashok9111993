using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using PaymentProcessor;

namespace PaymentProcessor.Bdd.Tests;

public class BDDTests
{
    [Fact]
    public void Successful_PayPal_Checkout()
    {
        var logs = new List<string>();
        var factory = new PaymentProcessorFactory();
        var service = new CheckoutService(factory, m => logs.Add(m));

        service.Checkout(PaymentMode.PayPal, 50.00);

        logs.Should().Contain(l => l.Contains("Processing PayPal payment", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Invalid_Payment_Mode()
    {
        var logs = new List<string>();
        var factory = new PaymentProcessorFactory();
        var service = new CheckoutService(factory, m => logs.Add(m));

        service.Checkout(PaymentMode.Unknown, 10);

        logs.Should().Contain(l => l.Contains("Invalid payment mode selected!", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Invalid_Amount()
    {
        var logs = new List<string>();
        var factory = new PaymentProcessorFactory();
        var service = new CheckoutService(factory, m => logs.Add(m));

        service.Checkout(PaymentMode.PayPal, 0);

        logs.Should().Contain(l => l.Contains("Amount must be greater than zero.", StringComparison.OrdinalIgnoreCase));
    }
}
