using System.Collections;
using System.Reflection;
using System.Text;
using BankingSystemOperations.Services.Contracts;

namespace BankingSystemOperations.Services;

public class CsvService : ICsvService
{
    public string PrepareCsvExport<BaseEntity>(IEnumerable<BaseEntity> entities)
    {
        if (entities is null || entities.Count() == 0)
        {
            return string.Empty;
        }
    
        var properties = typeof(BaseEntity).GetProperties()
            .Where(p => p.PropertyType.IsPrimitive || p.PropertyType.IsValueType || p.PropertyType == typeof(string))
            .ToList();
        
        var headers = properties.Select(p => p.Name).ToList();
        
        StringBuilder csvBuilder = new();

        csvBuilder.AppendLine(string.Join(",", headers));

        foreach (var entity in entities)
        {
            var row = headers.Select(h =>
            {
                var prop = properties.FirstOrDefault(p => p.Name.Equals(h, StringComparison.OrdinalIgnoreCase));
                var propValue = prop?.GetValue(entity, null);

                return propValue?.ToString()?.Replace(",", "");
            });
            
            csvBuilder.AppendLine(string.Join(",", row));
        }

        return csvBuilder.ToString().Trim();
    }
}