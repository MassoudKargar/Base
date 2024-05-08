// Global using directives

global using Base.Core.ApplicationServices.Commands;
global using Base.Core.ApplicationServices.Events;
global using Base.Core.ApplicationServices.Queries;
global using Base.EndPoints.Web.Controllers;
global using Base.EndPoints.Web.Extensions.DependencyInjection;
global using Base.EndPoints.Web.Extensions.ModelBinding;
global using Base.Infra.Data.Sql.Commands.Interceptors;
global using Base.Samples.Core.Contracts.People.Commands;
global using Base.Samples.EndPoints.WebApi.CustomDecorators;
global using Base.Samples.EndPoints.WebApi.Extensions;
global using Base.Samples.EndPoints.WebApi.Extensions.DependencyInjection.IdentityServer.Options;
global using Base.Samples.EndPoints.WebApi.Extensions.DependencyInjection.Swaggers.Extensions;
global using Base.Samples.EndPoints.WebApi.Extensions.DependencyInjection.Swaggers.Filters;
global using Base.Samples.EndPoints.WebApi.Extensions.DependencyInjection.Swaggers.Options;
global using Base.Samples.Infra.Data.Sql.Commands.Common;
global using Base.Samples.Infra.Data.Sql.Queries.Common;

global using Microsoft.AspNetCore.Cors.Infrastructure;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;


global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.SwaggerUI;

namespace Base;

public class BaseClass;