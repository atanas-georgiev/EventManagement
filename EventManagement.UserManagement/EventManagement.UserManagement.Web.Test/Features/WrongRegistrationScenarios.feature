Feature: WrongRegistrationScenarios


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
	Then I expect to see "The Password must be at least 6 and at max 100 characters long." text in "Password Error"
	Then I expect to see "The Password must be at least 6 and at max 100 characters long." text in "Validation Summary Errors" number "1"|

Scenario: Wrong confirm password
	Given registration page
	Then I enter "Sisi First Name" in "First Name"
	Then I enter "Sisi Last Name" in "Last Name"
	Then I enter "sisi@abv.bg" in "Email"
	Then I enter "Password" in "Password"
	Then I enter "Passwors" in "Confirm Password"
	Then I click on "Register Button"
	Then I expect to see "The password and confirmation password do not match" text in "Confirm Password Error"
	
Scenario: Wrong email
	Given registration page
	Then I enter "Sisi First Name" in "First Name"
	Then I enter "Sisi Last Name" in "Last Name"
	Then I enter "sisiabv.bg" in "Email"
	Then I enter "Password" in "Password"
	Then I enter "Password" in "Confirm Password"
	Then I click on "Register Button"
	Then I expect to see "The Email field is not a valid e-mail address." text in "Email Error"
	Then I expect to see "The Email field is not a valid e-mail address." text in "Validation Summary Errors" number "1"|
