# TigerCard
Cost computation service of a fictional metro service

# Solution Organization
1. TigerCard.Business 
   - Contains implementation
2. TigerCard.Test 
   - Contains unit tests of individual types
3. TigerCard.IntegrationTest 
   - Everything gets wired together here. No mocks
   - Contains tests to verify the overall implementation with default fare and cap settings

# TigerCard.Business

Uses Unity framework nuget package for dependency injection

Contents of the project file:
  - Extensions
    - TimeSpanExtensions.cs - Extension method to verify if a time span is contained within a duration
  - Grouping
    - GroupFareAggregatorBase.cs - Base implementation for fare computation. 
      - Template class that leaves the responsibility of grouping and fare computation of individual group of journeys
    - JourneyGroup.cs - Wrapper class for group of journeys
      - Contains a group of journeys
      - Applies a given cap (weekly/daily) for each of the journey
    - PerDayGroupFareAggregator.cs 
      - Given a list of journeys creates groups for each distinct day of week of individual journeys. Ex: M, M, M, Tu, Tu, T, F, F, Sa, Sa will generate 5 groups [M,M,M], [Tu,Tu], [T], [F,F], [Sa, Sa]
    - PerWeekGroupFareAggregator.cs 
      - Given a list of journeys groups the journeys into weeks. Ex: W, T, F, Sa, Su, Su, M, Tu, W, T will generate 2 groups [W,T,F,Sa,Su,Su] & [M,Tu,W,T]
  - Interfaces
    - Contains all marker interfaces for dependency resolution
  - Models
    - Duration.cs - Utility class to abstract a time range
    - FareCap.cs - Class that contains different fare caps for a journey based on zones (daily cap, weekly cap etc)
    - Journey.cs - Class that represents a journey between 2 zones on a given Day Of Week & time
    - JourneyFare.cs - Class that represents the fare for a given journey, kind of price (peak/off peak), capped/un-capped
    - TicketFare.cs - Class that represents the peak, off-peak prices for a journey between 2 zones
  - Settings 
    - FareSettings.cs 
      - Exposes method to get peak hours for a day of week
      - Exposes method to get ticket fare between a given origin & destination
    - FareSettingsFactory.cs 
      - Helper class to create the default settings (peak hours, daily cap & weekly caps are set up mentioned in the problem statement)

  - FareAggregator.cs - Fare aggregation and Fare limiter helper class
  - FareCalculator.cs - Calculates the Journey fare based on the Origin, Destination, Day & Time of the journey (peak/off-peak)
  - FareComputationService.cs - Class that abstracts the existence of Aggregator/Calculator/etc.
  - UnityRegistrar.cs - Registers the implementation class for a given dependency

# TigerCard.IntegrationTest

Uses Unity framework nuget package to set up dependency injection
Calls the TigerCard.Business.UnityRegistrar with a container instance to initialize the implementation

Each test file in the project sets up a scenario for different possible journeys

# TigerCard.Test

Uses Moq framework nuget package to mock dependencies of each type
Contains tests for each class according to the structure of TigerCard.Business project
