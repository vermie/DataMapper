Feature: DataMapper
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Map entity to object all properties correctly
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | Auto           |	
	And I create a TypeMapStore
	And I define AccountEntity to AccountObject  
	And I finish
	When I map AccountEntity to AccountObject
	Then the AccountObject should have the given AccountEntity values

Scenario: Map entity to object with enum conversion correctly
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | 1              |
	And I create a TypeMapStore
	And I define AccountEntity to AccountObjectWithEnums  
	And I finish
	When I map AccountEntity to AccountObjectWithEnums
	Then the AccountObjectWithEnums should have the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | Auto           |

Scenario: Map entity to object but target is missing properties
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | Auto           |	
	And I create a TypeMapStore
	And I define AccountEntity to AccountObjectMissingProperty  
	When I finish
	Then I should receive a validation error

Scenario: Try to map but finish before define
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | 1           |	
	And I create a TypeMapStore
	And I finish
	And I define AccountEntity to AccountObjectMissingProperty  
	When I map AccountEntity to AccountObject and catch exception
	Then I should receive a TypeMapException

Scenario: Map object to entity all properties correctly
	Given I have an AccountObject with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | Auto           |	
	And I create a TypeMapStore
	And I define AccountObject to AccountEntity  
	And I finish
	When I map AccountObject to AccountEntity
	Then the AccountEntity should have the given AccountObject values

Scenario: Map entity to object but ignore a property
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | Auto           |	
	And I create a TypeMapStore
	And I define AccountEntity to AccountObject and ignore property
	And I finish
	When I map AccountEntity to AccountObject 
	Then the AccountEntity should have the given AccountObject values minus the ignored property

Scenario: Map entity to object with extra properties correctly
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | 1              |
	And I create a TypeMapStore
	And I define AccountEntity to AccountObjectWithExtraProperties
	And I finish
	When I map AccountEntity to AccountObjectWithExtraProperties
	Then the AccountObjectWithExtraProperties should have the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | Auto           |

Scenario: Map entity to object all properties and back correctly
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | 1           |    
	And I create a TypeMapStore
	And I define AccountEntity to AccountObject   
	And I finish
	And I map AccountEntity to AccountObject  
	And I update AccountObject properties to the following
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy2      | 555555555    | 2           |
	When I map AccountObject to AccountEntity
	Then the AccountEntity should have the updated AccountObject values

Scenario: Try to map but use different type than has been defined
	Given I have an AccountEntity with the following properties
	| AccountNumber | PolicyNumber | LineOfBusiness |
	| Giggidy       | 123456789    | 1              |
	And I create a TypeMapStore
	And I define AccountEntity to AccountObject 
	And I finish   	
	When I map AccountEntity to AccountObjectWithEnums and catch exception
	Then I should receive a TypeMapException

Scenario: Try to map using a reference type I will only get the value types and map from source to target
	Given I have an SomeTypeEntity with the following properties
	| AccountNumber | PolicyNumber | 
	| Giggidy       | 123456789    | 
	| Giggidy       | 123456788    | 
	And I create a TypeMapStore
	And I define SomeTypeEntity to SomeTypeObject    	
	And I finish
	When I map SomeTypeEntity to SomeTypeObject
	Then I should only see value types on SomeTypeObject having data

Scenario: Try to map using a reference type I will only get the value types and map from target to source
	Given I have an SomeTypeObject with the following properties
	| AccountNumber | PolicyNumber | 
	| Giggidy       | 123456789    | 
	| Giggidy       | 123456788    | 
	And I create a TypeMapStore
	And I define SomeTypeEntity to SomeTypeObject    	
	And I finish
	When I map SomeTypeObject to SomeTypeEntity
	Then I should only see value types on SomeTypeEntity having data