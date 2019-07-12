using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class ModuleButtonConfiguration : EntityMappingConfiguration<ModuleButton>
    {
        public override void Map(EntityTypeBuilder<ModuleButton> b)
        {
            b.ToTable("ModuleButtons")
                .HasKey(p => p.Id);
        }
    }

}
