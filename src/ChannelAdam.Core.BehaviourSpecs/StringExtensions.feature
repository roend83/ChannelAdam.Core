Feature: String Extensions


@UnitTest
Scenario: Should format a string with named placeholders
	When a string with named placeholders is formatted
	Then the result has the named placeholders correctly replaced
