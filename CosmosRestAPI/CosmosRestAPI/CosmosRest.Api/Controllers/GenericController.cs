using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosRest.Domain;
using CosmosRest.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace CosmosRest.Api.Controllers
{
    [ApiController]
    [Route("api/{resourceType}/{resourceId}/{modelType}")]
    public class GenericController<T> : ControllerBase 
    {
        public IRepository<T> Repository { get; }

        public GenericController(IRepository<T> repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll(string resourceType, string resourceId)
        {
            var modelType = GetModelType();
            var forms = await Repository.GetAll(resourceType, resourceId, modelType);
            return Ok(forms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(string id, string resourceId, string? modelType)
        {
            modelType ??= GetModelType();
            var form = await Repository.Get(id, resourceId, modelType);
            return Ok(form);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string resourceId, T data, string? modelType)
        {
            modelType ??= GetModelType();
            await Repository.Add(data, resourceId, modelType);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, string resourceId, T data, string modelType)
        {
            modelType ??= GetModelType();

            await Repository.Update(data, resourceId, modelType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id, string resourceId, string modelType)
        {
            modelType ??= GetModelType();
            await Repository.Delete(id, resourceId, modelType);
            return NoContent();
        }

        private string GetModelType()
        {
            return ControllerContext.ActionDescriptor.Properties["ModelType"].ToString();
        }
    }
}