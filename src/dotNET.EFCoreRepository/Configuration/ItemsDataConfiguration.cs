using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class ItemsDataConfiguration : EntityMappingConfiguration<ItemsData>
    {
        public override void Map(EntityTypeBuilder<ItemsData> b)
        {
            b.ToTable("ItemsDatas")
                .HasKey(p => p.Id);
        }
    }

}
