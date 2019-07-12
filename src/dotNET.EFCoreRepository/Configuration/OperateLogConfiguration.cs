using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class OperateLogConfiguration : EntityMappingConfiguration<OperateLog>
    {
        public override void Map(EntityTypeBuilder<OperateLog> b)
        {
            b.ToTable("OperateLogs")
                .HasKey(p => p.Id);
        }
    }

}
