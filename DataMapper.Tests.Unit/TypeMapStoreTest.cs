
using DataMapper.TypeMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            var fakeTypeMapStore = new Mock<TypeMapStore>() { CallBase = true };

            var typeMapStore = fakeTypeMapStore.Object;

            // TODO: find more isolated way to set _finished to true
            typeMapStore.Finish();

            //act
            typeMapStore.Validate();


        }

        [TestMethod]
        [ExpectedException(typeof(TypeMapException))]
        public void TestMethod_Finish_AlreadyFinished()
        {
            //arrange
            var fakeTypeMapStore = new Mock<TypeMapStore>() { CallBase = true };

            var typeMapStore = fakeTypeMapStore.Object;

            // TODO: find more isolated way to set _finished to true
            typeMapStore.Finish();

            //act
            typeMapStore.Finish();


        }

        [TestMethod]
        [ExpectedException(typeof(TypeMapException))]
        public void TestMethod_Map_AlreadyFinished()
        {
            //arrange
            var fakeTypeMapStore = new Mock<TypeMapStore>() { CallBase = true };

            var typeMapStore = fakeTypeMapStore.Object;

            //act
            typeMapStore.Map<TestSource, TestTarget>(null, null);


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
