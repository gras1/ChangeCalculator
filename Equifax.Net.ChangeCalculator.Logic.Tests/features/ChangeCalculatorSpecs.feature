Feature: ChangeHandlerTests
	In order to buy products from a shop that only deals with cash
	As a checkout assistant
	I need an application running on the till to calculate what change I give to the customer

Scenario: Simple calculation that doesn't return any change
	Given the customer buys something for £5.50
	When the customer gives me £5.50
	Then I don't expect to receive any change back