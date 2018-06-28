using RESTfulFS.Infrastructure;
using RESTfulFS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RESTfulFS.Api.Filters
{
    /// <summary>
    ///     An IExceptionFilter that allows us to capture all excepts and have them returned;
    ///         1. As JSON to the client
    ///         2. With HTTP response of 500
    /// </summary>
    public class JsonExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;

        #region Constructor

        /// <summary>
        ///     Allows for injection of IHostingEnvironment
        /// </summary>
        /// <param name="env">Instance of IHostingEnvironment implementation</param>
        public JsonExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Allows us to intercept the exception and update the current context.
        /// </summary>
        /// <param name="context">ExceptionContext passed in</param>
        public void OnException(ExceptionContext context)
        {
            // Instantiate an ApiError object with basic exception information
            var error = new ApiError(
                Constants.JSONEXCEPTIONFILTER_MESSAGE,
                context.Exception.Message
            );

            // If we are in a development environment, also add the StackTrace!
            if (_env.IsDevelopment())
                error.StackTrace = context.Exception.StackTrace;

            // Return the result and set the HTTP status to represent server error
            context.Result = new ObjectResult(error)
            {
                StatusCode = Constants.HTTP_500
            };
        }

        #endregion

    }
}
