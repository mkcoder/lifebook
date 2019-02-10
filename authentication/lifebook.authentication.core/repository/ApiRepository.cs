using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using lifebook.authentication.core.dbcontext;
using lifebook.core.database.databaseprovider.services;
using Microsoft.EntityFrameworkCore;

namespace lifebook.authentication.core.repository
{
    public class ApiRepository
    {
        protected ApiResourcesDbContext ApiResourcesDbContext { get; set; }

        public ApiRepository(ApiResourcesDbContext apiResourcesDbContext)
        {
            ApiResourcesDbContext = apiResourcesDbContext;
        }

        public List<ApiResource> GetAllApiResources() => ApiResourcesDbContext.ApiResources.ToList();

        public void AddNewApiResource(ApiResource apiResource)
        {
            ApiResourcesDbContext.ApiResources.Add(apiResource);
            ApiResourcesDbContext.SaveChanges();
        }
    }
}
