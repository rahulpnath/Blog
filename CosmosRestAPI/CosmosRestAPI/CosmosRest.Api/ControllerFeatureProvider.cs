using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using System.Linq;
using CosmosRest.Api.Controllers;
using CosmosRest.Domain.Aggregates;

namespace CosmosRest.Api
{
    public class ControllerFeatureProvider :
        IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            // Using convention to register the models and associated configuraton
            // Configuration files must have the Model name + Configuraration
            // e.g for a model HseWasteDisposal the config will be named HseWasteDisposalConfiguration
            var assemblies = GetAssemblies();
            var aggregates =
                assemblies.SelectMany(a => a.GetTypes().Where(c => c.IsClass && c.IsSubclassOf(typeof(AggregateRoot))));

            foreach (var aggregate in aggregates)
            {
                var typeInfo = typeof(GenericController<>)
                    .MakeGenericType(aggregate)
                    .GetTypeInfo();
                feature.Controllers.Add(typeInfo);
            }

            // Add endpoint that can take any json object and save it
            feature.Controllers.Add(typeof(GenericController<JObject>).GetTypeInfo());
        }

        private Assembly[] GetAssemblies()
        {
            // TODO: Load assemblies required here
            return new[] {typeof(AggregateRoot).Assembly};
        }
    }
}