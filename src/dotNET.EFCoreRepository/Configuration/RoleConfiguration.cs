using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class RoleConfiguration : EntityMappingConfiguration<Role>
    {
        public override void Map(EntityTypeBuilder<Role> b)
        {
            b.ToTable("Roles")
                .HasKey(p => p.Id);
        }
    }

}
