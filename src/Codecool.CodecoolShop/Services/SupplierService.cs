﻿using Codecool.CodecoolShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Codecool.CodecoolShop.Services;

public class SupplierService
{
    private readonly CodeCoolShopDBContext _dbContext;

    public SupplierService(CodeCoolShopDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _dbContext.Suppliers.ToListAsync();
    }


}