using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class UserConfiguration : EntityMappingConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> b)
        {
            b.ToTable("Users")
                .HasKey(p => p.Id);
        }
    }

}
