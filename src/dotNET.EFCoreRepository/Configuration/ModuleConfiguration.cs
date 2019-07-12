using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class ModuleConfiguration : EntityMappingConfiguration<Module>
    {
        public override void Map(EntityTypeBuilder<Module> b)
        {
            b.ToTable("Modules")
                .HasKey(p => p.Id);
        }
    }

}
