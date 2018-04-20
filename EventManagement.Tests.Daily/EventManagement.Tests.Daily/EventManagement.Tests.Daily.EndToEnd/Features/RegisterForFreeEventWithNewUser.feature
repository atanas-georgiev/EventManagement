Feature: RegisterForFreeEventWithNewUser
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Missing all fields
	Given registration page
	Then I click on "Register Button"
	Then I expect to see "The First Name field is required." text in "First Name Error"
	Then I expect to see "The Last Name field is required." text in "Last Name Error"
	Then I expect to see "The Email field is required." text in "Email Error"
	Then I expect to see "The Password field is required." text in "Password Error"
	Then I expect to see "The First Name field is required." text in "First Name Error"
	Then I expect to see "The First Name field is required." text in "ValidationSummaryErrors" number "1"|
	Then I expect to see "The Last Name field is required." text in "ValidationSummaryErrors" number "2"|
	Then I expect to see "The Email field is required." text in "ValidationSummaryErrors" number "3"|
	Then I expect to see "The Password field is required." text in "ValidationSummaryErrors" number "4"|

Scenario: Wrong password
	Given registration page
	Then I enter "Sisi First Name" in "First Name"
	Then I enter "Sisi Last Name" in "Last Name"
	Then I enter "sisi@abv.bg" in "Email"
	Then I enter "Pass" in "Password"
	Then I enter "Pass" in "Confirm Password"
	Then I click on "Register Button"
	Then I expect to see "The Password must be at least 6 and at max 100 characters long" text in "Password Error"
	Then I expect to see "The Password must be at least 6 and at max 100 characters long." text in "Validation Summary Errors" number "1"|
