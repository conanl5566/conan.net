using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class ErrorLogConfiguration : EntityMappingConfiguration<ErrorLog>
    {
        public override void Map(EntityTypeBuilder<ErrorLog> b)
        {
            b.ToTable("ErrorLogs")
                .HasKey(p => p.Id);
        }
    }

}
