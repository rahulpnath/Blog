using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosRest.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CosmosRest.Api.Controllers
{
    [ApiController]
    [Route("{resourceType}/{resourceId}/{modelType}")]
    public class GenericController<T> : ControllerBase
    {
        public IGenericRepository<T> Repository { get; }

        public GenericController(IGenericRepository<T> repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll(string resourceType, Guid resourceId)
        {
            var modelType = GetModelType();
            var forms = await Repository.GetAll(resourceType, resourceId, modelType);
            return Ok(forms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(Guid id, Guid resourceId, string? modelType)
        {
            modelType ??= GetModelType();
            var form = await Repository.Get(id, resourceId, modelType);
            return Ok(form);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Guid resourceId, T data, string? modelType)
        {
            modelType ??= GetModelType();
            await Repository.Add(data, resourceId, modelType);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, Guid resourceId, T data, string modelType)
        {
            modelType ??= GetModelType();

            await Repository.Update(data, resourceId, modelType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, Guid resourceId, string modelType)
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