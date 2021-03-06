﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PrismTemplates.DalInterface.Models;

namespace PrismTemplates.DalInterface.Repositories
{
    public interface IEntityRepository
    {
        IEnumerable<Entity> GetEntities();
        Entity GetEntity(int entityId);
        Entity Create(Entity entity);
        int Update(Entity entity);
        int Delete(int entityId);
        void Reset();
    }
}