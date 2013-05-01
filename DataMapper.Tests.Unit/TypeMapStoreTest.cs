
using DataMapper.TypeMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using TypeMock.ArrangeActAssert;

namespace DataMapper.Tests.Unit
{
    [TestClass]
    public class TypeMapStoreTest
    {
        [TestMethod]
        [ExpectedException(typeof(TypeMapException))]
        public void TestMethod_Validate_AlreadyFinished()
        {
            //arrange
            var fakeTypeMapStore = Isolate.Fake.Instance<TypeMapStore>();
            Isolate.WhenCalled(()=>fakeTypeMapStore.Validate()).CallOriginal();
            ObjectState.SetField(fakeTypeMapStore, "_finished", true);

            //act
            fakeTypeMapStore.Validate();


        }

        [TestMethod]
        [ExpectedException(typeof(TypeMapException))]
        public void TestMethod_Finish_AlreadyFinished()
        {
            //arrange
            var fakeTypeMapStore = Isolate.Fake.Instance<TypeMapStore>();
            Isolate.WhenCalled(() => fakeTypeMapStore.Finish()).CallOriginal();
            ObjectState.SetField(fakeTypeMapStore, "_finished", true);

            //act
            fakeTypeMapStore.Finish();


        }

        [TestMethod]
        [ExpectedException(typeof(TypeMapException))]
        public void TestMethod_Map_AlreadyFinished()
        {
            //arrange
            var fakeTypeMapStore = Isolate.Fake.Instance<TypeMapStore>();

            Isolate.WhenCalled(() => fakeTypeMapStore.Map<TestSource, TestTarget>(null,null)).CallOriginal();
            ObjectState.SetField(fakeTypeMapStore, "_finished", false);

            //act
            fakeTypeMapStore.Map<TestSource, TestTarget>(null, null);


        }

        //[TestMethod]
        //[ExpectedException(typeof(TypeMapException))]
        //public void TestMethod_Map_CommandNull()
        //{
        //    //arrange
        //    var fakeTypeMapStore = Isolate.Fake.Instance<TypeMapStore>();

        //    Isolate.WhenCalled(() => fakeTypeMapStore.Map<TestSource, TestTarget>(null, null)).CallOriginal();
        //    Isolate.NonPublic.WhenCalled(fakeTypeMapStore, "TryResolveDataMapCommand").CallOriginal();
        //    ObjectState.SetField(fakeTypeMapStore, "_finished", true);
        //    Isolate.NonPublic.WhenCalled(fakeTypeMapStore, "TryResolveDataMapCommand", null, null, CommandChangeDirection.ApplyChangesFromSourceToTarget).WillReturn(null);
        //    Isolate.NonPublic.WhenCalled(fakeTypeMapStore, "TryResolveDataMapCommand", null, null, CommandChangeDirection.ApplyChangesFromTargetToSource).WillReturn(null);

        //    //act
        //    fakeTypeMapStore.Map<TestSource, TestTarget>(null, null);


        //}

    }

  
}
