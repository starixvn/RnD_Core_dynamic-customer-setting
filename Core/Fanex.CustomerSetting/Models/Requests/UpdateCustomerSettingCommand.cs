namespace Fanex.CustomerSetting.Models.Requests
{
    using Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using System;

    public class UpdateCustomerSettingCommand
    {
        private Level _uplineLevel;
        private ISetting _setting;

        public UpdateCustomerSettingCommand(
            ISetting setting,
            Level uplineLevel = null)
        {
            Setting = setting;
            UplineLevel = uplineLevel;
        }

        public ISetting Setting
        {
            get
            {
                return _setting;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Setting cannot be null when update customer setting.");
                }

                _setting = value;
            }
        }

        public Level UplineLevel
        {
            get
            {
                return _uplineLevel;
            }

            set
            {
                try
                {
                    if (value != null)
                    {
                        if (value < Level.Agent || value > Level.Super)
                        {
                            throw new ArgumentOutOfRangeException(nameof(value), "Upline Level is out of value range.");
                        }

                        if (Setting.CustomerLevel.IsNotMemberLevel())
                        {
                            throw new ArgumentOutOfRangeException(nameof(value), "Upline should be null when getting customer setting is not member");
                        }
                    }
#pragma warning disable S126 // "if ... else if" constructs should end with "else" clauses
                    else if (Setting.CustomerLevel.IsMemberLevel())
#pragma warning restore S126 // "if ... else if" constructs should end with "else" clauses
                    {
                        throw new ArgumentNullException(nameof(value), "Upline cannot be null when update member setting.");
                    }

                    _uplineLevel = value;
                }
                catch (ArgumentNullException ex)
                {
                    throw new InvalidInputException(ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new InvalidInputException(ex.Message);
                }
            }
        }
    }
}