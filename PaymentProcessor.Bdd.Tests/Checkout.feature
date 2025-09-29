Feature: Checkout
  Validate checkout behavior for different payment modes and edge cases

  Background:
    Given a checkout service

  Scenario: Successful payment using PayPal
    When I checkout using "PayPal" with amount 50.00
    Then the logs should contain "Processing PayPal payment"
    And the logs should contain "$50.00"

  Scenario: Successful payment using GooglePay
    When I checkout using "GooglePay" with amount 12.34
    Then the logs should contain "Processing GooglePay payment"
    And the logs should contain "$12.34"

  Scenario: Successful payment using CreditCard
    When I checkout using "CreditCard" with amount 75.5
    Then the logs should contain "Processing Credit Card payment"
    And the logs should contain "$75.50"

  Scenario: Invalid payment mode
    When I checkout using "Unknown" with amount 10.00
    Then the logs should contain "Invalid payment mode selected!"

  Scenario: Amount zero is rejected
    When I checkout using "PayPal" with amount 0
    Then the logs should contain "Amount must be greater than zero."

  Scenario: Negative amount is rejected
    When I checkout using "GooglePay" with amount -5.00
    Then the logs should contain "Amount must be greater than zero."

  Scenario: Very large amount is processed
    When I checkout using "CreditCard" with amount 1000000.99
    Then the logs should contain "Processing Credit Card payment"
    And the logs should contain "$1000000.99"

  Scenario: Multiple sequential payments
    When I checkout using "PayPal" with amount 10.00
    And I checkout using "GooglePay" with amount 20.00
    Then the logs should contain "Processing PayPal payment"
    And the logs should contain "Processing GooglePay payment"

  Scenario: Register and use a custom processor at runtime
    Given I register a processor named "TestPay" that logs "Processing TestPay payment"
    When I checkout using "TestPay" with amount 20.00
    Then the logs should contain "Processing TestPay payment"

  Scenario: Factory support checks
    Given I query support for "PayPal"
    Then the result should be true

    Given I query support for "FakePay"
    Then the result should be false
