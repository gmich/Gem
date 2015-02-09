Feature: Client Cases
Various scenarios involving client - server interactions

@client
Scenario: ClientSuccessfulConnect
	Given  A server is running
	And I connect to the server
	When I send a greeding message
	Then I should get a response message and conneciton approval
