using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class RoleAuthorizeConfiguration : EntityMappingConfiguration<RoleAuthorize>
    {
        public override void Map(EntityTypeBuilder<RoleAuthorize> b)
        {
            b.ToTable("RoleAuthorizes")
                .HasKey(p => p.Id);
        }
    }

}
