using System;
using System.Collections.Generic;
using System.Text;

namespace RESTfulFS.Services.Tests
{
    /// <summary>
    ///     Configuration of mapping profiles in AutoMapper is handled by a static class.
    ///     This helper class allows tests to initialise (mapping profiles) and reset (the mapper) in a threadsafe manner.
    ///     The AutoMapper.Mapper (static) instance does not provide for validation of configuration, thus requiring this approach.
    /// </summary>
    public static class AutoMapperHelper
    {
        private static object _lockobject = new object();
        private static int _requests = 0;

        /// <summary>
        ///     Calls Initialize on AutoMapper.Mapper in a threadsafe manner.
        /// </summary>
        public static void Initialize()
        {
            lock (_lockobject)
            {
                if (_requests == 0)
                {
                    AutoMapper.Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>();
                    });
                }
                _requests++;
            }
        }

        /// <summary>
        ///     Calls Reset on AutoMapper.Mapper in a threadsafe manner.
        /// </summary>
        public static void Reset()
        {
            lock (_lockobject)
            {
                _requests--;
                if (_requests == 0)
                    AutoMapper.Mapper.Reset();
            }
        }

    }
}
