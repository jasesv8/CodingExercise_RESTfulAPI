using System;
using System.Collections.Generic;
using System.Text;

namespace RESTfulFS.Infrastructure
{
    /// <summary>
    ///     Keeps all of the 'text' values in one spot.
    ///     Makes maintenance easier;
    ///         Easier to search code for use of constant than a literal value
    ///         Easier to update literals in single location
    ///     Provides for cleaner testing (ie; when doing string comparison of results).
    /// </summary>
    public static class Constants
    {
        public const int HTTP_500 = 500;

        public const string JSONEXCEPTIONFILTER_MESSAGE = "A server error occurred.";

        public const string QUERYARGSISSUES_FROMDATE = "The 'fromDate' cannot be null or less than the current date.";
        public const string QUERYARGSISSUES_TODATE = "The 'toDate' cannot be null or less than the fromDate date.";
        public const string QUERYARGSISSUES_DATERANGE = "The range between 'fromDate' and 'toDate' cannot exceed 14 days.";
        public const string QUERYARGSISSUES_PASSENGERS = "The 'passengers' cannnot be null or less than 1.";
    }
}
