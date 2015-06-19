namespace Fanex.CustomerSetting.Core.Tests.SPObjects.Command.Validation
{
    using Fanex.CustomerSetting.Core.SPObjects.Commands.Validation;
    using NUnit.Framework;

    [TestFixture]
    public class PermissionValidationCommandTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_ActorIdIsNegativeOrZero_ReturnFalse(int actorId)
        {
            //// Arrange
            var permissionValidationCommand = CreatePermissionValidationCommandFakeObject();
            permissionValidationCommand.ActorId = actorId;

            //// Act
            var actualResult = permissionValidationCommand.IsValid();

            //// Assert
            Assert.IsFalse(actualResult);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_CustomerIdIsNegativeOrZero_ReturnFalse(int customerId)
        {
            //// Arrange
            var permissionValidationCommand = CreatePermissionValidationCommandFakeObject();
            permissionValidationCommand.CustomerId = customerId;

            //// Act
            var actualResult = permissionValidationCommand.IsValid();

            //// Assert
            Assert.IsFalse(actualResult);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_CustomerLevelIdIsNegativeOrZero_ReturnFalse(int customerLevel)
        {
            //// Arrange
            var permissionValidationCommand = CreatePermissionValidationCommandFakeObject();
            permissionValidationCommand.CustomerLevelId = customerLevel;

            //// Act
            var actualResult = permissionValidationCommand.IsValid();

            //// Assert
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsValid_DataIsValid_ReturnTrue()
        {
            //// Arrange
            var permissionValidationCommand = CreatePermissionValidationCommandFakeObject();

            //// Act
            var actualResult = permissionValidationCommand.IsValid();

            //// Assert
            Assert.IsTrue(actualResult);
        }

        private PermissionValidationCommand CreatePermissionValidationCommandFakeObject()
        {
            return new PermissionValidationCommand
            {
                ActorId = 1,
                CustomerId = 1,
                CustomerLevelId = 1
            };
        }
    }
}