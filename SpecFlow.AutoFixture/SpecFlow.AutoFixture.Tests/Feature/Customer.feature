Feature: Customer
	In order to avoid silly mistakes
	As an API Developer
	I want to make sure I can manage customers

@subcutaneous
Scenario: Add a Customer
	Given I POST a valid customer to the API
	When I GET the customer using  the API
	Then the result should be the customer

@unit
Scenario: Full Name is First Name plus Last Name
	Given I have a valid customer
	When I set First name as 'Rahul' and Last name as 'Nath'
	Then the FullName must be 'Rahul Nath'