using System;
namespace PaymentProcessor;

public sealed class GooglePayProcessor : IPaymentProcessor
{
    private readonly Action<string> _log;
    public GooglePayProcessor(Action<string> log) => _log = log ?? throw new ArgumentNullException(nameof(log));
    public void Process(double amount) => _log($"Processing GooglePay payment of ${amount:F2}");
}
