using MongoDb.ASP.NETCore3CRUDSample.DataAccess.Models;
using MongoDb.ASP.NETCore3CRUDSample.Models;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.ASP.NETCore3CRUDSample.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerContext _context;
        public CustomerRepository(ICustomerContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
           // return await _context.Customers.Find(_ => true).ToListAsync(); // (_ => true )filter that passes everything
            return await _context.Customers.Find(Builders<Customer>.Filter.Empty).ToListAsync(); // same in the new driver
        }
         
        public Task<Customer> GetCustomer(int id)
        {
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x =>x.CustomerID, id);

            return _context.Customers.Find(filter).FirstOrDefaultAsync();

        }

        public async Task Create(Customer customer)
        {
            await _context.Customers.InsertOneAsync(customer);
        }

        public async Task<bool> Update(Customer customer)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Customers
                        .ReplaceOneAsync(
                            filter: c => c.Id == customer.Id,
                            replacement: customer);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(int id)
        {
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(m => m.CustomerID, id);

            DeleteResult deleteResult = await _context
                                                .Customers
                                                .DeleteOneAsync(filter);
           

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }


    }

    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomer(int id);
        Task Create(Customer customer);
        Task<bool> Update(Customer customer);
        Task<bool> Delete(int id);
    }

}
