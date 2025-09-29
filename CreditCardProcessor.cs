using System;
namespace PaymentProcessor;

public sealed class CreditCardProcessor : IPaymentProcessor
{
    private readonly Action<string> _log;
    public CreditCardProcessor(Action<string> log) => _log = log ?? throw new ArgumentNullException(nameof(log));
    public void Process(double amount) => _log($"Processing Credit Card payment of ${amount:F2}");
}
