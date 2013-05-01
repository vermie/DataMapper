
using System;
using DataMapper.Conversion;
using DataMapper.TypeMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMapper.Tests.Unit
{
    [TestClass]
    public class DataMapperStoreTest
    {
        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void TestMethod_NullConverter_Convert()
        {
            //arrange
            var nullConverter = new NullConverter();
            String s = String.Empty;
            Int32 i = 0;

            
            //act
            nullConverter.Convert(s.GetType(), i.GetType(), null);


        }

      
    }

}
