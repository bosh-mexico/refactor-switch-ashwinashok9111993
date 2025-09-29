using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using FluentAssertions;
using PaymentProcessor;

namespace PaymentProcessor.Bdd.Tests.Steps;

[Binding]
public class CheckoutSteps
{
    private readonly List<string> _logs = new();
    private readonly PaymentProcessorFactory _factory = new();
    private CheckoutService? _service;
    private bool _lastSupportResult;

    [Given("a checkout service")]
    public void GivenACheckoutService()
    {
        _service = new CheckoutService(_factory, m => _logs.Add(m));
    }

    [Given("I register a processor named \"(.*)\" that logs \"(.*)\"")]
    public void GivenIRegisterAProcessor(string name, string message)
    {
        _factory.Register(name, log => new TestProcessor(message, log));
    }

    [Given("I query support for \"(.*)\"")]
    public void GivenIQuerySupportFor(string name)
    {
        _lastSupportResult = _factory.Supports(name);
    }

    [When("I checkout using \"(.*)\" with amount (.*)")]
    public void WhenICheckoutUsingWithAmount(string name, double amount)
    {
        if (_service == null) GivenACheckoutService();
        // try parse name as enum first
        if (Enum.TryParse<PaymentMode>(name, true, out var mode))
        {
            _service!.Checkout(mode, amount);
            return;
        }

        // fallback to string-key checkout to support runtime-registered processors
        _service!.Checkout(name, amount);
    }

    [Then("the logs should contain \"(.*)\"")]
    public void ThenTheLogsShouldContain(string text)
    {
        _logs.Should().Contain(l => l.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    [Then("the result should be (true|false)")]
    public void ThenTheResultShouldBe(bool expected)
    {
        _lastSupportResult.Should().Be(expected);
    }

    class TestProcessor : IPaymentProcessor
    {
        private readonly string _msg;
        private readonly Action<string> _log;
        public TestProcessor(string msg, Action<string> log) { _msg = msg; _log = log; }
        public void Process(double amount) => _log(_msg + " of $" + amount.ToString("F2"));
    }
}
