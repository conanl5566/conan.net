using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNET.EFCoreRepository
{
 
    public class DepartmentConfiguration : EntityMappingConfiguration<Department>
    {
        public override void Map(EntityTypeBuilder<Department> b)
        {
            b.ToTable("Departments")
                .HasKey(p => p.Id);
        }
    }

}
