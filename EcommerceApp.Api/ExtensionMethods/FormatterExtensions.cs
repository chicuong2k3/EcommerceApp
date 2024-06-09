using EcommerceApp.Api.Formatters;

namespace EcommerceApp.Api.ExtensionMethods
{
    public static class FormatterExtensions
    {
        public static IMvcBuilder AddCSVFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }
    }
}