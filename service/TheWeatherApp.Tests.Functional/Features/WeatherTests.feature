Feature: Weather Tests

Scenario: A valid request is made and weather is retrieved and returned
	Given A valid input weather request
	When A request is made to the weather service
	Then a 200 response and the weather is returned