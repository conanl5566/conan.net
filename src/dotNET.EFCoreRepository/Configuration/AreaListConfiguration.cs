using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class AreaListConfiguration : EntityMappingConfiguration<AreaList>
    {
        public override void Map(EntityTypeBuilder<AreaList> b)
        {
            b.ToTable("AreaLists")
                .HasKey(p => p.Id);
        }
    }

}
