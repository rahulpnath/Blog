using System.Linq;
using CosmosRest.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CosmosRest.Api
{
    public class ControllerRouteConvention: IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                var data = controller.ControllerType.GetGenericArguments().FirstOrDefault();
                if (data != null)
                {

                    var modelType = data.IsSubclassOf(typeof(AggregateRoot)) ?  data.Name : "dynamic/{modelType}";
                    var selector = new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"api/{{resourceType}}/{{resourceId}}/{modelType}"))
                    };
                    controller.Properties["ModelType"] = modelType;
                    selector.EndpointMetadata.Add(modelType);
                    controller.Selectors.Add(selector);
                }
            }
        }
    }
}