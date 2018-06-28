using System;
using System.Collections.Generic;
using System.Text;

namespace RESTfulFS.Models
{
    /// <summary>
    ///     A simple wrapper that we can use for exception information.
    /// </summary>
    public class ApiError
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public string StackTrace { get; set; }

        #region Constructors

        /// <summary>
        ///     Default Constructor (no arguments)
        /// </summary>
        public ApiError()
        {
        }

        /// <summary>
        ///     Constructor to simplify use when we just have a message
        /// </summary>
        /// <param name="message">The error message</param>
        public ApiError(string message)
        {
            Message = message;
        }

        /// <summary>
        ///     Constructor to simplify use when we just have a message and some detail
        /// </summary>
        /// <param name="message">The error messasge</param>
        /// <param name="detail">The error detail</param>
        public ApiError(string message, string detail)
        {
            Message = message;
            Detail = detail;
        }

        #endregion
    }
}
