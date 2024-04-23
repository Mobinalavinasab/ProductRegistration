using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Core.Extension;

public static class ExtensionModelBuilder
{
    public static void allEntity<BaseEntity>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(p => p.IsClass && p.IsSubclassOf(typeof(BaseEntity)));
        foreach (Type type in types)
        {
            modelBuilder.Entity(type);
        }
    }
}