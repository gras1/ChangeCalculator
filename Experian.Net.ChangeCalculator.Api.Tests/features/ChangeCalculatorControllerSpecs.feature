Feature: ChangeCalculatorControllerTests
	In order to buy products from a shop that only deals with cash
	As a checkout assistant
	I need an application running on the till to calculate what change I give to the customer

Scenario: A purchase that returns £14.50 in change
	Given the customer buys something for £5.50
	When the customer gives me £20
	Then I expect to receive £14.50 in change back

Scenario: Simple calculation that doesn't return any change
	Given the customer buys something for £5.50
	When the customer gives me £5.50 exactly
	Then I don't expect to receive any change back

Scenario: Attempts a purchase without enough money
	Given the customer buys something for £5.50
	When the customer gives me £1
	Then A bad request is returned that states there is 'Not enough money to make the purchase'