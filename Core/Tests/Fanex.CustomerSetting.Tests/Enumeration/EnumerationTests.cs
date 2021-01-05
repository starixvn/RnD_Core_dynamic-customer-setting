namespace Fanex.CustomerSetting.Tests.Enumeration
{
    using Fanex.CustomerSetting.Models.Enumerations;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class EnumerationTests
    {
        [Test]
        public static void AbsoluteDifference_SuperAndMember_Return3()
        {
            Assert.AreEqual(3, Level.AbsoluteDifference(Level.Super, Level.Member));
        }

        [Test]
        public static void EqualOperator_EqualLeftRight_ReturnTrue()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;

            Assert.IsTrue(nullLeft == nullRight);
            Assert.IsTrue(Level.Super == Level.Super);
        }

        [Test]
        public static void EqualOperator_NotEqualLeftRight_ReturnFalse()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;

            Assert.IsFalse(Level.Super == nullRight);
            Assert.IsFalse(nullLeft == Level.Master);
            Assert.IsFalse(Level.Super == Level.Master);
        }

        [Test]
        public void NotEqualOperator_EqualLeftRight_ReturnFalse()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;

            Assert.IsFalse(nullLeft != nullRight);
            Assert.IsFalse(Level.Super != Level.Super);
        }

        [Test]
        public void NotEqualOperator_NotEqualLeftRight_ReturnTrue()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;

            Assert.IsTrue(Level.Super != nullRight);
            Assert.IsTrue(nullLeft != Level.Master);
            Assert.IsTrue(Level.Super != Level.Master);
        }

        [Test]
        public static void GreaterThan_LeftOrRightIsNull_ThrowArgumentNullException()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;
            bool result;
            Assert.Throws<ArgumentNullException>(() => result = nullLeft > Level.Master);
            Assert.Throws<ArgumentNullException>(() => result = Level.Super > nullRight);
            Assert.Throws<ArgumentNullException>(() => result = nullLeft > nullRight);
        }

        [Test]
        public static void GreaterThan_LeftGreaterThanRight_ReturnTrue()
        {
            Assert.IsTrue(Level.Super > Level.Master);
        }

        [Test]
        public static void GreaterThan_LeftNotGreaterThanRight_ReturnFalse()
        {
            Assert.IsFalse(Level.Super > Level.Super);
        }

        [Test]
        public static void GreaterThanOrEqual_LeftOrRightIsNull_ThrowArgumentNullException()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;
            bool result;
            Assert.Throws<ArgumentNullException>(() => result = null >= Level.Super);
            Assert.Throws<ArgumentNullException>(() => result = Level.Super >= null);
            Assert.Throws<ArgumentNullException>(() => result = nullLeft >= nullRight);
        }

        [Test]
        public static void GreaterThanOrEqual_LeftGreaterOrEqualThanRight_ReturnTrue()
        {
            Assert.IsTrue(Level.Super >= Level.Master);
            Assert.IsTrue(Level.Master >= Level.Master);
        }

        [Test]
        public static void GreaterThanOrEqual_LeftNotGreaterOrEqualThanRight_ReturnFalse()
        {
            Assert.IsFalse(Level.Master >= Level.Super);
            Assert.IsFalse(Level.Agent >= Level.Super);
        }

        [Test]
        public static void LesserThan_LeftOrRightIsNull_ThrowArgumentNullException()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;
            bool result;
            Assert.Throws<ArgumentNullException>(() => result = nullLeft < Level.Master);
            Assert.Throws<ArgumentNullException>(() => result = Level.Super < nullRight);
            Assert.Throws<ArgumentNullException>(() => result = nullLeft < nullRight);
        }

        [Test]
        public static void LesserThan_LeftLesserThanRight_ReturnTrue()
        {
            Assert.IsTrue(Level.Master < Level.Super);
        }

        [Test]
        public static void LesserThan_LeftNotLesserThanRight_ReturnFalse()
        {
            Assert.IsFalse(Level.Super < Level.Super);
        }

        [Test]
        public static void LesserThanOrEqual_LeftOrRightIsNull_ThrowArgumentNullException()
        {
            Enumeration nullLeft = null;
            Enumeration nullRight = null;
            bool result;
            Assert.Throws<ArgumentNullException>(() => result = null <= Level.Super);
            Assert.Throws<ArgumentNullException>(() => result = Level.Super <= null);
            Assert.Throws<ArgumentNullException>(() => result = nullLeft <= nullRight);
        }

        [Test]
        public static void LesserThanOrEqual_LeftGreaterOrEqualThanRight_ReturnTrue()
        {
            Assert.IsTrue(Level.Master <= Level.Super);
            Assert.IsTrue(Level.Master <= Level.Master);
        }

        [Test]
        public static void LesserThanOrEqual_LeftNotGreaterOrEqualThanRight_ReturnFalse()
        {
            Assert.IsFalse(Level.Super <= Level.Master);
            Assert.IsFalse(Level.Super <= Level.Agent);
        }

        [Test]
        public void Equals_Null_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.Equals(null));
        }

        [Test]
        public void Equals_TypeNotMatched_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.Equals(4));
        }

        [Test]
        public void Equals_TypeMatchedValueNotMatched_ReturnFalse()
        {
            Assert.IsFalse(Level.Super.Equals(Level.Master));
        }

        [Test]
        public void Equals_TypeMatchedValueMatched_ReturnTrue()
        {
            Assert.IsTrue(Level.Super.Equals(Level.Super));
        }

        [Test]
        public void GetHashCode_Always_Success()
        {
            Assert.DoesNotThrow(() => Level.Super.GetHashCode());
        }

        [Test]
        public void ToString_Always_ReturnDisplayName()
        {
            var expected = Level.Super.DisplayName;
            var actual = Level.Super.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareTo_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(()=>Level.Super.CompareTo(null));
        }

        [Test]
        public void CompareTo_LesserThan_ReturnLessThanZero()
        {
            Assert.That(Level.Agent.CompareTo(Level.Super) < 0);
        }

        [Test]
        public void CompareTo_GreaterThan_ReturnGreaterThanZero()
        {
            Assert.That(Level.Super.CompareTo(Level.Master) > 0);
        }

        [Test]
        public void CompareTo_Equals_ReturnEqualsThanZero()
        {
            Assert.That(Level.Super.CompareTo(Level.Super) == 0);
        }
    }
}
