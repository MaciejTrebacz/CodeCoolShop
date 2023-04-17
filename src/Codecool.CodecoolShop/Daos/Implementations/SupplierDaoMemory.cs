using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class SupplierDaoMemory : ISupplierDao
    {
        private List<Supplier> data = new List<Supplier>();
        private static SupplierDaoMemory instance = null;

        private SupplierDaoMemory()
        {
        }

        public static SupplierDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new SupplierDaoMemory();
            }

            return instance;
        }

        public void Add(Supplier item)
        {
            item.Id = data.Count + 1;
            data.Add(item);
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public Supplier Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<Supplier> GetAll()
        {
            return data;
        }
    }
}
