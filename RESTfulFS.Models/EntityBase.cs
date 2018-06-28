using System;
using System.Collections.Generic;
using System.Text;

namespace RESTfulFS.Models
{
    /// <summary>
    ///     EntityBase is an abstract class that should be inherited by an Entity.
    ///     The purpose is to provide basic properties like CreatedDate and ModifiedDate.
    /// </summary>
    public abstract class EntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        #region Constructor

        /// <summary>
        ///     Default Constructor (no args).
        ///     Initialises both CreatedDate and ModifiedDate properties to DateTime.UtcNow.
        /// </summary>
        protected EntityBase()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        #endregion
    }
}
