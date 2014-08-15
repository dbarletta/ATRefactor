﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface IAttributeRepository : IRepository<Entities.Attribute> { }

    public class AttributeRepository : RepositoryBase<Entities.Attribute>, IAttributeRepository
    {
        public AttributeRepository(IDataContextFactory factory)
            : base(factory)
        {

        }
    }
}
