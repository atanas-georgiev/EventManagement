Feature: 03_RegisterPayedEvent


Scenario: 01_RegisterPayedEvent
	Given I login with "Admin" user
	When I click "RowsPerPage"
	Then I select to see "100" "Per Page"
	Then I click "Registration Information Button For Specific Event" with name "Automation Paied"|
	Then I see page: "Registration Admin Information Page"
	Then I expect to see "Automation Paied" text in "Event Name"
	Then I expect to see "Jul 15, 2018, 8:00:00 AM - Jul 15, 2018, 10:00:00 AM" text in "Dates"
	Then I expect to see "01_KPMG" text in "Location Name"
	Then I expect "Registered Person Name" number "1"| to missing from page
	Then I click "Goback Button"
	Then I see page: "Registration Table Page"
	Then I click "LogOut"
	Then I login with "NoAdmin" user
	Then I click "Information Button For Specific Event" with name "Automation Paied"|
	Then I see page: "Information Page"
	Then I expect to see "Automation Paied" text in "Event Name"
	Then I expect to see "Jul 15, 2018, 8:00:00 AM - Jul 15, 2018, 10:00:00 AM" text in "Date"
	Then I expect to see "01_KPMG" text in "Event Location Name"
	Then I expect to see "Teodor, Marina" text in "Lecture Name"
	Then I expect to see "USD45.00" text in "Price"
	Then I expect to see "25" text in "Free Spaces"
	Then I click "Register Button"
	Then I see page: "Registration Table Page"
	Then I expect to see "24" text in "Free Spaces" for event "Automation Not Paied"|
	Then I click "LogOut"
	Then I login with "Admin" user
	When I click "RowsPerPage"
	Then I select to see "100" "Per Page"
	Then I click "Registration Information Button For Specific Event" with name "Automation Paied"|
	Then I see page: "Registration Admin Information Page"
	Then I expect to see "Automation Paied" text in "Event Name"
	Then I expect to see "Jul 15, 2018, 8:00:00 AM - Jul 15, 2018, 10:00:00 AM" text in "Dates"
	Then I expect to see "01_KPMG" text in "Location Name"
	Then I expect "Registered Person Name" number "1"| to missing from page
	Then I click "Goback Button"

Scenario: 02_PayedEventIsShownInRegistrationInformation
	Given last registration is payed
	Then I login with "Admin" user
	When I click "RowsPerPage"
	Then I select to see "100" "Per Page"
	Then I click "Registration Information Button For Specific Event" with name "Automation Paied"|
	Then I see page: "Registration Admin Information Page"
	Then I expect to see "Automation Paied" text in "Event Name"
	Then I expect to see "Jul 15, 2018, 8:00:00 AM - Jul 15, 2018, 10:00:00 AM" text in "Dates"
	Then I expect to see "01_KPMG" text in "Location Name"
	Then I expect to see "Regular User Automation" text in "Registered Person Name" number "1"|
	Then I click "Goback Button"
	Then I see page: "Registration Table Page"
	Then I expect to see "24" text in "Free Spaces" for event "Automation Not Paied"|