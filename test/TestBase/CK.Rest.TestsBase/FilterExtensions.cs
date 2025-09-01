using System;
using System.Collections.Immutable;

using CK.Entities;
using CK.Repository;

namespace CK.Rest.TestsBase
{
    public static class FilterExtensions
    {
        #region Public Methods

        public static bool ResolveFilters<T, TKey>(this IImmutableList<Filter<T>> filters, T entity)
            where T : Entity<TKey>
            where TKey : struct
        {
            var result = true;

            foreach (var filter in filters)
            {
                var property = entity.GetType().GetProperty(filter.Property);
                if (property is null)
                {
                    switch (filter.Property)
                    {
                        case "NameOrSurname":
                            var property1 = entity.GetType().GetProperty("Name");
                            var property2 = entity.GetType().GetProperty("Surname");
                            var value = filter.Value as (string Name, string Surname)?;
                            result = ((string)property1.GetValue(entity)).Contains(value.Value.Name.Replace('%', ' ').Trim(), StringComparison.InvariantCulture) ||
                                ((string)property2.GetValue(entity)).Contains(value.Value.Surname.Replace('%', ' ').Trim(), StringComparison.InvariantCulture);
                            break;
                    }
                }
                else if (property.PropertyType == typeof(bool))
                {
                    result = (bool)property.GetValue(entity) == (bool)filter.Value;
                }
                else if (property.PropertyType == typeof(string))
                {
                    result = ((string)property.GetValue(entity)).Contains(filter.Value.ToString().Replace('%', ' ').Trim(), StringComparison.InvariantCulture);
                }
                else if (property.PropertyType == typeof(uint))
                {
                    result = (uint)property.GetValue(entity) == (uint)filter.Value;
                }

                if (!result)
                    break;
            }

            return result;
        }

        #endregion Public Methods
    }
}