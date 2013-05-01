
using System;
using DataMapper.Conversion;
using DataMapper.TypeMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using TypeMock.ArrangeActAssert;

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
            var fakeNullConverter = Isolate.Fake.Instance<NullConverter>();
            String s = String.Empty;
            Int32 i = 0;
            Isolate.WhenCalled(() => fakeNullConverter.Convert(null, null,null)).CallOriginal();

            
            //act
            fakeNullConverter.Convert(s.GetType(), i.GetType(), null);


        }

      
    }

}
