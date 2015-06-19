namespace Fanex.CustomerSetting.Core.Tests.SPObjects.Command.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.SPObjects.Commands.Validation;
    using NUnit.Framework;

    [TestFixture]
    public class PermissionValidationMultipleCommandTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void IsValid_MemberIdsIsNullOrEmpty_ReturnFalse(string memberIds)
        {
            //// Arrange
            var permissionValidationMultipleCommand = CreatePermissionValidationMultipleCommandFakeObject();
            permissionValidationMultipleCommand.MemberIds = memberIds;

            //// Act
            var actualResult = permissionValidationMultipleCommand.IsValid();

            //// Assert
            Assert.IsFalse(actualResult);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_ActorIdIsNegativeOrZero_ReturnFalse(int actorId)
        {
            //// Arrange
            var permissionValidationMultipleCommand = CreatePermissionValidationMultipleCommandFakeObject();
            permissionValidationMultipleCommand.ActorId = actorId;

            //// Act
            var actualResult = permissionValidationMultipleCommand.IsValid();

            //// Assert
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsValid_DataIsValid_ReturnTrue()
        {
            //// Arrange
            var permissionValidationMultipleCommand = CreatePermissionValidationMultipleCommandFakeObject();

            //// Act
            var actualResult = permissionValidationMultipleCommand.IsValid();

            //// Assert
            Assert.IsTrue(actualResult);
        }

        private PermissionValidationMultipleCommand CreatePermissionValidationMultipleCommandFakeObject()
        {
            return new PermissionValidationMultipleCommand
            {
                ActorId = 1,
                MemberIds = "1,2"
            };
        }
    }
}
