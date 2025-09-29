using System;
using System.Collections.Generic;
using PaymentProcessor;

class Program
{
    static void Main()
    {
        var logs = new List<string>();
        var factory = new PaymentProcessorFactory();
        var service = new CheckoutService(factory, m => { logs.Add(m); Console.WriteLine(m); });

        service.Checkout(PaymentMode.PayPal, 12.34);
        service.Checkout(PaymentMode.GooglePay, 55.00);
        service.Checkout(PaymentMode.CreditCard, 75.5);
        service.Checkout("TestPay", 20.00); // not registered, should log invalid

        Console.WriteLine("Demo finished.");
    }
}