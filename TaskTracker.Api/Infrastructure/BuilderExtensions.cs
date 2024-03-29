﻿using Microsoft.EntityFrameworkCore;
using TaskTracker.Database;
using TaskTracker.Application.Authorization.Command;
using FluentValidation;
using TaskTracker.Application.Validator;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskTracker.Application;
using TaskTracker.Api.Infrastructure.Command;
using MediatR;

namespace TaskTracker.Api.Infrastructure
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IUserTaskRepository, UserTaskRepository>();
            builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
            builder.Services.AddScoped<IUserTaskStatusRepository, UserTaskStatusRepository>();
            builder.Services.AddScoped<ITaskStatusGroupRepository, TaskStatusGroupRepository>();
            builder.Services.AddScoped<IUserSpaceRepository, UserSpaceRepository>();
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
            builder.Services.AddScoped<IDocumentPageRepository, DocumentPageRepository>();
            builder.Services.AddScoped<ISpaceUserRepository, SpaceUserRepository>();
            builder.Services.AddScoped<ITaskAssignedUserRepository, TaskAssignedUserRepository>();
            builder.Services.AddScoped<ITaskFileAttachmentRepository, TaskFileAttachmentRepository>();
            builder.Services.AddScoped<ISpaceUserPermissionsRepository, SpaceUserPermissionsRepository>();

            return builder;
        }

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining<LoginCommand>();
                opt.RegisterServicesFromAssemblyContaining<SaveProfilePictureCommand>();
            });

            builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();

            builder.Services.AddScoped<IUserFactory, UserFactory>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IHashService, HashService>();
            builder.Services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
            builder.Services.AddScoped<IUserTaskFactory, UserTaskFactory>();
            builder.Services.AddScoped<ITaskListFactory, TaskListFactory>();
            builder.Services.AddScoped<IUserTaskStatusFactory, UserTaskStatusFactory>();
            builder.Services.AddScoped<ITaskStatusGroupFactory, TaskStatusGroupFactory>();
            builder.Services.AddScoped<IUserSpaceFactory,  UserSpaceFactory>();
            builder.Services.AddScoped<IDocumentFactory,  DocumentFactory>();
            builder.Services.AddScoped<IDocumentPageFactory, DocumentPageFactory>();
            builder.Services.AddScoped<ISpaceUserFactory, SpaceUserFactory>();
            builder.Services.AddScoped<ITaskAssignedUserFactory, TaskAssignedUserFactory>();
            builder.Services.AddScoped<ITaskFileAttachmentFactory, TaskFileAttachmentFactory>();
            builder.Services.AddScoped<ISpaceUserPermissionsFactory, SpaceUserPermissionsFactory>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DeleteTaskPipeline<,>));
            builder.Services.AddScoped<SpacePermissionsMiddleware>();

            return builder;
        }

        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "TaskTracker",
                    Description = ""
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]!);
                var key = new SymmetricSecurityKey(bytes);

                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],
                    ValidAudience = builder.Configuration["Auth:Audience"],
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256Signature },
                    ValidateIssuerSigningKey = true,
                };

                config.MapInboundClaims = false;
            });
        }
    }
}
