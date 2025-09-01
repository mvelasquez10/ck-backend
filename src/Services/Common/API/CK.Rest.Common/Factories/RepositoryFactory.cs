using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CK.Entities;
using CK.Repository;

namespace CK.Rest.Common.Factories
{
    public static class RepositoryFactory
    {
        #region Public Methods

        public static EntityRepository<T, TKey> GetRepository<T, TKey>(string assemblyPath, string connectionString, IEnumerable<T> seeds = null)
            where T : Entity<TKey>
            where TKey : struct
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                var type = assembly.GetTypes().First(t => typeof(EntityRepository<T, TKey>).IsAssignableFrom(t))
                    ?? throw new ArgumentNullException($"No class suitable to instantiate {typeof(EntityRepository<T, TKey>)} from {assemblyPath}");

                var instance = Activator.CreateInstance(type, new[] { connectionString })
                    ?? throw new ArgumentNullException($"No constructor suitable to instantiate {typeof(EntityRepository<T, TKey>)} from {assemblyPath}");

                var repository = instance as EntityRepository<T, TKey>;

                if (seeds != null && seeds.Any() && !repository.Exist())
                {
                    foreach (var seed in seeds)
                    {
                        repository.AddOrUpdate(seed);
                    }
                }

                return repository;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = Directory.GetFiles(Path.GetDirectoryName(args.RequestingAssembly.Location), $"{args.Name.Split(',')[0]}*").FirstOrDefault();
            if (assembly != null)
            {
                return Assembly.LoadFrom(assembly);
            }

            return null;
        }

        #endregion Private Methods
    }
}