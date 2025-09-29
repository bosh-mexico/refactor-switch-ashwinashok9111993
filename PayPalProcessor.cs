using System;
namespace PaymentProcessor;

public sealed class PayPalProcessor : IPaymentProcessor
{
    private readonly Action<string> _log;
    public PayPalProcessor(Action<string> log) => _log = log ?? throw new ArgumentNullException(nameof(log));
    public void Process(double amount) => _log($"Processing PayPal payment of ${amount:F2}");
}
