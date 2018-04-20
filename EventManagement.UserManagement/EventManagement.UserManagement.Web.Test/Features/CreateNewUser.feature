Feature: CreateNewUser
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Create user
	Given registration page
	Then I clean the databse
	Then I enter "Sisi First Name" in "First Name"
	Then I enter "Sisi Last Name" in "Last Name"
	Then I enter "sisi@abv.bg" in "Email"
	Then I enter "Stanislava1!" in "Password"
	Then I enter "Stanislava1!" in "Confirm Password"
	Then I click on "Register Button"
