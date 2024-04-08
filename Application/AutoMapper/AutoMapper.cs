using System.Reflection;
using AutoMapper;

namespace Application.AutoMapper;

public class AutoMapper : Profile
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            var allTypes = Assembly.GetEntryAssembly()!.GetExportedTypes();
            var dtoTypes = allTypes.Where(p =>
                p is { IsAbstract: false, IsClass: true, IsPublic: true } &&
                p.GetInterfaces().Contains(typeof(ICustomMapping)));
            foreach (var type in dtoTypes)
            {
                var createMapping = Activator.CreateInstance(type) as ICustomMapping;
                createMapping!.ApplyMapping(this);
            }
        }
    }
}

