using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Tinkr.Identity.Service.Entities
{
    [CollectionName("Roles")]
    public class ApplicationRoles : MongoIdentityRole<Guid>
    {

    }
}