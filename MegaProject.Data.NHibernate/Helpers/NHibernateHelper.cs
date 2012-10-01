using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Context;

namespace MegaProject.Data.NHibernate.Helpers
{
    public class NHibernateHelper
    {
        private static readonly object Locker = new object();
        private static ISessionFactory _factory;

        /// <summary>
        /// Thread safe NHibernate Session Factory
        /// </summary>
        public static ISessionFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    lock (Locker)
                    {
                        _factory = Fluently.Configure(new global::NHibernate.Cfg.Configuration().Configure())
                            //.CurrentSessionContext<ThreadStaticSessionContext>()
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
                            .BuildSessionFactory();                   
                    }
                }
                return _factory;
            }
        }
    }
}
