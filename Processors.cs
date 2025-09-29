using System;
namespace PaymentProcessor;
public interface IPaymentProcessor { void Process(double amount); }

public sealed class PayPalProcessor : IPaymentProcessor
{
    private readonly Action<string> _log;
    public PayPalProcessor(Action<string> log) => _log = log ?? throw new ArgumentNullException(nameof(log));
    public void Process(double amount) => _log($"Processing PayPal payment of ${amount:F2}");
}

public sealed class GooglePayProcessor : IPaymentProcessor
{
    private readonly Action<string> _log;
    public GooglePayProcessor(Action<string> log) => _log = log ?? throw new ArgumentNullException(nameof(log));
    public void Process(double amount) => _log($"Processing GooglePay payment of ${amount:F2}");
}

public sealed class CreditCardProcessor : IPaymentProcessor
{
    private readonly Action<string> _log;
    public CreditCardProcessor(Action<string> log) => _log = log ?? throw new ArgumentNullException(nameof(log));
    public void Process(double amount) => _log($"Processing Credit Card payment of ${amount:F2}");
}
