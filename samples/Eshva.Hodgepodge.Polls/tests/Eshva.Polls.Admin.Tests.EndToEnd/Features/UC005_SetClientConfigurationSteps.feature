@xunit:collection(UC005TestCollection)
Feature: UC005. Set client configuration
	Put feature description here

Scenario: Administrator opens Client configuration page, can edit and store the client configuration
	Given administrator open Client configuration page
	When administrator changed the configuration refresh interval
	And commands to store client configuration
    Then the configuration refresh interval should be changed in the product configuration database
