namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores result information for displaying on web.
    /// </summary>
    public interface IResult
    {
        int Code { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The customer id incase update multiple.
        /// The setting type id incase update all setting type.
        /// </value>
        int Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The customer name incase update multiple.
        /// The setting type name incase update all setting type.
        /// </value>
        string Name { get; }

        string Message { get; }

        /// <summary>
        /// Gets the type of the MSG.
        /// </summary>
        /// <value>
        /// Message type for more details about message return after updated.
        /// </value>
        int MsgType { get; }

        bool IsSuccessful { get; }

        /// <summary>
        /// Gets the sub results.
        /// </summary>
        /// <value>
        /// Store sub result in case update multiple customer or update all setting types.
        /// </value>
        IEnumerable<IResult> SubResults { get; }

        /// <summary>
        /// Gets the sub messages.
        /// </summary>
        /// <value>
        /// Contains sub message incase message contains a list of data.
        /// </value>
        IEnumerable<string> SubMessages { get; }
    }
}