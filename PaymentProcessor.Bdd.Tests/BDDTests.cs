using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using PaymentProcessor;

namespace PaymentProcessor.Bdd.Tests;

public class BDDTests
{
    // Helper to create service and capture logs
    private static (CheckoutService Service, List<string> Logs) CreateService()
    {
        var logs = new List<string>();
        var factory = new PaymentProcessorFactory();
        var service = new CheckoutService(factory, m => logs.Add(m));
        return (service, logs);
    }

    public static IEnumerable<object[]> SuccessfulModesData()
    {
        yield return new object[] { PaymentMode.PayPal, "Processing PayPal payment", 50.00 };
        yield return new object[] { PaymentMode.GooglePay, "Processing GooglePay payment", 12.34 };
        yield return new object[] { PaymentMode.CreditCard, "Processing Credit Card payment", 75.50 };
    }

    [Theory]
    [MemberData(nameof(SuccessfulModesData))]
    public void Successful_Mode_Processes(PaymentMode mode, string expectedFragment, double amount)
    {
        var (service, logs) = CreateService();

        service.Checkout(mode, amount);

        logs.Should().Contain(l => l.Contains("Processing", StringComparison.OrdinalIgnoreCase));
        logs.Should().Contain(l => l.Contains(expectedFragment, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Invalid_Payment_Mode_Logs_Error()
    {
        var (service, logs) = CreateService();

        service.Checkout(PaymentMode.Unknown, 10);

        logs.Should().Contain(l => l.Contains("Invalid payment mode selected!", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-5.0)]
    public void Invalid_Amounts_Are_Rejected(double amount)
    {
        var (service, logs) = CreateService();

        service.Checkout(PaymentMode.PayPal, amount);

        logs.Should().Contain(l => l.Contains("Amount must be greater than zero.", StringComparison.OrdinalIgnoreCase));
    }
}
