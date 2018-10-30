using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace CAFU.Generics.Domain.Model
{
    public class GenericStateModelTest
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private enum Fixture
        {
            Foo,
            Bar,
            Baz,
            Qux,
        }

        [Test]
        public void EnumNextTest()
        {
            var model = new GenericStateModel<Fixture>();
            Assert.AreEqual(default(Fixture), model.GetCurrent());
            model.Next();
            model.Next();
            Assert.AreEqual(Fixture.Baz, model.GetCurrent());
            model.Next();
            Assert.AreEqual(Fixture.Qux, model.GetCurrent());
            model.Next();
            model.Next();
            Assert.AreEqual(Fixture.Qux, model.GetCurrent());
        }

        [Test]
        public void EnumPreviousTest()
        {
            var model = new GenericStateModel<Fixture>(Fixture.Qux);
            Assert.AreEqual(Fixture.Qux, model.GetCurrent());
            model.Previous();
            Assert.AreEqual(Fixture.Baz, model.GetCurrent());
            model.Previous();
            model.Previous();
            Assert.AreEqual(Fixture.Foo, model.GetCurrent());
            model.Previous();
            model.Previous();
            Assert.AreEqual(Fixture.Foo, model.GetCurrent());
        }

        [Test]
        public void PrimitiveTest()
        {
            var model1 = new GenericStateModel<int>();
            model1.Change(2);
            Assert.Throws<InvalidOperationException>(() => model1.Next());
            Assert.Throws<InvalidOperationException>(() => model1.Previous());
            var model2 = new GenericStateModel<bool>();
            model2.Change(false);
            Assert.Throws<InvalidOperationException>(() => model2.Next());
            Assert.Throws<InvalidOperationException>(() => model2.Previous());
        }
    }
}