﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18034
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace DataMapper.EntityFramework.Tests.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class DataMapperFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "DataMapper.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "DataMapper", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "DataMapper")))
            {
                DataMapper.EntityFramework.Tests.Specs.DataMapperFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map entity to object all properties correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        public virtual void MapEntityToObjectAllPropertiesCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map entity to object all properties correctly", new string[] {
                        "mytag"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table1.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "Auto"});
#line 8
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table1, "Given ");
#line 11
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("I define AccountEntity to AccountObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.When("I map AccountEntity to AccountObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
 testRunner.Then("the AccountObject should have the given AccountEntity values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map entity to object with enum conversion correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void MapEntityToObjectWithEnumConversionCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map entity to object with enum conversion correctly", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table2.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "1"});
#line 18
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table2, "Given ");
#line 21
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.And("I define AccountEntity to AccountObjectWithEnums", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.When("I map AccountEntity to AccountObjectWithEnums", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table3.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "Auto"});
#line 25
 testRunner.Then("the AccountObjectWithEnums should have the following properties", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map entity to object but target is missing properties")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void MapEntityToObjectButTargetIsMissingProperties()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map entity to object but target is missing properties", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table4.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "Auto"});
#line 30
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table4, "Given ");
#line 33
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.And("I define AccountEntity to AccountObjectMissingProperty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.When("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 36
 testRunner.Then("I should receive a validation error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Try to map but finish before define")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void TryToMapButFinishBeforeDefine()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Try to map but finish before define", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table5.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "1"});
#line 39
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table5, "Given ");
#line 42
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.And("I define AccountEntity to AccountObjectMissingProperty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.When("I map AccountEntity to AccountObject and catch exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
 testRunner.Then("I should receive a TypeMapException", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map object to entity all properties correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void MapObjectToEntityAllPropertiesCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map object to entity all properties correctly", ((string[])(null)));
#line 48
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table6.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "Auto"});
#line 49
 testRunner.Given("I have an AccountObject with the following properties", ((string)(null)), table6, "Given ");
#line 52
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.And("I define AccountObject to AccountEntity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.When("I map AccountObject to AccountEntity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 56
 testRunner.Then("the AccountEntity should have the given AccountObject values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map entity to object but ignore a property")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void MapEntityToObjectButIgnoreAProperty()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map entity to object but ignore a property", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table7.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "Auto"});
#line 59
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table7, "Given ");
#line 62
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.And("I define AccountEntity to AccountObject and ignore property", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.When("I map AccountEntity to AccountObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 66
 testRunner.Then("the AccountEntity should have the given AccountObject values minus the ignored pr" +
                    "operty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map entity to object with extra properties correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void MapEntityToObjectWithExtraPropertiesCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map entity to object with extra properties correctly", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table8.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "1"});
#line 69
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table8, "Given ");
#line 72
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.And("I define AccountEntity to AccountObjectWithExtraProperties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.When("I map AccountEntity to AccountObjectWithExtraProperties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table9.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "Auto"});
#line 76
 testRunner.Then("the AccountObjectWithExtraProperties should have the following properties", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Map entity to object all properties and back correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void MapEntityToObjectAllPropertiesAndBackCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Map entity to object all properties and back correctly", ((string[])(null)));
#line 80
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table10.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "1"});
#line 81
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table10, "Given ");
#line 84
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
 testRunner.And("I define AccountEntity to AccountObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
 testRunner.And("I map AccountEntity to AccountObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table11.AddRow(new string[] {
                        "Giggidy2",
                        "555555555",
                        "2"});
#line 88
 testRunner.And("I update AccountObject properties to the following", ((string)(null)), table11, "And ");
#line 91
 testRunner.When("I map AccountObject to AccountEntity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 92
 testRunner.Then("the AccountEntity should have the updated AccountObject values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Try to map but use different type than has been defined")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void TryToMapButUseDifferentTypeThanHasBeenDefined()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Try to map but use different type than has been defined", ((string[])(null)));
#line 94
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber",
                        "LineOfBusiness"});
            table12.AddRow(new string[] {
                        "Giggidy",
                        "123456789",
                        "1"});
#line 95
 testRunner.Given("I have an AccountEntity with the following properties", ((string)(null)), table12, "Given ");
#line 98
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
 testRunner.And("I define AccountEntity to AccountObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.When("I map AccountEntity to AccountObjectWithEnums and catch exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 102
 testRunner.Then("I should receive a TypeMapException", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Try to map using a reference type I will only get the value types and map from so" +
            "urce to target")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void TryToMapUsingAReferenceTypeIWillOnlyGetTheValueTypesAndMapFromSourceToTarget()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Try to map using a reference type I will only get the value types and map from so" +
                    "urce to target", ((string[])(null)));
#line 104
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber"});
            table13.AddRow(new string[] {
                        "Giggidy",
                        "123456789"});
            table13.AddRow(new string[] {
                        "Giggidy",
                        "123456788"});
#line 105
 testRunner.Given("I have an SomeTypeEntity with the following properties", ((string)(null)), table13, "Given ");
#line 109
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
 testRunner.And("I define SomeTypeEntity to SomeTypeObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
 testRunner.When("I map SomeTypeEntity to SomeTypeObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 113
 testRunner.Then("I should only see value types on SomeTypeObject having data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Try to map using a reference type I will only get the value types and map from ta" +
            "rget to source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "DataMapper")]
        public virtual void TryToMapUsingAReferenceTypeIWillOnlyGetTheValueTypesAndMapFromTargetToSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Try to map using a reference type I will only get the value types and map from ta" +
                    "rget to source", ((string[])(null)));
#line 115
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountNumber",
                        "PolicyNumber"});
            table14.AddRow(new string[] {
                        "Giggidy",
                        "123456789"});
            table14.AddRow(new string[] {
                        "Giggidy",
                        "123456788"});
#line 116
 testRunner.Given("I have an SomeTypeObject with the following properties", ((string)(null)), table14, "Given ");
#line 120
 testRunner.And("I create a TypeMapStore", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
 testRunner.And("I define SomeTypeEntity to SomeTypeObject", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.And("I finish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
 testRunner.When("I map SomeTypeObject to SomeTypeEntity", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 124
 testRunner.Then("I should only see value types on SomeTypeEntity having data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
