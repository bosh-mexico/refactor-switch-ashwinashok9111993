using System;

namespace PaymentProcessor;

public class CheckoutService
{
    private readonly PaymentProcessorFactory _factory;
    private readonly Action<string> _logger;

    public CheckoutService(PaymentProcessorFactory factory, Action<string> logger)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Checkout(PaymentMode mode, double amount)
    {
        if (amount <= 0)
        {
            _logger("Amount must be greater than zero.");
            return;
        }

        var processor = _factory.Create(mode, _logger);
        if (processor != null)
        {
            processor.Process(amount);
        }
        else
        {
            _logger("Invalid payment mode selected!");
        }
    }

    // New overload: accept runtime-registered string keys
    public void Checkout(string modeKey, double amount)
    {
        if (amount <= 0)
        {
            _logger("Amount must be greater than zero.");
            return;
        }

        var processor = _factory.Create(modeKey, _logger);
        if (processor != null)
        {
            processor.Process(amount);
        }
        else
        {
            _logger("Invalid payment mode selected!");
        }
    }
}