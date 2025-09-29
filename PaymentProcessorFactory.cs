using System;
using System.Collections.Generic;

namespace PaymentProcessor;

public interface IPaymentProcessorFactory
{
    bool Supports(string modeKey);
    bool Supports(PaymentMode mode);
    IPaymentProcessor? Create(string modeKey, Action<string> log);
    IPaymentProcessor? Create(PaymentMode mode, Action<string> log);
    void Register(string modeKey, Func<Action<string>, IPaymentProcessor> factory);
    void Register(PaymentMode mode, Func<Action<string>, IPaymentProcessor> factory);
}

public sealed class PaymentProcessorFactory : IPaymentProcessorFactory
{
    private readonly Dictionary<string, Func<Action<string>, IPaymentProcessor>> _map;

    public PaymentProcessorFactory()
    {
        _map = new Dictionary<string, Func<Action<string>, IPaymentProcessor>>(StringComparer.OrdinalIgnoreCase);

        // Register built-in processors using the enum names as keys.
        Register(PaymentMode.PayPal, log => new PayPalProcessor(log));
        Register(PaymentMode.GooglePay, log => new GooglePayProcessor(log));
        Register(PaymentMode.CreditCard, log => new CreditCardProcessor(log));
    }

    public void Register(string modeKey, Func<Action<string>, IPaymentProcessor> factory)
    {
        if (string.IsNullOrWhiteSpace(modeKey)) throw new ArgumentException("modeKey must be provided", nameof(modeKey));
        _map[modeKey] = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void Register(PaymentMode mode, Func<Action<string>, IPaymentProcessor> factory) => Register(mode.ToString(), factory);

    public bool Supports(string modeKey) => !string.IsNullOrEmpty(modeKey) && _map.ContainsKey(modeKey);

    public bool Supports(PaymentMode mode) => Supports(mode.ToString());

    public IPaymentProcessor? Create(string modeKey, Action<string> log) => _map.TryGetValue(modeKey ?? string.Empty, out var f) ? f(log) : null;

    public IPaymentProcessor? Create(PaymentMode mode, Action<string> log) => Create(mode.ToString(), log);

    // backward-compat convenience
    public IPaymentProcessor? CreateProcessor(PaymentMode mode, Action<string> log) => Create(mode, log);
}
