using System;
using System.Collections.Generic;
using DataMapper.EntityFramework.Tests.Specs.Helpers;
using DataMapper.TypeMapping;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataMapper.Building;
using System.Linq;

namespace DataMapper.EntityFramework.Tests.Specs.StepDefinitions
{
    [Binding]
    public class DataMapperSteps
    {

        [Given(@"I have an AccountEntity with the following properties")]
        public void GivenIHaveAnAccountEntityWithTheFollowingProperties(Table table)
        {
            AccountEntity ae = table.CreateInstance<AccountEntity>();
            ScenarioContext.Current.Set(ae);
        }

        [Given(@"I define AccountEntity to AccountObject")]
        public void GivenIDefineAccountEntityToAccountObject()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<AccountEntity, AccountObject>();
        }

        [When(@"I map AccountEntity to AccountObject")]
        public void WhenIMapAccountEntityToAccountObject()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            AccountObject ao = new AccountObject();
            s.Map<AccountEntity, AccountObject>(ScenarioContext.Current.Get<AccountEntity>(), ao);
            ScenarioContext.Current.Set(ao);
        }

        [Then(@"the AccountObject should have the given AccountEntity values")]
        public void ThenTheAccountObjectShouldHaveTheGivenAccountEntityValues()
        {
            AccountObject ao = ScenarioContext.Current.Get<AccountObject>();
            AccountEntity ae = ScenarioContext.Current.Get<AccountEntity>();
            
            Assert.AreEqual(ae.AccountNumber, ao.AccountNumber);
            Assert.AreEqual(ae.PolicyNumber, ao.PolicyNumber);
            Assert.AreEqual(ae.LineOfBusiness, ao.LineOfBusiness);
        }


        [Given(@"I define AccountEntity to AccountObjectWithEnums")]
        public void GivenIDefineAccountEntityToAccountObjectWithEnums()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<AccountEntity, AccountObjectWithEnum>()
                .MapProperty(a => a.LineOfBusiness, b => b.LineOfBusiness);
        }

        [When(@"I map AccountEntity to AccountObjectWithEnums")]
        public void WhenIMapAccountEntityToAccountObjectWithEnums()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            AccountObjectWithEnum ao = new AccountObjectWithEnum();
            s.Map<AccountEntity, AccountObjectWithEnum>(ScenarioContext.Current.Get<AccountEntity>(), ao);
            ScenarioContext.Current.Set(ao);
        }

        [Then(@"the AccountObjectWithEnums should have the following properties")]
        public void ThenTheAccountObjectWithEnumsShouldHaveTheFollowingProperties(Table table)
        {
            AccountObjectWithEnum aoe = new AccountObjectWithEnum();
            aoe = table.CreateInstance<AccountObjectWithEnum>();
            AccountObjectWithEnum ao = ScenarioContext.Current.Get<AccountObjectWithEnum>();
            Assert.AreEqual(aoe.LineOfBusiness, ao.LineOfBusiness);
        }

        [When(@"I finish")]
        public void WhenIFinish()
        {  
            try
            {
                var s = ScenarioContext.Current.Get<TypeMapStore>();
                s.Finish();
            }
            catch (TypeMapValidationException e)
            {
                ScenarioContext.Current.Set(e);
            }
        }

        [When(@"I map AccountEntity to AccountObject and catch exception")]
        public void WhenIMapAccountEntityToAccountObjectAndCatchException()
        {
            try
            {
                var s = ScenarioContext.Current.Get<TypeMapStore>();
                AccountObject ao = new AccountObject();
                s.Map<AccountEntity, AccountObject>(ScenarioContext.Current.Get<AccountEntity>(), ao);
                ScenarioContext.Current.Set(ao);
            }
            catch (TypeMapException e)
            {
                ScenarioContext.Current.Set(e);
            }
        }


        [Then(@"I should receive a validation error")]
        public void ThenIShouldReceiveAValidationError()
        {
            Assert.IsNotNull(ScenarioContext.Current.Get<TypeMapValidationException>());
        }

       

        [When(@"I map AccountEntity to AccountObjectWithEnums and catch exception")]
        public void WhenIMapAccountEntityToAccountObjectWithEnumsAndCatchException()
        {
            try
            {
                var s = ScenarioContext.Current.Get<TypeMapStore>();
                AccountObjectWithEnum ao = new AccountObjectWithEnum();
                s.Map<AccountEntity, AccountObjectWithEnum>(ScenarioContext.Current.Get<AccountEntity>(), ao);
                ScenarioContext.Current.Set(ao);
            }
            catch (TypeMapException e)
            {
                ScenarioContext.Current.Set(e);
            }

        }

        [Then(@"I should receive a TypeMapException")]
        public void ThenIShouldReceiveATypeMapException()
        {
            Assert.IsNotNull(ScenarioContext.Current.Get<TypeMapException>());
        }

        [Given(@"I have an AccountObject with the following properties")]
        public void GivenIHaveAnAccountObjectWithTheFollowingProperties(Table table)
        {
            AccountObject ao = table.CreateInstance<AccountObject>();
            ScenarioContext.Current.Set(ao);
        }

        [Given(@"I define AccountObject to AccountEntity")]
        public void GivenIDefineAccountObjectToAccountEntity()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<AccountObject, AccountEntity>();
        }

        [Then(@"the AccountEntity should have the given AccountObject values")]
        public void ThenTheAccountEntityShouldHaveTheGivenAccountObjectValues()
        {
            AccountObject ao = ScenarioContext.Current.Get<AccountObject>();
            AccountEntity ae = ScenarioContext.Current.Get<AccountEntity>();

            Assert.AreEqual(ae.AccountNumber, ao.AccountNumber);
            Assert.AreEqual(ae.PolicyNumber, ao.PolicyNumber);
            Assert.AreEqual(ae.LineOfBusiness, ao.LineOfBusiness);
        }

        [Given(@"I define AccountEntity to AccountObjectMissingProperty")]
        public void GivenIDefineAccountEntityToAccountObjectMissingProperty()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<AccountEntity, AccountObjectMissingProperty>();
        }

        [Given(@"I define AccountEntity to AccountObject and ignore property")]
        public void GivenIDefineAccountEntityToAccountObjectAndIgnoreProperty()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<AccountEntity, AccountObject>().IgnoreProperty(a => a.PolicyNumber);
        }


        [Then(@"the AccountEntity should have the given AccountObject values minus the ignored property")]
        public void ThenTheAccountEntityShouldHaveTheGivenAccountObjectValuesMinusTheIgnoredProperty()
        {
            AccountObject ao = ScenarioContext.Current.Get<AccountObject>();
            AccountEntity ae = ScenarioContext.Current.Get<AccountEntity>();

            Assert.AreEqual(ae.AccountNumber, ao.AccountNumber);
            Assert.AreEqual(ae.LineOfBusiness, ao.LineOfBusiness);
            Assert.IsNull(ao.PolicyNumber);
        }

        [Given(@"I define AccountEntity to AccountObjectWithExtraProperties")]
        public void GivenIDefineAccountEntityToAccountObjectWithExtraProperties()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<AccountEntity, AccountObjectWithExtraProperties>();
        }

        [When(@"I map AccountEntity to AccountObjectWithExtraProperties")]
        public void WhenIMapAccountEntityToAccountObjectWithExtraProperties()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            AccountObjectWithExtraProperties ao = new AccountObjectWithExtraProperties();
            s.Map<AccountEntity, AccountObjectWithExtraProperties>(ScenarioContext.Current.Get<AccountEntity>(), ao);
            ScenarioContext.Current.Set(ao);
        }

        [Then(@"the AccountObjectWithExtraProperties should have the following properties")]
        public void ThenTheAccountObjectWithExtraPropertiesShouldHaveTheFollowingProperties(Table table)
        {
            AccountObjectWithExtraProperties ao = ScenarioContext.Current.Get<AccountObjectWithExtraProperties>();
            AccountEntity ae = ScenarioContext.Current.Get<AccountEntity>();

            Assert.AreEqual(ae.AccountNumber, ao.AccountNumber);
            Assert.AreEqual(ae.LineOfBusiness, ao.LineOfBusiness);
            Assert.AreEqual(ae.PolicyNumber, ao.PolicyNumber);
            Assert.IsNull(ao.StateAbbreviation);
            Assert.AreEqual(DateTime.MinValue,ao.EffectiveDate);
            Assert.IsNull(ao.PhoneNumber);

        }

        [Given(@"I map AccountEntity to AccountObject")]
        public void GivenIMapAccountEntityToAccountObject()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            AccountObject ao = new AccountObject();
            s.Map<AccountEntity, AccountObject>(ScenarioContext.Current.Get<AccountEntity>(),ao);
            ScenarioContext.Current.Set(ao);
        }

        [When(@"I map AccountObject to AccountEntity")]
        public void WhenIMapAccountObjectToAccountEntity()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            AccountEntity ae = new AccountEntity();
            s.Map<AccountObject, AccountEntity>(ScenarioContext.Current.Get<AccountObject>(), ae);
            ScenarioContext.Current.Set(ae);
        }

        [Given(@"I update AccountObject properties to the following")]
        public void GivenIUpdateAccountObjectPropertiesToTheFollowing(Table table)
        {
            AccountObject ao = ScenarioContext.Current.Get<AccountObject>();
            ao.AccountNumber = table.Rows[0][0].ToString();
            ao.PolicyNumber = table.Rows[0][1].ToString();
            ao.LineOfBusiness = Convert.ToInt32(table.Rows[0][2]);
            ScenarioContext.Current.Set(ao);
        }

        [Then(@"the AccountEntity should have the updated AccountObject values")]
        public void ThenTheAccountEntityShouldHaveTheUpdatedAccountObjectValues()
        {
            AccountObject ao = ScenarioContext.Current.Get<AccountObject>();
            AccountEntity ae = ScenarioContext.Current.Get<AccountEntity>();

            Assert.AreEqual(ae.AccountNumber, ao.AccountNumber);
            Assert.AreEqual(ae.LineOfBusiness, ao.LineOfBusiness);
            Assert.AreEqual(ae.PolicyNumber, ao.PolicyNumber);
        }

     
        [Given(@"I finish")]
        public void GivenIFinish()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Finish();
        }


        [Given(@"I create a TypeMapStore")]
        public void GivenICreateATypeMapStore()
        {
            var s = new TypeMapStore();
            ScenarioContext.Current.Set(s);
        }

        [Given(@"I have an SomeTypeEntity with the following properties")]
        public void GivenIHaveAnSomeTypeEntityWithTheFollowingProperties(Table table)
        {
            SomeTypeEntity ste = new SomeTypeEntity();
            ste.AccountNumber = table.Rows[0][0].ToString();
            List<SomeTypeItem> itemList = new List<SomeTypeItem>();
            SomeTypeItem sti = new SomeTypeItem();
            sti.ItemNumber = table.Rows[0][1].ToString();
            itemList.Add(sti);
            SomeTypeItem sti2 = new SomeTypeItem();
            sti2.ItemNumber = table.Rows[1][1].ToString();
            itemList.Add(sti2);
            ste.SomeTypeItemList = itemList;
            ste.Bytes = new byte[] { 12, 3, 5, 76, 8, 0, 6, 125 };
            ste.StringArray = new string[] { "Hello" };
            ste.ObjectArray = new SomeTypeItem[] { sti };
            ScenarioContext.Current.Set(ste);
        }

        [Given(@"I have an SomeTypeObject with the following properties")]
        public void GivenIHaveAnSomeTypeObjectWithTheFollowingProperties(Table table)
        {
            SomeTypeObject sto = new SomeTypeObject();
            sto.AccountNumber = table.Rows[0][0].ToString();
            List<SomeTypeItem> itemList = new List<SomeTypeItem>();
            SomeTypeItem sti = new SomeTypeItem();
            sti.ItemNumber = table.Rows[0][1].ToString();
            itemList.Add(sti);
            SomeTypeItem sti2 = new SomeTypeItem();
            sti2.ItemNumber = table.Rows[1][1].ToString();
            itemList.Add(sti2);
            sto.SomeTypeItemList = itemList;
            sto.Bytes = new byte[] { 12, 3, 5, 76, 8, 0, 6, 125 };
            sto.StringArray = new string[] { "Hello" };
            sto.ObjectArray = new SomeTypeItem[] { sti };
            ScenarioContext.Current.Set(sto);
        }

        [Given(@"I define SomeTypeEntity to SomeTypeObject")]
        public void GivenIDefineSomeTypeEntityToSomeTypeObject()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            s.Define<SomeTypeEntity, SomeTypeObject>();
            ScenarioContext.Current.Set(s.Validate());
        }

        [When(@"I map SomeTypeEntity to SomeTypeObject")]
        public void WhenIMapSomeTypeEntityToSomeTypeObject()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            SomeTypeObject sto = new SomeTypeObject();
            s.Map<SomeTypeEntity, SomeTypeObject>(ScenarioContext.Current.Get<SomeTypeEntity>(), sto);
            ScenarioContext.Current.Set(sto);
        }

        [When(@"I map SomeTypeObject to SomeTypeEntity")]
        public void WhenIMapSomeTypeObjectToSomeTypeEntity()
        {
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            SomeTypeEntity ste = new SomeTypeEntity();
            s.Map<SomeTypeObject, SomeTypeEntity>(ScenarioContext.Current.Get<SomeTypeObject>(), ste);
            ScenarioContext.Current.Set(ste);
        }


        [Then(@"I should only see value types on SomeTypeObject having data")]
        public void ThenIShouldOnlySeeValueTypesOnSomeTypeObjectHavingData()
        {
            SomeTypeEntity ste = ScenarioContext.Current.Get<SomeTypeEntity>();
            SomeTypeObject sto = ScenarioContext.Current.Get<SomeTypeObject>();
            
            var s = ScenarioContext.Current.Get<TypeMapStore>();
            DataMapValidationList validationList = ScenarioContext.Current.Get<DataMapValidationList>();
            Boolean foundIgnoredObjectProperty = false;
            Boolean foundIgnoredObjectArrayProperty = false;
            foreach (var validationItem in validationList)
            {
                foreach (var item in validationItem.PropertyMapList)
                {
                   if (item.Description.ToLower().Contains("ignored"))
                   {
                       if (item.Description.ToLower().Contains("sometypeitemlist"))
                       {
                           foundIgnoredObjectProperty = true;
                       }
                       if (item.Description.ToLower().Contains("objectarray"))
                       {
                           foundIgnoredObjectArrayProperty = true;
                       }
                   }
                }
            }
            Assert.IsTrue(foundIgnoredObjectProperty, "Object property not ignored");
            Assert.IsTrue(foundIgnoredObjectArrayProperty, "Object array property not ignored");
            Assert.AreEqual(ste.AccountNumber, sto.AccountNumber,"Account numbers should match");
            Assert.IsNull(sto.SomeTypeItemList,"Some Type Item List should be null");
            Assert.IsTrue(ste.Bytes.SequenceEqual(sto.Bytes),"Byte array should match");
            Assert.IsTrue(ste.StringArray.SequenceEqual(sto.StringArray), "String array should match");

            ste.Bytes[0] = 5;
            //verify byte array is a copy and not a reference
            Assert.IsFalse(ste.Bytes.SequenceEqual(sto.Bytes),"Byte array should not match");
            
        }

        [Then(@"I should only see value types on SomeTypeEntity having data")]
        public void ThenIShouldOnlySeeValueTypesOnSomeTypeEntityHavingData()
        {
            SomeTypeEntity ste = ScenarioContext.Current.Get<SomeTypeEntity>();
            SomeTypeObject sto = ScenarioContext.Current.Get<SomeTypeObject>();

            var s = ScenarioContext.Current.Get<TypeMapStore>();
            DataMapValidationList validationList = ScenarioContext.Current.Get<DataMapValidationList>();
            Boolean foundIgnoredObjectProperty = false;
            Boolean foundIgnoredObjectArrayProperty = false;
            foreach (var validationItem in validationList)
            {
                foreach (var item in validationItem.PropertyMapList)
                {
                    if (item.Description.ToLower().Contains("ignored"))
                    {
                        if (item.Description.ToLower().Contains("sometypeitemlist"))
                        {
                            foundIgnoredObjectProperty = true;
                        }
                        if (item.Description.ToLower().Contains("objectarray"))
                        {
                            foundIgnoredObjectArrayProperty = true;
                        }
                    }
                }
            }
            Assert.IsTrue(foundIgnoredObjectProperty, "Object property not ignored");
            Assert.IsTrue(foundIgnoredObjectArrayProperty, "Object array property not ignored");
            Assert.AreEqual(ste.AccountNumber, sto.AccountNumber, "Account numbers should match");
            Assert.IsNull(ste.SomeTypeItemList, "Some Type Item List should be null");
            Assert.IsTrue(ste.Bytes.SequenceEqual(sto.Bytes), "Byte array should match");
            Assert.IsTrue(ste.StringArray.SequenceEqual(sto.StringArray), "String array should match");

            sto.Bytes[0] = 5;
            //verify byte array is a copy and not a reference
            Assert.IsFalse(ste.Bytes.SequenceEqual(sto.Bytes), "Byte array should not match");
        }



    }
}
